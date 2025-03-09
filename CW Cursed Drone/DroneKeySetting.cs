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

        public override int GetDefaultValue()
        {
            return (int)this.GetDefaultKey();
        }
        public override void ApplyValue()
        {
            Debug.Log("Set Mode Key to " + base.Value.ToString());
        }
        protected override KeyCode GetDefaultKey()
        {
            return KeyCode.G;
        }
    }
}
