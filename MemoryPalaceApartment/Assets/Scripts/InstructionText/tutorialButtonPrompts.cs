using System.Data.SqlClient;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem.Sample;

public class tutorialButtonPrompts : MonoBehaviour
{
    public SteamVR_Action_Boolean Teleport;
    public SteamVR_Action_Boolean GrabPinch;
    public SteamVR_Action_Boolean GrabGrip;
    public SteamVR_Action_Boolean PressMenuButton;

    private buttonHintsScript[] hintScripts;
    private int currentState = 0; //0 - teleport, 1 - menu, 2-5 - trigger, 6 - text change, 7 - trigger (delete), 8-11 - text change
    private AddText textChanger;
    private MakeLog logger; //game object that has logging interface


    // a reference to the hand
    public SteamVR_Input_Sources handType;

    //The unity gameobject component that displays the text on screen
    public Text UiText = null;

    void Start()
    {
        logger = GameObject.FindWithTag("logger").GetComponent<MakeLog>();
        GrabPinch.AddOnStateDownListener(TriggerDown, handType);
        GrabPinch.AddOnStateUpListener(TriggerUp, handType);
        Teleport.AddOnStateDownListener(TeleportDown, handType);
        PressMenuButton.AddOnStateDownListener(MenuDown, handType);
        GrabGrip.AddOnStateDownListener(GripDown, handType);
        GameObject[] hands = GameObject.FindGameObjectsWithTag("hand");
        hintScripts = new buttonHintsScript[]{ hands[0].GetComponent<buttonHintsScript>(), hands[1].GetComponent<buttonHintsScript>()};
        textChanger = this.GetComponent<AddText>();
        
    }
    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("trigger up called, current state: " + currentState);
        if(currentState == 3 || currentState == 14 || currentState == 17)
        {
            currentState++;
        }
        else if(currentState == 5)
        {
            updateHintHighlight(3);
            UiText.text = "Press the grab grip button to change the text on the tablet.";
            logger.makeLogEntry("changeText", this.gameObject);
            currentState++;
        }
        else if (currentState == 7 || currentState == 18)
        {
            updateHintHighlight(3);
            currentState++;
        }
        else if (currentState == 15)
        {
            updateHintHighlight(11);
            currentState++;
        }
        //GrabPinch.RemoveOnStateUpListener(TriggerUp, handType);

    }
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if(currentState == 2)
        {
            currentState++;
        }
        if (currentState == 4)
        {
            GrabPinch.RemoveOnStateDownListener(TriggerDown, handType);
            UiText.text = "Release trigger to place an object. Grab an object by pressing trigger to pick it up again.";
            logger.makeLogEntry("changeText", this.gameObject);
            currentState++;
        }
        //hintScript.HideButtonHint(2);
        
    }

    public void TeleportDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if(currentState == 0)
        {
            UiText.text = "Press the menu button to open the menu.";
            logger.makeLogEntry("changeText", this.gameObject);
            foreach (buttonHintsScript hintScript in hintScripts) hintScript.ShowButtonHint(11);
            currentState = 1;
        }
        else if(currentState == 12)
        {
            updateHintHighlight(11);
            currentState++;
        }
    }

    public void MenuDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (currentState == 1)
        {
            UiText.text = "Reach out and grab an object in the menu by pressing trigger to select a submenu. Grab an object in a submenu to select it.";
            logger.makeLogEntry("changeText", this.gameObject);
            updateHintHighlight(2);
            currentState++;
        }
        else if(currentState == 13 || currentState == 16)
        {
            updateHintHighlight(2);
            currentState++;
        }
    }

    public void GripDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (currentState == 6)
        {
            UiText.text = "Hold an object over your shoulder to delete it";
            logger.makeLogEntry("changeText", this.gameObject);
            updateHintHighlight(2);
            currentState++;
        }
        else if (currentState == 8 || currentState == 9 || currentState == 10)
        {
            textChanger.updateTutorialText();
            currentState++;
        }
        else if (currentState == 11)
        {
            textChanger.updateTutorialText();
            currentState++;
            updateHintHighlight(1);
        }
        else if(currentState == 19)
        {
            currentState++;
            textChanger.updateTutorialText();
            textChanger.isTutorial = false;
            foreach (buttonHintsScript hintScript in hintScripts)
            {
                hintScript.HideButtonHint(3);
            }
            removeAllListeners();
        }
    }

    private void updateHintHighlight(int index)
    {
        foreach (buttonHintsScript hintScript in hintScripts)
        {
            hintScript.HideButtonHint(index);
            hintScript.ShowButtonHint(index);
        }
    }

    private void removeAllListeners()
    {
        GrabPinch.RemoveOnStateUpListener(TriggerUp, handType);
        GrabPinch.RemoveOnStateDownListener(TriggerDown, handType);
        Teleport.RemoveOnStateDownListener(TeleportDown, handType);
        PressMenuButton.RemoveOnStateDownListener(MenuDown, handType);
        GrabGrip.RemoveOnStateDownListener(GripDown, handType);
    }
}
