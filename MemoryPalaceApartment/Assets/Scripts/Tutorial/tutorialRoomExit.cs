using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//checks if all the tutorial conditions have been met & deactivates the grey box blocking the door if they have
public class tutorialRoomExit : MonoBehaviour
{
    public bool object1 = false;
    public bool object2 = false;
    public bool object3 = false;
    // Start is called before the first frame update


    public void deactivateDoor()
    {
        if (object1 == true && object2 == true && object3 == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}
