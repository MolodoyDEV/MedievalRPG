using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Management.BuildingSystem
{
    [DisallowMultipleComponent]
    public class BuildingPermitChecker : MonoBehaviour
    {
        private Dictionary<int, List<BuildingTerritory>> buildingTerritoriesByGroup = new Dictionary<int, List<BuildingTerritory>>();
        private int[] uniqBuildingTerritoryGroups = new int[0];
        private BuildingTerritory[] allBuildingTerritories = new BuildingTerritory[0];

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (EditorApplication.isPlaying == false)
            {
                Initialize();
            }
        }
#endif

        public void Initialize()
        {
            allBuildingTerritories = GetComponentsInChildren<BuildingTerritory>();

            for (int i = 0; i < allBuildingTerritories.Length; i++)
            {
                BuildingTerritory territory = allBuildingTerritories[i];

                if (buildingTerritoriesByGroup.ContainsKey(territory.TerritoryGroup))
                {
                    buildingTerritoriesByGroup[territory.TerritoryGroup].Add(territory);
                }
                else
                {
                    buildingTerritoriesByGroup.Add(territory.TerritoryGroup, new List<BuildingTerritory>() { territory });
                }
            }

            uniqBuildingTerritoryGroups = buildingTerritoriesByGroup.Keys.ToArray();

            for (int i = 0; i < uniqBuildingTerritoryGroups.Length; i++)
            {
                List<BuildingTerritory> buildingTerritories = buildingTerritoriesByGroup[uniqBuildingTerritoryGroups[i]];
                float maxHeightDifference = buildingTerritories[0].MaxHeightDifference;

                foreach (BuildingTerritory territory in buildingTerritories)
                {
                    if (territory.MaxHeightDifference != maxHeightDifference)
                    {
                        throw new System.Exception("Max height difference in one group must be the same!");
                    }
                }
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < allBuildingTerritories.Length; i++)
            {
                allBuildingTerritories[i].enabled = false;
            }

            allBuildingTerritories = null;
            uniqBuildingTerritoryGroups = null;
            buildingTerritoriesByGroup = null;
        }

        public bool CheckAllowToBuild()
        {
            for (int i = 0; i < uniqBuildingTerritoryGroups.Length; i++)
            {
                List<BuildingTerritory> buildingTerritories = buildingTerritoriesByGroup[uniqBuildingTerritoryGroups[i]];
                float maxHeightDifference = buildingTerritories[0].MaxHeightDifference;
                float minHeight = Mathf.Infinity;
                float maxHeight = 0;

                foreach (BuildingTerritory territory in buildingTerritories)
                {
                    if (territory.HasCollisionWithTerritory())
                    {
                        return false;
                    }
                    else if (maxHeightDifference != Mathf.Infinity)
                    {
                        Vector2 minMaxHeiht = territory.GetMinAndMaxHeiht();

                        if (minMaxHeiht.x < minHeight)
                        {
                            minHeight = minMaxHeiht.x;
                        }

                        if (minMaxHeiht.y > maxHeight)
                        {
                            maxHeight = minMaxHeiht.y;
                        }
                    }
                }

                if (maxHeightDifference != Mathf.Infinity)
                {
                    if (((maxHeight - minHeight) > maxHeightDifference) || (minHeight == Mathf.Infinity && maxHeight == Mathf.Infinity))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}