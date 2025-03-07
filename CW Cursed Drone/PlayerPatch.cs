using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Cursed_Drone
{
    [HarmonyPatch(typeof(Player))]
    public class PlayerPatch
    {
        private static GameObject dronePrefab;

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void SpawnDroneOnInput(Player __instance)
        {
            if (__instance != Player.localPlayer ||
                !__instance.refs.view.IsMine ||
                !Input.GetKeyDown(DroneSettings.SummonKey))
                return;

            if (dronePrefab == null)
                dronePrefab = Resources.Load<GameObject>("DronePrefab");
            
            Vector3 spawnPos = __instance.transform.position + Vector3.up * 3f;
            PhotonNetwork.Instantiate(dronePrefab.name, spawnPos, Quaternion.identity);

        }
    }
}
