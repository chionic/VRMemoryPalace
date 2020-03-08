using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChange : MonoBehaviour
{
    public Text UiText = null;
    public int arrayIndex = 0;
    public string arrayName;
    private string[] peacePrize = new string[] { "", "Sirleaf \n for their non-violent struggle for the safety of women and for women's rights to full participation in peace-building work",
                                                 "Liu \n for his long and non-violent struggle for fundamental human rights in China",
                                                 "Yunus \n for their efforts to create economic and social development from below",
                                                 "Arafat \n for their efforts to create peace in the Middle East",
                                                 "Ebadi \n for her efforts for democracy and human rights. She has focused especially on the struggle for the rights of women and children",
                                                 "Santos \n for his resolute efforts to bring the country's (colombia) more than 50-year-long civil war to an end",
                                                 "Maathai \n for her contribution to sustainable development, democracy and peace"};

    private string[] physicsPrize = new string[] { "","de Broglie \n for his discovery of the wave nature of electrons",
                                                   "Hess \n for his discovery of cosmic radiation",
                                                   "Chadwick \n for the discovery of the neutron",
                                                   "Anderson \n for his discovery of the positron",
                                                   "Wien \n for his discoveries regarding the laws governing the radiation of heat",
                                                   "Gabor \n for his invention and development of the holographic method",
                                                   "Ruska \n for his fundamental work in electron optics, and for the design of the first electron microscope"};

    private string[] tutorialText = new string[] { "Press grab grip to change text",
                                                   "Use the circle pad to teleport. \n Mov eyour thumb on the pad to pick where to teleport to. Press down on the pad to teleport.",
                                                   "Teleport points can be used to get to higher floors.",
                                                   "Press the menu button to bring up the menu, press it again to close the menu.",
                                                   "Hover over an object and press trigger to select it. The menu will bring up a submenu from which you can pick objects.",
                                                   "Release trigger to drop an object",
                                                   "Hover over a dropped object and press trigger to pick up an interactable object again.",
                                                   "Hold an object over your shoulder to delete it",
                                                   "Complete the tutorial by following the instructions:",
                                                   "Place the gold coin on the green pedestal",
                                                   "Place the top hat on the pink pedestal",
                                                   "Place the smiley face on the bright blue pedestal",
                                                   "A memory palace works by using objects to represent information in a space.",
                                                   "Try to remember the names and achievements of the following Nobel Prize Winners by placing objects to represent them along a path through the apartment."};
    // Start is called before the first frame update
    void Awake()
    {
        UiText = this.GetComponent<Text>();

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
        if (arrayIndex > sArray.Length)
        {
            if (arrayName.Equals(tutorialText))
            {
                arrayName = "peacePrize";
                sArray = peacePrize;
            }
            arrayIndex = 0;
        }
        arrayIndex++;
        UiText.text = sArray[arrayIndex];
    }
}
