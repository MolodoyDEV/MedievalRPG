using Molodoy.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Molodoy.CoreComponents.Saves
{
    public static class BinarySaveLoader
    {
        private static bool loadGameAtStart;
        private static string saveFilePath;
        private static string loadFilePath;
        private static List<Saveable> saveableObjects;
        private static string saveFileType = ".savedata";
        private static string autoSaveFilePrefix = ".autosave";
        private static string persistentDataPath;
        private static SaveLoader saveLoader;

        public static void Awake(SaveLoader _saveLoader)
        {
            saveLoader = _saveLoader;
            persistentDataPath = Application.persistentDataPath + "/";
        }

        public static void OnApplicationQuit()
        {
            StartAutoSaveGame();
        }

        public static void OnDisable()
        {
            saveLoader.StopAllCoroutines();
        }

        public static bool SaveFileAvailable(string forSceneName)
        {
            return File.Exists(GenerateSaveFilePath(forSceneName));
        }

        public static bool AutoSaveFileAvailable(string forSceneName)
        {
            return File.Exists(GenerateSaveFilePath(forSceneName + autoSaveFilePrefix));
        }

        private static string GetLastSavePath(string forSceneName)
        {
            if (SaveFileAvailable(SceneManager.GetActiveScene().name) && AutoSaveFileAvailable(SceneManager.GetActiveScene().name))
            {
                if (File.GetLastWriteTime(GenerateSaveFilePath(forSceneName)) >= File.GetLastWriteTime(GenerateSaveFilePath(forSceneName + autoSaveFilePrefix)))
                {
                    return GenerateSaveFilePath(forSceneName);
                }
                else
                {
                    return GenerateSaveFilePath(forSceneName + autoSaveFilePrefix);
                }
            }
            else if (SaveFileAvailable(SceneManager.GetActiveScene().name))
            {
                return GenerateSaveFilePath(forSceneName);
            }
            else if (AutoSaveFileAvailable(SceneManager.GetActiveScene().name))
            {
                return GenerateSaveFilePath(forSceneName + autoSaveFilePrefix);
            }
            else
            {
                throw new Exception("Save for scene not found");
            }
        }

        public static string GenerateSaveFilePath(string fileName)
        {
            return persistentDataPath + "saves/" + fileName + saveFileType;
        }

        public static void DeleteSave(string forSceneName)
        {
            File.Delete(GenerateSaveFilePath(forSceneName));
            File.Delete(GenerateSaveFilePath(forSceneName + autoSaveFilePrefix));
            Debug.LogWarning("Deleted all saves for scene " + forSceneName);
            loadGameAtStart = false;
        }

        public static void LoadGameAtStart(bool loadAtStart) => loadGameAtStart = loadAtStart;

        public static bool NeedLoadGameAtStart() => loadGameAtStart;

        public static Coroutine StartSaveGame()
        {
            if (saveLoader)
            {
                saveFilePath = GenerateSaveFilePath(SceneManager.GetActiveScene().name);
                return saveLoader.StartCoroutine(SaveGameCoroutine());
            }

            return null;
        }

        public static void StartAutoSaveGame()
        {
            saveFilePath = GenerateSaveFilePath(SceneManager.GetActiveScene().name + autoSaveFilePrefix);
            SaveGame();
        }

        public static Coroutine StartLoadGame()
        {
            loadFilePath = GetLastSavePath(SceneManager.GetActiveScene().name);
            return saveLoader.StartCoroutine(LoadGameCoroutine());
        }

        private static IEnumerator SaveGameCoroutine()
        {
            //PlayerCore.OnGameSaved();
            yield return new WaitForEndOfFrame();

            SaveGame();

            yield return new WaitForEndOfFrame();
            yield break;
        }

        private static void SaveGame()
        {
            if (Directory.Exists(persistentDataPath + "saves") == false)
            {
                Directory.CreateDirectory(persistentDataPath + "saves");
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(saveFilePath, FileMode.Create);

            GameSave gameSave = new GameSave();

            //if (saveLoader.saveableObjectsParent != null)
            //{
            //    saveableObjects = saveLoader.saveableObjectsParent.GetComponentsInChildren<Saveable>().ToList();
            //}
            //else
            //{
            saveableObjects = MonoBehaviour.FindObjectsOfType<Saveable>().ToList();
            //}

            saveableObjects = saveableObjects.SortBySaveableStandart();
            gameSave.SaveObjects(saveableObjects);

            binaryFormatter.Serialize(fileStream, gameSave);

            fileStream.Close();

            Debug.LogWarning("Game saved to " + saveFilePath);
        }

        private static IEnumerator LoadGameCoroutine()
        {
            yield return new WaitForEndOfFrame();

            LoadGame();

            yield break;
        }

        private static void LoadGame()
        {
            if (File.Exists(loadFilePath))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(loadFilePath, FileMode.Open);

                GameSave savedGame = (GameSave)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();

                //if(saveLoader.saveableObjectsParent != null)
                //{
                //    saveableObjects = saveLoader.saveableObjectsParent.GetComponentsInChildren<Saveable>().ToList();
                //}
                //else
                //{
                saveableObjects = MonoBehaviour.FindObjectsOfType<Saveable>().ToList();
                //}

                saveableObjects = saveableObjects.SortBySaveableStandart();
                savedGame.LoadObjects(saveableObjects);

                Debug.LogWarning("Game loaded from " + loadFilePath);
            }
            else
            {
                Debug.LogWarning("Save not found on " + loadFilePath);
            }
        }

        public static Saveable CreateObject(string objectTypeTag)
        {
            GameObject prefab = null;

            foreach (GameObject objectPrefab in saveLoader.InstantiatableObjectsPrefabs)
            {
                if (objectPrefab.tag == objectTypeTag)
                {
                    prefab = objectPrefab;
                    break;
                }
            }

            if (prefab == null)
            {
                string logMessage = objectTypeTag + " not found in DestroyableObjectsPrefab list";
                throw new Exception(logMessage);
            }

            return prefab.Instantiate().GetComponent<Saveable>();

        }
    }
}