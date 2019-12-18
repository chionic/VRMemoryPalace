using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : Interactable
{
    private Socket socket = null;

    private void Awake()
    {
        socket = GetComponent<Socket>();
    }

    public override void StartInteraction(Hand2 hand)
    {
        if (hand.HasHeldObject())
        {
            TryStore(hand);
            Debug.Log("try store hand complete");
        }
        else
        {
            TryRetrieve(hand);
        }
    }

    private void TryStore(Hand2 hand)
    {
        Debug.Log("try store called");
        if (socket.GetStoredObject())
        {
            Debug.Log("socket already had stored object");
            return;
        }
        Moveable objectToStore = hand.Drop();
        Debug.Log(objectToStore);
        objectToStore.AttachNewSocket(socket);
        Debug.Log(socket);
    }

    private void TryRetrieve(Hand2 hand)
    {
        if (!socket.GetStoredObject())
        {
            return;
        }
        Moveable objectToRetrieve = socket.GetStoredObject();
        hand.PickUp(objectToRetrieve);
    }
}
