using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : Interactable
{
    private Socket activeSocket = null; //the socket the gameobject is attached to (if any)

    public override void StartInteraction(Hand2 hand)
    {
        hand.PickUp(this);
    }

    public override void Interaction(Hand2 hand)
    {
        GetComponent<ColorToggle>().ToggleColor();
    }

    public override void EndInteraction(Hand2 hand)
    {
        hand.Drop();
    }

    //changes the socket the object is attached to
    public void AttachNewSocket(Socket newSocket)
    {
        //Debug.Log(this + " attach new socket ran");
        if (newSocket.GetStoredObject()) //if there is already an object in the socket, do nothing
        {
            return;
        }
        ReleaseOldSocket(); //otherwise release the gameobject from it's previous socket
        activeSocket = newSocket;
        activeSocket.Attach(this); //attaches the game object to the specified new socket
        isAvailable = false;
    }

    public void ReleaseOldSocket()
    {
        if (!activeSocket) //if there is no previous active socket, do nothing
        {
            return;
        }
        activeSocket.Detach(); //remove the object from the socket
        activeSocket = null;
        isAvailable = true; //says the object is available to attach to a new socket
    }

    public Boolean hasSocket()
    {
        if (activeSocket == null) return false;
        return true;
    }
}
