using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuItem : MonoBehaviour
{
    private GameObject parent = null;
    public bool isTopLayer = true;
    public GameObject objectToSpawn;
    // Start is called before the first frame update


    void Awake()
    {
        //parent of our current game object.
        parent = gameObject.transform.parent.gameObject;
    }


}