//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Adding this component to an object will allow the player to
//			initiate teleporting while that object is attached to their hand
//
//=============================================================================

using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	public class NoTeleportWhileObjectHeld : MonoBehaviour
	{
		public bool teleportAllowed = false;
		public bool overrideHoverLock = false;
	}
}
