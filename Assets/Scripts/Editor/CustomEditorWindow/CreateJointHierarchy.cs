using UnityEditor;
using UnityEngine;

namespace Molodoy.Editor.Tools
{
    public class CreateJointHierarchy : EditorWindow
    {
        private Joint parentJoint;
        private int modeNumber;

        [MenuItem("Tools/Create Joint Hierarchy")]
        public static void CreateHierarchyWindow()
        {
            GetWindow<CreateJointHierarchy>();
        }


        private void OnGUI()
        {
            parentJoint = Selection.activeGameObject?.GetComponent<Joint>();
            EditorGUILayout.ObjectField("Parent Joint", parentJoint, typeof(Joint), true);
            EditorGUILayout.LabelField("0 Rigidbody, 1 ArticulationBody");
            modeNumber = EditorGUILayout.IntSlider(modeNumber, 0, 1);

            if (parentJoint == null)
            {
                return;
            }

            if (GUILayout.Button("Create hierarchy"))
            {
                int iterations = 0;
                Joint startJoint = parentJoint;

                while (startJoint != null && iterations < 100)
                {

                    if (modeNumber == 0)
                    {
                        Rigidbody childRigidbody = null;

                        for (int i = 0; i < startJoint.transform.childCount; i++)
                        {
                            if (startJoint.transform.GetChild(i).TryGetComponent(out childRigidbody))
                            {
                                break;
                            }
                        }

                        Undo.RegisterCompleteObjectUndo(startJoint, "joint");
                        startJoint.connectedBody = childRigidbody;
                        startJoint.connectedArticulationBody = null;
                        startJoint = childRigidbody?.GetComponent<Joint>();
                    }
                    else if (modeNumber == 1)
                    {
                        ArticulationBody childArticulationBody = null;

                        for (int i = 0; i < startJoint.transform.childCount; i++)
                        {
                            if (startJoint.transform.GetChild(i).TryGetComponent(out childArticulationBody))
                            {
                                break;
                            }
                        }

                        Undo.RegisterCompleteObjectUndo(startJoint, "joint");
                        startJoint.connectedArticulationBody = childArticulationBody;
                        startJoint.connectedBody = null;
                        startJoint = childArticulationBody?.GetComponent<Joint>();
                    }

                    iterations++;
                }
            }

            if (GUILayout.Button("Clear hierarchy"))
            {
                Undo.RegisterCompleteObjectUndo(parentJoint, "parent joint");
                parentJoint.connectedBody = null;
                parentJoint.connectedArticulationBody = null;
                int iterations = 0;
                Joint startJoint = parentJoint;

                while (startJoint != null && iterations < 100)
                {
                    Joint childJoint = null;

                    for (int i = 0; i < startJoint.transform.childCount; i++)
                    {
                        if (startJoint.transform.GetChild(i).TryGetComponent(out childJoint))
                        {
                            break;
                        }
                    }

                    if (childJoint)
                    {
                        Undo.RegisterCompleteObjectUndo(childJoint, "joint");
                        childJoint.connectedBody = null;
                        childJoint.connectedArticulationBody = null;
                    }

                    startJoint = childJoint;
                    iterations++;
                }
            }
        }
    }
}