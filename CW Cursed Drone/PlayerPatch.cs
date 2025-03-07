using HarmonyLib;
using Photon.Pun;
using System.Reflection;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace CW_Cursed_Drone
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Update")]
    public class PlayerPatch
    {
        [HarmonyPostfix]
        public static void SpawnDroneOnInput(Player __instance)
        {
            // проверяем нажата ли кнопка
            if (!GlobalInputHandler.GetKeyDown(DroneKeySetting.SummonKey))
                return;
            Debug.Log($"[Cursed Drone] Pressed Summon key {DroneKeySetting.SummonKey}");
            DronePlugin drone = new();
            //Debug.Log($"[Cursed Drone] drone name: {_currentDrone.name} spawn pos: {spawnPos}");
        }
    }
}
