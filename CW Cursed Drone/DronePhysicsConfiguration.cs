using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CW_Cursed_Drone
{
    [System.Serializable]
    public class DronePhysicsConfiguration
    {
        public DronePhysicsConfiguration()
        {
            autoStabilizeSpeed = 90f;
            thrust = 28f;
            mass = 2f;
            drag = 1.2f;
            angularDrag = 45f;
            p = 0.8f;
            d = 0.2f;
            pAltitude = 8f;
            dAltitude = 4f;
        }
        [SerializeField] public float autoStabilizeSpeed;

        [Header("Maximum Thrust [N]")]
        public float thrust;

        [Header("Physics")]
        public float mass;
        public float drag;
        public float angularDrag;

        [Header("PID Rotation [Nm/Deg]")]
        public float p;
        public float i;
        public float d;

        [Header("PID Altitude [N/m]")]
        public float pAltitude;
        public float iAltitude;
        public float dAltitude;
    }

}
