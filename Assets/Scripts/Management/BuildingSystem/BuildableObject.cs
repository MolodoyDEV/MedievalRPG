using Assets.Scripts.Buildings;
using Assets.Scripts.Management.Registrators;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Management.BuildingSystem
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BaseBuilding))]
    [RequireComponent(typeof(BuildingPermitChecker))]
    [RequireComponent(typeof(BuildingPermitVisualizer))]
    public class BuildableObject : MonoBehaviour
    {
        [SerializeField] private bool isBuilded;
        private BaseBuilding baseBuilding;
        private BuildingPermitChecker buildingPermitChecker;
        private BuildingPermitVisualizer buildingPermitVisualizer;
        private bool previousAllowToBuildState;
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

            if(isBuilded == false)
            {
                buildingPermitChecker.Initialize();
                buildingPermitVisualizer.Initialize();
            }
        }

        private void Start()
        {
            if (isBuilded)
            {
                OnBuilded();
            }
            else
            {
                if (IsAllowToBuild)
                {
                    buildingPermitVisualizer.OnAllowToBuld();
                }
                else
                {
                    buildingPermitVisualizer.OnProhibitedToBuld();
                }
            }
        }

        private void FixedUpdate()
        {
            if (myTransform.position != previousPosition || myTransform.rotation != previousRotation)
            {
                previousPosition = myTransform.position;
                previousRotation = myTransform.rotation;

                if (previousAllowToBuildState == false && IsAllowToBuild)
                {
                    previousAllowToBuildState = true;
                    buildingPermitVisualizer.OnAllowToBuld();
                }
                else if (previousAllowToBuildState && IsAllowToBuild == false)
                {
                    previousAllowToBuildState = false;
                    buildingPermitVisualizer.OnProhibitedToBuld();
                }
            }
        }

        public void OnBuilded()
        {
            isBuilded = true;
            BuildingsRegistrator.RegisterBuilding(baseBuilding);
            buildingPermitVisualizer.enabled = false;
            buildingPermitChecker.enabled = false;
            enabled = false;
        }
    }
}