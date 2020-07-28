using UnityEngine;

public class logMov : logBase
{
    private string DIMLabel;
    private logMov nextLog;
    private string scriptType;

    //constructor
    public logMov(string script, string label, logMov next) : base(script, label, next)
    {
        scriptType = script;
        DIMLabel = label;
        nextLog = next;
    }

    //Check if it should go to next method
    public void check(string script, string x, Vector3 playerPos)
    {
        if (string.Equals(script, scriptType)) log(x, playerPos);
        else if (nextLog != null) nextLog.check(script, x, playerPos);
        else Debug.Log("logBase No correct log type found " + script + " " + x);
    }

    //logs teleport actions
    public void log(string x, Vector3 playerPos)
    {
        string log1 = getId() + "; " + getTimestamp() + "; " + DIMLabel + "; " + playerPos.x + "; " + playerPos.y + "; " + playerPos.z 
            + ";;;;; {\"action\": \"" + scriptType + "\", \"type\": \"" + x + "\"}";
        writeLog.WriteString(log1);
    }
}