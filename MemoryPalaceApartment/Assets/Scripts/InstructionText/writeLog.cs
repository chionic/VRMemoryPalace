using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//singletong class used to write to the log file
public static class writeLog
{
    static string path = Application.dataPath + "/JSON_Files/log.txt";
    static StreamWriter writer = new StreamWriter(path, true);
    

    public static void WriteString(string logMessage)
    {        
        writer.WriteLine(logMessage);
    }

    public static void closeLog(string logMessage)
    {
        writer.WriteLine(logMessage);
        writer.Close();
    }

}
