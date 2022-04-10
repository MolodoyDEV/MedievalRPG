using Assets.Scripts.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Molodoy.Interfaces
{
    public class UIPageSelection : MonoBehaviour
    {
        [SerializeField] private List<GameObject> pages;

        private void Awake()
        {
            DeselectAllPages();
        }

        public void DeselectAllPages()
        {
            foreach (GameObject page in pages)
            {
                page.SetActive(false);
            }
        }
    }
}