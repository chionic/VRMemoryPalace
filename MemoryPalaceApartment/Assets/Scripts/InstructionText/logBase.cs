using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logBase
{
    private string DIMLabel;
    private logBase nextLog;
    private string scriptType;

    //constructor
    public logBase(string script, string label, logBase next)
    {
        scriptType = script;
        DIMLabel = label;
        nextLog = next;
    }

    public logBase(logBase next)
    {
        scriptType = "";
        DIMLabel = "";
        nextLog = next;
    }

    //Check if it should go to next method
    public virtual void check(string script, GameObject ob)
    {
        if (string.Equals(script, scriptType)) log(ob);
        else if (nextLog != null) nextLog.check(script, ob);
        else Debug.Log("logBase No correct log type found " + script + " " + ob);
    }

    //Basic log event
    protected virtual void log(GameObject ob)
    {
        string log1 = getTimestamp() + "; " + DIMLabel + "; " + scriptType + "; " + ob.name;
        writeLog.WriteString(log1);
    }

    //Get timestamp
    protected string getTimestamp()
    {
        return Time.time.ToString("f6");
    }

    public void setNext(logBase next)
    {
        nextLog = next;
    }

    public logBase getNext()
    {
        return nextLog;
    }
}
