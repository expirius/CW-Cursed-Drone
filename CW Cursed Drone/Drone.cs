using DefaultNamespace.Artifacts;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CW_Cursed_Drone
{
    public class Drone : MonoBehaviourPun //, IArtifactCurse // дрон как сущность или монстр
    {
        [SerializeField] static private GameObject dronePrefab;
        //
        public float Rarity => 0.7f; // шанс появления
        public float BudgetCost => 25f; // цена
        //
        [SerializeField] static private float curseDuration = 30f; // длительность дрона
        [SerializeField] static private SFX_Instance activationSound; // звук при активации
        //
        private PhotonView photonView; // для синхры
        private GameObject currentDrone; // активный дрон
        private Player cursedPlayer; // target на конкретного игрока
        //
        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
            dronePrefab = Resources.Load<GameObject>("Drone");
        }
        private void Start()
        {
            this.currentDrone = base.transform.GetChild(0);
            this.rig = base.GetComponentInChildren<Rigidbody>();
        }
        /* public void CastCurse(ItemInstanceBehaviour cursedItem, Player player)
         {
             if (!player.refs.view.IsMine) return;

             photonView.RPC(nameof(RPCA_AttachDrone), RpcTarget.All, player.refs.view.OwnerActorNr); // прикрепляет дрон к игроку
             photonView.RPC(nameof(RPCA_PlayActivationSound), RpcTarget.All); // звук активации
         }*/
        // Сетевые методы прикрепления дрона и проигрывания звука
        /* Алгоритм неткода:
         * Получаем игрока по ID через 
         * Если игрок найден, создаем дрон помещая его над игроком
         * Через curseDuration секунд дрон уничтожается
         * Звук активации воспроизводится в центре модели игрока
         * */
        [PunRPC]
        private void RPCA_AttachDrone(int playerId)
        {
            if (PlayerHandler.instance.TryGetPlayerFromOwnerID(playerId, out Player player))
            {
                Vector3 spawnPos = player.transform.position + Vector3.up * 3f;

                currentDrone = PhotonNetwork.Instantiate(
                    dronePrefab.name,
                    spawnPos,
                    Quaternion.identity
                );
                //photonView.RPC(nameof(RPCA_AttachDrone), RpcTarget.AllViaServer, photonView.OwnerActorNr);

                //Destroy(currentDrone, curseDuration);
                activationSound.Play(player.Center());
                Debug.Log($"RPC вызван для игрока {playerId}"); // В RPCA_AttachDrone
            }
        }
        /*
          [PunRPC]
          private void RPCA_PlayActivationSound()
          {
              activationSound.Play(cursedPlayer.Center());
          }*/
        //
        public float GetRarity() => Rarity;
        public float GetBudgetCost() => BudgetCost;
    }
}
