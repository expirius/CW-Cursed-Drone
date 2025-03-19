using DefaultNamespace.Artifacts;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Object = UnityEngine.Object;

namespace CW_Cursed_Drone
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Update")]
    public class PlayerPatch
    {
        public static DroneKeySetting SpawnButton = GameHandler.Instance.SettingsHandler.GetSetting<DroneKeySetting>();

        [HarmonyPostfix]
        public static void SpawnDroneOnInput(Player __instance)
        {
            bool flag = Input.GetKeyDown(SpawnButton.Keycode()); //&& Player.localPlayer.data.dead && PhotonGameLobbyHandler.IsSurface;
            if (flag)
            {
                Debug.Log($"[Cursed Drone] Pressed Summon key {SpawnButton.Keycode()}");
                CreateDrone(__instance);

            }
        }

        static GameObject dronePrefab; // DELETE
        static GameObject currentDrone;// deleete
        static void CreateDrone(Player __instance) // DELETE
        {

            Debug.Log("[Cursed Drone] RPCA_SpawnCursedDrone is invoked");


            // Проверяем загружен ли префаб
            if (dronePrefab == null)
            {
                dronePrefab = CursedDroneInfo._bundle.LoadAsset<GameObject>("AltitudeHoldQuad");
                Debug.Log($"[Cursed Drone] Prefab is {dronePrefab.ToString()}!");

                var rb = dronePrefab.AddComponent<Rigidbody>();
                rb.useGravity = false; // Отключаем гравитацию
                rb.drag = 1.5f; // Добавляем сопротивление для плавности
                dronePrefab.name = "Cursed Drone";
            }

            var cameraTransform = __instance.refs.cameraPos.transform;

            currentDrone = Object.Instantiate(
                dronePrefab,
                cameraTransform.position + cameraTransform.forward * 2f,
                Quaternion.identity
            );

            // Добавляем компонент для управления движением
            var controller = currentDrone.AddComponent<CursedDroneBehaviour>();
            controller.Initialize(__instance);

            Debug.Log($"[Cursed Drone] droneObj = {controller.gameObject.name} ");
        }
    }
}