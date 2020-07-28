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

    //logs the size of the object along with everything else
    protected override void log(GameObject ob, Vector3 playerPos)
    {
        log1 = getId() + "; " + getTimestamp() + "; " + DIMLabel + "; " + playerPos.x + "; " + playerPos.y + "; " + playerPos.z + "; " + ob.name + 
            "; "  + ob.transform.position.x + "; " + ob.transform.position.y + "; " + ob.transform.position.z + "; {\"action\": \"" + scriptType 
            + "\", \"size\": \"" + ob.transform.localScale + "\"}";
        writeLog.WriteString(log1);
    }
}
