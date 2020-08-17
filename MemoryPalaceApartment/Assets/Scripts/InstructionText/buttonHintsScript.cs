//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Demonstrates the use of the controller hint system
//
//=============================================================================

using UnityEngine;
using System.Collections;
using Valve.VR;
using System;

namespace Valve.VR.InteractionSystem.Sample
{
	//-------------------------------------------------------------------------
	public class buttonHintsScript : MonoBehaviour
	{
		private Coroutine buttonHintCoroutine;
		private IEnumerator textHintCoroutine;
		private Boolean highlightButton = true;

		//-------------------------------------------------
		/*private void Start()
		{
			ShowButtonHint(2);
		}*/

			public void startHint(int index)
		{
			ShowButtonHint(index);
		}

		//-------------------------------------------------

		public void ShowButtonHint(int index)
		{
			
			if (textHintCoroutine != null || !highlightButton)
			{
				StopCoroutine(textHintCoroutine);
				Debug.Log("stop coroutine called");
			}
			textHintCoroutine = TestButtonHint(this.GetComponent<Hand>(), index);
			StartCoroutine(textHintCoroutine);
			Debug.Log("showButtonHints called");
		}

		public void HideButtonHint(int index)
		{
			StopAllCoroutines();
			ControllerButtonHints.HideButtonHint(this.GetComponent<Hand>(), SteamVR_Input.actionsIn[index]);
			Debug.Log("hide button hint called");
		}

		//-------------------------------------------------
		public void DisableHints()
		{
			if (buttonHintCoroutine != null)
			{
				StopCoroutine(buttonHintCoroutine);
				buttonHintCoroutine = null;
			}

			if (textHintCoroutine != null)
			{
				StopCoroutine(textHintCoroutine);
				textHintCoroutine = null;
			}

			foreach (Hand hand in Player.instance.hands)
			{
				ControllerButtonHints.HideAllButtonHints(hand);
				ControllerButtonHints.HideAllTextHints(hand);
			}
		}


		//-------------------------------------------------
		// Highlights a button hint on the controller
		//-------------------------------------------------
		private IEnumerator TestButtonHint(Hand hand, int actionIndex)
		{
			ControllerButtonHints.HideAllButtonHints(hand);
			while (true)
			{
				
					ISteamVR_Action_In action = SteamVR_Input.actionsIn[actionIndex];
					if (action.GetActive(hand.handType))
					{
						ControllerButtonHints.ShowButtonHint(hand, action);
						Debug.Log("ran controller button hint...." + actionIndex);
						yield return new WaitForSeconds(1.0f);
				}
					yield return null;

				ControllerButtonHints.HideAllButtonHints(hand);
				yield return new WaitForSeconds(0.01f);
			}
		}

	}
}
