using Assets.Scripts.Buildings;
using Assets.Scripts.Management.Registrators;
using UnityEngine;

namespace Assets.Scripts.Management.BuildingSystem
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BaseBuilding))]
    [RequireComponent(typeof(BuildingPermitChecker))]
    [RequireComponent(typeof(BuildingPermitVisualizer))]
    public class BuildableObject : MonoBehaviour
    {
        [SerializeField] private bool isFullyBuilded;
        [SerializeField] private bool isPlaced;
        private BaseBuilding baseBuilding;
        private BuildingPermitChecker buildingPermitChecker;
        private BuildingPermitVisualizer buildingPermitVisualizer;
        private Vector3 previousPosition;
        private Quaternion previousRotation;
        private Transform myTransform;

        public bool IsAllowToBuild { get => buildingPermitChecker.CheckAllowToBuild(); }

        private void Awake()
        {
            myTransform = transform;
            buildingPermitChecker = GetComponent<BuildingPermitChecker>();
            buildingPermitVisualizer = GetComponent<BuildingPermitVisualizer>();
            baseBuilding = GetComponent<BaseBuilding>();
        }

        private void Start()
        {
            if (isFullyBuilded == false)
            {
                buildingPermitChecker.Initialize();
                //buildingPermitVisualizer.Initialize();

                if (isPlaced)
                {
                    OnUnfinishedBuildingPlaced();
                }
            }
            else
            {
                OnFullyBuilded();
            }

            if (isFullyBuilded == false)
            {
                if (IsAllowToBuild)
                {
                    buildingPermitVisualizer.OnAllowToBuild();
                }
                else
                {
                    buildingPermitVisualizer.OnProhibitedToBuild();
                }
            }
        }

        private void FixedUpdate()
        {
            if (myTransform.position != previousPosition || myTransform.rotation != previousRotation)
            {
                previousPosition = myTransform.position;
                previousRotation = myTransform.rotation;

                if (IsAllowToBuild)
                {
                    buildingPermitVisualizer.OnAllowToBuild();
                }
                else
                {
                    buildingPermitVisualizer.OnProhibitedToBuild();
                }
            }
        }

        public void OnFullyBuilded()
        {
            isFullyBuilded = true;
            BuildingsRegistrator.RegisterBuilding(baseBuilding);
            buildingPermitVisualizer.OnFullyBuilded();
            buildingPermitVisualizer.enabled = false;
            enabled = false;
        }

        public void OnUnfinishedBuildingPlaced()
        {
            isPlaced = true;
            buildingPermitVisualizer.OnUnfinishedBuildingPlaced();
            buildingPermitChecker.enabled = false;
        }
    }
}