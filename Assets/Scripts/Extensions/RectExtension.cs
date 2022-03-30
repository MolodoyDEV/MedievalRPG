using UnityEngine;

namespace Molodoy.Extensions
{
    public static class RectExtension
    {
        public static void Set(this Rect rect, Rect newRect)
        {
            rect.Set(newRect.x, newRect.y, newRect.width, newRect.height);
        }
    }
}