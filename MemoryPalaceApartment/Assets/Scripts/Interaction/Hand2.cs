﻿using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Valve.VR.InteractionSystem.Sample;

public class Hand2 : MonoBehaviour
{

    //the hand/controller game object
    private Socket socket = null; //the socket attached to the controller
    private Socket otherSocket = null;
    private SteamVR_Behaviour_Pose pose = null; //what pose the controller/hand have in game space
    private Hand2 myHandScript = null; //the hand script attached to the controller
    private GameObject spawnedObject = null;
    public List<Interactable> contactInteractables = new List<Interactable>(); //a list of interactable gameobjects that could be picked up by the controller (based on proximity) 

    //Menu script for toggling the menu
    public GameObject menu = null;
    private Menu menuScript;
    private Transform controller = null;
    public Transform otherController = null;
    private float distance;
    private Moveable nObject;
    private resizeObject resizeScript;
    private MakeLog logger; //game object that has logging interface

    //UI text related
    public GameObject UiText = null; //the text game object (represented by the tablet tied to the left hand controller in game)
    private AddText textChange = null; //script that changes UI text

    protected bool leftDown;

    public void Awake() //initialises the various variables listed above
    {
        logger = GameObject.FindWithTag("logger").GetComponent<MakeLog>();
        menu = GameObject.FindWithTag("Menu");
        socket = GetComponent<Socket>();
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        myHandScript = this;
        menuScript = menu.GetComponent<Menu>();
        controller = gameObject.transform;
        textChange = UiText.GetComponent<AddText>();
        otherSocket = otherController.GetComponent<Socket>();
    }

    //when another gameobject interacts with the controller collider, add it to the contact Interactables list
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hand2 on trigger enter called");
        if (other.gameObject.CompareTag("menuItem"))
        {
            AddInteractable(other.gameObject);

        }
        else
        {
            AddInteractable(other.gameObject);
        }
    }

    //adds a game object's Interactable script to the list of gameobjects the controller can interact with
    private void AddInteractable(GameObject newObject)
    {
        Interactable newInteractable = newObject.GetComponent<Interactable>();
        contactInteractables.Add(newInteractable);
    }

    //when another gameobject's collider stops colliding with the controller, remove it from the contact interactables list
    private void OnTriggerExit(Collider other)
    {
        RemoveInteractable(other.gameObject);
    }

    //removes a game object's interactable script from the list of game objects the controller can interact with
    private void RemoveInteractable(GameObject newObject)
    {
        Interactable existingInteractable = newObject.GetComponent<Interactable>();
        contactInteractables.Remove(existingInteractable);
    }

    //called when the trigger button on the controller is pressed, attempts to start an interaction with an object
    public void TryInteraction()
    {
        if (NearestInteraction())
        {
            return;
        }
        HeldInteraction();
    }

    //checks for the nearest game object and decides whether it is a menu interaction, a pick up interaction or no possible objects to interact with exist
    public bool NearestInteraction()
    {
        contactInteractables.Remove(socket.GetStoredObject()); //removes an object already in the hand from contact interactables list
        Interactable nearestObject = Utility.GetNearestInteractable(transform.position, contactInteractables); //finds the nearest object to interact with (if any)
        if (nearestObject)
        {
            if (nearestObject.gameObject.CompareTag("menuItem")) //if the nearest item is a menu item, run create menu item
            {
                createMenuItem(nearestObject.gameObject);
                return nearestObject;
            }
            if (nearestObject.gameObject.CompareTag("object")) //checks if the nearest object is an interactable object
            {
                try
                {
                    nObject = nearestObject.gameObject.GetComponent<Moveable>(); //gets the moveable script attached to it
                    resizeScript = nObject.gameObject.GetComponent<resizeObject>(); //gets resize script attached to it
                    if (nObject.hasSocket()) //checks if the object is already attached to a controller
                    {
                        leftDown = true; //while leftDown is true the resize script is called
                        return nearestObject;
                    }
                    else //if the nearest object is not already attached to a socket
                    {
                        nearestObject.StartInteraction(myHandScript); //calls the hand pick up function from the selected (closest) game object's interactable script
                        return nearestObject;
                    }

                }
                catch
                {
                    Debug.LogWarning("" + "casting error");
                }
            }
            nearestObject.StartInteraction(myHandScript);  //calls the hand pick up function from the selected (closest) game object's interactable script
        }
        return nearestObject; //returns true if a nearest object exists
    }

    //changes the colour of the game object in the controller's socket (if possible)
    private void HeldInteraction()
    {
        if (!HasHeldObject()) //if the hand/controller is empty return
        {
            return;
        }
        Moveable heldObject = socket.GetStoredObject();//get the game object in the hand
        heldObject.Interaction(this); //change the colour of the selected game object (from the game object's moveable script)
    }

    //removes a game object from the socket of the controller
    public void StopInteraction()
    {
        if (leftDown) //if an object has been resized
        {
            logger.makeLogEntry("resizeObject", nObject.gameObject); //logs the event
        }
        leftDown = false; //stops the resize script from being called
        nObject = null; //sets the object being resized to null
        resizeScript = null;
        if (!HasHeldObject()) //if the controller socket is empty, return
        {
            return;
        }
        Moveable heldObject = socket.GetStoredObject();
        heldObject.EndInteraction(this); //otherwise drop the object from the game object's moveable script
    }

    public void PickUp(Moveable moveable) //add the selected game object to the hand/controller's socket
    {
        moveable.AttachNewSocket(socket);
    }

    public Moveable Drop() //removes a game object from the controller's socket
    {
        if (!HasHeldObject()) //if the controller's socket is empty, return
        {
            return null;
        }
        Moveable detachedObject = socket.GetStoredObject();
        detachedObject.ReleaseOldSocket(); //otherwise remove the object from the socket
        //Rigidbody rigidbody = detachedObject.gameObject.GetComponent<Rigidbody>(); //sets the velocity of the detached game object (commented out so object floats gently to ground or hovers without gravity enabled)
        //rigidbody.velocity = pose.GetVelocity();
        //rigidbody.angularVelocity = pose.GetAngularVelocity();
        return detachedObject; //return the freed game object
    }

    public bool HasHeldObject()  //check if the hand is already holding something
    {
        return socket.GetStoredObject();
    }



    //spawns the relevant submenu/game object when an object in the menu is selected
    public void createMenuItem(GameObject other)
    {
        menuItem currentMenuItem = other.GetComponent<menuItem>();
        if (currentMenuItem.isTopLayer) //if the item is a top level menu item, activate the relevant submenu (and remove the previous menu items from contact interactables)
        {
            removeMenuItems(contactInteractables);
            spawnedObject = currentMenuItem.objectToSpawn;
            menuScript.toggleMenu(spawnedObject, controller);
        }
        else //if the item is a submenu level item, spawn the relevant game object into the controller socket
        {

            //removeMenuItems(contactInteractables);
            //spawnedObject = currentMenuItem.objectToSpawn;
            GameObject object1 = Resources.Load("Object_prefabs/" + currentMenuItem.findObject) as GameObject;
            GameObject spawnedOb = Instantiate(object1, transform.position, Quaternion.identity);
            spawnedOb.name = spawnedOb.name + uniqueIdGenerator.getID();
            uniqueIdGenerator.setID();
            logger.makeLogEntry("createObject", spawnedOb.gameObject, this.gameObject.name.ToString());
            //spawnedOb.transform.SetParent(this.transform, true);
            Moveable moveScript = spawnedOb.gameObject.GetComponent<Moveable>();
            PickUp(moveScript);
            menuScript.toggleMenu(null, controller);
        }
    }

    //when a submenu becomes hidden/the menu changes state, removes the game objects in the previous submenu from the list of contact interactables
    public void removeMenuItems(List<Interactable> list)
    {
        foreach (var item in list.ToArray())
        {
            if (item == null)
            {
                return;
            }
            if (item.gameObject.CompareTag("menuItem"))
            {
                list.Remove(item);
            }
        }
        contactInteractables = list;
    }

    //changes the text on the in game tablet
    public void updateUIText()
    {
        textChange.updateUIText();
        ControllerHintsExample x = this.gameObject.GetComponent<ControllerHintsExample>();
    }

    //placing these three lines at the top of the 'stopInteraction' method instead creates a more efficient script but it will only change size once, no inbetween
    public void Update() //called every frame
    {
        if (leftDown) {
            //get distance between controllers
            distance = Vector3.Distance(otherController.position, transform.position);
            growSize(distance, resizeScript); //ca;; growSize
        }
    }

    public void growSize(float distance, resizeObject script)
    {
            if (script != null)
            {
                script.grow(distance); //sends data to resizeObject script
            }

    }

}
