using System;
using UnityEditor;
using System.Reflection;

namespace Molodoy.Inspector.Extentions
{
    public static class ConsoleManager
    {
        private static Assembly assembly = Assembly.GetAssembly(typeof(SceneView));

        [MenuItem("Tools/Clear Console %#l")]
        public static void ClearLog()
        {
            Type type = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }
}