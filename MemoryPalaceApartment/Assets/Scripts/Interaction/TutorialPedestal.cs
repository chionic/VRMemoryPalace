using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPedestal : MonoBehaviour
{
    //public bool object1 = false;
    public GameObject exit = null;
    public string myTag = null;
    public int booleanAssigned = 0;

    private void Awake()
    {
        //object1 = exit.GetComponent<tutorialRoomExit>().object1;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider triggered");
        if (other.CompareTag(myTag))
        {
            Debug.Log("compare tag " + myTag + " ran");
            if (booleanAssigned == 1)
                exit.GetComponent<tutorialRoomExit>().object1 = true;
            else if (booleanAssigned == 2)
                exit.GetComponent<tutorialRoomExit>().object2 = true;
            else if (booleanAssigned == 3)
                exit.GetComponent<tutorialRoomExit>().object3 = true;
            else
                Debug.Log("Error finding correct boolean");
            exit.GetComponent<tutorialRoomExit>().deactivateDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(myTag))
        {
            if (booleanAssigned == 1)
                exit.GetComponent<tutorialRoomExit>().object1 = false;
            else if (booleanAssigned == 2)
                exit.GetComponent<tutorialRoomExit>().object2 = false;
            else if (booleanAssigned == 3)
                exit.GetComponent<tutorialRoomExit>().object3 = false;
            else
                Debug.Log("Error finding correct boolean");
        }
    }
}
