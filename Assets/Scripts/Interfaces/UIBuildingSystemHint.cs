using Assets.Scripts.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public class UIBuildingSystemHint : MonoBehaviour, IWindow
    {
        [SerializeField] private Transform hintWindow;

        public void CloseWindow()
        {
            hintWindow.gameObject.SetActive(false);
        }

        public void OpenWindow()
        {
            hintWindow.gameObject.SetActive(true);
        }
    }
}