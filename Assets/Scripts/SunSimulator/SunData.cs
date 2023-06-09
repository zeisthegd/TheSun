using System;
using UnityEngine;
using Penwyn.Tools;

namespace Penwyn.TheSun
{
    [System.Serializable]
    public class SunData
    {
        //Input
        public float Latitude;
        public float Longitude;
        public float ObjectHeight = 1.75f;
        public float TimeZone = 0;
        public DateTime Date;

        //Output
        public float SunElevationAngle;
        public float AzimuthAngle;
        public float DeclinationAngle;
        public float ShadowLength;
        public float ZenithAngle { get => 90 - SunElevationAngle; }

        public SunData() { }

        public void ComputeOutputData()
        {
            DeclinationAngle = GetDeclinationAngle();
            SunElevationAngle = GetElevationAngle();
            AzimuthAngle = GetAzimuthAngle();
            ShadowLength = GetShadowLength();
        }

        public float GetShadowLength()
        {
            return Mathf.Max(0, ObjectHeight / Mathf.Tan(SunElevationAngle * Mathf.Deg2Rad));
        }

        private float GetElevationAngle()
        {
            float radianLat = Latitude * Mathf.Deg2Rad;
            float declination = DeclinationAngle * Mathf.Deg2Rad;
            float localHourRad = LocalHour() * Mathf.Deg2Rad;
            return Mathf.Asin((Mathf.Sin(declination) * Mathf.Sin(radianLat)) + (Mathf.Cos(declination) * Mathf.Cos(radianLat) * Mathf.Cos(localHourRad))) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Calculate the DeclinationAngle of the Sun.
        /// </summary>
        /// <returns>Declination in Degrees.</returns>
        private float GetDeclinationAngle()
        {
            return -23.44f * Mathf.Cos(Mathf.Deg2Rad * (360f / 365f) * (float)(Date.DateSinceYearStart() + (10f / 24f) + 10));
        }

        private float GetAzimuthAngle()
        {
            float radianLat = Latitude * Mathf.Deg2Rad;
            float declination = DeclinationAngle * Mathf.Deg2Rad;
            float localHourRad = LocalHour() * Mathf.Deg2Rad;
            float elevation = SunElevationAngle * Mathf.Deg2Rad;

            float azimuthAngle = Mathf.Acos(Mathf.Clamp((Mathf.Sin(declination) * Mathf.Cos(radianLat) - Mathf.Cos(declination) * Mathf.Sin(radianLat) * Mathf.Cos(localHourRad)) / Mathf.Cos(elevation), -1, 1)) * Mathf.Rad2Deg;
            return localHourRad < 0 ? azimuthAngle : 360 - azimuthAngle;
        }

        #region Date Utility

        /// <summary>
        /// Input UTC zero time.
        /// </summary>
        private float LocalHour()
        {
            float b = (360f / 365f) * ((float)Date.DateSinceYearStart() - 81f) * Mathf.Deg2Rad;
            float equationOfTime = 9.87f * Mathf.Sin(2 * b) - 7.53f * Mathf.Cos(b) - 1.5F * Mathf.Sin(b);
            float tcf = 4 * (Longitude - TimeZone * 15) + equationOfTime;
            float localSolarTime = Date.Hour + (tcf / 60f);
            float hra = 15 * (localSolarTime - 12);
            return hra;
        }
        // Basic:15 * (utcDate.AddHours(-different).Hour - 12);

        #endregion
    }
}
