using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

//Switch out which script 
[CustomEditor(typeof(ChangeTabletScript))]
[CanEditMultipleObjects]
public class ChangeTabletScript : EditorWindow
{

    private static string fileName = "";
    private static string pathToFile = "";
    private filePathText script;

    [MenuItem("Window/TabletScript")]
    public static void ShowWindow() //create a unity editor window that can be accessed from the windows tab
    {
        EditorWindow.GetWindow(typeof(ChangeTabletScript));
    }

    void OnGUI() //how the editor window looks
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel); //defines the settings for running the script
        GUILayout.Space(10);
        fileName = EditorGUILayout.TextField("Name of JSON input file:", fileName); //the name of the json file in question
        if (GUI.Button(new Rect(200, 90, 150, 30), "Select JSON input file")) //lets the user manually input where the json file is in the folder structure
            pathToFile = EditorUtility.OpenFolderPanel("Select json file folder", UnityEngine.Application.dataPath, UnityEngine.Application.dataPath);
        pathToFile = EditorGUILayout.TextField("Path to json file:", pathToFile); //the name of said file
        GUILayout.Space(10);
        if (GUI.Button(new Rect(200, 130, 120, 30), "Change File")) //button that when pressed executes the make menu script
        {
            executeScript();
        }
    }

    void executeScript() //create the menu by first creating the object prefabs, then menu prefabs and finally the menu
    {
        filePathText fp = new filePathText(); //creates a new filePathText script
        fp.fileName = fileName;
        fp.pathToFile = pathToFile;
        string path = Application.dataPath + "/Data/tabletText.json";
        string jsonString = JsonUtility.ToJson(fp);
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(jsonString);
        writer.Close();
        AssetDatabase.ImportAsset(path);
        Debug.Log(jsonString);
    }

}
