using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

//calls the toggleMenu function when the menu button on a controller is pressed or the change colour function if the controller is holding an object
public class MenuPopup : MonoBehaviour
{
    private Transform controller = null; //where the controller the script is attached to is in space
    public GameObject menu; //the menu game object
    private Menu menuScript; //the menu script
    private Socket hand2Socket;
    private ColorToggle ct;

    void Start()
    {
        //initialize menu objects
        controller = gameObject.transform;
        menu = GameObject.FindWithTag("Menu");
        menuScript = menu.GetComponent<Menu>();
        hand2Socket = this.gameObject.GetComponent<Socket>();
        
    }

    public void toggleMenu()
    {
        //get game object in hand (if attached)
        if(hand2Socket != null)
        {
            if(hand2Socket.GetStoredObject() != null)
            {
                ct = hand2Socket.GetStoredObject().GetComponent<ColorToggle>();
                ct.ToggleColor();
                return;
            }
            else { menuScript.toggleMenu(null, controller); return; }
        }
        else
        {
            hand2Socket = controller.gameObject.GetComponent<Socket>();
        }
        //else toggle the menu
        menuScript.toggleMenu(null, controller);
    }


}
