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

    // a reference to the hand
    public SteamVR_Input_Sources handType;

    //The unity gameobject component that displays the text on screen
    public Text UiText = null;

    void Start()
    {
        GrabPinch.AddOnStateDownListener(TriggerDown, handType);
        GrabPinch.AddOnStateUpListener(TriggerUp, handType);
        GameObject[] hands = GameObject.FindGameObjectsWithTag("hand");
        hintScripts = new buttonHintsScript[]{ hands[0].GetComponent<buttonHintsScript>(), hands[1].GetComponent<buttonHintsScript>()};
        UiText.text = "Reach out and grab an object by pressing trigger to select it.";
        foreach (buttonHintsScript hintScript in hintScripts) hintScript.ShowButtonHint(2);
    }
    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        foreach (buttonHintsScript hintScript in hintScripts) hintScript.HideButtonHint(2);
        GrabPinch.RemoveOnStateUpListener(TriggerUp, handType);

    }
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        //hintScript.HideButtonHint(2);
        GrabPinch.RemoveOnStateDownListener(TriggerDown, handType);
        UiText.text = "Release trigger to drop an object.";
    }
}
