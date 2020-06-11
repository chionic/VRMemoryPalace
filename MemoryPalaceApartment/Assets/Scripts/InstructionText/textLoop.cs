using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class textLoop
{
    public int id;
    public int runOrder;
    public bool loop = false;
    public string[] Values;

    public static textLoop CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<textLoop>(jsonString);
    }

}
