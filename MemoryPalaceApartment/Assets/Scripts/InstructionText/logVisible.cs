using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class logVisible : logBase
{
    private string DIMLabel;
    private logBase nextLog;
 
    public logVisible(logBase next) : base(next)
    {
        nextLog = next;
        DIMLabel = "D_Obj";

    }

    public new void check(string scriptType, GameObject ob)
    {
        if (scriptType == "") log(ob);
        else nextLog.check(scriptType, ob);

    }
}
