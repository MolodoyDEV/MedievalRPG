using System;
using System.Linq;

namespace Molodoy.CoreComponents.Saves
{
    /// <summary>
    /// Gets saveable classes from current object;
    /// </summary>
    public class SaveableCurrentAggregator : Saveable
    {
        private void Awake()
        {
            if (GetComponentsInParent<SaveableAllAggregator>().Count() > 1)
            {
                throw new Exception("На родительском объекте уже находится SaveableAllAggregator скрипт");
            }

            saveableObjects = GetComponents<ISaveable>()?.ToList();

            if (saveableObjects == null)
            {
                throw new Exception("Хотя бы один класс должен реализовывать интерфейс ISaveable");
            }

            saveableObjects.Reverse();
        }
    }
}