using UnityEngine;

//Attach this to an object to track when it becomes visible/invisible
public class isVisible : MonoBehaviour
{
    private MakeLog logger; //a reference to the logging component
    private GameObject parent; //a reference to the object that is visible/invisible
    public bool hasChildRenderer = true; //sometimes the mesh is attached to the child object instead of the object itself, true if this is the case

    private void Awake()
    {
        logger = GameObject.FindWithTag("logger").GetComponent<MakeLog>(); //finds the current logging component to send things to
        if (hasChildRenderer && gameObject.transform.parent.gameObject != null) //if the gameObject has a parent in the hierarchy
        {
            parent = gameObject.transform.parent.gameObject; //get the parent game object...
        }
        else { parent = gameObject; } //or if the renderer is part of the game object we want to log (true for some objects and most loci) set it to itself
    }
    void OnBecameInvisible() //Runs whenever the object becomes invisible from the player camera
    {
        //call to the log, feeds in what is happening (the object became invisible) and what the object is (parent)
        logger.makeLogEntry("isInvisible", parent); 
    }


    void OnBecameVisible() //Runs whenever the object becomes visible from the player camera
    {
        logger.makeLogEntry("isVisible", parent);
    }

    
}
