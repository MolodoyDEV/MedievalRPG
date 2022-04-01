using Molodoy.CoreComponents;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Management.BuildingSystem
{
    [DisallowMultipleComponent]
    [SelectionBase]
    public class BuildingTerritory : MonoBehaviour
    {
        [SerializeField] private int territoryGroup;
        [SerializeField] private float maxHeightDifference = 3f;
        [SerializeField] private bool ignoreCollision;
        private float hightPointsAdditionalY = 1f;
        private Transform myTransform;
        private LayerMask collisionLayerMask;
        private LayerMask heightLayerMask;
        private Transform[] heightPoints = null;
        RaycastHit hit;

        public int TerritoryGroup { get => territoryGroup;}
        public float MaxHeightDifference { get => maxHeightDifference; }

        private void Awake()
        {
            collisionLayerMask.value = ~0;
            collisionLayerMask.value &= ~(1 << GameConstants.Layer_GroundNumber);
            heightLayerMask.value = 0;
            heightLayerMask.value ^= 1 << GameConstants.Layer_GroundNumber;
            myTransform = transform;

            CreateHeightPoints();
        }

        private void OnDisable()
        {
            for (int i = 0; i < heightPoints.Length; i++)
            {
                Destroy(heightPoints[i].gameObject);
            }

            heightPoints = null;
        }

        private void CreateHeightPoints()
        {
            heightPoints = new Transform[4];
            Vector3[] heightPointsPositions = GetBottomBorderPoints();

            for (int i = 0; i < heightPoints.Length; i++)
            {
                Transform newHeightPoint = new GameObject("HeightPoint" + (i + 1)).transform;
                newHeightPoint.gameObject.layer = GameConstants.Layer_TransparentFXNumber;
                newHeightPoint.position = heightPointsPositions[i];
                newHeightPoint.parent = myTransform;
                heightPoints[i] = newHeightPoint;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //if (heightPoints != null)
            //{
            //    Gizmos.color = Color.red;
            //    Gizmos.DrawSphere(heightPoints[0].position, 0.5f);
            //    Gizmos.DrawSphere(heightPoints[1].position, 0.5f);
            //    Gizmos.DrawSphere(heightPoints[2].position, 0.5f);
            //    Gizmos.DrawSphere(heightPoints[3].position, 0.5f);
            //}


            Gizmos.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 1f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
#endif

        public bool HasCollisionWithTerritory()
        {
            if (ignoreCollision)
            {
                return false;
            }

            if (Physics.CheckBox(GetCenter(), myTransform.lossyScale / 2, myTransform.rotation, collisionLayerMask))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //public bool HasCriticalHeightDifference()
        //{
        //    if(maxHeightDifference == Mathf.Infinity)
        //    {
        //        return false;
        //    }

        //    Vector2 minMaxHeight = GetMinAndMaxHeiht();

        //    return ((minMaxHeight.y - minMaxHeight.x) > MaxHeightDifference) || (minMaxHeight.y == Mathf.Infinity && minMaxHeight.x == Mathf.Infinity);
        //}

        public Vector2 GetMinAndMaxHeiht()
        {
            float minHeight = Mathf.Infinity;
            float maxHeight = 0;

            foreach (Transform point in heightPoints)
            {
                Debug.DrawLine(point.position, new Vector3(point.position.x, -10000f, point.position.z), Color.red);
                Ray ray = new Ray(point.position, new Vector3(point.position.x, -10000f, point.position.z));

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

            return new Vector2(minHeight, maxHeight);
        }

        private Vector3[] GetBottomBorderPoints()
        {
            return new Vector3[] {
                GetLeftBottomPoint(),
                new Vector3(GetLeftBottomPoint().x, GetLeftBottomPoint().y, GetLeftBottomPoint().z + myTransform.lossyScale.z),
                new Vector3(GetLeftBottomPoint().x + myTransform.lossyScale.x, GetLeftBottomPoint().y, GetLeftBottomPoint().z + myTransform.lossyScale.z),
                new Vector3(GetLeftBottomPoint().x + myTransform.lossyScale.x, GetLeftBottomPoint().y, GetLeftBottomPoint().z),
                };
        }

        private Vector3 GetCenter()
        {
            return transform.position;
        }

        private Vector3 GetLeftBottomPoint()
        {
            Vector3 vector3 = (GetCenter() - transform.lossyScale / 2);
            vector3.y += hightPointsAdditionalY;
            return vector3;
        }

        private Vector3 GetRightTopPoint()
        {
            Vector3 vector3 = (GetCenter() + transform.lossyScale / 2);
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