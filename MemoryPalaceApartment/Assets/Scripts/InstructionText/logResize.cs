using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logResize : logBase
{
    private string DIMLabel;
    private logBase nextLog;
    private string scriptType;
    private string log1;

    //constructor
    public logResize(string script, string label, logBase next) : base(script, label, next)
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
            log1 = getTimestamp() + "; " + DIMLabel + "; " + scriptType + "; " + ob.name + "; place " + ob.transform.position + "; Local size " + ob.transform.localScale;
            writeLog.WriteString(log1);
    }
}
