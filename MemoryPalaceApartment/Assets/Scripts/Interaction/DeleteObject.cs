using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Deletes a game object when it hits the collider of a gameobject this script is attached to.
public class DeleteObject : MonoBehaviour
{
    private GameObject menu1 = null;
    private Menu menuScript1 = null;

    void Awake()
    {
        menu1 = GameObject.FindGameObjectWithTag("Menu");
        menuScript1 = menu1.GetComponent<Menu>();
        Debug.Log("The menu is: " + menu1 + " and the script is: " + menuScript1);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(menuScript1.getState() == 0)
        {
            if(other.gameObject == null)
            {
                //return;
            }
            //only destroys objects with specific tags assigned to them
            else if (other.gameObject.CompareTag("object") || other.gameObject.CompareTag("coin") ||
            other.gameObject.CompareTag("smilingFace") || other.gameObject.CompareTag("topHat"))
            {
                if (other.gameObject.GetComponent<Moveable>().hasSocket()) //only deletes objects if they are in the players hands (or attached to another socket)
                {
                    //destroy it all!!!
                    Destroy(other.gameObject);
                }
                
            }
        }

    }
}
