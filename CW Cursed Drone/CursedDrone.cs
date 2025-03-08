using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CW_Cursed_Drone
{
    public class CursedDrone : Drone
    {
        private DronePlugin dronePlugin;
        private PhotonView photonView;
        private Transform playerTarget;
        private Vector3 velocity;
        private float smoothSpeed = 3f;
        public void Initialize(Transform target)
        {
            playerTarget = target;
            // Отключаем автоматическое уничтожение
            this.counter = 0f;
            this.done = false;
            this.ropeLength = 2f; // Дистанция следования
            this.spring = 8f;     // Жесткость пружины
            this.grav = 5f;       // Гравитация
        }
        //private DroneKeySetting _droneKeySpawnSetting;
        private void Awake()
        {
            // Важно: Не уничтожать объект здесь!
            if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

            Debug.Log("Drone created successfully");
            // Добавьте инициализацию компонентов здесь
        }
        private void Start()
        {
            // Перенесите логику инициализации сюда
            if (!photonView.IsMine) return;

            // Настройка управления и физики
            //GetComponent<Rigidbody>().isKinematic = false;
        }
        private void Update()
        {
            if (playerTarget == null || done) return;
            // Переопределяем физику следования
            float distance = Vector3.Distance(rig.position, dron.position);
            float forceMultiplier = forceCurve.Evaluate(distance);

            Vector3 targetPos = dron.position + (rig.position - dron.position).normalized * ropeLength;
            rig.velocity *= Mathf.Lerp(1f, drag, forceMultiplier);

            Vector3 forceDirection = (targetPos - rig.position) * forceMultiplier * spring;
            rig.AddForce(forceDirection, ForceMode.Acceleration);
            rig.AddForce(Vector3.down * forceMultiplier * grav, ForceMode.Acceleration);
        }
       /* public void Spawn()
        {
            // Проверяем, что мы в сети и являемся владельцем
            if (!PhotonNetwork.IsConnectedAndReady || !photonView.IsMine)
                return;

            // Получаем позицию спавна перед игроком
            Vector3 spawnPosition = playerTarget.position + playerTarget.forward * 2f + Vector3.up;

            // Создаем дрон через Photon
            GameObject droneObj = PhotonNetwork.Instantiate(
                "drone", // Имя префаба в Resources
                spawnPosition,
                Quaternion.identity
            );

            // Получаем компонент дрона
            CursedDrone spawnedDrone = droneObj.GetComponent<CursedDrone>();

            if (spawnedDrone != null)
            {
                // Инициализируем дрон с целью игрока
                spawnedDrone.Initialize(playerTarget);
            }
            else
            {
                Debug.LogError("[Cursed Drone] CursedDrone component missing on prefab!");
                PhotonNetwork.Destroy(droneObj);
            }
        }*/
    }
}
