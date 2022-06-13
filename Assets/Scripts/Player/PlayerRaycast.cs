using System.Collections;
using UnityEngine;

namespace Molodoy.Characters.Player
{
    public class PlayerRaycast : MonoBehaviour
    {
        private Ray ray;
        private RaycastHit hit;
        private GameObject hitedObject;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private LayerMask hittableLayers;
        [SerializeField] private float maxRaycastDistance;
        public Vector3 RayDirection { get => ray.direction; }
        public GameObject HitedObject { get => GetLookedObject(); }
        public Vector3 RayHitPoint { get => GetRayHitPoint(); }

        private void Start()
        {
            if (maxRaycastDistance == 0)
            {
                maxRaycastDistance = 2f;
            }
        }

        //private void Update()
        //{
        //    UpdateRay();
        //}

        private void UpdateRay()
        {
            ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward * maxRaycastDistance);
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * maxRaycastDistance, Color.yellow);

            if (Physics.Raycast(ray, out hit, maxRaycastDistance, hittableLayers.value))
            {
                hitedObject = hit.collider.gameObject;
            }
            else
            {
                hitedObject = null;
            }
        }

        public GameObject GetLookedObject()
        {
            UpdateRay();
            return hitedObject;
        }

        public Vector3 GetRayHitPoint()
        {
            if (HitedObject)
            {
                return hit.point;
            }
            else
            {
                return ray.GetPoint(maxRaycastDistance);
            }
        }
    }
}