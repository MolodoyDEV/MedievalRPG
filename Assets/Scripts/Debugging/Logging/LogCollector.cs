using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Molodoy.Debugging
{
    public static class LogCollector
    {
        private static Queue<string> lastLogs = new Queue<string>();

        public static void OnApplicationQuit()
        {
            WriteLogInFileAsync("=====================================================");

            while (lastLogs.Count != 0)
            {
                WriteLogInFileAsync(lastLogs.Dequeue());
            }
        }

        public static void WriteLog(string logString, string stackTrace, LogType logType)
        {
            //if (logType == LogType.Log) { return; }

            string newString = DateTime.Now + " [" + logType + "] : " + logString;

            if (logType == LogType.Exception)
            {
                newString += "\n" + stackTrace;
            }

            lastLogs.Enqueue(newString);
        }

        private static void WriteLogInFileAsync(string logString)
        {
            string persistentDataPath = Application.persistentDataPath;

            if (Directory.Exists(persistentDataPath + "/logs") == false)
            {
                Directory.CreateDirectory(persistentDataPath + "/logs");
            }

            string logFilePath = $"{persistentDataPath}/logs/game_version_{Application.version}_log.txt";
            StreamWriter streamWriter;

            if (File.Exists(logFilePath))
            {
                streamWriter = new StreamWriter(logFilePath, true, System.Text.Encoding.Default);
            }
            else
            {
                streamWriter = new StreamWriter(logFilePath, false, System.Text.Encoding.Default);
            }

            //await streamWriter.WriteLineAsync(logString);
            streamWriter.WriteLine(logString);

            streamWriter.Close();
        }
    }
}
