using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resizeObject : MonoBehaviour
{
    float result = Mathf.Lerp(3f, 5f, 0.5f);
    Transform transform1;

    // Start is called before the first frame update
    void Start()
    {
        transform1 = this.transform;
    }

    public void grow(float change)
    {
        if (change < 0.01f) change = 0.01f;
        if((transform1.localScale.x < 10 || change < 10f) && (transform1.localScale.x > 0.02f || change > 0.01f))
        {
            transform1.localScale = new Vector3(change *10, change *10, change*10);
        }
        
    }

    public void shrink()
    {
        if(transform1.localScale.x > 0.1)
        {
            transform1.localScale = new Vector3(transform1.localScale.x - 0.1f, transform1.localScale.y - 0.1f, transform1.localScale.z - 0.1f);
        }

    }
}
