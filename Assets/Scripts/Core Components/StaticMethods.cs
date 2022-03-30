using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Molodoy.CoreComponents
{
    //От класса надо избавиться
    public static class StaticMethods
    {
        public delegate void AnyMethod0args();
        public delegate void AnyMethod1args(object arg);
        //public delegate void AnyMethodGMargs(GameObject gameObject);

        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static IEnumerator RunMethodAfterSecondsCoroutine(float waitSeconds, AnyMethod0args methodToRun)
        {
            if (waitSeconds != 0)
            {
                yield return new WaitForSeconds(waitSeconds);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }

            methodToRun();
            yield break;
        }

        public static IEnumerator RunMethodAfterSecondsCoroutine(float waitSeconds, AnyMethod1args methodToRun, object arg)
        {
            if (waitSeconds != 0)
            {
                yield return new WaitForSeconds(waitSeconds);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }

            methodToRun(arg);
            yield break;
        }
    }
}