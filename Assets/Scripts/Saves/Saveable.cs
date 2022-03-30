using System;
using System.Collections.Generic;
using UnityEngine;

namespace Molodoy.CoreComponents.Saves
{
    public abstract class Saveable : MonoBehaviour
    {
        [SerializeField] protected int loadOnStage = 1;
        protected List<ISaveable> saveableObjects;

        protected virtual string GetTag()
        {
            string objectTypeTag = transform.tag;

            if (objectTypeTag == "Untagged")
            {
                objectTypeTag = transform.parent?.tag;

                //if (gameObject.layer == GameConstants.Layer_InstantiatedNumber && (objectTypeTag == null || objectTypeTag == "Untagged"))
                //{
                //    string logMessage = objectTypeTag + " Не задан тэг у " + gameObject.name;
                //    Debug.LogError("Error", gameObject);
                //    throw new Exception(logMessage);
                //}
            }

            return objectTypeTag;
        }

        public virtual void LoadData(SavedData savedData)
        {
            string logMessage = "";
            string objectTypeTag = GetTag();

            if (savedData.SavedObjectTypeTag != objectTypeTag)
            {
                Debug.LogError("Error", gameObject);
                logMessage = gameObject.name + " Получил сохраненные данные от другого объекта " + savedData.SavedObjectTypeTag;
                throw new Exception(logMessage);
            }
            else if (savedData.SavedObjectData.Count != saveableObjects.Count)
            {
                Debug.LogError("Error", gameObject);
                logMessage = "Не совпадает количество данных " + savedData.SavedObjectData.Count + " с количеством объектов " + saveableObjects.Count;
                throw new Exception(logMessage);
            }

            if (saveableObjects.Count != 0)
            {
                for (int i = 0; i < saveableObjects.Count; i++)
                {
                    saveableObjects[i].LoadData(savedData.SavedObjectData[i]);
                }
            }
        }

        public virtual SavedData GetDataToSave()
        {
            List<SavedData.ISaveData> saveableData = new List<SavedData.ISaveData>();

            if (saveableObjects.Count != 0)
            {
                for (int i = 0; i < saveableObjects.Count; i++)
                {
                    saveableData.Add(saveableObjects[i].GetDataToSave());
                }
            }

            string objectTypeTag = GetTag();

            if (saveableData.Count != saveableObjects.Count)
            {
                Debug.LogError("Error", gameObject);
                string logMessage = "Не совпадает количество данных " + saveableData.Count + " с количеством объектов " + saveableObjects.Count;
                throw new Exception(logMessage);
            }

            return new SavedData(saveableData, objectTypeTag, gameObject.layer, GetSaveableStage());
        }

        public virtual int GetSaveableStage()
        {
            return loadOnStage;
        }
    }
}