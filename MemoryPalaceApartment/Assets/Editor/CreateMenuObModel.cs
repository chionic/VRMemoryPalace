using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

//1. Navigate to folder with all the assets
//2. For each asset in the folder, create a menu prefab object and save it to the menu folder
//3. For each asset in the folder, create an object prefab and save it to the menu folder

//Later: allow this to be done through a json file with the ability to specify certain features

[CustomEditor(typeof(CreateMenuObModel))]
[CanEditMultipleObjects]
public class CreateMenuObModel : Editor
{

    [MenuItem("Examples/Execute menu items")]
    static void EditorPlaying()
    {
        Debug.Log("CreateMenuObModel script ran!!!");
        menuJson jsonParameters = parseJsonFile();
        //EditorApplication.ExecuteMenuItem("GameObject/3D Object/Cube");
        //EditorApplication.ExecuteMenuItem("GameObject/Create Empty");
        Debug.Log(jsonParameters.BaseFolder + "  " + jsonParameters.isAllInOneFolder);
        /*foreach(submenuJson submenu in jsonParameters.Menu)
        {
            foreach(objectJson objectJ in submenu.Values)
            {
                Debug.Log(objectJ.objectName);
            }
        }*/

    }

    //Parse Json File
    private static menuJson parseJsonFile()
    {
        string jsonString = File.ReadAllText(Application.dataPath + "/JSON_Files/sampleMenu");
        menuJson menuObject = JsonUtility.FromJson<menuJson>(jsonString);
        return menuObject;
    }

    //Import assets
    private static void importAsset(string filePath, string folderName)
    {

    }

    //
}
