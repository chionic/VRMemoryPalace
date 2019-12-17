using UnityEngine;


public class Interactable : MonoBehaviour
{
    protected bool isAvailable = true;

    public virtual void StartInteraction(Handy hand)
    {
        print("start");
    }

    public virtual void Interaction(Handy hand)
    {
        print("interaction");
    }

    public virtual void EndInteraction(Handy hand)
    {
        print("end");
    }

    public bool GetAvailability()
    {
        return isAvailable;
    }
}
