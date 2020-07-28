using UnityEngine;

public class resizeObject : MonoBehaviour
{
    float result = Mathf.Lerp(3f, 5f, 0.5f);
    Transform transform1;
    float defaultScale;

    // Start is called before the first frame update
    void Start()
    {
        transform1 = this.transform;
        defaultScale = transform1.localScale.x;
    }

    public void grow(float change)
    {
        if (change < 0.01f) change = 0.01f;
        if((transform1.localScale.x < 10 || change < 10f) && (transform1.localScale.x > 0.02f || change > 0.01f))
        {
            transform1.localScale = new Vector3(change *10 * defaultScale, change *10 * defaultScale, change*10 * defaultScale);
        }
        
    }

    public void baseScale()
    {
        defaultScale = transform1.localScale.x;
    }
}
