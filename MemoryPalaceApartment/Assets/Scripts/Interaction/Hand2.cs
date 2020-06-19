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
    public GameObject UiText= null;
    private TextChange textChange = null;
    //public GameObject spawnedObjects = null;

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
        UiText = GameObject.Find("UIText");
        textChange = UiText.GetComponent<TextChange>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("menuItem"))
        {
            AddInteractable(other.gameObject);
            
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
        //Debug.Log("remove interactable started");
    }

    private void RemoveInteractable(GameObject newObject)
    {
        Interactable existingInteractable = newObject.GetComponent<Interactable>();
        contactInteractables.Remove(existingInteractable);
        //Debug.Log("removed interactable");
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
       // Debug.Log("NearestInteraction ran");
        contactInteractables.Remove(socket.GetStoredObject());
        Interactable nearestObject = Utility.GetNearestInteractable(transform.position, contactInteractables);
        if (nearestObject)
        {
           // Debug.Log(nearestObject + "    " + nearestObject.gameObject.CompareTag("menuItem"));
            if (nearestObject.gameObject.CompareTag("menuItem"))
            {
                createMenuItem(nearestObject.gameObject);
                //Debug.Log("createMenuItem started");
                return nearestObject;
            }
            nearestObject.StartInteraction(myHandScript);
        }
        
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
        //rigidbody.velocity = pose.GetVelocity();
        //rigidbody.angularVelocity = pose.GetAngularVelocity();
        return detachedObject;
    }

    public bool HasHeldObject()
    {
        return socket.GetStoredObject();
    }

    public void createMenuItem(GameObject other)
    {
        //Debug.Log("ran menuItem");
        menuItem currentMenuItem = other.GetComponent<menuItem>();
        if (currentMenuItem.isTopLayer)
        {
            removeMenuItems(contactInteractables);
            //Debug.Log("Menu is top layer true ran");
            spawnedObject = currentMenuItem.objectToSpawn;
            menuScript.toggleMenu(spawnedObject, controller);
        }
        else
        {
            //removeMenuItems(contactInteractables);
            //Debug.Log("top layer false ran");
            spawnedObject = currentMenuItem.objectToSpawn;
            GameObject spawnedOb = Instantiate(spawnedObject, transform.position, Quaternion.identity);
            //spawnedOb.transform.SetParent(spawnedObjects.transform, true);
            Moveable moveScript = spawnedOb.gameObject.GetComponent<Moveable>();
            PickUp(moveScript);
            menuScript.toggleMenu(null, controller);
        }
    }

    public void removeMenuItems(List<Interactable> list)
    {
        foreach(var item in list.ToArray())
        {
            if(item == null)
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

    public void updateUIText()
    {
        textChange.updateUIText();
    }
}
