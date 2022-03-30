using UnityEngine;
using UnityEditor;

namespace Molodoy.Editor.Tools
{
    public class TransformEditor : EditorWindow
    {
        private Vector3 moveVector;
        private Vector3 rotateVector;
        private Vector3 newLosyScale;
        private Transform parent;
        private Transform lookAtTarget;

        [MenuItem("Tools/Transform Editor %#t")]
        static void CreateMoveSelectedObjects()
        {
            GetWindow<TransformEditor>();
        }

        private void OnGUI()
        {
            var selection = Selection.gameObjects;

            EditorGUILayout.LabelField("");
            parent = (Transform)EditorGUILayout.ObjectField("Parent", parent, typeof(Transform), true);

            if (GUILayout.Button("Set parent for selection") && parent != null)
            {
                foreach (GameObject obj in Selection.objects)
                {
                    Undo.RegisterFullObjectHierarchyUndo(obj, "gameobject");
                    obj.transform.parent = parent;
                }
            }


            EditorGUILayout.LabelField("");
            moveVector = EditorGUILayout.Vector3Field("Move by Vector:", moveVector);

            if (GUILayout.Button("Move"))
            {
                foreach (var _object in selection)
                {
                    Undo.RegisterFullObjectHierarchyUndo(_object, "Move Selected Objects");
                    _object.transform.position += moveVector;
                }
            }

            EditorGUILayout.LabelField("");
            rotateVector = EditorGUILayout.Vector3Field("Rotate by Vector:", rotateVector);

            if (GUILayout.Button("Set Rotation"))
            {
                foreach (var _object in selection)
                {
                    Undo.RegisterFullObjectHierarchyUndo(_object, "Set Rotation Selected Objects");
                    _object.transform.rotation = Quaternion.Euler(rotateVector.x, rotateVector.y, rotateVector.z);
                }
            }

            if (GUILayout.Button("Rotate"))
            {
                foreach (var _object in selection)
                {
                    Undo.RegisterFullObjectHierarchyUndo(_object, "Set Rotation Selected Objects");
                    Vector3 currentRotation = _object.transform.rotation.eulerAngles;
                    _object.transform.rotation = Quaternion.Euler(currentRotation.x + rotateVector.x, currentRotation.y + rotateVector.y, currentRotation.z + rotateVector.z);
                }
            }

            EditorGUILayout.LabelField("");
            lookAtTarget = (Transform)EditorGUILayout.ObjectField("Look at target", lookAtTarget, typeof(Transform), true);

            if (GUILayout.Button("Rotate " + Selection.objects.Length + " selected objects to target") && lookAtTarget != null)
            {
                foreach (GameObject obj in Selection.objects)
                {
                    Undo.RegisterFullObjectHierarchyUndo(obj, "gameobject");
                    obj.transform.LookAt(lookAtTarget);
                }
            }

            EditorGUILayout.LabelField("");
            newLosyScale = EditorGUILayout.Vector3Field("New Losy Scale", newLosyScale);

            if (GUILayout.Button("Set Losy Scale"))
            {
                foreach (var _object in selection)
                {
                    Transform objectTransform = _object.transform;
                    Undo.RegisterFullObjectHierarchyUndo(_object.gameObject, "change scale");
                    Vector3 scaleDelta = newLosyScale - objectTransform.lossyScale;
                    float scaleMultiplier = objectTransform.localScale.x / objectTransform.lossyScale.x;

                    objectTransform.localScale += (scaleDelta * scaleMultiplier);
                }
            }

            if (selection.Length != 0)
            {
                Transform objectTransform = selection[0].transform;
                EditorGUILayout.LabelField("Losy Scale " + objectTransform.lossyScale);
                EditorGUILayout.LabelField("Local Rotation Euler " + objectTransform.localRotation.eulerAngles);
                EditorGUILayout.LabelField("Local Position " + objectTransform.localPosition);
            }

            GUI.enabled = false;
            EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
        }
    }
}