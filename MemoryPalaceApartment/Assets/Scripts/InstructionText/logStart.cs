using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class logStart : logBase
{
    private string DIMLabel;
    private logBase nextLog;
    private string scriptType;
    public logStart(string script, string label, logBase next) : base(script, label, next)
    {

    }

    public logStart(logBase next) : base(next)
    {
        nextLog = next;
    }

    public override void check(string type, GameObject ob, Vector3 playerPos)
    {
        if (string.Equals(type, "start")) startedGame();
        else if (string.Equals(type, "stop")) stoppedGame();
        else if (nextLog != null) nextLog.check(type, ob, playerPos);
        else Debug.Log("logStart No correct log type found " + type + " " + ob);
    }

    public void startedGame()
    {
        writeLog.WriteString(getId() + "; " + getTimestamp() + ";;;;;;;; {\"action\": \"Started Game\", \"date\": \"" + DateTime.Now.ToString("HH:mm dd MMMM, yyyy") + "\"}");
    }

    public void stoppedGame()
    {
        writeLog.closeLog(getId() + "; " + getTimestamp() + ";;;;;;;; {\"action\": \"Stopped Game\", \"date\": \"" + DateTime.Now.ToString("HH:mm dd MMMM, yyyy") + "\"}");
    }

}
