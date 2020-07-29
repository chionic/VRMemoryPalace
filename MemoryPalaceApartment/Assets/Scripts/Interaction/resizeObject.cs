using UnityEngine;

public class resizeObject : MonoBehaviour
{
    Transform transform1; //the transform attached to the gameObject this script is also attached to
    float defaultScale; //the scale of the object initially

    void Start()
    {
        transform1 = this.transform;
        defaultScale = transform1.localScale.x;
    }

    public void grow(float change) //changes the size of the object
    {
        if (change < 0.01f) change = 0.01f; //doesn't allow the object to be less than 0.1 in size
        //checks that the object is not too big or small to change the size of
        if((transform1.localScale.x < 8 || change < 8f) && (transform1.localScale.x > 0.02f || change > 0.01f))
        {
            //rescales the object
            transform1.localScale = new Vector3(change *10 * defaultScale, change *10 * defaultScale, change*10 * defaultScale);
        }
        
    }
}
