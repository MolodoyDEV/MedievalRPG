using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Villagers
{
    public enum VillagerGrade { peasant, nobleman, merchant, feudalLord }
    [Serializable]
    public enum VillagerProfession { unemployed, alchemist, farmer, hunter, woodman, coock, healer, priest }

    [Serializable]
    [CreateAssetMenu(fileName = "New VillagerProperties", menuName = "VillagerProperties")]
    public class VillagerPropertiesSO : ScriptableObject
    {
        [SerializeField] private List<Sprite> faceVariants = new List<Sprite>();
        [SerializeField] private List<string> nameVariants = new List<string>();
        [SerializeField] private VillagerGrade grade;
        private System.Random random = new System.Random();

        public VillagerGrade Grade { get => grade; }

        public Sprite GetRandomFace()
        {
            return faceVariants[random.Next(0, faceVariants.Count - 1)];
        }

        public string GetRandomName()
        {
            return nameVariants[random.Next(0, nameVariants.Count - 1)];
        }
    }

}