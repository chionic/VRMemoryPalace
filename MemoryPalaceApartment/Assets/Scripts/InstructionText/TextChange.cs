using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChange : MonoBehaviour
{
    public Text UiText = null;
    public int arrayIndex = 1;
    public string arrayName;
    private string[] peacePrize = new string[] { "", "Sirleaf \n for their non-violent struggle for the safety of women and for women's rights to full participation in peace-building work",
                                                 "Liu \n for his long and non-violent struggle for fundamental human rights in China",
                                                 "Yunus \n for their efforts to create economic and social development from below",
                                                 "Arafat \n for their efforts to create peace in the Middle East",
                                                 "Ebadi \n for her efforts for democracy and human rights. She has focused especially on the struggle for the rights of women and children",
                                                 "Santos \n for his resolute efforts to bring the country's (colombia) more than 50-year-long civil war to an end",
                                                 "Maathai \n for her contribution to sustainable development, democracy and peace"};

    private string[] physicsPrize = new string[] { "","de Broglie \n for his discovery of the wave nature of electrons",
                                                   
                                                   "Chadwick \n for the discovery of the neutron",
                                                   "Anderson \n for his discovery of the positron",
                                                   "Wien \n for his discoveries regarding the laws governing the radiation of heat",
                                                   "Gabor \n for his invention and development of the holographic method",
                                                   "Ruska \n for his fundamental work in electron optics, and for the design of the first electron microscope"};

    private string[] tutorialText = new string[] { "Press grab grip to change text",
                                                   "Use the circle pad to teleport. \n Move your thumb on the pad to pick where to teleport to. Press down on the pad to teleport.",
                                                   "Teleport points can be used to get to higher floors.",
                                                   "Press the menu button to bring up the menu, press it again to close the menu.",
                                                   "Hover over an object and press trigger to select it. The menu will bring up a submenu from which you can pick objects.",
                                                   "Release trigger to drop an object",
                                                   "Hover over a dropped object and press trigger to pick up an interactable object again.",
                                                   "Hold an object over your shoulder to delete it",
                                                   "A memory palace is a way to remember ideas and concepts by using objects to represent information in a space.",
                                                   "Complete the tutorial by following the example of representing information with objects:",
                                                   "We want to represent: \n \n Hess \n for his discovery of cosmic radiation",
                                                   "The name Hess rhymes with mess, so lets make a mess by placing a bunch of objects on the green pedestal",
                                                   "And he discovered cosmic radiation. We can use a sun to represent cosmic since it is in space. Lets put it high up in space on the pink pedestal.",
                                                   "Radiation can be represented with the green radiation warning symbol. Lets place it on the bright blue pedestal.",
                                                   "Try to remember the names and achievements of the following Nobel Prize Winners,",
                                                    "by placing objects to represent them along a path through the apartment."};
    // Start is called before the first frame update
    void Awake()
    {
        UiText = this.GetComponent<Text>();

    }

    //helper function while no vr controls for testing purposes, changes text when space key is pressed
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            updateUIText();
        }
    }

    public void updateUIText()
    {
        if (arrayName.Equals("peacePrize"))
        {
            changeText(peacePrize);
        }
        else if (arrayName.Equals("physicsPrize"))
        {
            changeText(physicsPrize);
        }
        else
        {
            changeText(tutorialText);
        }
    }

    //Change text to next piece of text
    private void changeText(string[] sArray)
    {
        if (arrayIndex >= sArray.Length - 1)
        {
            if (sArray.Equals(tutorialText))
            {
                arrayName = "peacePrize";
                sArray = peacePrize;
                arrayIndex = 0;
            }
            arrayIndex = 0;
        }
        arrayIndex++;
        UiText.text = sArray[arrayIndex];
    }
}
