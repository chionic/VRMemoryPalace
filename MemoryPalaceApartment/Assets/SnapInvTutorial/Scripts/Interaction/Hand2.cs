using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand2 : MonoBehaviour
{

    private Socket socket = null;
    private SteamVR_Behaviour_Pose pose = null;
    private GameObject spawnedObject = null;
    public List<Interactable> contactInteractables = new List<Interactable>();
    private Hand2 myHandScript = null;

    //Menu script for toggling the menu
    public GameObject menu = null;
    private Menu menuScript;
    private Transform controller = null;

    public void Awake()
    {
        socket = GetComponent<Socket>();
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        myHandScript = this;
        menuScript = menu.GetComponent<Menu>();
        controller = gameObject.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("menuItem"))
        {
            //Debug.Log("ran menuItem");
            menuItem currentMenuItem = other.GetComponent<menuItem>();
            if (currentMenuItem.isTopLayer)
            {
                //Debug.Log("Menu is top layer true ran");
                spawnedObject = currentMenuItem.objectToSpawn;
                menuScript.toggleMenu(spawnedObject, controller);
            }
            else
            {
                //Debug.Log("top layer false ran");
                spawnedObject = currentMenuItem.objectToSpawn;
                GameObject spawnedOb = Instantiate(spawnedObject, transform.position, Quaternion.identity);
                Moveable moveScript = spawnedOb.gameObject.GetComponent<Moveable>();
                PickUp(moveScript);
                menuScript.toggleMenu(null, controller);
            }

        }
        else
        {
            AddInteractable(other.gameObject);
        }
    }

    private void AddInteractable(GameObject newObject)
    {
        Interactable newInteractable = newObject.GetComponent<Interactable>();
        contactInteractables.Add(newInteractable);
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveInteractable(other.gameObject);
    }

    private void RemoveInteractable(GameObject newObject)
    {
        Interactable existingInteractable = newObject.GetComponent<Interactable>();
        contactInteractables.Remove(existingInteractable);
    }

    public void TryInteraction()
    {
        if (NearestInteraction())
        {
            return;
        }
        HeldInteraction();
    }

    public bool NearestInteraction()
    {
        contactInteractables.Remove(socket.GetStoredObject());
        Interactable nearestObject = Utility.GetNearestInteractable(transform.position, contactInteractables);
        if (nearestObject)
            nearestObject.StartInteraction(myHandScript);
        return nearestObject;
    }

    private void HeldInteraction()
    {
        if (!HasHeldObject())
        {
            return;
        }
        Moveable heldObject = socket.GetStoredObject();
        heldObject.Interaction(this);
    }

    public void StopInteraction()
    {
        if (!HasHeldObject())
        {
            return;
        }
        Moveable heldObject = socket.GetStoredObject();
        heldObject.EndInteraction(this);
    }

    public void PickUp(Moveable moveable)
    {
        //Debug.Log("The socket is: " + socket + " and the moveable is: " + moveable);
        moveable.AttachNewSocket(socket);
    }

    public Moveable Drop()
    {
        if (!HasHeldObject())
        {
            return null;
        }
        Moveable detachedObject = socket.GetStoredObject();
        detachedObject.ReleaseOldSocket();
        Rigidbody rigidbody = detachedObject.gameObject.GetComponent<Rigidbody>();
        rigidbody.velocity = pose.GetVelocity();
        rigidbody.angularVelocity = pose.GetAngularVelocity();
        return detachedObject;
    }

    public bool HasHeldObject()
    {
        return socket.GetStoredObject();
    }
}
