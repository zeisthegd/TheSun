using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.TheSun
{
    [System.Serializable]
    public class SunData
    {
        //Input
        public float Latitude;//Vi Do
        public float Longitude;//Kinh Do
        public float Altitude;
        public DateTime Date;

        //Output
        public float SunAngle;
        public float DeclinationAngle;
        public float ZenithAngle { get => 90 - SunAngle; }
        public float ShadowLength;

        public SunData()
        {

        }
    }
}
