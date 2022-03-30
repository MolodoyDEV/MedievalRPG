using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class UnitPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshPro nameUI;
        [SerializeField] private TextMeshPro healthUI;

        public void SetName(string name)
        {
            nameUI.text = name;
        }

        public void SetHealth(string health)
        {
            healthUI.text = health;
        }

        public void SetHealth(int health)
        {
            healthUI.text = health.ToString();
        }

        public void OnUnitDeath()
        {
            healthUI.enabled = false;
            nameUI.enabled = false;
        }
    }
}