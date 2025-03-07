using HarmonyLib;
using Photon.Pun;
using UnityEngine;
using Zorro.Settings;
using Zorro.Settings.UI;

namespace CW_Cursed_Drone
{
    [ContentWarningSetting]
    public class DroneKeySetting : KeyCodeSetting, IExposedSetting
    {
        public SettingCategory GetSettingCategory() => SettingCategory.Mods;
        public string GetDisplayName() => "Drone Summon Key";
        protected override KeyCode GetDefaultKey() => KeyCode.G;
    }
    public static class DroneSettings
    {
        private static DroneKeySetting _keySetting;
        public static KeyCode SummonKey => _keySetting?.Keycode() ?? KeyCode.G;
    }

}
