using System;
using System.Collections.Generic;
using UnityEngine;

namespace Molodoy.CoreComponents.Saves
{
    [Serializable]
    public class SavedData
    {
        public interface ISaveData { };

        public List<ISaveData> SavedObjectData;
        public int LoadOrderNumber = 1;
        public int ObjectLayerNumber;
        public string SavedObjectTypeTag;

        public SavedData(List<ISaveData> saveData, string objectTypeTag, int objectLayerNumber, int loadOrder)
        {
            SavedObjectData = saveData;
            SavedObjectTypeTag = objectTypeTag;
            ObjectLayerNumber = objectLayerNumber;
            LoadOrderNumber = loadOrder;
        }

        public SavedData(ISaveData saveData, string objectTypeTag, int objectLayerNumber, int loadOrder)
        {
            SavedObjectData = new List<ISaveData> { saveData };
            SavedObjectTypeTag = objectTypeTag;
            ObjectLayerNumber = objectLayerNumber;
            LoadOrderNumber = loadOrder;
        }

        [Serializable]
        public class SynchronizationToken { }

        [Serializable]
        public class TransformData : ISaveData
        {
            public SerializableVector3 SavedPosition, SavedRotation, SavedLocalScale;

            public TransformData(Transform transform)
            {
                SavedPosition = new SerializableVector3(transform.position);
                SavedRotation = new SerializableVector3(transform.rotation.eulerAngles);
                SavedLocalScale = new SerializableVector3(transform.localScale);
            }

            public TransformData(Vector3 position, Vector3 rotation, Vector3 localScale)
            {
                SavedPosition = new SerializableVector3(position);
                SavedRotation = new SerializableVector3(rotation);
                SavedLocalScale = new SerializableVector3(localScale);
            }

            [Serializable]
            public struct SerializableVector3
            {
                public float x, y, z;

                public SerializableVector3(Vector3 vector3)
                {
                    x = vector3.x;
                    y = vector3.y;
                    z = vector3.z;
                }

                public Vector3 ToVector3()
                {
                    return new Vector3(x, y, z);
                }
            }
        }

        [Serializable]
        public class PowerSupplyData : ISaveData
        {
            public bool SavedPowerSupplyAvailableState;
            public bool SavedPowerState;

            public PowerSupplyData(bool powerSupplyAvailableState, bool powerState)
            {
                SavedPowerSupplyAvailableState = powerSupplyAvailableState;
                SavedPowerState = powerState;
            }
        }
    }
}