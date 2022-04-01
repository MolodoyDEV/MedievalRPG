using UnityEngine;

namespace Assets.Scripts.Management.BuildingSystem
{
    [RequireComponent(typeof(Collider))]
    public class BuildingPermitVisualizer : MonoBehaviour
    {
        [SerializeField] private Material allowToBuildMaterial;
        [SerializeField] private Material prohibitedToBuildMaterial;
        private Material[] defaultMaterials;
        private Material[] prohibitedToBuildMaterials;
        private Material[] allowToBuildMaterials;
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
        }

        public void Initialize()
        {
            Awake();
            //myObstacle = GetComponent<NavMeshObstacle>();
            //myObstacle.shape = NavMeshObstacleShape.Box;
            //myObstacle.carving = true;

            prohibitedToBuildMaterials = new Material[defaultMaterials.Length];
            allowToBuildMaterials = new Material[defaultMaterials.Length];

            for (int i = 0; i < defaultMaterials.Length; i++)
            {
                prohibitedToBuildMaterials[i] = prohibitedToBuildMaterial;
                allowToBuildMaterials[i] = allowToBuildMaterial;
            }
        }

        private void OnDisable()
        {
            //myObstacle.enabled = true;
            myColider.enabled = true;
            myRenderer.materials = defaultMaterials;
            defaultMaterials = null;
            allowToBuildMaterials = null;
            allowToBuildMaterial = null;
            prohibitedToBuildMaterials = null;
            prohibitedToBuildMaterial = null;
        }

        public void OnAllowToBuld()
        {
            myRenderer.materials = allowToBuildMaterials;
        }

        public void OnProhibitedToBuld()
        {
            myRenderer.materials = prohibitedToBuildMaterials;
        }

        public void OnDestroyed()
        {

        }
    }
}