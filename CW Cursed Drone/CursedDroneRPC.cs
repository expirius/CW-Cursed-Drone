using Photon.Pun;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CW_Cursed_Drone
{
    public class CursedDroneRPC : MonoBehaviourPunCallbacks
    {
        [PunRPC]
        public void RPCA_SpawnCursedDrone(Player __instance)
        {
            Debug.Log($"[Cursed Drone] Log ");

        }
    }
}
