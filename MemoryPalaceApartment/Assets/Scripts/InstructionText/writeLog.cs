using UnityEngine;
using System.IO;

//Writes the string fed into the class to a text file
public static class writeLog
{
    static string path = Application.dataPath + "/Data/"; //The path to the logFile
    static StreamWriter writer; //A new streamwriter (allows the code to write to the file)

    public static void makeLogFile()
    {
        int  randomInt = Random.Range(1, 99999999);
        //File.CreateText("log_" + randomInt);
        writer = new StreamWriter(path + "log_" + randomInt, true);
        Debug.Log(path + "log_" + randomInt);
    }

    //writes a new log message to the log
    public static void WriteString(string logMessage) 
    {        
        writer.WriteLine(logMessage);
    }

    //writes the final log message to the log and then closes the streamWriter so that the file can't be edited anymore
    public static void closeLog(string logMessage) 
    {
        //Debug.Log(Application.persistentDataPath);
        writer.WriteLine(logMessage);
        writer.Close();
    }

}
