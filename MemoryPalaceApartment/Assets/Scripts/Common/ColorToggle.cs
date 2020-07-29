using UnityEngine;

public class ColorToggle : MonoBehaviour
{
    private Color[] colors; //array of preselected colours
    private MakeLog logger; //logging script to send colour change events to
    private int i = 0;

    private void Start()
    {
        logger = GameObject.FindWithTag("logger").GetComponent<MakeLog>();
        //add colours to array to add them to possible selections
        colors = new Color[] { Color.green, Color.red, Color.blue, Color.yellow, Color.black, Color.clear, Color.cyan, Color.magenta, Color.grey };
    }
    public void ToggleColor() //cycles through the colours and adds them to the image
    {
        MeshRenderer[] renderers = this.GetComponentsInChildren<MeshRenderer>(); //gets all the mesh renderers of the object the script is attached to
        logger.makeLogEntry("colourChange", this.gameObject, ColorUtility.ToHtmlStringRGBA(colors[i])); //logs the colour change
        foreach (Renderer renderer in renderers) //for every renderer
        {       
            foreach (Material m in renderer.materials) //goes through the list of its materials
            {
                m.color = colors[i]; //changes the colour of the material to the ith colour in the array
                i++; //picks the next colour in the array
                if (i >= colors.Length) i = 0; //resets the array to the start when the end is reached
            }
        }
        
        
    }
}
