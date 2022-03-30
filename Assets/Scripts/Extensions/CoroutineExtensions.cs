using UnityEngine;

namespace Molodoy.Extensions
{
    public static class CoroutineExtensions
    {
        public static void Stop(this Coroutine coroutine, MonoBehaviour whereCoroutineStarted)
        {
            whereCoroutineStarted.StopCoroutine(coroutine);
        }
    }
}