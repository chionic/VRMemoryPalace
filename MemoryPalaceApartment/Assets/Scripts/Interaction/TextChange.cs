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

    private string[] tutorialText = new string[] { "", "Here is the first instruction!",
                                                   "Here is the second instruction!",
                                                   "HERE IS THE LAST INSTRUCTION?!"};
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
            arrayIndex = 0;
        }
        arrayIndex++;
        UiText.text = sArray[arrayIndex];
    }
}
