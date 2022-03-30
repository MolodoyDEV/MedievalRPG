using UnityEngine;
using System;
using Molodoy.Extensions;

namespace Assets.Scripts.Units
{
    public enum Classes { Warrior, Swordman, Bowman}
    public enum DamageTypes { Physical, Magical }

    [Serializable]
    [CreateAssetMenu(fileName = "New UnitCharacteristicValues", menuName = "UnitCharacteristicValues")]
    public class UnitCharacteristicValues : ScriptableObject
    {
        public float RotationSpeed;
        public float MovementSpeed;
        public float RunSpeedMultiplier;
        public int MaximumHealth;
        public FloatRange AttackRange;
        public float AttackSpeedSeconds;
        [Range(0f, 100f)]
        public float PhysicalDamageResistPercent;
        [Range(0f, 100f)]
        public float MagicDamageResistPercent;
        [Range(0f, 100f)]
        public float BlockChainsPercent;
        [Range(0f, 100f)]
        public float EvasionChainsPercent;
        [Range(0f, 100f)]
        public float CritChainsPercent;
        public float HealMultiplier;
        public float ReceiveHealMultiplier;
        public float CritDamageMultiplier;
        public IntRange Damage;
        public Classes Class;
        public DamageTypes DamageType;
    }
}