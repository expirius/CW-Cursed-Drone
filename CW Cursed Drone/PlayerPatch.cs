using DefaultNamespace.Artifacts;
using HarmonyLib;
using Photon.Pun;
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
        private GameObject droneObject;
        private PhotonView m_PhotonView;
        public static DroneKeySetting SpawnButton = GameHandler.Instance.SettingsHandler.GetSetting<DroneKeySetting>();

        [HarmonyPostfix]
        public static void SpawnDroneOnInput(Player __instance)
        {
            bool flag = Input.GetKeyDown(SpawnButton.Keycode()); //&& Player.localPlayer.data.dead && PhotonGameLobbyHandler.IsSurface;
            if (flag)
            {
                Debug.Log($"[Cursed Drone] Pressed Summon key {SpawnButton.Keycode()}");

                //Player.localPlayer.CallRevive();

                this.m_PhotonView.RPC("RPCA_SpawnCursedDrone", RpcTarget.All);

                Object.Instantiate<GameObject>(this.droneObject, Vector3.zero, Quaternion.Euler(new Vector3(0f, -30f, 0f))).GetComponent<CursedDrone>();

                Debug.Log($"[Cursed Drone] Spawned: ");

            }
        }
        [PunRPC]
        private void RPCA_SpawnCursedDrone()
        {
            //List<Item> list = new List<Item>();

            Object.Instantiate<GameObject>(this.droneObject, Vector3.zero, Quaternion.Euler(new Vector3(0f, -30f, 0f))).GetComponent<CursedDrone>();
        }
    }

}
