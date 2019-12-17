using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Handy : MonoBehaviour
{
 
    private Socket socket = null;
    private SteamVR_Behaviour_Pose pose = null;

    public List<Interactable> contactInteractables = new List<Interactable>();


    public void Awake()
    {
        socket = GetComponent<Socket>();
        pose = GetComponent<SteamVR_Behaviour_Pose>();

    }

    private void OnTriggerEnter(Collider other)
    {
        AddInteractable(other.gameObject);
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
            nearestObject.StartInteraction(this);
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

    public void PickUp (Moveable moveable)
    {
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