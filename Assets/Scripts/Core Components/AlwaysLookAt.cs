using UnityEngine;

namespace Molodoy.CoreComponents
{
    public class AlwaysLookAt : MonoBehaviour
    {
        [SerializeField] private bool lookAtMainCamera;
        [Header("OR")]
        [SerializeField] private Transform targetTransfrom;
        [Header("OR")]
        [SerializeField] private Vector3 targetPosition;
        private Transform myTransform;

        private void Awake()
        {
            myTransform = transform;
        }

        private void Update()
        {
            if (lookAtMainCamera)
            {
                myTransform.LookAt(Camera.main.transform);
            }
            else if (targetTransfrom)
            {
                myTransform.LookAt(targetTransfrom);
            }
            else
            {
                myTransform.LookAt(targetPosition);
            }
        }
    }
}