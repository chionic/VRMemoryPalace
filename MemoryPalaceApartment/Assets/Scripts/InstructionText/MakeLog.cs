using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

//Instantiates instances of the various log classes and sends data to the last log class in the list
public class MakeLog : MonoBehaviour
{
    private List<logBase> logs; //a list of basic logs
    private List<logMov> logsMov; //a list (currently of length 1) of movement logs
    private Transform player; //used to get the position of the player in the Memory Palace

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;
        logMaker();
        makeLogEntry("start", gameObject);
    }

    private void logMaker() //instantiate the various log types
    {
        logs = new List<logBase>();
        logsMov = new List<logMov>();

        //log classes - most used type of logs near the end of the list (it is called up in reverse)
        //ie the last created log class is called first in the list
        logBase log1 = new logStart(null); //logs the start/end of the application being run, no next log
        logs.Add(log1); //adds the newly made log class instance to the list of logs
        logBase logColour = new logBase("colourChange", "I_U_Obj", log1, "\"colour\" : \"");
        logs.Add(logColour);
        logBase logSize = new logResize("resizeObject", "I_U_Obj", logColour); //resize is a child class of logBase with added functionality
        logs.Add(logSize);
        logBase logText = new logText("changeText", "I_Menu2D", logSize); //so are logText, logMenu2 and logMenu
        logs.Add(logText);
        logBase logCreate = new logBase("createObject", "I_P_Obj", logText);
        logs.Add(logCreate);
        logBase logDelete = new logBase("delete", "I_U_Obj", logCreate);
        logs.Add(logDelete);
        logBase logOpenMenu = new logMenu2("toggleMenu", "I_Menu2D", logDelete);
        logs.Add(logOpenMenu);
        logBase logAttach = new logBase("attachObject", "I_P_Obj", logOpenMenu, "\"hand\" : \"");
        logs.Add(logAttach);
        logBase logDetach = new logBase("releaseObject", "I_P_Obj", logAttach, "\"hand\" : \"");
        logs.Add(logDetach);
        logBase logVisible0 = new logBase("isVisible", "D_Obj", logDetach);
        logs.Add(logVisible0);
        logBase logVisible1 = new logBase("isInvisible", "D_Obj", logVisible0);
        logs.Add(logVisible1);
        logBase logVisibleMenu = new logMenu("isVisible", "D_Menu2D", logVisible1);
        logs.Add(logVisibleMenu);
        logBase logVisibleMenu1 = new logMenu("isInvisible", "D_Menu2D", logVisibleMenu);
        logs.Add(logVisibleMenu1);
        //add new logging sub classes here

        logMov logTeleport = new logMov("teleport", "M_Tele", null); //used specifically for teleport logs
        logsMov.Add(logTeleport);
    }

    //Called when various events happen - adds player position data to the event and sends it to the last log class in the list
    //(in this case logVisibleMenu1 which checks if a menu is invisible)
    public void makeLogEntry(string scriptType, GameObject ob)
    {

            logs.Last().check(scriptType, ob, player.position);
    }

    //Called when the player teleports, feeds into logMov instance
    public void makeLogEntry(string scriptType, TeleportMarkerBase x)
    {
        //gets the type of object the player used to teleport (area/point) and removes the rest of the info
        string y = x.ToString().Substring(0, x.ToString().IndexOf("(")); 
        logsMov.Last().check(scriptType, y, player.position);
    }

    //Called when an event witrh extra data happens (such as a colour change or a pickup event)
    public void makeLogEntry(string scriptType, GameObject ob, string hand)
    {
        logs.Last().check(scriptType, ob, hand, player.position);
    }
    //When the unity application is quit out of/stops running, sends the event that will close the text writer.
    void OnApplicationQuit()
    {
        makeLogEntry("stop", gameObject);
    }
}
