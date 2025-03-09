using HarmonyLib;
using Photon.Pun;
using System.Reflection;
using System.Text;
using UnityEngine;
using Zorro.Settings;

namespace CW_Cursed_Drone
{
    //[ContentWarningPlugin("expirius.cursedrone", "1.0", false)]
    public class DronePlugin
    {
        private static AssetBundle _bundle;
        public static GameObject _dronePrefab;
        public static string PluginDirPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }
        static DronePlugin()
        {
            LoadAssetBundle(); // Загрузка бандла при инициализации
            Debug.Log("[Cursed Drone] initialising via the vanilla mod loader.");
            Initialise(); // Регистрация настроек
        }
        public static void InitialiseSettings()
        {
            DronePlugin.InitialiseGameSetting<DroneKeySetting>();
            Debug.Log("[Cursed Drone] expirius.cursedrone settings initialised.");
        }
        private static void InitialiseGameSetting<T>() where T : Setting, new()
        {
            if (GameHandler.Instance.SettingsHandler.GetSetting<T>() == null)
            {
                GameHandler.Instance.SettingsHandler.AddSetting(new T());
            }
        }
        private static void Initialise()
        {
            _bundle = DronePlugin.LoadAssetBundle();
            if (_bundle == null)
            {
                Debug.LogError("[Cursed Drone] Bundle loading failed!");
                return;
            }
            //_dronePrefab = _bundle.GetAllAssetNames().Last();
            _dronePrefab = _bundle.LoadAsset<GameObject>("assets/drone/prefab/drone.prefab");

            Debug.Log($"[Cursed Drone] Prefab loaded: {_dronePrefab != null}");
        }
        public static AssetBundle LoadAssetBundle()
        {
            Debug.Log($"[Cursed Drone] LoadAssetBundle: active directory {DronePlugin.PluginDirPath}");
            return AssetBundle.LoadFromFile(Path.Join(DronePlugin.PluginDirPath, "dronebundle"));
        }
    }
}
