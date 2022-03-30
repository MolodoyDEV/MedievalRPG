using Molodoy.CoreComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Molodoy.Interfaces
{
    [DisallowMultipleComponent]
    public class Hud : MonoBehaviour
    {
        [SerializeField] private GameObject MenuObject;
        [SerializeField] private GameObject TasksMenuObject;
        [SerializeField] private GameObject ManagementMenuObject;
        [SerializeField] private GameObject elementsObject;
        [SerializeField] private Text itemInfo;
        [SerializeField] private GameObject gameHint;
        private IHudInfo preHudInfo;


        private void OnEnable()
        {
            gameHint.SetActive(false);
        }

        public void ToggleGameHint()
        {
            gameHint.SetActive(!gameHint.activeSelf);

            if (gameHint.activeSelf)
            {
                GameProcess.FreezePlayer();
            }
            else
            {
                GameProcess.UnFreezePlayer();
            }
        }

        public void ToggleManagementMenu()
        {
            ManagementMenuObject.SetActive(!ManagementMenuObject.activeSelf);
        }

        public void SetAllElementsState(bool state)
        {
            elementsObject.SetActive(state);
        }

        public void ToggleInGameMenu()
        {
            MenuObject.SetActive(!MenuObject.activeSelf);
        }

        public void CloseInGameMenu()
        {
            MenuObject.SetActive(false);
        }

        public void ToggleTasksMenu()
        {
            TasksMenuObject?.SetActive(!TasksMenuObject.activeSelf);
        }
    }

    public delegate void PlayerHudDelegate(IHudInfo hudInfo);
    public interface IHudInfo
    {
        event PlayerHudDelegate OnItemDestroyed;
        event PlayerHudDelegate OnItemInfoUpdated;
        string GetItemInfo();
    }
}