using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    //StateTracker
    private int currentState = 0;
    //0=hidden, 1= top layer, 2 = second layer
    private GameObject currentlyActive = null;

    public Hand2 right = null;
    public Hand2 left = null;

    //this is how you should do this...
    public enum currentState1
    {
        HIDDEN,
        TOP_LAYER,
        SECOND_LAYER
    };

    //List of submenus
    public List<GameObject> subMenus = new List<GameObject>();
    Transform menuTransform = null;

    private void Awake()
    {
        menuTransform = gameObject.transform;
        foreach (Transform child in menuTransform)
        {
            subMenus.Add(child.gameObject);
        }
        
    }

    //Toggle menu layer
    public void toggleMenu(GameObject submenu, Transform controller)
    {
        if (currentState == 0)
        {
            subMenus[0].SetActive(true);
            currentlyActive = subMenus[0];
            currentlyActive.transform.position = new Vector3(controller.transform.position.x, controller.transform.position.y, controller.transform.position.z);
            currentlyActive.transform.rotation = Quaternion.Euler(new Vector3(currentlyActive.transform.rotation.eulerAngles.x, controller.transform.rotation.eulerAngles.y, currentlyActive.transform.rotation.eulerAngles.z));
            currentState = 1;
            //Debug.Log("set state 1");
        }

        else if (currentState == 1 && submenu != null)
        {
            currentlyActive.SetActive(false);
            submenu.SetActive(true);
            currentlyActive = submenu;
            currentlyActive.transform.position = new Vector3(controller.transform.position.x, controller.transform.position.y , controller.transform.position.z);
            currentlyActive.transform.rotation = Quaternion.Euler(new Vector3(currentlyActive.transform.rotation.eulerAngles.x, controller.transform.rotation.eulerAngles.y, currentlyActive.transform.rotation.eulerAngles.z));
            currentState = 2;
            //Debug.Log("set state 2");
        }

        else if (currentState == 2 || (currentState == 1 && submenu == null))
        {
            right.removeMenuItems(right.contactInteractables);
            left.removeMenuItems(left.contactInteractables);
            currentlyActive.SetActive(false);
            currentlyActive = null;
            currentState = 0;
            //Debug.Log("set state 0");
        }
    }
}
