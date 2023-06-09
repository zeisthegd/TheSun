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
            Debug.Log($"{objectHeight}|{sunAngle}|{Mathf.Tan(sunAngle)}");
            return Mathf.Max(0, objectHeight / Mathf.Tan(sunAngle * Mathf.Deg2Rad));
        }

        public static float SunAngle(DateTime date, float latitude, float utcDifference)
        {
            float radianLat = latitude * Mathf.Deg2Rad;
            float declination = DeclinationAngle(date) * Mathf.Deg2Rad;
            float localHourRad = LocalHour(date, utcDifference) * Mathf.Deg2Rad;
            return Mathf.Asin(Mathf.Sin(declination) * Mathf.Sin(radianLat) + Mathf.Cos(declination) * Mathf.Cos(radianLat) * Mathf.Cos(localHourRad)) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Calculate the DeclinationAngle of the Sun.
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns></returns>
        public static float DeclinationAngle(DateTime date)
        {
            return -23.44f * Mathf.Cos(Mathf.Deg2Rad * (360f / 365f) * (float)(DateSinceJanuarySameYear(date) + (10f / 24f) + 10));
        }

        #region Date Utility

        /// <summary>
        /// Calculate total days between input date and January 1st of the same year.
        /// </summary>
        public static double DateSinceJanuarySameYear(DateTime date)
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
        public static float LocalHour(DateTime utcDate, double different)
        {
            return 15 * (utcDate.AddHours(-different).Hour - 12);
        }


        #endregion
    }
}
