using Molodoy.CoreComponents.Saves;
using System.Collections.Generic;
using System.Linq;

namespace Molodoy.Extensions
{
    public static class ListExtension
    {
        public static void ToLower(this List<string> list)
        {
            for (int i = 0; i < list.Count() - 1; i++)
            {
                list[i] = list[i].ToLower();
            }
        }

        public static void ToUpper(this List<string> list)
        {
            for (int i = 0; i < list.Count() - 1; i++)
            {
                list[i] = list[i].ToUpper();
            }
        }

        public static bool TryGetLastElement<T>(this List<T> list, out T lastelement)
        {
            if (list.Count != 0)
            {
                lastelement = list[list.Count - 1];
                return true;
            }
            else
            {
                lastelement = default(T);
                return false;
            }
        }

        public static T GetLastElementOrDefault<T>(this List<T> list)
        {
            if (list.Count == 0)
            {
                return default(T);
            }
            else
            {
                return list[list.Count - 1];
            }
        }

        public static void RemoveLastElement<T>(this List<T> list)
        {
            if (list.Count != 0)
            {
                list.RemoveAt(list.Count - 1);
            }
        }

        public static string ListToString<T>(this List<T> list, string delimiter = " ")
        {
            string result = "";

            if (list.Count == 0)
            {
                return result;
            }

            foreach (T item in list)
            {
                result += item.ToString() + delimiter;
            }

            return result.Substring(0, result.Length - delimiter.Length);
        }

        public static void RemoveString(this string[] array, string item)
        {
            int remInd = -1;

            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i] == item)
                {
                    remInd = i;
                    break;
                }
            }

            string[] retVal = new string[array.Length - 1];

            for (int i = 0, j = 0; i < retVal.Length; ++i, ++j)
            {
                if (j == remInd)
                    ++j;

                retVal[i] = array[j];
            }

            array = retVal;
        }

        public static List<Saveable> SortBySaveableStandart(this List<Saveable> saveableObjects)
        {
            int totalStages = 1;

            List<Saveable> sortedSaveableObjects = new List<Saveable>();
            List<Saveable> saveableObjectsOnThisStage = new List<Saveable>();

            for (int stage = 1; stage <= totalStages; stage++)
            {
                for (int i = 0; i < saveableObjects.Count; i++)
                {
                    int objectSaveableStage = saveableObjects[i].GetSaveableStage();

                    if (objectSaveableStage > totalStages)
                    {
                        totalStages = objectSaveableStage;
                    }
                    else if (objectSaveableStage == stage)
                    {
                        saveableObjectsOnThisStage.Add(saveableObjects[i]);

                        saveableObjects.RemoveAt(i);
                        i--;
                    }
                }

                //Порядок в массиве - static1, instantianted1, static2, instantiated2...
                sortedSaveableObjects.AddRange(saveableObjectsOnThisStage);

                saveableObjectsOnThisStage.Clear();
            }

            return sortedSaveableObjects;
        }
    }
}