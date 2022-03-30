using Molodoy.CoreComponents;
using UnityEngine;

namespace Assets.Scripts.Interfaces.InGameMenu
{
    [DisallowMultipleComponent]
    public class InGameManagementMenu : MonoBehaviour
    {
        [SerializeField] private GameObject defaultElements;

        private void Awake()
        {
            defaultElements.SetActive(true);
        }

        private void OnEnable()
        {
            CursorManager.SetCursorState(GetHashCode(), true, CursorLockMode.Confined);
            GameProcess.FreezeGame(GetHashCode());
        }

        private void OnDisable()
        {
            CursorManager.ForgetCursorState(GetHashCode());
            GameProcess.UnFreezeGame(GetHashCode());
        }

        public void CloseWindow()
        {
            gameObject.SetActive(false);
        }
    }
}