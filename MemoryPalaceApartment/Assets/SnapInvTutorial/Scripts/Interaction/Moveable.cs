using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : Interactable
{
    private Socket activeSocket = null;

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

    public void AttachNewSocket(Socket newSocket)
    {
        if (newSocket.GetStoredObject())
        {
            return;
        }
        ReleaseOldSocket();
        activeSocket = newSocket;
        activeSocket.Attach(this);
        isAvailable = false;
    }

    public void ReleaseOldSocket()
    {
        if (!activeSocket)
        {
            return;
        }
        activeSocket.Detach();
        activeSocket = null;
        isAvailable = true;
    }
}
