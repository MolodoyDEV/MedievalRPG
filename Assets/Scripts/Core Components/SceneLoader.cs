using UnityEngine;
using UnityEngine.SceneManagement;

namespace Molodoy.CoreComponents
{
    public static class SceneLoader
    {
        public static AsyncOperation LoadSceneAsync(string sceneName)
        {
            return SceneManager.LoadSceneAsync(sceneName);
        }

        public static AsyncOperation ReloadCurrentScene()
        {
            Debug.Log($"Scene {SceneManager.GetActiveScene().name} reloaded");
            return LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }
}