using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//singletong class used to write to the log file
public static class writeLog
{
    static string path = Application.dataPath + "/Data/logFile.txt";
    static StreamWriter writer = new StreamWriter(path, true);
    

    public static void WriteString(string logMessage)
    {        
        writer.WriteLine(logMessage);
    }

    public static void closeLog(string logMessage)
    {
        Debug.Log(Application.persistentDataPath);
        Debug.Log("close writer called");
        writer.WriteLine(logMessage);
        writer.Close();
    }

}
