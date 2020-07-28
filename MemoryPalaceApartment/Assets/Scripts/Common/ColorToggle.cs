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
    private MakeLog logger;
    private int i = 0;

    private void Start()
    {
        logger = GameObject.FindWithTag("logger").GetComponent<MakeLog>();
        colors = new Color[] { Color.white, Color.green, Color.red, Color.blue, Color.black, Color.clear, Color.cyan, Color.magenta, Color.grey };
    }
    public void ToggleColor()
    {
        MeshRenderer[] renderers = this.GetComponentsInChildren<MeshRenderer>();
        logger.makeLogEntry("colourChange", this.gameObject, ColorUtility.ToHtmlStringRGBA(colors[i])); //.ToString("f2"));
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
