using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

//This is used with steamVR to assign player actions when buttons are pressed on the controller.
public class InputManager : MonoBehaviour
{

    [Header("Trigger")]
    public SteamVR_Action_Boolean TriggerAction = null;
    public UnityEvent OnTriggerDown = new UnityEvent();
    public UnityEvent OnTriggerUp = new UnityEvent();


    [Header("Menu Button")]
    public SteamVR_Action_Boolean MenuButtonAction = null;
    public UnityEvent OnMenuButtonDown = new UnityEvent();
    public UnityEvent OnMenuButtonUp = new UnityEvent();

    [Header("Grab Grip")]
    public SteamVR_Action_Boolean GrabGripAction = null;
    public UnityEvent GrabGripDown = new UnityEvent();
    public UnityEvent GrabGripUp = new UnityEvent();


    private SteamVR_Behaviour_Pose Pose = null;

    private void Awake()
    {
        Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Update()
    {
        if (TriggerAction.GetStateDown(Pose.inputSource))
            OnTriggerDown.Invoke();

        if (TriggerAction.GetStateUp(Pose.inputSource))
            OnTriggerUp.Invoke();


        //if (TouchpadAction.GetAxis(Pose.inputSource) > 0)
        //{
        //    OnTempDown.Invoke();
        //    Debug.Log("Touch pad 2 called");
        //}
        //if (TouchpadAction.GetAxis(Pose.inputSource) < 0)
        //{
        //    OnTempUp.Invoke();
        //}


        //if (Temp.GetAxis(Pose.inputSource) > 0)
        //{
        //    OnTempDown.Invoke();
        //    Debug.Log("Touch pad 2 called");
        //}
        //if (Temp.GetAxis(Pose.inputSource) < 0)
        //{
        //    OnTempUp.Invoke();
        //}

        if (MenuButtonAction.GetStateDown(Pose.inputSource))
            OnMenuButtonDown.Invoke();

        if (MenuButtonAction.GetStateUp(Pose.inputSource))
            OnMenuButtonUp.Invoke();

        if (GrabGripAction.GetStateDown(Pose.inputSource))
            GrabGripDown.Invoke();

        if (GrabGripAction.GetStateUp(Pose.inputSource))
           GrabGripUp.Invoke();
    }

}
