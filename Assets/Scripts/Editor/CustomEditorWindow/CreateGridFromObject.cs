using UnityEngine;
using UnityEditor;
using System;

namespace Molodoy.Editor.Tools
{
    public class CreateGridFromObject : EditorWindow
    {
        [SerializeField] private GameObject firstObject;
        [SerializeField] private GameObject objectPrefab;
        [SerializeField] private int gridHeight = 1;
        [SerializeField] private int gridWidth = 1;
        [SerializeField] private float gridHeightStep = 0.01f;
        [SerializeField] private float gridWidthStep = 0.01f;
        [SerializeField] private bool heightLadder;
        [SerializeField] private bool widthLadder;
        [SerializeField] private float heightLadderStep = 0.01f;
        [SerializeField] private float widthLadderStep = 0.01f;

        [MenuItem("Tools/Create grid from object %#g")]
        public static void CreateGridFromObjectWindow()
        {
            GetWindow<CreateGridFromObject>();
        }

        private void OnGUI()
        {
            objectPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab (not necessary)", objectPrefab, typeof(GameObject), true);
            Debug.LogWarning(PrefabUtility.GetPrefabAssetType(objectPrefab));

            gridWidth = EditorGUILayout.IntField("Grid width:", gridWidth);
            if (gridWidth > 1)
            {
                gridWidthStep = EditorGUILayout.FloatField("   Grid width step:", gridWidthStep);
            }
            else
            {
                gridWidth = 1;
            }

            gridHeight = EditorGUILayout.IntField("Grid height:", gridHeight);
            if (gridHeight > 1)
            {
                gridHeightStep = EditorGUILayout.FloatField("   Grid height step:", gridHeightStep);

            }

            heightLadder = EditorGUILayout.Toggle("Create height ladder", heightLadder);
            if (heightLadder)
            {
                heightLadderStep = EditorGUILayout.FloatField("   Height ladder step:", heightLadderStep);
            }

            widthLadder = EditorGUILayout.Toggle("Create width ladder", widthLadder);
            if (widthLadder)
            {
                widthLadderStep = EditorGUILayout.FloatField("   Width ladder step:", widthLadderStep);
            }

            if (Selection.gameObjects.Length != 0)
            {
                firstObject = Selection.gameObjects[0];
            }
            else
            {
                firstObject = null;
            }

            if (GUILayout.Button("Create grid"))
            {
                if (firstObject == null)
                {
                    Debug.LogError("No one object selected");
                }
                else if (gridHeight <= 0 || gridWidth <= 0)
                {
                    Debug.LogError("The value must be greater than zero");
                }
                else if (gridHeight == 1 && gridWidth == 1)
                {

                }
                else
                {
                    GameObject newObject;
                    GameObject objectToClone;
                    Transform firstObjectTransform = firstObject.transform;
                    int siblingIndex = firstObjectTransform.GetSiblingIndex() + 1;

                    if (objectPrefab)
                    {
                        objectToClone = objectPrefab;
                    }
                    else
                    {
                        objectToClone = firstObject;
                    }

                    float yPos = firstObject.transform.position.y;
                    float xPos = firstObject.transform.position.x;
                    float zPos = firstObject.transform.position.z;

                    for (int heightCell = 1; heightCell <= gridHeight; heightCell++)
                    {
                        for (int widthCell = 1; widthCell <= gridWidth; widthCell++)
                        {
                            if (heightCell != 1 || widthCell != 1)
                            {
                                newObject = Instantiate(objectToClone, new Vector3(xPos, yPos, zPos), firstObjectTransform.rotation, firstObjectTransform.parent);
                                newObject.transform.SetSiblingIndex(siblingIndex);
                                Undo.RegisterCreatedObjectUndo(newObject, "CreateGridFromObject");

                                siblingIndex++;
                            }

                            if (widthLadder)
                            {
                                yPos += widthLadderStep;//
                            }

                            xPos += gridWidthStep;

                            if (widthCell == gridWidth)
                            {
                                xPos = firstObject.transform.position.x;
                            }
                        }

                        if (heightLadder)
                        {
                            yPos += heightLadderStep;//
                        }
                        else
                        {
                            yPos = firstObject.transform.position.y;//
                        }

                        zPos -= gridHeightStep;
                    }
                }
            }


            GUI.enabled = false;
            EditorGUILayout.LabelField("Objects in grid: " + gridHeight * gridWidth);

            if (firstObject == null)
            {
                EditorGUILayout.LabelField("Object for make grid: NONE");
            }
            else
            {
                EditorGUILayout.LabelField("Object for make grid: " + firstObject.name);
            }
        }
    }
}
