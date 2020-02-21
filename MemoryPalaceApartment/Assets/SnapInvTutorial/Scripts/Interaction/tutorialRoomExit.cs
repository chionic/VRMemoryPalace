using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialRoomExit : MonoBehaviour
{
    public bool object1 = false;
    public bool object2 = false;
    public bool object3 = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    //this could be very much improved
    void Update()
    {
        if(object1 == object2 == object3 == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}
