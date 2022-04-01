using Assets.Scripts.Villagers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    //[RequireComponent(typeof(BuildableObject))]
    [RequireComponent(typeof(Collider))]
    public class BaseBuilding : MonoBehaviour
    {
        [SerializeField] BuildingPropertiesSO propertiesSO;
        [SerializeField] private List<int> villagersInBuilding = new List<int>();

        public BuildingPropertiesSO Properties { get => propertiesSO; private set => propertiesSO = value; }
        public List<int> VillagersInBuilding { get => villagersInBuilding; }

        public bool IsAllowToWorkHere(VillagerGrade villagerGrade)
        {
            return Properties.AllowedVillagersClasses.Contains(villagerGrade);
        }

        public bool CanAttachVillager()
        {
            return villagersInBuilding.Count < propertiesSO.MaximumVillagers;
        }

        public void OnVillagerAttachedToBuilding(int villagerId)
        {
            VillagersInBuilding.Add(villagerId);
        }

        public void OnVillagerDetachedFromBuilding(int villagerId)
        {
            VillagersInBuilding.Remove(villagerId);
        }
    }
}