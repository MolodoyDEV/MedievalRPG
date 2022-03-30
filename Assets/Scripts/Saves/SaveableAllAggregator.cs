using System;
using System.Linq;

namespace Molodoy.CoreComponents.Saves
{
    /// <summary>
    /// Use only one in object! Gets all saveable classes in object;
    /// </summary>
    public class SaveableAllAggregator : Saveable
    {
        private void Awake()
        {
            if (GetComponentsInChildren<SaveableAllAggregator>().Count() > 1)
            {
                throw new Exception("На этом объекте находится больше одного SaveableAllAggregator скрипта");
            }

            saveableObjects = GetComponentsInChildren<ISaveable>()?.ToList();

            if (saveableObjects == null)
            {
                throw new Exception("Хотя бы один класс должен реализовывать интерфейс ISaveable");
            }

            saveableObjects.Reverse();

            //Debug.LogWarning(saveableObjects.Count, gameObject);
        }
    }
}