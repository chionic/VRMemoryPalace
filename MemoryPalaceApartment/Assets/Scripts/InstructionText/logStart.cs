using UnityEngine;
using System;

//logs specifically the start and end of the application runtime
public class logStart : logBase
{
    private string DIMLabel;
    private logBase nextLog;
    private string scriptType;

    public logStart(logBase next) : base(next)
    {
        nextLog = next;
    }

    //checks if the start or stop keywords are used
    public override void check(string type, GameObject ob, Vector3 playerPos)
    {
        if (string.Equals(type, "start")) startedGame();
        else if (string.Equals(type, "stop")) stoppedGame();
        else if (nextLog != null) nextLog.check(type, ob, playerPos);
        else Debug.Log("logStart No correct log type found " + type + " " + ob);
    }

    public void startedGame() //if the game just started say so and give a date and time (based on the computer's date and time)
    {
        writeLog.makeLogFile();
        writeLog.WriteString(getId() + "; " + getTimestamp() + ";;;;;;;; {\"action\": \"Started Game\", \"date\": \"" + DateTime.Now.ToString("HH:mm dd MMMM, yyyy") 
            + "\"}");
    }

    public void stoppedGame() //if the game stopped say so and give a date and time (based on the computer's date and time)
    {
        writeLog.closeLog(getId() + "; " + getTimestamp() + ";;;;;;;; {\"action\": \"Stopped Game\", \"date\": \"" + DateTime.Now.ToString("HH:mm dd MMMM, yyyy") 
            + "\"}");
    }

}
