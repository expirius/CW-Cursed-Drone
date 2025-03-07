using HarmonyLib;
using Photon.Pun;
using UnityEngine;
using Zorro.Settings;
using Zorro.Settings.UI;

namespace Cursed_Drone
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

        [HarmonyPatch(typeof(SettingsHandler))]
        private static class SettingsRegisterPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch("Initialize")]
            static void Register(SettingsHandler __instance)
            {
                _keySetting = new DroneKeySetting();
                __instance.AddSetting(_keySetting);
            }
        }
    }

}
