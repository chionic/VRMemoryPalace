using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

//calls the toggleMenu function when the menu button on a controller is pressed
public class MenuPopup : MonoBehaviour
{
    private Transform controller = null; //where the controller the script is attached to is in space
    public GameObject menu; //the menu game object
    private Menu menuScript; //the menu script

    void Start()
    {
        //initialize menu objects
        controller = gameObject.transform;
        menu = GameObject.FindWithTag("Menu");
        menuScript = menu.GetComponent<Menu>();
    }

    public void toggleMenu()
    {
        menuScript.toggleMenu(null, controller);
    }


}
