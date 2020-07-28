using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEditorInternal;
using Valve.VR.InteractionSystem;

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
    static bool createObjects = true; //sets whether to create/update the menu and objects based on the imported objects
    static private float rescale = 1;


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
        jsonFilePath= EditorGUILayout.TextField("Path to json file:", jsonFilePath); //the name of said file
        GUILayout.Space(10);
        createObjects = EditorGUILayout.Toggle("Create imported objects: ", createObjects);
        if (GUI.Button(new Rect(200, 130, 120, 30), "Create JSON file")) //button that when pressed executes the make menu script
        {
            executeScript();
        }
    }

    void executeScript() //create the menu by first creating the object prefabs, then menu prefabs and finally the menu
    {
        submenus = new List<GameObject>();
        menuJson jsonParameters = parseJsonFile();
        if (jsonParameters == null) return;
        if (!checkSize(jsonParameters)) return;
        basePath = jsonParameters.BaseFolder;
        GameObject top = createFinalMenu(jsonParameters.menuName); //creates the top level menu game object wthat the other submenus are children of
        foreach (submenuJson submenu in jsonParameters.Menu) //for every submenu mentionec in the json file
        {
            if (createObjects) //if game objects need to be generated
            {
                foreach (objectJson objectJ in submenu.Values) //for each object mentioned in the menu json file
                {
                    string prefabName = obPrefab(objectJ.objectName, objectJ.objectPath, jsonParameters.assetPath, objectJ.colliderType); //create an object game object
                    menuPrefab(objectJ.objectName, objectJ.objectPath, jsonParameters.assetPath, prefabName); //create a menu game object
                }
            }
            createSubmenu(submenu, top); //create a submenu game object under the top level menu
        }
        createTopmenu(submenus, jsonParameters, top); //create the first menu layer that the submenus can be accessed from
    }

    private static menuJson parseJsonFile() //Parse Json File
    {
        menuJson menuObject;
        if(File.Exists(jsonFilePath + "/" + jsonFileName + ".json"))
        {
            string jsonString = File.ReadAllText(jsonFilePath + "/" + jsonFileName + ".json");
            menuObject = JsonUtility.FromJson<menuJson>(jsonString);
        }
        else
        {
            Debug.LogError("The specified json file " + jsonFileName + " was not found at the file path " + jsonFilePath);
            return null;
        }
        return menuObject;
    }

    //creates a menu object based on an imported mesh object mentioned in the json
    private static void menuPrefab(string meshName, string path, string assetPath, string object_prefab)
    {
        //1. navigate to mesh
        GameObject meshToSpawn = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath + "/" + path, typeof(GameObject));
        //2. Create object from mesh and add the relevant components
        GameObject clone = Instantiate(meshToSpawn, Vector3.zero, Quaternion.identity) as GameObject;
        clone.gameObject.tag = "menuItem";
        clone.gameObject.layer = 8; //the interactable layer
        clone.AddComponent<Interactable>();
        clone.AddComponent<menuItem>();
        clone.GetComponent<menuItem>().findObject = object_prefab;
        clone.GetComponent<menuItem>().isTopLayer = false;
        BoxCollider box = clone.AddComponent<BoxCollider>();
        if(clone.transform.localScale.x >= 100)
        {
            box.size = new Vector3(0.00075f, 0.00075f, 0.002f);
        }
        else box.size = new Vector3(0.075f, 0.075f, 0.2f);
        if (clone.gameObject.transform.childCount > 0)
        {
            Renderer childMesh = clone.gameObject.transform.GetChild(0).GetComponent<Renderer>();
            if (childMesh != null)
            {
                box.center = childMesh.bounds.center;
                
            }
            else { Debug.LogWarning("The menu object " + clone.name + " has no mesh renderer, manually center the collider for the object."); }
            box.isTrigger = true;
        }
        clone.name = "Menu_" + meshName;

        //3. save the created object as a menu prefab
        PrefabUtility.SaveAsPrefabAsset(clone, "Assets/Resources/Menu_prefabs/" + clone.name + ".prefab");

        //4. Destroy the instance of the new prefab
        DestroyImmediate(clone);
    }

    private static string obPrefab(string meshName, string path, string assetPath, string colliderType)
    {
        //1. navigate to mesh
        GameObject meshToSpawn = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath + "/" + path, typeof(GameObject));

        //2. Create object from mesh and add the relevant components
        GameObject clone = Instantiate(meshToSpawn, Vector3.zero, Quaternion.identity) as GameObject;

        //2.5Check that the imported object mesh is of an acceptable scale (ie not bigger than a room in the house)
        if(clone.gameObject.transform.childCount > 0)
        {
            clone.gameObject.transform.GetChild(0).gameObject.AddComponent<isVisible>();
            Renderer childMesh = clone.gameObject.transform.GetChild(0).GetComponent<Renderer>();
            if (childMesh != null)
            {
                if (childMesh.bounds.extents.x > 0.1 || childMesh.bounds.extents.y > 0.1 || childMesh.bounds.extents.z > 0.1)
                {
                    Debug.LogWarning("Warning: object bounds are too large to fit inside the menu for the mesh " + path + ". Rescale model to fit properly into menu.");
                }
                //3. Resize the object and the collider
                colliderResizing(clone, colliderType, childMesh);
            }
            else {
                clone.AddComponent<isVisible>();
                Debug.LogWarning("The game object " + clone.name + " does not have a renderer attached to it, manually add a collider to the object.");
            }
        }
        else
        {
            Renderer meshy = clone.gameObject.GetComponent<Renderer>();
            if (meshy != null)
            {
                if (meshy.bounds.extents.x > 0.1 || meshy.bounds.extents.y > 0.1 || meshy.bounds.extents.z > 0.1)
                {
                    Debug.LogWarning("Warning: object bounds are too large to fit inside the menu for the mesh " + path + ". Rescale model to fit properly into menu.");
                }
                //3. Resize the object and the collider
                colliderResizing(clone, colliderType, meshy);
            }
            else
            {
                clone.AddComponent<isVisible>();
                Debug.LogWarning("The game object " + clone.name + " does not have a renderer attached to it, manually add a collider to the object.");
            }
        }
        
        //2.
        clone.gameObject.tag = "object";
        clone.gameObject.layer = 8; //the interactable layer
        clone.AddComponent<Moveable>();
        clone.AddComponent<ColorToggle>();
        clone.AddComponent<NoTeleportWhileObjectHeld>();
        Rigidbody rb = clone.AddComponent<Rigidbody>();
        clone.AddComponent<resizeObject>();
        rb.useGravity = false;
        rb.mass = 1;
        rb.drag = 10;
        rb.angularDrag = 10;

        //4. save the created object as a menu prefab
        clone.name = "Ob_" + meshName;
        string temp = clone.name;
        PrefabUtility.SaveAsPrefabAsset(clone, "Assets/Resources/Object_prefabs/" + clone.name + ".prefab");

        //5. Destroy the instance of the new prefab
        DestroyImmediate(clone);
        return temp;
    }

    //creates/changes the collider around an object for best fit possible while still being autogenerated
    private static void colliderResizing(GameObject ob, string colliderType, Renderer childMesh)
    {
        if(colliderType.Equals("mesh"))
        {
            //make the mesh collider convex so it can bump into other objects in unity
            MeshCollider coll = ob.gameObject.transform.GetChild(0).GetComponent<MeshCollider>();
            coll.convex = true;
        }
        else
        {
            Debug.Log(ob.transform.localScale.x + " " + ob.name);
            if(ob.transform.localScale.x >= 100)
            {
                rescale = 0.01f;
                Debug.Log(rescale);
            } else { rescale = 1; }
            //get the size of the mesh, and calculate best fit based on it's rough size
            //based on the colliderType given, add a best fit collider to the gameobject
            if (colliderType.Equals("capsule"))
            {
                //needs more work!
                CapsuleCollider box = ob.AddComponent<CapsuleCollider>();
                float max = Mathf.Max(childMesh.bounds.extents.x, childMesh.bounds.extents.y, childMesh.bounds.extents.z);
                float min = Mathf.Min(childMesh.bounds.extents.x, childMesh.bounds.extents.y, childMesh.bounds.extents.z);
                box.radius = min*2*rescale;
                box.height = max*2*rescale;
                box.direction = 1;
                box.center = childMesh.bounds.center;
            }
            else if (colliderType.Equals("sphere"))
            {
                SphereCollider sphere = ob.AddComponent<SphereCollider>();
                sphere.radius = (childMesh.bounds.extents.magnitude/2 - (childMesh.bounds.extents.magnitude / 20))*rescale;
                sphere.center = childMesh.bounds.center * rescale;
            }
            else
            {
                
                BoxCollider box = ob.AddComponent<BoxCollider>();
                box.size = childMesh.bounds.extents * rescale;
                box.center = childMesh.bounds.center * rescale;
                Debug.Log(ob.name + " " + rescale +" "+ box.size.x.ToString("f5"));
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
        //add menu_ prefabs to submenu - in a circle around the submenu disk
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

    //create a new first level menu to access the submenus
    private static void createTopmenu(List<GameObject> submenus, menuJson menu, GameObject topMenu)
    {
        //load the submenu prefab and give it a name
        GameObject sb = Resources.Load("submenu_prefab") as GameObject;
        GameObject submenuBase = Instantiate(sb, Vector3.zero, Quaternion.identity) as GameObject;
        submenuBase.name = "topLevelMenu";
        submenuBase.transform.SetParent(topMenu.transform);
        //add menu objects to the first level menu in a circle
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
            //tie the submenu identifier to spawn the relevant submenu
            si.GetComponent<menuItem>().objectToSpawn = submenus[i];
            si.GetComponent<menuItem>().isTopLayer = true;
            i++;
        }
        submenuBase.SetActive(false);
    }

    //creates the top level menu game object which the submenus are children of
    private static GameObject createFinalMenu(string name)
    {
        GameObject menu = Resources.Load("Menu_Final") as GameObject;
        GameObject menuContainer = Instantiate(menu, Vector3.zero, Quaternion.identity) as GameObject;
        menuContainer.name = "Menu_" + name;
        Menu script = menuContainer.GetComponent<Menu>();
        Hand2[] hands = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Hand2>();
        script.right = hands[1];
        script.left = hands[0];
        return menuContainer;
    }

    //create a prefab based on the specified gameobject in the resources folder
    private static void createPrefab(GameObject prefabToBe)
    {
        PrefabUtility.SaveAsPrefabAsset(prefabToBe, "Assets/Resources/" + prefabToBe.name + ".prefab");
    }

    //check there are less than 10 objects in any single menu layer
    private static bool checkSize(menuJson jsonParameters)
    {
        if (jsonParameters.Menu.Count > 10)
        {
            Debug.LogError("The first level menu has more than ten objects in it. Remove excess submenu folders and regenerate the json file.");
            return false;
        }
        foreach(submenuJson submenu in jsonParameters.Menu)
        {
            if (submenu.Values.Count > 10)
            {
                Debug.LogError("The submenu " + submenu.name + " has more than ten objects in it. Remove excess object model in the folder and regenerate the json file.");
                return false;
            }
        }
        return true;
    }
}
