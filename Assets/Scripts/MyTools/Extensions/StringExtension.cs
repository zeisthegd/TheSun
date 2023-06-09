using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Tools
{
    public static class StringExtension
    {
        public static float ToFloat(this string input)
        {
            return float.Parse(input);
        }

        public static int ToInt(this string input)
        {
            return int.Parse(input);
        }

        public static double ToDouble(this string input)
        {
            return double.Parse(input);
        }

        public static long ToLong(this string input)
        {
            return long.Parse(input);
        }
    }
}