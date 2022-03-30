using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Management.Registrators
{
    public class VillagersRegistrator : EntityRegistrator
    {
        private static Dictionary<int, VillagerCore> villagersById = new Dictionary<int, VillagerCore>();

        public static void AttachVillagerToBuilding(int villagerId, int buildingId)
        {
            BuildingsRegistrator.GetBuildingById(buildingId).OnVillagerAttachedToBuilding(villagerId);
            GetVillagerById(villagerId).OnAttachedToBuilding(buildingId);
        }

        public static void DetachVillagerFromBuilding(int villagerId)
        {
            int buildingId = GetVillagerById(villagerId).InBuldingID;
            GetVillagerById(villagerId).OnDetachedFromBuilding();
            BuildingsRegistrator.GetBuildingById(buildingId).OnVillagerDetachedFromBuilding(villagerId);
        }

        public static VillagerCore GetVillagerById(int id)
        {
            return villagersById[id];
        }

        public static List<VillagerCore> GetRegisteredVillagers()
        {
            return villagersById.Values.ToList();
        }

        public static void RegisterVillager(VillagerCore villagerCore)
        {
            if (villagersById.ContainsKey(villagerCore.GetHashCode()))
            {
                throw new Exception("Villager already registered!");
            }

            villagersById.Add(villagerCore.GetHashCode(), villagerCore);
        }

        public static void UnRegisterVillager(VillagerCore villagerCore)
        {
            if (villagersById.ContainsKey(villagerCore.GetHashCode()) == false)
            {
                throw new Exception("Villager not registered!");
            }

            villagersById.Remove(villagerCore.GetHashCode());
        }
    }
}