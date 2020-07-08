using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ColorToggle : MonoBehaviour
{
    //public Material PinkMaterial = null;
    //public Material GreyMaterial = null;
    //public string[] materialsArray;
    private Color[] colors;

    private int i = 0;

    private void Start()
    {
        colors = new Color[] { Color.white, Color.green, Color.red, Color.blue, Color.black, Color.clear, Color.cyan, Color.magenta, Color.grey };
    }
    public void ToggleColor()
    {
        MeshRenderer[] renderers = this.GetComponentsInChildren<MeshRenderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material m in renderer.materials)
            {
                m.color = colors[i];
                i++;
                if (i >= colors.Length) i = 0;
            }
        }
        
        
    }
}
