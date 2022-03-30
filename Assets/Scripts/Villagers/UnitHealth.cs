using Molodoy.Inspector.Extentions;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Villagers
{
    [Serializable]
    public class UnitHealth : MonoBehaviour
    {
        [HideInInspector] public UnityEvent VillagerDeath;
        [HideInInspector] public UnityEvent<int> HealthChange;
        [ReadOnlyInspector] [SerializeField] private bool isDeath;
        [SerializeField] private int maximumHealth;
        [SerializeField] private int currentHealth;

        public bool IsDeath { get => isDeath; private set => isDeath = value; }
        public int MaximumHealth { get => maximumHealth; private set => maximumHealth = value; }
        public int CurrentHealth { get => currentHealth; private set => currentHealth = value; }

        private void Awake()
        {
            CurrentHealth = MaximumHealth;
            isDeath = false;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDeath();
            }
        }

        private void OnDeath()
        {
            isDeath = true;
            VillagerDeath?.Invoke();
        }

        private void OnHealthChanged()
        {
            HealthChange?.Invoke(currentHealth);
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= Mathf.RoundToInt(damage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDeath();
            }

            OnHealthChanged();
        }

        public void TakeHeal(int heal)
        {
            currentHealth += heal;

            if (currentHealth > maximumHealth)
            {
                currentHealth = maximumHealth;
            }

            OnHealthChanged();
        }
    }
}