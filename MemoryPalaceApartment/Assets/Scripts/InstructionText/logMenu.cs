using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logMenu : logBase
{
    private string DIMLabel;
    private logBase nextLog;
    private string scriptType;

//constructor
    public logMenu (string script, string label, logBase next) : base(script, label, next)
    {
        scriptType = script;
        DIMLabel = label;
        nextLog = next;
    }

    public logMenu(logBase next) : base(next)
    {
        scriptType = "";
        DIMLabel = "";
        nextLog = next;
    }

    //Check if it should go to next method
    public override void check(string script, GameObject ob)
    {
        if (string.Equals(script, scriptType) && ob.CompareTag("submenu")) log(ob);
        else if (nextLog != null) nextLog.check(script, ob);
        else Debug.Log("logBase No correct log type found " + script + " " + ob);
    }
}
