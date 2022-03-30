using System.Collections.Generic;
using UnityEngine;

namespace Molodoy.CoreComponents.Saves
{
    public class SaveLoader : MonoBehaviour
    {
        public GameObject InstantiatableObjectsParent;
        public List<GameObject> InstantiatableObjectsPrefabs = new List<GameObject>();

        private void Awake()
        {
            BinarySaveLoader.Awake(this);
        }

        private void OnApplicationQuit()
        {
            BinarySaveLoader.OnApplicationQuit();
        }

        private void OnDisable()
        {
            BinarySaveLoader.OnDisable();
        }
    }
}