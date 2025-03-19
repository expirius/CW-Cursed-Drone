using System.Reflection;
using UnityEngine;

namespace CW_Cursed_Drone
{
    [ContentWarningPlugin("Cursed Drone", "1.0.0", false)]
    public class CursedDroneInfo
    {
        public static AssetBundle _bundle;

        static CursedDroneInfo()
        {
            LoadAssetBundle();
            Debug.Log($"[{nameof(CursedDroneInfo)}] loaded!");
        }
        static string PluginDirPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }
        private static void LoadAssetBundle()
        {
            CursedDroneInfo._bundle = AssetBundle.LoadFromFile(Path.Join(CursedDroneInfo.PluginDirPath, "curseddrone"));
        }
    }
}
