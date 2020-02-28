using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger ran");
        if (other.gameObject.CompareTag("object") || other.gameObject.CompareTag("coin") ||
            other.gameObject.CompareTag("smilingFace") || other.gameObject.CompareTag("topHat"))
        {
            //destroy it all!!!
            Destroy(other.gameObject);
            Debug.Log("Destroy ran");
        }
    }
}
