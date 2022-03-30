using Assets.Scripts.Buildings;
using Assets.Scripts.Management.Registrators;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(BaseBuilding))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(NavMeshObstacle))]
    public class BuildableObject : MonoBehaviour
    {
        [SerializeField] private Material allowToBuildMaterial;
        [SerializeField] private Material prohibitedToBuildMaterial;
        [SerializeField] private bool isBuilded;
        private BuildingTerritory buildingTerritory;
        private Material[] defaultMaterials;
        private Material[] prohibitedToBuildMaterials;
        private Material[] allowToBuildMaterials;
        private BaseBuilding baseBuilding;
        private Collider myColider;
        private Renderer myRenderer;
        private bool previousAllowToBuildState;
        private NavMeshObstacle myObstacle;

        public bool IsAllowToBuild { get => CheckAllowToBuild(); }

        private void Awake()
        {
            myObstacle = GetComponent<NavMeshObstacle>();
            myObstacle.shape = NavMeshObstacleShape.Box;
            myObstacle.carving = true;
            buildingTerritory = GetComponentInChildren<BuildingTerritory>();
            baseBuilding = GetComponent<BaseBuilding>();
            myColider = GetComponent<Collider>();
            myRenderer = GetComponentInChildren<Renderer>();
            defaultMaterials = myRenderer.materials;

            if (isBuilded)
            {
                OnBuilded();
            }
            else
            {
                myObstacle.enabled = false;
                myColider.enabled = false;
                prohibitedToBuildMaterials = new Material[defaultMaterials.Length];
                allowToBuildMaterials = new Material[defaultMaterials.Length];

                for (int i = 0; i < defaultMaterials.Length; i++)
                {
                    prohibitedToBuildMaterials[i] = prohibitedToBuildMaterial;
                    allowToBuildMaterials[i] = allowToBuildMaterial;
                }
            }
        }

        private void Start()
        {
            if (IsAllowToBuild)
            {
                OnAllowToBuld();
            }
            else
            {
                OnProhibitedToBuld();
            }
        }

        private void FixedUpdate()
        {
            if (previousAllowToBuildState == false && IsAllowToBuild)
            {
                previousAllowToBuildState = true;
                OnAllowToBuld();
            }
            else if (previousAllowToBuildState && IsAllowToBuild == false)
            {
                previousAllowToBuildState = false;
                OnProhibitedToBuld();
            }
        }

        private bool CheckAllowToBuild()
        {
            return buildingTerritory.HasCollisionWithTerritory() == false && buildingTerritory.HasCriticalHeightDifference() == false;
        }

        public void OnBuilded()
        {
            isBuilded = true;
            myColider.enabled = true;
            myObstacle.enabled = true;
            myRenderer.materials = defaultMaterials;
            defaultMaterials = null;
            allowToBuildMaterials = null;
            allowToBuildMaterial = null;
            prohibitedToBuildMaterials = null;
            prohibitedToBuildMaterial = null;
            BuildingsRegistrator.RegisterBuilding(baseBuilding);
            buildingTerritory.enabled = false;
            enabled = false;
        }

        private void OnAllowToBuld()
        {
            myRenderer.materials = allowToBuildMaterials;
        }

        private void OnProhibitedToBuld()
        {
            myRenderer.materials = prohibitedToBuildMaterials;
        }

        public void OnDestroyed()
        {

        }
    }
}