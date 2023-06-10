using System;
using UnityEngine;
namespace Penwyn.Tools
{
    [System.Serializable]
    public class FloatMinMax
    {
        [SerializeField] float min;
        [SerializeField] float max;

        public FloatMinMax()
        {
        }
        public FloatMinMax(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
        public float Median()
        {
            return (min + max) / 2;
        }

        public float Random()
        {
            return Randomizer.Range(min, max);
        }

        public float Min { get => min; set => min = value; }
        public float Max { get => max; set => max = value; }
    }

    [System.Serializable]
    public class IntMinMax
    {
        [SerializeField] int min;
        [SerializeField] int max;

        public IntMinMax(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public float Median()
        {
            return (min + max) / 2;
        }

        public int Random()
        {
            return (int)Randomizer.Range(min, max);
        }

        public int Min { get => min; set => min = value; }
        public int Max { get => max; set => max = value; }
    }
}
