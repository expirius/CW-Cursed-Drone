﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CW_Cursed_Drone
{
    public class DronePIDController
    {
        private Vector3 pd;

        private Vector3 p;
        private Vector3 d;
        private Vector3 lastE;

        public DronePIDController()
        {

        }

        public Vector3 CalculatePD(Vector3 e, float pk, float dk)
        {
            p = e * pk;
            d = ((e - lastE) / Time.fixedDeltaTime) * dk;
            pd = p + d;

            lastE = e;
            return pd;
        }
    }
}
