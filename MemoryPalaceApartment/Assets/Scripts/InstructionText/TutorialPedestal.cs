using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Assigned to each box in the tutorial, keeps track if the right object is on it or not
public class TutorialPedestal : MonoBehaviour
{
    //public bool object1 = false;
    public GameObject exit = null; //the grey box bloxking the exit to the tutorial room
    public string myTag = null; //the tag assigned to the object that must be placed on the box to complete the tutorial section
    public int booleanAssigned = 0; //which box it is

    private void Awake()
    {
        //object1 = exit.GetComponent<tutorialRoomExit>().object1;
    }
    private void OnTriggerEnter(Collider other)
    {
        //if the object with the right tag is placed on top of the box...
        if (other.CompareTag(myTag))
        {
            //set the corresponding boolean to true
            if (booleanAssigned == 1)
                exit.GetComponent<tutorialRoomExit>().object1 = true;
            else if (booleanAssigned == 2)
                exit.GetComponent<tutorialRoomExit>().object2 = true;
            else if (booleanAssigned == 3)
                exit.GetComponent<tutorialRoomExit>().object3 = true;
            else
                Debug.Log("Error finding correct boolean");
            //...and see if all the tutorial conditions to open the door have been met
            exit.GetComponent<tutorialRoomExit>().deactivateDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if the object with the right tag is Removed from the top of the box...
        if (other.CompareTag(myTag))
        {
            //set the corresponding boolean to false again
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
