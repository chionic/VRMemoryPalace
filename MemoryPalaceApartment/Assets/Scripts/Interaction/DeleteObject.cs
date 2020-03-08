using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    //public GameObject spawnedObjects = null;
    //int instanceIDspawnedObjects = 0;

    //private void Awake()
    //{
    //    int instanceIDspawnedObjects = spawnedObjects.GetInstanceID();
    //}
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger ran");
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
