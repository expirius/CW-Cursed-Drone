/*using Photon.Pun;
using UnityEngine;

namespace Cursed_Drone
{
    public class DroneController : MonoBehaviourPun, IPunObservable
    {
        [Header("Settings")]
        [SerializeField] private float maxHeight = 10f;
        [SerializeField] private float minHeight = 2f;
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float rotationSpeed = 5f;

        [Header("Effects")]
        [SerializeField] private ParticleSystem engineParticles;
        [SerializeField] private Light droneLight;
        [SerializeField] private AudioClip[] engineSounds;

        private Vector3 targetPosition;
        private Quaternion targetRotation;
        private Player owner;
        private AudioSource audioSource;
        private float currentHeight;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            currentHeight = transform.position.y;
        }

        public void Initialize(Player ownerPlayer)
        {
            owner = ownerPlayer;
            targetPosition = transform.position;
            targetRotation = transform.rotation;

            if (photonView.IsMine)
            {
                droneLight.color = Color.blue;
                engineParticles.Play();
                PlayEngineSound();
            }
        }

        private void Update()
        {
            if (!photonView.IsMine) return;

            UpdateMovement();
            HandleInput();
            ClampPosition();
        }

        private void HandleInput()
        {
            // Vertical movement
            float vertical = Input.GetKey(KeyCode.Space) ? 1 : Input.GetKey(KeyCode.LeftControl) ? -1 : 0;
            currentHeight += vertical * moveSpeed * Time.deltaTime;
            currentHeight = Mathf.Clamp(currentHeight, minHeight, maxHeight);

            // Horizontal movement
            Vector3 moveDirection = new Vector3(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
            );

            targetPosition += transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;
            targetPosition.y = currentHeight;

            // Rotation
            if (Input.GetKey(KeyCode.Q)) targetRotation *= Quaternion.Euler(0, -rotationSpeed, 0);
            if (Input.GetKey(KeyCode.E)) targetRotation *= Quaternion.Euler(0, rotationSpeed, 0);
        }

        private void UpdateMovement()
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        private void ClampPosition()
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);
            transform.position = pos;
        }

        private void PlayEngineSound()
        {
            if (engineSounds.Length > 0 && !audioSource.isPlaying)
            {
                audioSource.clip = engineSounds[UnityEngine.Random.Range(0, engineSounds.Length)];
                audioSource.Play();
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(targetPosition);
                stream.SendNext(targetRotation);
                stream.SendNext(droneLight.color);
            }
            else
            {
                targetPosition = (Vector3)stream.ReceiveNext();
                targetRotation = (Quaternion)stream.ReceiveNext();
                droneLight.color = (Color)stream.ReceiveNext();
            }
        }

        private void OnDestroy()
        {
            if (photonView.IsMine)
            {
                engineParticles.Stop();
                audioSource.Stop();
            }
        }
    }
}*/

using Photon.Pun;
using UnityEngine;

namespace Cursed_Drone
{
    public class DroneController : MonoBehaviourPun, IPunObservable
    {
        [Header("Settings")]
        [SerializeField] public float followSpeed = 5f; // скорость дрона
        public float detectionRadius = 15f; // радиус обнаружения
        //
        [Header("Effects")]
        public ParticleSystem curseParticles; // эффекты активности
        public Light droneLight; // свет дрона, меняет при активации
        //
        private Player targetPlayer;
        private bool isCurseActive;

        private void Start()
        {
            if (photonView.IsMine)
            {
                targetPlayer = Player.localPlayer;
                GetComponent<PhotonView>().RPC("InitializeDrone", RpcTarget.All, targetPlayer.refs.view.ViewID);
            }
        }

        [PunRPC]
        public void InitializeDrone(int playerViewId)
        {
            PhotonView playerView = PhotonView.Find(playerViewId);
            if (playerView != null)
                targetPlayer = playerView.GetComponent<Player>();
        }

        private void Update()
        {
            if (targetPlayer == null || !photonView.IsMine) 
                return;

            Vector3 targetPos = targetPlayer.transform.position + Vector3.up * 3f;
            transform.position = Vector3.Lerp(
                transform.position, 
                targetPos, 
                followSpeed * Time.deltaTime
        );
    }

        private void FollowPlayer()
        {
            if (targetPlayer == null) return;

            Vector3 targetPosition = targetPlayer.transform.position + Vector3.up * 3f; // 3f высота над игроком
            transform.position = Vector3.Lerp( // lerp для плавного движения
                transform.position,
                targetPosition,
                followSpeed * Time.deltaTime
            );
        }
        private void CheckForMonsters() //проверка, нужно ли позвать к себе монстров (активировать курсу)
        {
            if (!isCurseActive && CheckPlayerBehind())
            {
                photonView.RPC(nameof(ActivateCurse), RpcTarget.All);
            }
        }
        private bool CheckPlayerBehind()
        {
            Vector3 dirToDrone = (transform.position - targetPlayer.transform.position).normalized;
            return Vector3.Angle(targetPlayer.refs.headPos.forward, dirToDrone) > 150f;
            // если дрон за игроком то true
        }

        // НЕТКОД
        [PunRPC]
        private void ActivateCurse() // активация призыва монстров к себе
        {
            isCurseActive = true;
            droneLight.color = Color.red;

            if (PhotonNetwork.IsMasterClient)
            {
                // не придумал пока как их созывать
                //MonsterSpawner.SpawnMonstersNearPosition(transform.position, 3);
            }
        }

        private void UpdateVisuals()
        {
            droneLight.intensity = Mathf.PingPong(Time.time, 1f) + 0.5f; // пульсирующий свет
        }
        // синхронизация дрона для всех игроков
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(isCurseActive);
                stream.SendNext(droneLight.color);
            }
            else
            {
                isCurseActive = (bool)stream.ReceiveNext();
                droneLight.color = (Color)stream.ReceiveNext();
            }
        }
    }
}