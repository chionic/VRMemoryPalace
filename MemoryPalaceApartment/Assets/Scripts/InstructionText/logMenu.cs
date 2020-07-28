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


    //Check if it should go to next method
    public override void check(string script, GameObject ob, Vector3 playerPos)
    {
        //This version of the check function considers whether the object passed into it is a menu object or not
        //by adding an extra condition to the if statement, and thus it only handles specifically menu related visibility
        if (string.Equals(script, scriptType) && ob.CompareTag("submenu")) log(ob, playerPos);
        else if (nextLog != null) nextLog.check(script, ob, playerPos);
        else Debug.Log("logBase No correct log type found " + script + " " + ob);
    }

}
