using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CW_Cursed_Drone
{
    //[System.Serializable]
    public class DronePhysicsConfiguration
    {
        [SerializeField] public float autoStabilizeSpeed = 90f; // Скорость стабилизации в градусах/сек
/*        public DronePhysicsConfiguration(DronePhysicsConfiguration physicsConfig)
        {
            thrust = physicsConfig.thrust;

            mass = physicsConfig.mass;
            drag = physicsConfig.drag;
            angularDrag = physicsConfig.angularDrag;

            p = physicsConfig.p;
            i = physicsConfig.i;
            d = physicsConfig.d;

            pAltitude = physicsConfig.pAltitude;
            iAltitude = physicsConfig.iAltitude;
            dAltitude = physicsConfig.dAltitude;
        }*/

        [Header("Maximum Thrust [N]")]
        public float thrust = 25;

        [Header("Physics")]
        public float mass = 2f;
        public float drag = 1.2f;
        public float angularDrag = 4f;

        [Header("PID Rotation [Nm/Deg]")]
        public float p = 0.8f;
        public float i = 0f;
        public float d = 0.2f;

        [Header("PID Altitude [N/m]")]
        public float pAltitude = 8f;
        public float iAltitude = 0f;
        public float dAltitude = 4f;
    }
}
