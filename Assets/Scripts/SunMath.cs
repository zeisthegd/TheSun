using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.TheSun
{
    public static class SunMath
    {
        public static float ShadowLength(float objectHeight, float sunAngle)
        {
            return Mathf.Max(0, objectHeight / Mathf.Tan(sunAngle * Mathf.Deg2Rad));
        }

        public static float ElevationAngle(DateTime date, float latitudeDeg, float longitude, float utcDifference = 0)
        {
            float radianLat = latitudeDeg * Mathf.Deg2Rad;
            float declination = DeclinationAngle(date) * Mathf.Deg2Rad;
            float localHourRad = LocalHour(date, longitude, utcDifference) * Mathf.Deg2Rad;
            return Mathf.Asin(Mathf.Sin(declination) * Mathf.Sin(radianLat) + Mathf.Cos(declination) * Mathf.Cos(radianLat) * Mathf.Cos(localHourRad)) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Calculate the DeclinationAngle of the Sun.
        /// </summary>
        /// <returns>Declination in Degrees.</returns>
        public static float DeclinationAngle(DateTime date)
        {
            return -23.44f * Mathf.Cos(Mathf.Deg2Rad * (360f / 365f) * (float)(DateSinceYearStart(date) + (10f / 24f) + 10));
        }

        public static float AzimuthAngle(DateTime date, float latitudeDeg, float longitude, float utcDifference = 0)
        {
            float radianLat = latitudeDeg * Mathf.Deg2Rad;
            float declination = DeclinationAngle(date) * Mathf.Deg2Rad;
            float localHourRad = LocalHour(date, longitude, utcDifference) * Mathf.Deg2Rad;
            float elevation = ElevationAngle(date, latitudeDeg, utcDifference) * Mathf.Deg2Rad;

            float azimuthAngle = Mathf.Acos(Mathf.Clamp((Mathf.Sin(declination) * Mathf.Cos(radianLat) - Mathf.Cos(declination) * Mathf.Sin(radianLat) * Mathf.Cos(localHourRad)) / Mathf.Cos(elevation), -1, 1)) * Mathf.Rad2Deg;
            return localHourRad < 0 ? azimuthAngle : 360 - azimuthAngle;
        }

        #region Date Utility

        /// <summary>
        /// Calculate total days between input date and January 1st of the same year.
        /// </summary>
        public static double DateSinceYearStart(DateTime date)
        {
            return DateDifference(date, new DateTime(date.Year, 1, 1));
        }

        /// <summary>
        /// Calculate total days between 2 date.
        /// </summary>
        public static double DateDifference(DateTime first, DateTime second)
        {
            return (first - second).TotalDays;
        }

        /// <summary>
        /// Input UTC zero time.
        /// </summary>
        public static float LocalHour(DateTime utcDate, float longitude, float different)
        {
            float b = (360f / 365f) * ((float)DateSinceYearStart(utcDate) - 81f) * Mathf.Deg2Rad;
            float equationOfTime = 9.87f * Mathf.Sin(2 * b) - 7.53f * Mathf.Cos(b) - 1.5F * Mathf.Sin(b);
            float tcf = 4 * (longitude - different * 15) + equationOfTime;
            float localSolarTime = utcDate.Hour + (tcf / 60f);
            float hra = 15 * (localSolarTime - 12);
            return hra;
        }
        // Basic:15 * (utcDate.AddHours(-different).Hour - 12);

        #endregion
    }
}
