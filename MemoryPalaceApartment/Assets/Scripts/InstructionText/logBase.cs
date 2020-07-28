using UnityEngine;

//Base class for creating a log, specifies what the log looks like depending on what kind of information is fed in
public class logBase
{
    private string DIMLabel; //The associated DIM model label
    private logBase nextLog; //The nextLog in the linked list
    private string scriptType; //The type of event that happened (eg an object becoming visible)
    private string extraLog = "\"hand\": \""; //in the cases where extra data is fed into the log, the formatting around the data

    private static int id = 0; //all log classes share this number, used to count the number of logs
    private string log1; //a string sent to the writeLog script to log

    //constructors
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
    //end constructors

    //Check if the event (called script here) fed in can be handled by this class
    public virtual void check(string script, GameObject ob, Vector3 playerPos)
    {
        //if the script/event matches up, send to the log function of this class
        if (string.Equals(script, scriptType)) log(ob, playerPos);
        //if the string/event does not match and there is a next element in the linked list, send it there
        else if (nextLog != null) nextLog.check(script, ob, playerPos);
        //otherwise send a log message to the unity log to let the developer know the event/script had no match
        else Debug.Log("logBase No correct log type found " + script + " " + ob);
    }

    //Check if the event (called script here) fed in can be handled by this class, comes with extra data fed in as a string
    public virtual void check(string script, GameObject ob, string hand, Vector3 playerPos)
    {
        if (string.Equals(script, scriptType)) log(ob, hand, playerPos);
        else if (nextLog != null) nextLog.check(script, ob, hand, playerPos);
        else Debug.Log("logBase No correct log type found " + script + " " + ob);
    }

    //Basic log event
    protected virtual void log(GameObject ob, Vector3 playerPos)
    {
        if (ob == null) //if there is no object attached to the event, log only the event type and player position
        {
            log1 = getId() + "; " + getTimestamp() + "; " + DIMLabel + "; " + playerPos.x + "; " + playerPos.y + "; " + playerPos.z 
                + ";;;;; {\"action\": \"" + scriptType + "\"}";
        }
        else //otherwise log the object name and position as well
        {
            log1 = getId() + "; " + getTimestamp() + "; " + DIMLabel + "; " + playerPos.x + "; " + playerPos.y + "; " + playerPos.z + "; " + ob.name 
               + "; "+ ob.transform.position.x + "; " + ob.transform.position.y + "; " + ob.transform.position.z + "; {\"action\": \"" + scriptType 
               + "\"}";
        }
        writeLog.WriteString(log1); //send to writeLog to write into text file
    }

    //log event with extra data fed in
    protected virtual void log(GameObject ob, string hand, Vector3 playerPos)
    {
        //logs everything above and the extra information sent in (note the extraLog and hand string)    
        log1 = getId() + "; " + getTimestamp() + "; " + DIMLabel + "; " + playerPos.x + "; " + playerPos.y + "; " + playerPos.z + "; " + ob.name + 
            "; "+ ob.transform.position.x + "; " + ob.transform.position.y + "; " + ob.transform.position.z + "; { \"action\": \"" + scriptType 
            + "\", "  + extraLog + hand + "\"}";
        writeLog.WriteString(log1);
    }

    //Get timestamp - gets the timestamp to six decimal places
    protected string getTimestamp()
    {
        return Time.time.ToString("f6");
    }

    //if you want to see/change the next log class at run time use these
    public void setNext(logBase next)
    {
        nextLog = next;
    }

    public logBase getNext()
    {
        return nextLog;
    }

    //increases the static id (same across all log classes) and returns to be used in the log
    protected int getId()
    {
        id++;
        return id;
    }
}
