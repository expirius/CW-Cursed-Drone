using UnityEngine;

namespace CW_Cursed_Drone
{
    public class DroneSoundBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float pitchFactor = 1.5f;
        [SerializeField]
        private float volumeFactor = 0.4f;

        [SerializeField]
        private float pitchOffset = 0.33f;
        [SerializeField]
        private float volumeOffset = 0.7f;

        [SerializeField]
        public AudioClip motorSound;
        [SerializeField]
        public DronePhysicsBehaviour dronePhysics = new();
        [SerializeField]
        public AudioSource source;



        private void Start()
        {
            if (!source)
                source = GetComponent<AudioSource>();

            if (!dronePhysics)
                dronePhysics = GetComponent<DronePhysicsBehaviour>();
        }
        void Update()
        {
            //if (dronePhysics.armed)
            //{
                if (!source.isPlaying)
                    source.PlayOneShot(motorSound);

                source.pitch = pitchOffset + (dronePhysics.appliedForce.magnitude / dronePhysics.physicsConfig.thrust) * pitchFactor;
                source.volume = volumeOffset + (dronePhysics.appliedForce.magnitude / dronePhysics.physicsConfig.thrust) * volumeFactor;
            //}
        }
    }
}
