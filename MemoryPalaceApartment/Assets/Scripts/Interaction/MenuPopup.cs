using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MenuPopup : MonoBehaviour
{
    private Transform controller = null;
    public GameObject menu;
    private Menu menuScript;
    // Start is called before the first frame update
    void Start()
    {
        //initialize menu objects
        controller = gameObject.transform;
        menuScript = menu.GetComponent<Menu>();
    }

    public void toggleMenu()
    {
        menuScript.toggleMenu(null, controller);
    }


}
