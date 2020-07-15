using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeLog : MonoBehaviour
{
    private List<logBase> logs;
    private List<logBase> logsPlace;

    private void Start()
    {
        logMaker();
        makeLogEntry("start", null);
        foreach (logBase logger in logs)
        {
            Debug.Log(logger);
        }
        Debug.Log(logs.Last());
    }

    private void logMaker()
    {
        logs = new List<logBase>();
        logsPlace = new List<logBase>();

        //logs (no place)
        logBase logText = new logText("changeText", "I_Text", null);
        logs.Add(logText);
        logPlace logOpenMenu = new logPlace("toggleMenu", "I_Menu2D", logText);
        logs.Add(logOpenMenu);
        logBase logVisible0 = new logBase("isVisible", "D_Obj", logOpenMenu);
        logs.Add(logVisible0);
        logBase logVisible1 = new logBase("isInvisible", "D_Obj", logVisible0);
        logs.Add(logVisible1);
        logBase logVisibleMenu = new logMenu("isVisible", "D_Menu2D", logVisible1);
        logs.Add(logVisibleMenu);
        logBase logVisibleMenu1 = new logMenu("isInvisible", "D_Menu2D", logVisibleMenu);
        logs.Add(logVisibleMenu1);
        logBase log1 = new logStart(logVisibleMenu1);
        logs.Add(log1);
        logBase logAttach = new logPlace("AttachObject", "I_P_Obj", log1);
        logs.Add(logAttach);
        logBase logDetach = new logPlace("DetachObject", "I_P_Obj", logAttach);
        logs.Add(logDetach);
        logBase logDelete = new logBase("Delete", "I_U_Obj", logDetach);
        logs.Add(logDelete);
        //add new logging sub classes here
    }

    public void makeLogEntry(string scriptType, GameObject ob)
    {

            logs.Last().check(scriptType, ob);
    }

    void OnApplicationQuit()
    {
        makeLogEntry("stop", null);
    }
}
