using Molodoy.CoreComponents;
using Molodoy.Extensions;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Molodoy.Debugging
{
    [DisallowMultipleComponent]
    public class InGameConsole : MonoBehaviour
    {
        [SerializeField] private int consoleMaxChars = 18000;
        [SerializeField] private Text content;
        [SerializeField] private GameObject window;
        private static InGameConsole instance;

        private void Awake()
        {
            instance = this;
            window.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
                window.SetActive(!window.activeSelf);

                if (window.activeSelf)
                {
                    CursorManager.SetCursorState(GetHashCode(), true, CursorLockMode.Confined);
                }
                else
                {
                    CursorManager.ForgetCursorState(GetHashCode());
                }
            }
        }

        public static void WriteLogLine(string logString, string stackTrace, LogType logType)
        {
            //if (instance.content.text.Length - instance.content.text.Replace("\n", "").Length == instance.consoleMaxLines)
            //{
            //    instance.content.text = instance.content.text.RemoveLastLine("\n");
            //}

            if (instance == null) { return; }

            if (instance.content.text.Length > instance.consoleMaxChars)
            {
                instance.content.text = instance.content.text.Substring(0, instance.consoleMaxChars);
                instance.content.text = instance.content.text.RemoveLastLine("\n");
            }

            string newString = DateTime.Now + " [" + logType + "] : " + logString;
            string color = "gray";

            if (logType == LogType.Exception)
            {
                newString += "\n" + stackTrace;
                color = "red";
            }
            else if (logType == LogType.Error)
            {
                color = "red";
            }
            else if (logType == LogType.Warning)
            {
                color = "yellow";
            }

            newString = $"<color={color}>{newString}</color>";
            instance.content.text = newString + "\n" + instance.content.text;
        }
    }
}