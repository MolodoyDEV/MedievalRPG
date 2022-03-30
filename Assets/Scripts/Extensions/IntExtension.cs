using System;
using System.Collections;
using UnityEngine;

namespace Molodoy.Extensions
{
    public static class IntExtension
    {
        public static float SmoothMovement(this int value)
        {
            return value * Time.deltaTime;
        }
    }

    [Serializable]
    public struct IntRange
    {
        public int Start;
        public int End;

        public IntRange(int _start, int _end)
        {
            Start = _start;
            End = _end;
        }

        public bool InRange(int value)
        {
            return value >= Start && value <= End;
        }

        public static IntRange operator *(IntRange range, int value)
        {
            return new IntRange(range.Start * value, range.End * value);
        }

        public static IntRange operator /(IntRange range, int value)
        {
            return new IntRange(range.Start / value, range.End / value);
        }
    }
}