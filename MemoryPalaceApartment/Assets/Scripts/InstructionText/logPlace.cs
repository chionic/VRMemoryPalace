using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logPlace : logBase
{
    private string DIMLabel;
    private logBase nextLog;
    private string scriptType;
    private string log1;

    //constructor
    public logPlace(string script, string label, logBase next) : base(script, label, next)
    {
        scriptType = script;
        DIMLabel = label;
        nextLog = next;
    }

    public logPlace(logBase next) : base(next)
    {
        scriptType = "";
        DIMLabel = "";
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
        if(ob != null)
        {
            log1 = getTimestamp() + "; " + DIMLabel + "; " + scriptType + "; " + ob.name + "; place " + ob.transform.position;
        }
        else
        {
            log1 = getTimestamp() + "; " + DIMLabel + "; " + scriptType + "; closed";
        }
        writeLog.WriteString(log1);
    }
}
