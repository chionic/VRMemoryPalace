using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Keeps track of what state the menu is currently in and changes the state
public class Menu : MonoBehaviour
{
    private int currentState = 0; //State Tracker: 0=hidden, 1= top layer, 2 = second layer
    private GameObject currentlyActive = null; //stores the currently active submenu (if any)

    public Hand2 right = null;
    public Hand2 left = null;

    //List of submenus
    public List<GameObject> subMenus = new List<GameObject>(); //List of submenus
    Transform menuTransform = null; //where the menu object is in space (transform)


    private void Awake()
    {
        menuTransform = gameObject.transform; //lets the menu transform be the current gameobject
        foreach (Transform child in menuTransform) //adds all the submenu gameobjects to a list
        {
            subMenus.Add(child.gameObject);
        }

    }

    //Toggle menu layer - reads in the selected submenu and where the vontroller is in space
    public void toggleMenu(GameObject submenu, Transform controller)
    {
        if (currentState == 0) //0=hidden
        {
            subMenus[0].SetActive(true); //sets the first submenu ("top menu") to active
            currentlyActive = subMenus[0];
            //moves the active submenu to where the players controller is and rotates it to face the player
            currentlyActive.transform.position = new Vector3(controller.transform.position.x, controller.transform.position.y, controller.transform.position.z);
            currentlyActive.transform.rotation = Quaternion.Euler(new Vector3(currentlyActive.transform.rotation.eulerAngles.x, controller.transform.rotation.eulerAngles.y, currentlyActive.transform.rotation.eulerAngles.z));
            currentState = 1;
            //Debug.Log("set state 1");
        }

        else if (currentState == 1 && submenu != null) //1= top layer, and the fed in submenu exists
        {
            currentlyActive.SetActive(false); //sets the top layer menu to inactive
            submenu.SetActive(true); //sets the selected submenu to active
            currentlyActive = submenu;
            currentlyActive.transform.position = new Vector3(controller.transform.position.x, controller.transform.position.y, controller.transform.position.z);
            currentlyActive.transform.rotation = Quaternion.Euler(new Vector3(currentlyActive.transform.rotation.eulerAngles.x, controller.transform.rotation.eulerAngles.y, currentlyActive.transform.rotation.eulerAngles.z));
            currentState = 2;
            //Debug.Log("set state 2");
        }

        else if (currentState == 2 || (currentState == 1 && submenu == null)) //2= second layer, and the fed in submenu exists
        {
            //hides the menu again and removes the potential menu selections from the list of selectable objects
            right.removeMenuItems(right.contactInteractables);
            left.removeMenuItems(left.contactInteractables);
            currentlyActive.SetActive(false);
            currentlyActive = null;
            currentState = 0;
            //Debug.Log("set state 0");
        }
    }

    public int getState()
    {
        return currentState;
    }
}
