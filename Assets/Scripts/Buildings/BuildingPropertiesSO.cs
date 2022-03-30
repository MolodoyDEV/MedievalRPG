using Assets.Scripts.Villagers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    [Serializable]
    [CreateAssetMenu(fileName = "New BuildingProperties", menuName = "BuildingProperties")]
    public class BuildingPropertiesSO : ScriptableObject
    {
        [SerializeField] private List<VillagerGrade> allowedVillagersGrades = new List<VillagerGrade>();
        [SerializeField] private Sprite buildingPreview;
        [SerializeField] private string title;
        [SerializeField] private int maximumVillagers;

        public List<VillagerGrade> AllowedVillagersClasses { get => allowedVillagersGrades; }
        public Sprite BuildingPreview { get => buildingPreview; }
        public string Title { get => title; }
        public int MaximumVillagers { get => maximumVillagers; }
    }
}