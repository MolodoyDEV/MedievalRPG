using Molodoy.CoreComponents;
using UnityEngine;

namespace Molodoy.Extensions
{
    public static class GameObjectExtensions
    {
        public static GameObject Instantiate(this GameObject prefab)
        {
            GameObject newObject = Object.Instantiate(prefab);
            newObject.layer = GameConstants.Layer_InstantiatedNumber;
            ObjectsInitializer.InitializeObject(newObject);
            return newObject;
        }

        public static GameObject Instantiate(this GameObject prefab, Transform parent)
        {
            GameObject newObject = Object.Instantiate(prefab, parent);
            newObject.layer = GameConstants.Layer_InstantiatedNumber;
            return newObject;
        }

        public static GameObject Instantiate(this GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject newObject = Object.Instantiate(prefab, position, rotation);
            newObject.layer = GameConstants.Layer_InstantiatedNumber;
            return newObject;
        }
    }
}