using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class ListenTeleport : MonoBehaviour
    {
        public GameObject player1;
        private MakeLog logger;

        // Start is called before the first frame update
        void Start()
        {
            Teleport.Player.AddListener(printIt);
            logger = GameObject.FindWithTag("logger").GetComponent<MakeLog>();
            player1 = GameObject.FindGameObjectWithTag("MainCamera");
            //InvokeRepeating("makeLog", 1.0f, 1.0f);
        }

        void printIt(TeleportMarkerBase x)
        {
            logger.makeLogEntry("teleport", x);
            
        }

    }

}
