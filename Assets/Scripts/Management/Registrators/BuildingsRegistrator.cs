using Assets.Scripts.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Management.Registrators
{
    public class BuildingsRegistrator : EntityRegistrator
    {
        private static Dictionary<int, BaseBuilding> buildingsById = new Dictionary<int, BaseBuilding>();

        public static void AttachVillagerToBuilding(int villagerId, int buildingId)
        {
            GetBuildingById(buildingId).OnVillagerAttachedToBuilding(villagerId);
            VillagersRegistrator.GetVillagerById(villagerId).OnAttachedToBuilding(buildingId);
        }

        public static void DetachVillagerFromBuilding(int villagerId)
        {
            int buildingId = VillagersRegistrator.GetVillagerById(villagerId).InBuldingID;
            VillagersRegistrator.GetVillagerById(villagerId).OnDetachedFromBuilding();
            GetBuildingById(buildingId).OnVillagerDetachedFromBuilding(villagerId);
        }

        public static BaseBuilding GetBuildingById(int id)
        {
            return buildingsById[id];
        }

        public static List<BaseBuilding> GetRegisteredBuildings()
        {
            return buildingsById.Values.ToList();
        }

        public static void RegisterBuilding(BaseBuilding buildingBase)
        {
            if (buildingsById.ContainsKey(buildingBase.GetHashCode()))
            {
                throw new Exception("Building already registered!");
            }

            buildingsById.Add(buildingBase.GetHashCode(), buildingBase);
        }

        public static void UnRegisterBuilding(BaseBuilding buildingBase)
        {
            if (buildingsById.ContainsKey(buildingBase.GetHashCode()) == false)
            {
                throw new Exception("Building not registered!");
            }

            buildingsById.Remove(buildingBase.GetHashCode());
        }
    }
}