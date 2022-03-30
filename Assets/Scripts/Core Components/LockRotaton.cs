using UnityEngine;

namespace Molodoy.CoreComponents
{
    public class LockRotaton : MonoBehaviour
    {
        [SerializeField] private Vector3 rotation;
        private Transform myTransform;
        private Quaternion rotationQuaternion;

        private void Awake()
        {
            myTransform = transform;
            rotationQuaternion = Quaternion.Euler(rotation);
        }

        private void Update()
        {
            myTransform.rotation = rotationQuaternion;
        }
    }
}