using UnityEngine;

public class isVisible : MonoBehaviour
{
    private MakeLog logger;
    isVisible currentScript;
    private GameObject parent;
    public bool hasChildRenderer = true;

    private void Awake()
    {
        currentScript = this;
        logger = GameObject.FindWithTag("logger").GetComponent<MakeLog>();
        if (hasChildRenderer && gameObject.transform.parent.gameObject != null)
        {
            parent = gameObject.transform.parent.gameObject; //get the parent game object...
        }
        else { parent = gameObject; } //or if the renderer is part of the game object we want to log (true for some objects and most loci) set it to itself
    }
    void OnBecameInvisible()
    {
        logger.makeLogEntry("isInvisible", parent);
    }


    void OnBecameVisible()
    {
        logger.makeLogEntry("isVisible", parent);
    }

    
}
