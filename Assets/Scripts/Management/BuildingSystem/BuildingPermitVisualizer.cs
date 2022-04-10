using UnityEngine;

namespace Assets.Scripts.Management.BuildingSystem
{
    [RequireComponent(typeof(Collider))]
    public class BuildingPermitVisualizer : MonoBehaviour
    {
        [SerializeField] private Material allowToBuildMaterial;
        [SerializeField] private Material prohibitedToBuildMaterial;
        [SerializeField] private Material unfinishedBuldingMaterial;
        private Material[] defaultMaterials;
        private Material[] prohibitedToBuildMaterials;
        private Material[] allowToBuildMaterials;
        private Material[] unfinishedBuldingMaterials;
        private Collider myColider;
        private MeshRenderer myRenderer;
        //private NavMeshObstacle myObstacle;

        private void Awake()
        {
            myColider = GetComponent<Collider>();
            myRenderer = GetComponentInChildren<MeshRenderer>();

            //for wrong prefabs
            if (myRenderer == null)
            {
                myRenderer = GetComponent<MeshRenderer>();
            }

            defaultMaterials = myRenderer.materials;
            myColider.enabled = false;

            prohibitedToBuildMaterials = new Material[defaultMaterials.Length];
            allowToBuildMaterials = new Material[defaultMaterials.Length];
            unfinishedBuldingMaterials = new Material[defaultMaterials.Length];

            for (int i = 0; i < defaultMaterials.Length; i++)
            {
                prohibitedToBuildMaterials[i] = prohibitedToBuildMaterial;
                allowToBuildMaterials[i] = allowToBuildMaterial;
                unfinishedBuldingMaterials[i] = unfinishedBuldingMaterial;
            }
        }

        private void OnDisable()
        {
            //myObstacle.enabled = true;
            defaultMaterials = null;
            allowToBuildMaterials = null;
            allowToBuildMaterial = null;
            prohibitedToBuildMaterials = null;
            prohibitedToBuildMaterial = null;
            unfinishedBuldingMaterial = null;
            unfinishedBuldingMaterials = null;
        }

        public void OnAllowToBuild()
        {
            myRenderer.materials = allowToBuildMaterials;
        }

        public void OnProhibitedToBuild()
        {
            myRenderer.materials = prohibitedToBuildMaterials;
        }

        public void OnDestroyed()
        {

        }

        public void OnFullyBuilded()
        {
            myColider.enabled = true;
            myRenderer.materials = defaultMaterials;
        }

        public void OnUnfinishedBuildingPlaced()
        {
            myColider.enabled = true;
            myRenderer.materials = unfinishedBuldingMaterials;
        }
    }
}