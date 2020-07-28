using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//checks if all the tutorial conditions have been met & deactivates the grey box blocking the door if they have
public class tutorialRoomExit : MonoBehaviour
{
    public bool object1 = false;
    public bool object2 = false;
    public bool object3 = false;
    private AudioSource source;
    // Start is called before the first frame update

    private void Awake()
    {
        source = this.GetComponent<AudioSource>();
    }
    public void deactivateDoor()
    {
        if (object1 == true && object2 == true && object3 == true)
        {
            source.Play();
            StartCoroutine(waitForSound());
           // this.gameObject.SetActive(false) ;
        }
    }

    IEnumerator waitForSound()
    {
        //Wait Until Sound has finished playing
        while (source.isPlaying)
        {
            yield return null;
        }

        //Auidio has finished playing, disable GameObject
        this.gameObject.SetActive(false);
    }
}
