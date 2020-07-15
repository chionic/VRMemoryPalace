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

    //Check if it should go to next method
    public override void check(string script, GameObject ob)
    {
        if (string.Equals(script, scriptType)) log(ob);
        else if (nextLog != null) nextLog.check(script, ob);
        else Debug.Log("logBase No correct log type found " + script + " " + ob);
    }

    protected override void log(GameObject ob)
    {
        if (ob.GetComponent<Text>() != null)
        {
            log1 = getTimestamp() + "; " + DIMLabel + "; " + scriptType + "; " + ob.name + "; " + ob.GetComponent<Text>().text;
            writeLog.WriteString(log1);
        }
        else
        {
            log1 = getTimestamp() + "; No text object assigned, but text called";
            writeLog.WriteString(log1);
        }

    }
}
