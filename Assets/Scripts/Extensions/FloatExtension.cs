using System;
using UnityEngine;

namespace Molodoy.Extensions
{
    public static class FloatExtension
    {
        public static float SmoothMovement(this float value)
        {
            return value * Time.deltaTime;
        }
    }


    [Serializable]
    public struct FloatRange
    {
        public float Start;
        public float End;

        public FloatRange(float _start, float _end)
        {
            Start = _start;
            End = _end;
        }

        public bool InRange(float value)
        {

            return value >= Start && value <= End;
        }

        public bool InRange(int value)
        {
            return value >= Start && value <= End;
        }


        public static FloatRange operator -(FloatRange range, float value)
        {
            return new FloatRange(range.Start - value, range.End - value);
        }

        public static FloatRange operator +(FloatRange range, float value)
        {
            return new FloatRange(range.Start + value, range.End + value);
        }

        public static FloatRange operator *(FloatRange range, float value)
        {
            return new FloatRange(range.Start * value, range.End * value);
        }

        public static FloatRange operator /(FloatRange range, float value)
        {
            return new FloatRange(range.Start / value, range.End / value);
        }
    }
}