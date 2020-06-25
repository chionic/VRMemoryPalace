using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using UnityEditor.VersionControl;
using System.Text.RegularExpressions;

//creates a JSON file based on the folder structure given

    //to do tomorrow: separate out into two scripts, get a button that runs the second script with the specified parameters
[CustomEditor(typeof(CreateMenuObModel))]
[CanEditMultipleObjects]
public class CreateJSON : EditorWindow
{
    static string basePath;
    static menuJson menuJ;
    static int countID;

    string outputFileName = "Menu_1";
    string bp = "";
    bool groupEnabled;
    bool myBool = true;
    float myInt = 2;

    [MenuItem("Window/CreateJSON")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateJSON));
    }

    void OnGUI()
    {
        // The actual window code goes here
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        GUILayout.Space(10);
        outputFileName = EditorGUILayout.TextField("Name of JSON file:", outputFileName);
        GUILayout.Space(10);
        if (GUI.Button(new Rect(200, 90, 120, 30), "Select Path"))
            bp = EditorUtility.OpenFolderPanel("Select base menu folder", UnityEngine.Application.dataPath, UnityEngine.Application.dataPath + "/Resources/Imported_Models/");
        bp = EditorGUILayout.TextField("Data path:", bp);
        GUILayout.Space(40);
        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        GUILayout.Space(10);
        myBool = EditorGUILayout.Toggle("Is all in one folder", myBool);
        GUILayout.Space(10);
        myInt = EditorGUILayout.Slider("Number of menu layers", myInt, 0, 5);
        EditorGUILayout.EndToggleGroup();
        GUILayout.Space(10);
        if (GUI.Button(new Rect(200, 300, 120, 30), "Create JSON file"))
            executeScript();
        

    }
    void executeScript()
    {
        countID = 0;
        menuJ = new menuJson(); //creates a new menuJson script
        //basePath = UnityEngine.Application.dataPath + "/Resources/Imported_Models/";
        Debug.Log(UnityEngine.Application.dataPath + "/Resources/Imported_Models/");
        Debug.Log(bp);
        basePath = bp + "/";
        string[] files = Directory.GetFiles(basePath); //gets the files in the folder where the menu system is
        menuJ.isAllInOneFolder = true; //sets default parameters for the menuJson
        menuJ.BaseFolder = basePath;
        menuJ.menuName = outputFileName;
        menuJ.Menu = new List<submenuJson>();
        foreach (string file in files)
        {
            String fileName = Path.GetFileName(file);
            if (isMeta(fileName)) fileName = fileName.Replace(".meta", ""); ;
            Debug.Log(fileName);
            submenuJson submenu = makeSubmenu(fileName);
            menuJ.Menu.Add(submenu);
        }
        WriteString(menuJ, menuJ.menuName); //turns the menuJson to a string and writes it to a file
    }
    [MenuItem("Examples/Create JSON")] //Runs the script to create the json file
    static void EditorPlaying()
    {
        countID = 0;
        menuJ = new menuJson(); //creates a new menuJson script
        basePath = UnityEngine.Application.dataPath + "/Resources/Imported_Models/";
        string[] files = Directory.GetFiles(basePath); //gets the files in the folder where the menu system is
        menuJ.isAllInOneFolder = true; //sets default parameters for the menuJson
        menuJ.BaseFolder = basePath;
        menuJ.menuName = "menu1";
        menuJ.Menu = new List<submenuJson>();
        foreach (string file in files)
        {
            String fileName = Path.GetFileName(file);
            if (isMeta(fileName)) fileName = fileName.Replace(".meta", ""); ;
            Debug.Log(fileName);
            submenuJson submenu = makeSubmenu(fileName);
            menuJ.Menu.Add(submenu);
        }
        WriteString(menuJ, menuJ.menuName); //turns the menuJson to a string and writes it to a file
    }

    //creates a submenuJson object based on the objects in the folder given
    static submenuJson makeSubmenu(String folderName)
    {
        submenuJson submenu = new submenuJson(); //sets default submenuJson elements
        submenu.name = folderName;
        submenu.id = countID;
        countID++;
        submenu.isSubmenu = true;
        submenu.Values = new List<objectJson>();
        string[] assets = Directory.GetFiles(basePath + folderName);
        submenu.topLevelMenuRep = Path.GetFileName(assets[0]);
        foreach (String asset in assets) //creates a new objectJson object for every item in the folder that is not a meta file, material or png file
        {
            if (isMtl(Path.GetFileName(asset)))
            {
                Debug.Log("Error, the file " + Path.GetFileName(asset) + " in " + folderName + " is not a model object file");
            }
            else if (isMeta(Path.GetFileName(asset)))
            {
                //Nothing (for now)
            }
            else
            {
                objectJson ob = makeObject(Path.GetFileName(asset), folderName);
                Debug.Log(ob.objectName + " " + ob.objectPath + " " + ob.colliderType);
                submenu.Values.Add(ob);
            }
        }
        return submenu;
    }

    //creates a new objectJson for the object file name given to it
    static objectJson makeObject(String ob, String folderName)
    {
        objectJson obJ = new objectJson();
        ob = removeFileEnding(ob);
        obJ.objectName = ob;
        obJ.objectPath = folderName + "/" + ob;
        obJ.defaultSize = 1;
        obJ.colliderType = "box"; //default collider type box
        return obJ;
    }

    //checks if a file is a .meta file
    private static bool isMeta(string fileName)
    {
     
        if(Regex.IsMatch(fileName, @"\.*\.meta")) { return true;  }
        return false;

    }

    //checks if a file is a .mtl or .png file
    private static bool isMtl(string fileName)
    {

        if (Regex.IsMatch(fileName, @"\.*\.mtl")) { return true; }
        if (Regex.IsMatch(fileName, @"\.*\.png")) { return true; }
        return false;

    }

    //removes the ending of a file name (.ie .obj, .fbx etc) (needed for paths)
    private static string removeFileEnding(string s)
    {
        int index = s.IndexOf(".");
        if (index > 0)
            s = s.Substring(0, index);
        return s;
    }

    static void WriteString(menuJson menu, string fileName)
    {
        string path = "Assets/JSON_Files/" + fileName + ".json";
        string jsonMenu = JsonUtility.ToJson(menuJ);
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(jsonMenu);
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        Debug.Log(jsonMenu);
    }

}
