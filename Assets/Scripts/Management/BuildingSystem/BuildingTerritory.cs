using Molodoy.CoreComponents;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [SelectionBase]
    [CreateAssetMenu(fileName = "BuildingTerritory", menuName = "Utils")]
    public class BuildingTerritory : MonoBehaviour
    {
        [SerializeField] private float maxHeightDifference = 1f;
        private float hightPointsAdditionalY = 0f;
        private Transform myTransform;
        private LayerMask collisionLayerMask;
        private LayerMask heightLayerMask;
        RaycastHit hit;

        private void Awake()
        {
            collisionLayerMask.value = ~0;
            collisionLayerMask.value &= ~(1 << GameConstants.Layer_GroundNumber);
            heightLayerMask.value = 0;
            heightLayerMask.value ^= 1 << GameConstants.Layer_GroundNumber;
            myTransform = transform;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(GetLeftBottomPoint(), 0.5f);
            Gizmos.DrawSphere(new Vector3(GetLeftBottomPoint().x, GetLeftBottomPoint().y, GetLeftBottomPoint().z + transform.lossyScale.z), 0.5f);
            Gizmos.DrawSphere(new Vector3(GetLeftBottomPoint().x + transform.lossyScale.x, GetLeftBottomPoint().y, GetLeftBottomPoint().z + transform.lossyScale.z), 0.5f);
            Gizmos.DrawSphere(new Vector3(GetLeftBottomPoint().x + transform.lossyScale.x, GetLeftBottomPoint().y, GetLeftBottomPoint().z), 0.5f);
            //Gizmos.DrawSphere(new Vector3(matrix[0, 0], matrix[0, 1], matrix[0, 2]), 1f);
            //Gizmos.DrawSphere(new Vector3(matrix[1, 0], matrix[1, 1], matrix[1, 2]), 1f);
            //Gizmos.DrawSphere(new Vector3(matrix[2, 0], matrix[2, 1], matrix[2, 2]), 1f);
            //Gizmos.DrawSphere(new Vector3(matrix[3, 0], matrix[3, 1], matrix[3, 2]), 1f);

            Gizmos.color = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.1f);
            Gizmos.DrawCube(transform.position, transform.lossyScale);
        }
#endif

        public bool HasCollisionWithTerritory()
        {
            if (Physics.CheckBox(GetCenter(), myTransform.lossyScale / 2, myTransform.rotation, collisionLayerMask))
            {
                Debug.LogWarning("Collision box");
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasCriticalHeightDifference()
        {
            float minHeight = Mathf.Infinity;
            float maxHeight = 0;

            foreach (Vector3 point in GetBottomBorderPoints())
            {
                Debug.DrawLine(point, new Vector3(point.x, -10000f, point.z), Color.red);
                Ray ray = new Ray(point, new Vector3(point.x, -10000f, point.z));

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, heightLayerMask))
                {
                    if (hit.distance < minHeight)
                    {
                        minHeight = hit.distance;
                    }

                    if (hit.distance > maxHeight)
                    {
                        maxHeight = hit.distance;
                    }
                }
                else
                {
                    maxHeight = Mathf.Infinity;
                }
            }

            Debug.LogWarning("Max " + maxHeight);
            Debug.LogWarning("Min " + minHeight);
            Debug.LogWarning("======");
            return ((maxHeight - minHeight) > maxHeightDifference) || (maxHeight == Mathf.Infinity && minHeight == Mathf.Infinity);
        }

        public Vector3[] GetBottomBorderPoints()
        {
            return new Vector3[] {
                GetLeftBottomPoint(),
                new Vector3(GetLeftBottomPoint().x, GetLeftBottomPoint().y, GetLeftBottomPoint().z + myTransform.lossyScale.z),
                new Vector3(GetLeftBottomPoint().x + myTransform.lossyScale.x, GetLeftBottomPoint().y, GetLeftBottomPoint().z + myTransform.lossyScale.z),
                new Vector3(GetLeftBottomPoint().x + myTransform.lossyScale.x, GetLeftBottomPoint().y, GetLeftBottomPoint().z),
                };
        }

        public Vector3 GetCenter()
        {
            return transform.position;
        }

        public Vector3 GetLeftBottomPoint()
        {
            Vector3 vector3 = GetCenter() - transform.lossyScale / 2;
            vector3.y += hightPointsAdditionalY;
            return vector3;
        }

        public Vector3 GetRightTopPoint()
        {
            Vector3 vector3 = GetCenter() + transform.lossyScale / 2;
            vector3.y += hightPointsAdditionalY;
            return vector3;
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Utils/BuildingTerritory", false, 10)]
        private static void CreateBuildingTerritoryFromEditor(MenuCommand menuCommand)
        {
            if (Selection.activeObject)
            {
                GameObject buildingTerritory = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("BuildingTerritory", typeof(GameObject)));
                GameObjectUtility.SetParentAndAlign(buildingTerritory, menuCommand.context as GameObject);
                Undo.RegisterCreatedObjectUndo(buildingTerritory, "Create " + buildingTerritory.name);
                Selection.activeObject = buildingTerritory;
            }
            else
            {
                Debug.LogError("Select parent before creating a BuildingTerritory!");
            }
        }
#endif
    }
}