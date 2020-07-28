using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using UnityEditor.VersionControl;
using System.Text.RegularExpressions;

//creates a JSON file based on the folder structure given
[CustomEditor(typeof(CreateMenuObModel))]
[CanEditMultipleObjects]
public class CreateJSON : EditorWindow
{
    static string basePath;
    static menuJson menuJ;
    static int countID;

    string outputFileName = "Menu_1";
    string bp = "";
    static colliderTypes ct;
    

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
        bp = EditorGUILayout.TextField("Path to Imported Models: ", bp);
        GUILayout.Space(40);
        GUILayout.Space(10);
        ct = (colliderTypes)EditorGUILayout.EnumPopup("Default collider type: ", ct);
        GUILayout.Space(10);
        if (GUI.Button(new Rect(200, 300, 120, 30), "Create JSON file"))
            executeScript();
    }
    void executeScript()
    {
        countID = 0;
        menuJ = new menuJson(); //creates a new menuJson script
        Debug.Log(bp);
        basePath = bp + "/";
        String assetPath = "";
        if(bp.Contains("Assets/")) { assetPath = bp.Substring(bp.IndexOf("Assets/")); }
        else { Debug.Log("Error, invalid filepath, path is not inside unity project folder"); return; }
        menuJ.assetPath = assetPath;
        string[] files = Directory.GetFiles(basePath); //gets the files in the folder where the menu system is
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
        submenu.topLevelMenuRep = Path.GetFileName(removeFileEnding(assets[0]));
        foreach (String asset in assets) //creates a new objectJson object for every item in the folder that is not a meta file, material or png file
        {
            if (isMtl(Path.GetFileName(asset)))
            {
                //Debug.Log("Error, the file " + Path.GetFileName(asset) + " in " + folderName + " is not a model object file");
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
        obJ.objectPath = folderName + "/" + ob;
        ob = removeFileEnding(ob);
        obJ.objectName = ob;
        obJ.defaultSize = 1;
        obJ.colliderType = ct.ToString(); //default collider type box
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
        if (Regex.IsMatch(fileName, @"\.*\.mat")) { return true; }
        if (Regex.IsMatch(fileName, @"\.*\.jpg")) { return true; }
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

    //writes the data structure to a json file to be used later
    static void WriteString(menuJson menu, string fileName)
    {
        string path = Application.dataPath + "/Data/" + fileName + ".json";
        string jsonMenu = JsonUtility.ToJson(menuJ);
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(jsonMenu);
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(Application.dataPath + "/Data/" + fileName + ".json");
        Debug.Log(jsonMenu);
    }

}

public enum colliderTypes{ box, mesh, sphere, capsule};