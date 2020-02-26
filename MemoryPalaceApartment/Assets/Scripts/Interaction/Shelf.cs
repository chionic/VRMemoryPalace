using UnityEngine;
using Valve.VR;

public class Shelf : MonoBehaviour
{
    [Range(0.5f, 0.75f)]
    public float height = 0.5f;

    private Transform head = null;

    private void Start()
    {
        head = SteamVR_Render.Top().head;
    }

    private void Update()
    {
        PositionUnderHead();
        TransformRelativePlayer();
    }

    private void PositionUnderHead()
    {
        Vector3 adjustedHeight = head.localPosition;
        adjustedHeight.y = Mathf.Lerp(0.0f, adjustedHeight.y, height);
        transform.localPosition = adjustedHeight;

    }

    private void TransformRelativePlayer()
    {
        Vector3 headPosition = head.localPosition;
        headPosition.y = Mathf.Lerp(0.0f, headPosition.y, height);
        transform.localPosition = new Vector3(headPosition.x - 1, headPosition.y, headPosition.z - 1);
    }

    
}
