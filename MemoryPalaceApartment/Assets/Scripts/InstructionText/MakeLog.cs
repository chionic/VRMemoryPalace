using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MakeLog : MonoBehaviour
{
    private List<logBase> logs;
    private List<logMov> logsMov;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;
        logMaker();
        makeLogEntry("start", gameObject);

    }

    private void logMaker()
    {
        logs = new List<logBase>();
        logsMov = new List<logMov>();

        //logs (no place)
        logBase log1 = new logStart(null);
        logs.Add(log1);
        logBase logColour = new logBase("colourChange", "I_U_Obj", log1, "\"colour\" : \"");
        logs.Add(logColour);
        logBase logSize = new logResize("resizeObject", "I_U_Obj", logColour);
        logs.Add(logSize);
        logBase logText = new logText("changeText", "I_Menu2D", logSize);
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

        logMov logTeleport = new logMov("teleport", "M_Tele", null);
        logsMov.Add(logTeleport);
    }

    public void makeLogEntry(string scriptType, GameObject ob)
    {

            logs.Last().check(scriptType, ob, player.position);
    }

    public void makeLogEntry(string scriptType, TeleportMarkerBase x)
    {
        string y = x.ToString().Substring(0, x.ToString().IndexOf("("));
       logsMov.Last().check(scriptType, y, player.position);
    }

    public void makeLogEntry(string scriptType, GameObject ob, string hand)
    {
        logs.Last().check(scriptType, ob, hand, player.position);
    }
    void OnApplicationQuit()
    {
        makeLogEntry("stop", gameObject);
    }
}
