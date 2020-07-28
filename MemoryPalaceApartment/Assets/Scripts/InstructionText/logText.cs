using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class logText : logBase
{
    private string DIMLabel;
    private logBase nextLog;
    private string scriptType;
    private string log1;

    //constructor
    public logText(string script, string label, logBase next) : base(script, label, next)
    {
        scriptType = script;
        DIMLabel = label;
        nextLog = next;
    }

    protected override void log(GameObject ob, Vector3 playerPos)
    {
        if (ob.GetComponent<Text>() != null)
        {
            log1 = getId() + "; " + getTimestamp() + "; " + DIMLabel + "; " + playerPos.x + "; " + playerPos.y + "; " + playerPos.z + "; " + ob.name + "; "  + ob.transform.position.x + "; " + ob.transform.position.y + "; " + ob.transform.position.z + "; { \"action\": \"" + scriptType + "\", \"text\": \"" + ob.GetComponent<Text>().text + "\"}";
            writeLog.WriteString(log1);
        }
        else
        {
            log1 = getId() + "; " + getTimestamp() + ";;;;;;;;;;{ \"error\": \"No text object assigned, but text called\"}";
            writeLog.WriteString(log1);
        }

    }
}
