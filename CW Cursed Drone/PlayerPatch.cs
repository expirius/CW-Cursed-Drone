using HarmonyLib;
using UnityEngine;

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
            // Проверяем загружен ли префаб
            if (dronePrefab == null)
            {
                dronePrefab = CursedDroneInfo._bundle.LoadAsset<GameObject>("Drone");
                Debug.Log($"[Cursed Drone] Prefab is {dronePrefab.ToString()}!");
                dronePrefab.name = "Cursed Drone";
            }

            var cameraTransform = __instance.refs.cameraPos.transform;
            
            // Создаем экземпляр объекта
            currentDrone = GameObject.Instantiate(
                dronePrefab,
                cameraTransform.position + cameraTransform.forward * 2f,
                Quaternion.identity
            );

            var dronePhysics = currentDrone.AddComponent<DronePhysicsBehaviour>();
            
            // Находим дочерний объект "Model"
            Transform modelTransform = currentDrone.transform.Find("Model");
            GameObject model = modelTransform.gameObject;

            // Добавляем AudioSource к "Model"
            AudioSource audioSource = model.AddComponent<AudioSource>();
            audioSource.clip = CursedDroneInfo._bundle.LoadAsset<AudioClip>("MotorSound");

            // Добавляем DroneAudioBehaviour к "Model"
            DroneSoundBehaviour audioBehaviour = model.AddComponent<DroneSoundBehaviour>();
            audioBehaviour.motorSound = CursedDroneInfo._bundle.LoadAsset<AudioClip>("MotorSound");
            audioBehaviour.source = audioSource;
            audioBehaviour.dronePhysics = dronePhysics;
            //
            var drone = currentDrone.AddComponent<CursedDroneBehaviour>();
            drone.Initialize(__instance);

            Debug.Log($"[Cursed Drone] droneObj = {dronePrefab.gameObject.name} ");
        }
    }
}