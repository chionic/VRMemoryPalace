//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Demonstrates the use of the controller hint system
//
//=============================================================================

using UnityEngine;
using System.Collections;
using Valve.VR;

namespace Valve.VR.InteractionSystem.Sample
{
	//-------------------------------------------------------------------------
	public class ControllerHintsExample : MonoBehaviour
	{
		private Coroutine buttonHintCoroutine;
		private Coroutine textHintCoroutine;

		//-------------------------------------------------
		public void ShowButtonHints( Hand hand )
		{
			if ( buttonHintCoroutine != null )
			{
				StopCoroutine( buttonHintCoroutine );
				Debug.Log("stop coroutine called");
			}
			buttonHintCoroutine = StartCoroutine( TestButtonHints( hand ) );
			Debug.Log("showButtonHints called");
		}
		private void Start()
		{
			ShowButtonHints(this.GetComponent<Hand>());
		}


		//-------------------------------------------------
		public void ShowTextHints( Hand hand )
		{
			if ( textHintCoroutine != null )
			{
				StopCoroutine( textHintCoroutine );
			}
			textHintCoroutine = StartCoroutine( TestTextHints( hand ) );
		}


		//-------------------------------------------------
		public void DisableHints()
		{
			if ( buttonHintCoroutine != null )
			{
				StopCoroutine( buttonHintCoroutine );
				buttonHintCoroutine = null;
			}

			if ( textHintCoroutine != null )
			{
				StopCoroutine( textHintCoroutine );
				textHintCoroutine = null;
			}

			foreach ( Hand hand in Player.instance.hands )
			{
				ControllerButtonHints.HideAllButtonHints( hand );
				ControllerButtonHints.HideAllTextHints( hand );
			}
		}


		//-------------------------------------------------
		// Cycles through all the button hints on the controller
		//-------------------------------------------------
		private IEnumerator TestButtonHints( Hand hand )
		{
			Debug.Log("test button hints started");
			ControllerButtonHints.HideAllButtonHints( hand );

			while ( true )
            {
                for (int actionIndex = 0; actionIndex < SteamVR_Input.actionsIn.Length; actionIndex++)
                {
                    ISteamVR_Action_In action = SteamVR_Input.actionsIn[actionIndex];
					Debug.Log(SteamVR_Input.actionsIn[actionIndex]);
                    if (action.GetActive(hand.handType))
                    {
                        ControllerButtonHints.ShowButtonHint(hand, action);
						Debug.Log("ran controller button hints....");
						yield return new WaitForSeconds(1.0f);
                        ControllerButtonHints.HideButtonHint(hand, action);
                        yield return new WaitForSeconds(0.5f);
                    }
                    yield return null;
                }

				ControllerButtonHints.HideAllButtonHints( hand );
				yield return new WaitForSeconds( 1.0f );
			}
		}


		//-------------------------------------------------
		// Cycles through all the text hints on the controller
		//-------------------------------------------------
		private IEnumerator TestTextHints( Hand hand )
		{
			ControllerButtonHints.HideAllTextHints( hand );

			while ( true )
            {
                for (int actionIndex = 0; actionIndex < SteamVR_Input.actionsIn.Length; actionIndex++)
                {
                    ISteamVR_Action_In action = SteamVR_Input.actionsIn[actionIndex];
                    if (action.GetActive(hand.handType))
                    {
                        ControllerButtonHints.ShowTextHint(hand, action, action.GetShortName());
						
                        yield return new WaitForSeconds(3.0f);
                        ControllerButtonHints.HideTextHint(hand, action);
                        yield return new WaitForSeconds(0.5f);
                    }
                    yield return null;
                }

                ControllerButtonHints.HideAllTextHints(hand);
                yield return new WaitForSeconds(3.0f);
			}
		}
	}
}
