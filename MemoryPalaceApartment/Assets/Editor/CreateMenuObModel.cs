using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System;

//Creates an in game menu based on the specified json file and object folder
[CustomEditor(typeof(CreateMenuObModel))]
[CanEditMultipleObjects]
public class CreateMenuObModel : EditorWindow
{
    public static string basePath; //the path to the top level menu folder
    public static GameObject prefab_menu; //the prefab for the menu (a gameobject with a transform and menu script attached)
    public static List<GameObject> submenus; //a list of all the submenus the menu has
    static string jsonFilePath = ""; //the path to the json file
    static string jsonFileName = "menu_1"; //the name of the json file


    [MenuItem("Window/MakeMenu")]
    public static void ShowWindow() //create a unity editor window that can be accessed from the windows tab
    {
        EditorWindow.GetWindow(typeof(CreateMenuObModel));
    }

    void OnGUI() //how the editor window looks
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel); //defines the settings for running the script
        GUILayout.Space(10);
        jsonFileName = EditorGUILayout.TextField("Name of JSON input file:", jsonFileName); //the name of the json file in question
        if (GUI.Button(new Rect(200, 90, 150, 30), "Select JSON input file")) //lets the user manually input where the json file is in the folder structure
            jsonFilePath = EditorUtility.OpenFolderPanel("Select base menu folder", UnityEngine.Application.dataPath, UnityEngine.Application.dataPath);
        jsonFilePath= EditorGUILayout.TextField("Data path:", jsonFilePath); //the name of said file
        GUILayout.Space(10);
        if (GUI.Button(new Rect(200, 130, 120, 30), "Create JSON file")) //button that when pressed executes the make menu script
            executeScript();

    }

    void executeScript() //create the menu by first create the object prefabs, then menu prefabs and finally the menu
    {
        submenus = new List<GameObject>();
        menuJson jsonParameters = parseJsonFile();
        basePath = jsonParameters.BaseFolder;
        int i = 0;
        GameObject top = createFinalMenu(jsonParameters.menuName);
        foreach (submenuJson submenu in jsonParameters.Menu)
        {
            foreach (objectJson objectJ in submenu.Values)
            {
                string prefabName = obPrefab(objectJ.objectName, objectJ.objectPath, objectJ.colliderType);
                menuPrefab(objectJ.objectName, objectJ.objectPath, prefabName);
            }
            createSubmenu(submenu, top);
            i++;
        }
        createTopmenu(submenus, jsonParameters, top);
    }




    [MenuItem("Examples/Execute menu items")]
    static void EditorPlaying()
    {
        submenus = new List<GameObject>();
        menuJson jsonParameters = parseJsonFile();
        Debug.Log(jsonParameters.BaseFolder + "  " + jsonParameters.isAllInOneFolder);
        basePath = Application.dataPath + "/Resources/Imported_Models/";
        int i = 0;
        GameObject top = createFinalMenu(jsonParameters.menuName);
        foreach (submenuJson submenu in jsonParameters.Menu)
        {
            foreach(objectJson objectJ in submenu.Values)
            {
                
                string prefabName = obPrefab(objectJ.objectName, objectJ.objectPath, objectJ.colliderType);
                menuPrefab(objectJ.objectName, objectJ.objectPath, prefabName);
            }
            createSubmenu(submenu, top);
            Debug.Log(submenus[i]);
            i++;
        }
        createTopmenu(submenus, jsonParameters, top);
        //createPrefab(top);   //creating new instance of prefab removes script references
    }

    //Parse Json File
    private static menuJson parseJsonFile()
    {
        string jsonString = File.ReadAllText(jsonFilePath + "/" + jsonFileName + ".json");
        menuJson menuObject = JsonUtility.FromJson<menuJson>(jsonString);
        return menuObject;
    }

    private static void menuPrefab(string meshName, string path, string object_prefab)
    {
        //1. navigate to mesh
        GameObject meshToSpawn = Resources.Load("Imported_Models/" + path) as GameObject;
        
        //2. Create object from mesh and add the relevant components
        GameObject clone = Instantiate(meshToSpawn, Vector3.zero, Quaternion.identity) as GameObject;
        clone.gameObject.tag = "menuItem";
        clone.gameObject.layer = 8; //the interactable layer
        clone.AddComponent<Interactable>();
        clone.AddComponent<menuItem>();
        clone.GetComponent<menuItem>().findObject = object_prefab;
        clone.GetComponent<menuItem>().isTopLayer = false;
        BoxCollider box = clone.AddComponent<BoxCollider>();
        box.size = new Vector3(0.075f, 0.075f, 0.2f);
        Renderer childMesh = clone.gameObject.transform.GetChild(0).GetComponent<Renderer>();
        box.center = childMesh.bounds.center;
        box.isTrigger = true;
        clone.name = "Menu_" + meshName;
        //3. save the created object as a menu prefab
        PrefabUtility.SaveAsPrefabAsset(clone, "Assets/Resources/Menu_prefabs/" + clone.name + ".prefab");
        //4. Destroy the instance of the new prefab
        DestroyImmediate(clone);
    }

    private static string obPrefab(string meshName, string path, string colliderType)
    {
        //1. navigate to mesh
        GameObject meshToSpawn = Resources.Load("Imported_Models/" + path) as GameObject;

        //2. Create object from mesh and add the relevant components
        GameObject clone = Instantiate(meshToSpawn, Vector3.zero, Quaternion.identity) as GameObject;
        clone.gameObject.tag = "object";
        clone.gameObject.layer = 8; //the interactable layer
        //clone.AddComponent<Interactable>();
        clone.AddComponent<Moveable>();
        clone.AddComponent<ColorToggle>();
        Rigidbody rb = clone.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 1;
        rb.drag = 10;
        rb.angularDrag = 10;

        //3. Resize the object and the collider
        colliderResizing(clone, colliderType);

        //4. save the created object as a menu prefab
        clone.name = "Ob_" + meshName;
        string temp = clone.name;
        PrefabUtility.SaveAsPrefabAsset(clone, "Assets/Resources/Object_prefabs/" + clone.name + ".prefab");

        //5. Destroy the instance of the new prefab
        DestroyImmediate(clone);
        return temp;
    }

    private static void colliderResizing(GameObject ob, string colliderType)
    {
        if(colliderType.Equals("mesh"))
        {
            Debug.Log("mesh collider selected...");
            //transfer to parent object, disable on child
            MeshCollider coll = ob.gameObject.transform.GetChild(0).GetComponent<MeshCollider>();
            coll.convex = true;
        }
        else
        {
            //get the size of the mesh, and calculate best fit based on it's rough size
            Renderer childMesh = ob.gameObject.transform.GetChild(0).GetComponent<Renderer>();
            //based on the colliderType given, add a best fit collider to the gameobject
            if (colliderType.Equals("capsule"))
            {
                //needs more work!
                CapsuleCollider box = ob.AddComponent<CapsuleCollider>();
                float max = Mathf.Max(childMesh.bounds.extents.x, childMesh.bounds.extents.y, childMesh.bounds.extents.z);
                float min = Mathf.Min(childMesh.bounds.extents.x, childMesh.bounds.extents.y, childMesh.bounds.extents.z);
                box.radius = min*2;
                box.height = max*2;
                box.direction = 1;
                box.center = childMesh.bounds.center;
            }
            else if (colliderType.Equals("sphere"))
            {
                SphereCollider sphere = ob.AddComponent<SphereCollider>();
                sphere.radius = childMesh.bounds.extents.magnitude/2;
                sphere.center = childMesh.bounds.center;
            }
            else
            {
                BoxCollider box = ob.AddComponent<BoxCollider>();
                box.size = childMesh.bounds.extents * 2;
                box.center = childMesh.bounds.center;
            }
        }
        
        
    }
    //creates a new submenu
    private static void createSubmenu(submenuJson submenu, GameObject topMenu)
    {
        //Initialise new prefab instance
        GameObject sb = Resources.Load("submenu_prefab") as GameObject;
        GameObject submenuBase = Instantiate(sb, Vector3.zero, Quaternion.identity) as GameObject;
        //name submenu
        submenuBase.name = "submenu_" + submenu.name;
        submenuBase.transform.SetParent(topMenu.transform);
        //add menu_ prefabs to submenu
        float radius = 0.24f;
        int i = 0;
        foreach (objectJson objectJ in submenu.Values)
        {
            float angle = i * Mathf.PI * 2 / submenu.Values.Count;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            GameObject clone = Instantiate(Resources.Load("Menu_prefabs/Menu_"+objectJ.objectName), pos, Quaternion.identity) as GameObject;
            clone.transform.SetParent(submenuBase.transform);
            i++;
        }
        //add the submenu gameObject to a list of submenus
        submenus.Add(submenuBase);
        submenuBase.SetActive(false);
    }

    private static void createTopmenu(List<GameObject> submenus, menuJson menu, GameObject topMenu)
    {
        GameObject sb = Resources.Load("submenu_prefab") as GameObject;
        GameObject submenuBase = Instantiate(sb, Vector3.zero, Quaternion.identity) as GameObject;
        submenuBase.name = "topLevelMenu";
        submenuBase.transform.SetParent(topMenu.transform);
        int i = 0;
        int j = menu.Menu.Count;
        float radius = 0.24f;
        foreach (submenuJson submenu in menu.Menu)
        {
            float angle = i * Mathf.PI * 2 / j;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            GameObject submenuIdentifier = Resources.Load("Menu_prefabs/Menu_" + submenu.topLevelMenuRep) as GameObject;
            GameObject si = Instantiate(submenuIdentifier, pos, Quaternion.identity) as GameObject;
            si.transform.SetParent(submenuBase.transform);
            si.GetComponent<menuItem>().objectToSpawn = submenus[i];
            si.GetComponent<menuItem>().isTopLayer = true;
            i++;
        }
        submenuBase.SetActive(false);
    }

    private static GameObject createFinalMenu(string name)
    {
        GameObject menu = Resources.Load("Menu_Final") as GameObject;
        GameObject menuContainer = Instantiate(menu, Vector3.zero, Quaternion.identity) as GameObject;
        menuContainer.name = "Menu_" + name;
        Menu script = menuContainer.GetComponent<Menu>();
        Debug.Log(GameObject.Find("Controller (right)").GetComponent<Hand2>());
        script.right = GameObject.Find("Controller (right)").GetComponent<Hand2>();
        script.left = GameObject.Find("Controller (left)").GetComponent<Hand2>();
        return menuContainer;
    }

    private static void createPrefab(GameObject prefabToBe)
    {
        PrefabUtility.SaveAsPrefabAsset(prefabToBe, "Assets/Resources/" + prefabToBe.name + ".prefab");
    }
}
