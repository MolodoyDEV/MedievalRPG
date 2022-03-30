using UnityEngine;

namespace Molodoy.Extensions
{
    public static class Vector3Extension
    {
        /// <summary>
        /// Returns percent of x, y, z as Vector3
        /// </summary>
        /// <param name="vector3"></param>
        /// <param name="percent"></param>
        /// <returns>new Vector3</returns>
        public static Vector3 AppendPercent(this Vector3 vector3, float percent)
        {
            return new Vector3(vector3.x + vector3.x / 100 * percent, vector3.y + vector3.y / 100 * percent, vector3.z + vector3.z / 100 * percent);
        }

        public static Vector3 SmoothMovement(this Vector3 vector3)
        {
            return vector3 * Time.deltaTime;
        }

        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }
    }
}