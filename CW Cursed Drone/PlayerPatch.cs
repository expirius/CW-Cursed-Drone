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
            dronePhysics.physicsConfig = new DronePhysicsConfiguration();

            // Находим дочерний объект "Model"
            Transform modelTransform = currentDrone.transform.Find("Model");
            GameObject model = modelTransform.gameObject;

/*            // Добавляем AudioSource к "Model"
            AudioSource audioSource = CursedDroneInfo._bundle.LoadAsset<AudioSource>("MotorSound");
*/
            // Находим дочерний объект "MotorSound"
            Transform motorSoundTransform = currentDrone.transform.Find("MotorSound");
            AudioSource motorSoundSource = motorSoundTransform.GetComponent<AudioSource>();

            // Добавляем DroneAudioBehaviour к "Model"
            DroneSoundBehaviour audioBehaviour = model.AddComponent<DroneSoundBehaviour>();

            audioBehaviour.motorSound = motorSoundSource.clip;
            audioBehaviour.source = motorSoundSource;
            audioBehaviour.dronePhysics = dronePhysics;
            //
            var drone = currentDrone.AddComponent<CursedDroneBehaviour>();
            drone.Initialize(__instance);

            Debug.Log($"[Cursed Drone] droneObj = {dronePrefab.gameObject.name} ");
        }
    }
}