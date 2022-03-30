using System;
using UnityEngine;

namespace Molodoy.Debugging
{
    [DisallowMultipleComponent]
    public class LogHandler : MonoBehaviour
    {
        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType logType)
        {
            if (logType != LogType.Log)
            {
                InGameConsole.WriteLogLine(logString, stackTrace, logType);
            }
            
            LogCollector.WriteLog(logString, stackTrace, logType);
        }
    }
}