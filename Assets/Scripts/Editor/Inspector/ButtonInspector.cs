using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Molodoy.Inspector.Extentions
{
    /// <summary>
    /// Draw button in Inspector
    /// </summary>
    [CustomEditor(typeof(Object), true, isFallback = false)]
    [CanEditMultipleObjects]
    public class ButtonInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            foreach (var target in targets)
            {
                var mis = target.GetType().GetMethods().Where(m => m.GetCustomAttributes().Any(a => a.GetType() == typeof(InspectorButtonAttribute)));
                if (mis != null)
                {
                    foreach (var mi in mis)
                    {
                        if (mi != null)
                        {
                            var attribute = (InspectorButtonAttribute)mi.GetCustomAttribute(typeof(InspectorButtonAttribute));
                            if (GUILayout.Button(attribute.name))
                            {
                                mi.Invoke(target, null);
                            }
                        }
                    }
                }
            }
        }
    }
}