using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class that keeps track of the variables in each menu object
public class menuItem : MonoBehaviour
{
    private GameObject parent = null; //the parent of the menu object
    public bool isTopLayer = true; //if the menu object is in the top layer of the menu
    public GameObject objectToSpawn; //the object spawned when the menu object is selected (either an interactable item or a submenu layer)
    void Awake()
    {
        //parent of our current game object.
        parent = gameObject.transform.parent.gameObject;
    }


}