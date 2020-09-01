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
    GameObject[] hands;
    bool currentController = false; //0= false, 1 = true

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
        hands = GameObject.FindGameObjectsWithTag("hand");
        hintScripts = new buttonHintsScript[]{ hands[0].GetComponent<buttonHintsScript>(), hands[1].GetComponent<buttonHintsScript>()};
        hands[0].gameObject.GetComponent<Socket>().Attached.AddListener(attachObjectL);
        hands[1].gameObject.GetComponent<Socket>().Attached.AddListener(attachObjectR);
        hands[0].gameObject.GetComponent<Hand2>().Grow.AddListener(growCalled);
        hands[1].gameObject.GetComponent<Hand2>().Grow.AddListener(growCalled);
        textChanger = this.GetComponent<AddText>();
        
        
    }
    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("trigger up called, current state: " + currentState);
        if(currentState == 3 || currentState == 18 || currentState == 21)
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
        else if(currentState == 8)
        {
            updateHintHighlight(2);
            currentState = 7;
        }
        if(currentState == 9)
        {
            updateHintHighlight(2);
        }
        else if (currentState == 11 || currentState == 22)
        {
            //from here on high jack the state
            updateHintHighlight(3);
            currentState++;
        }
        else if (currentState == 19)
        {
            updateHintHighlight(11);
            currentState++;
        }
        //GrabPinch.RemoveOnStateUpListener(TriggerUp, handType);

    }
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log(currentState);
        if (currentState == 2)
        {
            currentState++;
        }

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
        else if(currentState == 16)
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
        else if(currentState == 2 || currentState == 3 || currentState == 4)
        {
            updateHintHighlight(11);
            currentState = 1;
        }
        else if(currentState == 8)
        {
            UiText.text = "Change the size of an object by holding onto the object with one hand and grabbing it with the other and then pulling.";
            logger.makeLogEntry("changeText", this.gameObject);
            if (currentController)
            {
                hintScripts[1].HideButtonHint(11);
                hintScripts[0].ShowButtonHint(2);
            }
            else
            {
                hintScripts[0].HideButtonHint(11);
                hintScripts[1].ShowButtonHint(2);
            }
            currentState++;
        }
        else if(currentState == 17 || currentState == 20)
        {
            updateHintHighlight(2);
            currentState++;
        }
    }

    public void GripDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (currentState == 6)
        {
            updateHintHighlight(2);
            UiText.text = "Grab an object and press the menu button on the same controller to change the colour of an object.";
            logger.makeLogEntry("changeText", this.gameObject);
            currentState++;
        }
        else if (currentState == 12 || currentState == 13 || currentState == 14)
        {
            textChanger.updateTutorialText();
            currentState++;
        }
        else if (currentState == 15)
        {
            textChanger.updateTutorialText();
            currentState++;
            updateHintHighlight(1);
        }
        else if(currentState == 23)
        {
            currentState++;
            //textChanger.updateTutorialText();
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
        hands[0].gameObject.GetComponent<Socket>().Attached.RemoveListener(attachObjectL);
        hands[1].gameObject.GetComponent<Socket>().Attached.RemoveListener(attachObjectR);
        hands[0].gameObject.GetComponent<Hand2>().Grow.RemoveListener(growCalled);
        hands[1].gameObject.GetComponent<Hand2>().Grow.RemoveListener(growCalled);
    }

    private void attachObjectL()
    {
        Debug.Log("l");
        if (currentState == 4)
        {
            UiText.text = "Release trigger to place an object. Grab an object by pressing trigger to pick it up again.";
            logger.makeLogEntry("changeText", this.gameObject);
            currentState++;
        }
        else if (currentState == 7)
        {
            hintScripts[0].HideButtonHint(2);
            hintScripts[1].HideButtonHint(2);
            hintScripts[0].ShowButtonHint(11);
            currentState++;
        }
        else if (currentState == 9)
        {
            hintScripts[0].HideButtonHint(2);
            hintScripts[1].ShowButtonHint(2);
        }

        currentController = false;
    }

    private void attachObjectR()
    {
        Debug.Log("r");
        if (currentState == 4)
        {
            UiText.text = "Release trigger to place an object. Grab an object by pressing trigger to pick it up again.";
            logger.makeLogEntry("changeText", this.gameObject);
            currentState++;
        }
        else if (currentState == 7)
        {
            hintScripts[0].HideButtonHint(2);
            hintScripts[1].HideButtonHint(2);
            hintScripts[1].ShowButtonHint(11);
            currentState++;
        }
        else if(currentState == 9)
        {
            hintScripts[1].HideButtonHint(2);
            hintScripts[0].ShowButtonHint(2);
        }
        currentController = true;
    }

    private void growCalled()
    {
        if(currentState == 9)
        {
            UiText.text = "Hold an object over your shoulder to delete it";
            logger.makeLogEntry("changeText", this.gameObject);
            if (currentController)
            {
                hintScripts[0].HideButtonHint(2);
                hintScripts[1].ShowButtonHint(2);
            }
            else
            {
                hintScripts[1].HideButtonHint(2);
                hintScripts[0].ShowButtonHint(2);
            }
            currentState++;
            currentState++;
        }
    }
}
