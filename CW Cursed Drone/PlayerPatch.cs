using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Text;
using UnityEngine;

namespace CW_Cursed_Drone
{
    [HarmonyPatch(typeof(Player))]
    public class PlayerPatch
    {
        private static Drone drone;

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void SpawnDroneOnInput(Player __instance)
        {
            if (__instance != Player.localPlayer ||
                !__instance.refs.view.IsMine ||
                !Input.GetKeyDown(DroneSettings.SummonKey))
                return;
            // Получаем компонент Drone на этом же объекте
            Drone drone = __instance.GetComponent<Drone>();
            if (drone == null) return;
            // Вызываем RPC через PhotonView игрока
            __instance.refs.view.RPC(
                "RPCA_AttachDrone",
                RpcTarget.AllViaServer,
                __instance.refs.view.OwnerActorNr); // передаем ID игрока
        }
    }
}
