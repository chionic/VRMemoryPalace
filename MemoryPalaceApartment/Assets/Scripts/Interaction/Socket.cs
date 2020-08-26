using UnityEngine;
using UnityEngine.Events;

public class Socket : MonoBehaviour
{
    public Moveable storedObject = null; //what gameobject is currently stored in the socket
    private FixedJoint joint = null; //the joint the socket is attached to
    //private MakeLog logger;
    private UnityEvent _attached = new UnityEvent();
    public UnityEvent Attached {  get { return _attached; } }

    public void Awake()
    {
        joint = GetComponent<FixedJoint>();
        //logger = GameObject.FindWithTag("logger").GetComponent<MakeLog>();
    }

    public void Attach(Moveable newObject)
    {
        if (storedObject) return; //if there is already an object stored in the socket, return
        //otherwise put the new game object into the socket by attaching it to the joint component and setting its position to the position of the socket
        storedObject = newObject;
        storedObject.transform.position = transform.position;
        storedObject.transform.rotation = Quaternion.identity;
        Rigidbody storedBody = storedObject.gameObject.GetComponent<Rigidbody>();
        joint.connectedBody = storedBody;
        //logger.makeLogEntry("AttachObject", newObject.gameObject, this.gameObject);
        Debug.Log("attach called " + storedObject + getStoredobject());
        Attached.Invoke();
    }

    public void Detach()
    {
        if (!storedObject) return; //if the socket is empty, return
        //logger.makeLogEntry("releaseObject", storedObject.gameObject, this.gameObject.name.ToString());
        joint.connectedBody = null; //otherwise remove the stored object from the joint
        storedObject = null;

    }

    public Moveable GetStoredObject()
    {
        return storedObject;
    }

    public bool getStoredobject()
    {
        if (!storedObject) return false;
        return true;
    }

}