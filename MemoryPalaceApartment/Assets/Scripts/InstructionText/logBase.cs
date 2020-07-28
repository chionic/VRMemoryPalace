using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class logBase
{
    private string DIMLabel;
    private logBase nextLog;
    private string scriptType;
    private string extraLog = "\"hand\": \"";

    private static int id = 0;
    private string log1;

    //constructor
    public logBase(string script, string label, logBase next)
    {
        scriptType = script;
        DIMLabel = label;
        nextLog = next;
    }

    public logBase(string script, string label, logBase next, string extra)
    {
        scriptType = script;
        DIMLabel = label;
        nextLog = next;
        extraLog = extra;
    }

    public logBase(logBase next)
    {
        scriptType = "";
        DIMLabel = "";
        nextLog = next;
    }

    //Check if it should go to next method
    public virtual void check(string script, GameObject ob, Vector3 playerPos)
    {
        if (string.Equals(script, scriptType)) log(ob, playerPos);
        else if (nextLog != null) nextLog.check(script, ob, playerPos);
        else Debug.Log("logBase No correct log type found " + script + " " + ob);
    }

    public virtual void check(string script, GameObject ob, string hand, Vector3 playerPos)
    {
        if (string.Equals(script, scriptType)) log(ob, hand, playerPos);
        else if (nextLog != null) nextLog.check(script, ob, hand, playerPos);
        else Debug.Log("logBase No correct log type found " + script + " " + ob);
    }

    //Basic log event
    protected virtual void log(GameObject ob, Vector3 playerPos)
    {
        if (ob == null)
        {
            log1 = getId() + "; " + getTimestamp() + "; " + DIMLabel + "; " + playerPos.x + "; " + playerPos.y + "; " + playerPos.z + ";;;;; {\"action\": \"" + scriptType + "\"}";
        }
        else
        {
            log1 = getId() + "; " + getTimestamp() + "; " + DIMLabel + "; " + playerPos.x + "; " + playerPos.y + "; " + playerPos.z + "; " + ob.name + "; "+ ob.transform.position.x + "; " + ob.transform.position.y + "; " + ob.transform.position.z + "; {\"action\": \"" + scriptType + "\"}";
        }
        
        writeLog.WriteString(log1);
    }

    protected virtual void log(GameObject ob, string hand, Vector3 playerPos)
    {
      
            log1 = getId() + "; " + getTimestamp() + "; " + DIMLabel + "; " + playerPos.x + "; " + playerPos.y + "; " + playerPos.z + "; " + ob.name + "; " + ob.transform.position.x + "; " + ob.transform.position.y + "; " + ob.transform.position.z + "; { \"action\": \"" + scriptType + "\", " + extraLog + hand + "\"}";

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

    protected int getId()
    {
        id++;
        return id;
    }


}
