using System;
using UnityEngine;

namespace Penwyn.Tools
{
    public static class DateHelper
    {
        public static DateTime RandomDate()
        {
            int year = Randomizer.Range(1900, 2051);
            int month = Randomizer.Range(1, 13);
            int day = Randomizer.Range(1, 32);
            int hour = Randomizer.Range(0, 25);
            int minute = Randomizer.Range(0, 60);
            int second = Randomizer.Range(0, 60);
            if (IsDateValid(year, month, day, hour, minute, second))
                return new DateTime(year, month, day, hour, minute, second);
            else
                return RandomDate();
        }
        public static bool IsDateValid(int year, int month, int day, int hour, int minute, int seconds)
        {
            try
            {
                DateTime date = new System.DateTime(year, month, day, hour, minute, seconds);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Calculate total days between input date and January 1st of the same year.
        /// </summary>
        public static double DateSinceYearStart(this DateTime date)
        {
            return date.DaysTo(new DateTime(date.Year, 1, 1));
        }

        /// <summary>
        /// Calculate total days between 2 date.
        /// </summary>
        public static double DaysTo(this DateTime first, DateTime second)
        {
            return (first - second).TotalDays;
        }

    }
}
