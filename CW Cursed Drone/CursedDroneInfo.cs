using UnityEngine;

namespace CW_Cursed_Drone
{
    [ContentWarningPlugin("Cursed Drone", "1.0.0", false)]
    public class CursedDroneInfo
    {
        static CursedDroneInfo() => Debug.Log($"[{nameof(CursedDroneInfo)}] loaded!");
    }
}
