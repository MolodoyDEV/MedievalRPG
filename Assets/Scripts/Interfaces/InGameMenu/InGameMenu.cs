using Assets.Scripts.UI;
using Molodoy.CoreComponents;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;

namespace Molodoy.Interfaces
{
    [DisallowMultipleComponent]
    public class InGameMenu : MonoBehaviour, IWindow
    {
        [SerializeField] private Text mouseSpeedValue;
        [SerializeField] private Slider mouseSensitivitySlider;
        [SerializeField] private Toggle fpsToggle;
        [SerializeField] private FPSCounter fPSCounter;
        private static InGameMenu instance;
        public delegate void MouseSpeed(int newSpeed);
        public static event MouseSpeed MouseSpeedChanged;

        private void Awake()
        {
            instance = this;
            OnMouseSpeedChanged();
            fPSCounter = FindObjectOfType<FPSCounter>();
            OnFpsToggle();

            if (fPSCounter == false)
            {
                fpsToggle.interactable = false;
                fpsToggle.isOn = false;
            }
        }

        private void OnEnable()
        {
            GameProcess.FreezeGame(GetHashCode());
            CursorManager.SetCursorState(GetHashCode(), true, CursorLockMode.Confined);
        }

        private void OnDisable()
        {
            CursorManager.ForgetCursorState(GetHashCode());
            GameProcess.UnFreezeGame(GetHashCode());
        }

        public void OnFpsToggle()
        {
            if (fPSCounter)
            {
                fPSCounter.enabled = fpsToggle.isOn;
            }
        }

        private void OnSwitchingScene()
        {
            SaveGame();
            CloseWindow();
        }

        //Кнопка Exit to Main Menu в InGameManu 
        public void SaveAndExit()
        {
            OnSwitchingScene();
            //SceneTransition.SwitchToScene(GameConstants.Scene_MainMenuName);
        }

        public void CloseWindow()
        {
            gameObject.SetActive(false);
        }

        public void OnMouseSpeedChanged()
        {
            int newMouseSpeed = Mathf.RoundToInt(mouseSensitivitySlider.value);
            //GameSettings.SetPlayerMouseSpeed(newMouseSpeed);
            mouseSpeedValue.text = newMouseSpeed.ToString();
            MouseSpeedChanged?.Invoke(newMouseSpeed);
        }

        //Кнопка Reload в InGameManu 
        public void ReloadScene()
        {
            SceneLoader.ReloadCurrentScene();
        }

        //Кнопка в InGameManu 
        public void SceneForTest()
        {
            OnSwitchingScene();
            //SceneTransition.SwitchToScene(GameConstants.Scene_ForTestName);
        }

        //Кнопка Save в InGameManu 
        public void SaveGame()
        {
            // BinarySaveLoader.StartSaveGame();
        }

        //Кнопка Load в InGameManu 
        public void LoadGame()
        {
            //BinarySaveLoader.LoadGameAtStart(true);
            SceneLoader.ReloadCurrentScene();
        }

        public void OpenWindow()
        {
            throw new System.NotImplementedException();
        }
    }
}