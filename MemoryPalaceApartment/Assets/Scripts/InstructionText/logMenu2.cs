using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logMenu2 : logBase
{
    private string DIMLabel;
    private logBase nextLog;
    private string scriptType;
    private string log1;

    //constructor
    public logMenu2(string script, string label, logBase next) : base(script, label, next)
    {
        scriptType = script;
        DIMLabel = label;
        nextLog = next;
    }

    protected override void log(GameObject ob, Vector3 playerPos)
    {
        if (ob == null)
        {
            log1 = getId() + "; " + getTimestamp() + "; " + DIMLabel + "; " + playerPos.x + "; " + playerPos.y + "; " + playerPos.z + ";;;;; {\"action\": \"" + scriptType + "\", \"type\": \"menuClose\" }";
        }
        else
        {
            log1 = getId() + "; " + getTimestamp() + "; " + DIMLabel + "; " + playerPos.x + "; " + playerPos.y + "; " + playerPos.z + "; " + ob.name + "; " + ob.transform.position.x + "; " + ob.transform.position.y + "; " + ob.transform.position.z + "; {\"action\": \"" + scriptType + "\", \"type\": \"menuOpen\" }";
        }

        writeLog.WriteString(log1);
    }
}
