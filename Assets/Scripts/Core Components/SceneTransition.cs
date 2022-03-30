using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Molodoy.CoreComponents
{
    [DisallowMultipleComponent]
    public class SceneTransition : MonoBehaviour
    {
        public static SceneTransition instance;
        private AsyncOperation asyncOperation;
        public Text LoadingProcentage;
        public GameObject LoadingScreen;
        private int totalLoadingSteps;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            LoadingScreen.SetActive(false);
        }

        void Update()
        {
            if (asyncOperation != null)
            {
                LoadingProcentage.text = "1/2 Loading level... " + Mathf.RoundToInt(asyncOperation.progress * 100) + "%";
            }
        }

        public static void SwitchToScene(string sceneName)
        {
            instance.LoadingScreen.SetActive(true);
            Resources.UnloadUnusedAssets();
            InitializeSceneProperties();
            instance.asyncOperation = SceneLoader.LoadSceneAsync(sceneName);
            Debug.Log($"------------Loading Scene {sceneName}------------");
        }

        public static void OnObjectInitializationStarted(int totalSteps)
        {
            instance.totalLoadingSteps = totalSteps;
            instance.LoadingProcentage.text = "2/2 Loading objects... 0%";
            instance.LoadingScreen.SetActive(true);
            ObjectsInitializer.ObjectLoadingStagePassed.AddListener(instance.IncreaseProgressBar);
            ObjectsInitializer.AllObjectsInitialized.AddListener(instance.OnLoadingComplete);
        }

        private void IncreaseProgressBar(int progress)
        {
            LoadingProcentage.text = "2/2 Loading objects... " + Mathf.RoundToInt(100f * (progress / totalLoadingSteps)) + "%";
        }

        private void OnLoadingComplete()
        {
            ObjectsInitializer.ObjectLoadingStagePassed.RemoveListener(IncreaseProgressBar);
            ObjectsInitializer.AllObjectsInitialized.RemoveListener(OnLoadingComplete);
            LoadingScreen.SetActive(false);
        }

        public static void InitializeSceneProperties()
        {
            SceneProperties sceneProperties = GamePropertiesConstants.GetScenePropertiesOrDefault(SceneManager.GetActiveScene().name);
            CursorManager.SetBaseCursorState(instance.GetHashCode(), sceneProperties.CursorProperties.IsVisible, sceneProperties.CursorProperties.LockMode);
            GameProcess.SetBaseGameSpeed(sceneProperties.GameSpeed, instance.GetHashCode());
        }
    }
}