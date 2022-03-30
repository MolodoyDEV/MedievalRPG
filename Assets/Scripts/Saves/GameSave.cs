using System;
using System.Collections.Generic;

namespace Molodoy.CoreComponents.Saves
{
    [Serializable]
    public class GameSave
    {
        private List<SavedData> savedDataList = new List<SavedData>();

        public void SaveObjects(List<Saveable> objectsToSave)
        {
            foreach (Saveable saveable in objectsToSave)
            {
                savedDataList.Add(saveable.GetDataToSave());
            }
        }

        public void LoadObjects(List<Saveable> objectsToLoad)
        {
            if (savedDataList.Count != 0)
            {
                for (int i = 0; i < savedDataList.Count; i++)
                {
                    //Порядок в массиве - static1, instantianted1, static2, instantiated2...
                    if (savedDataList[i].ObjectLayerNumber == GameConstants.Layer_InstantiatedNumber)
                    {
                        Saveable newSaveableObject = BinarySaveLoader.CreateObject(savedDataList[i].SavedObjectTypeTag);
                        newSaveableObject.LoadData(savedDataList[i]);
                        savedDataList.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        objectsToLoad[i].LoadData(savedDataList[i]);
                    }
                }
            }
        }
    }
}
