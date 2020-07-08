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

    public void grow()
    {
        if(transform1.localScale.x < 10)
        {
            transform1.localScale = new Vector3(transform1.localScale.x + 0.1f, transform1.localScale.y + 0.1f, transform1.localScale.z + 0.1f);
        }
        
    }

    public void shrink()
    {
        if(transform1.localScale.x > 0.002)
        {
            transform1.localScale = new Vector3(transform1.localScale.x - 0.1f, transform1.localScale.y - 0.1f, transform1.localScale.z - 0.1f);
        }

    }
}
