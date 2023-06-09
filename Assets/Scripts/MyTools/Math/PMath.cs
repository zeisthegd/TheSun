using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Game;

namespace Penwyn.Tools
{

    public class PMath
    {
        public static class Position
        {
            public static Vector3 GetCenterOf(List<Transform> objects)
            {
                return GetCenterOf(objects.ToArray());
            }

            public static Vector3 GetCenterOf<T>(T[] objects) where T : Transform
            {
                Vector3 culmulativePositions = new Vector3();
                for (int i = 0; i < objects.Length; i++)
                {
                    culmulativePositions += @objects[i].transform.position;
                }
                return culmulativePositions / objects.Length;
            }
        }

        public static class General
        {
            public static float Clip(float value, float lower, float upper)
            {
                return Mathf.Min(upper, Mathf.Max(value, lower));
            }

            /// <summary>
            /// If Value is lower than MIN returns MAX, and vice versa.
            /// </summary>
            public static int SnapReverse(int value, int min, int max)
            {
                if (value < min)
                    return max;
                if (value > max)
                    return min;
                return value;
            }

            /// <summary>
            /// If Value is lower than MIN returns MAX, and vice versa.
            /// </summary>
            public static float SnapReverse(float value, float min, float max)
            {
                if (value < min)
                    return max;
                if (value > max)
                    return min;
                return value;
            }
        }

        public static class Vector
        {
            /// <summary>
            /// Snap to the closet cardinal direction. Including 3 axis X, Y, Z.
            /// </summary>
            public static Vector3 Snap(Vector3 original)
            {
                Vector3 absOriginal = Abs(original);

                if (absOriginal.x >= absOriginal.y && absOriginal.x >= absOriginal.z)
                    return Vector3.right * Mathf.Sign(original.x);

                if (absOriginal.y >= absOriginal.x && absOriginal.y >= absOriginal.z)
                    return Vector3.up * Mathf.Sign(original.y);

                if (absOriginal.z >= absOriginal.x && absOriginal.z >= absOriginal.y)
                    return Vector3.forward * Mathf.Sign(original.z);

                return Vector3.zero;
            }

            /// <summary>
            /// Returns a new vector with all its value absoluted.
            /// </summary>
            public static Vector3 Abs(Vector3 original)
            {
                return new Vector3(Mathf.Abs(original.x), Mathf.Abs(original.y), Mathf.Abs(original.z));
            }

            /// <summary>
            /// Returns a new vector with all its value absoluted.
            /// </summary>
            public static Vector3 RandomNormalized()
            {
                return new Vector3(Randomizer.Range(-10, 10), Randomizer.Range(-10, 10), Randomizer.Range(-10, 10)).normalized;
            }

            /// <summary>
            /// Returns a new vector with all its value absoluted.
            /// </summary>
            public static Vector3 RandomNormalized(float x, float y, float z)
            {
                return new Vector3(Randomizer.Range(-x, x), Randomizer.Range(-y, y), Randomizer.Range(-z, z)).normalized;
            }

            /// <summary>
            /// Multiply each components of 2 vectors together.
            /// </summary>
            public static Vector3 Multiply(Vector3 first, Vector3 second)
            {
                return new Vector3(first.x * second.x, first.y * second.y, first.z * second.z);
            }
        }
    }
}
