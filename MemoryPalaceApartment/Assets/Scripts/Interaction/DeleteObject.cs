using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    public GameObject menu1 = null;
    private Menu menuScript1 = null;
    //public GameObject spawnedObjects = null;
    //int instanceIDspawnedObjects = 0;

    void Awake()
    {
        Debug.Log("ran awake");
        //int instanceidspawnedobjects = spawnedobjects.getinstanceid();
        menuScript1 = menu1.GetComponent<Menu>();
        Debug.Log("The menu is: " + menu1 + " and the script is: " + menuScript1);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger ran");
        if(menuScript1.getState() == 0)
        {
            if (other.gameObject.CompareTag("object") || other.gameObject.CompareTag("coin") ||
            other.gameObject.CompareTag("smilingFace") || other.gameObject.CompareTag("topHat"))
            {
                Debug.Log(other.gameObject + "   " + other.gameObject.tag);
                //destroy it all!!!
                Destroy(other.gameObject);
                Debug.Log("Destroy ran");
            }
            //if (other.gameObject.transform.parent.GetInstanceID() == instanceIDspawnedObjects)
            //{
            //    Debug.Log(other.gameObject + "   " + other.gameObject.tag);
            //       //destroy it all!!!
            //       Destroy(other.gameObject);
            //       Debug.Log("Destroy ran");
            //}
        }

    }
}
