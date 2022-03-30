using Assets.Scripts.Buildings;
using Assets.Scripts.Interfaces.Tiles;
using Assets.Scripts.Management.Registrators;
using Assets.Scripts.Units;
using Molodoy.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interfaces.ManagementMenuPages
{
    public class UIBuildingsSelection : MonoBehaviour
    {
        private List<BaseBuilding> buildings = new List<BaseBuilding>();
        private List<BuildingTile> buildingsTiles = new List<BuildingTile>();
        private List<VillagerTile> villagersTiles = new List<VillagerTile>();
        [SerializeField] private GameObject addVillagersDropdown;
        [SerializeField] private Text villagersCount;
        [SerializeField] private Transform buildingTilesParent;
        [SerializeField] private GameObject buildingTilePrefab;
        [SerializeField] private Transform villagersInBuldingTilesParent;
        [SerializeField] private GameObject villagerInBuildingTilePrefab;
        private int selectedBuildingId = -1;

        public int SelectedBuildingId { get => selectedBuildingId; private set => selectedBuildingId = value; }

        private void Awake()
        {
            ClearVillagersTiles();
            ClearBuildingsTiles();
        }

        private void OnEnable()
        {
            buildings = BuildingsRegistrator.GetRegisteredBuildings();
            selectedBuildingId = -1;
            addVillagersDropdown.SetActive(false);
            DrawBuildingsTiles();
        }

        private void OnDisable()
        {
            ClearBuildingsTiles();
            ClearVillagersTiles();
        }

        private void DrawBuildingsTiles()
        {
            foreach (BaseBuilding building in buildings)
            {
                BuildingTile tile = Instantiate(buildingTilePrefab, buildingTilesParent, false).GetComponent<BuildingTile>();
                tile.SetValues($"{building.Properties.Title}\nAllowed for: {building.Properties.AllowedVillagersClasses.ListToString(", ")}",
                    building.Properties.BuildingPreview, building.GetHashCode());
                buildingsTiles.Add(tile);
            }
        }

        public void ClearBuildingsTiles()
        {
            for (int i = 0; i < buildingsTiles.Count; i++)
            {
                Destroy(buildingsTiles[i].gameObject);
            }

            buildingsTiles.Clear();
        }

        public void OnBuildingSelected(int buildingId)
        {
            addVillagersDropdown.SetActive(true);
            SelectedBuildingId = buildingId;
            ClearVillagersTiles();
            DeselectAllBuldingsTiles();
            DrawVillagersTiles(buildingId);
        }

        public void DeselectAllBuldingsTiles()
        {
            foreach (var tile in buildingsTiles)
            {
                tile.Deselect();
            }
        }

        public void DeselectAllVillagersTiles()
        {
            foreach (var tile in buildingsTiles)
            {
                tile.Deselect();
            }
        }

        public bool TryAttachVillagerToSelectedBuilding(VillagerCore selectedVillager)
        {
            BaseBuilding selectedBuilding = BuildingsRegistrator.GetBuildingById(SelectedBuildingId);

            if (villagersTiles.Count >= selectedBuilding.Properties.MaximumVillagers)
            {
                return false;
            }

            if (selectedBuilding.IsAllowToWorkHere(selectedVillager.Grade) == false)
            {
                return false;
            }

            VillagerTile villagerTileLite = Instantiate(villagerInBuildingTilePrefab, villagersInBuldingTilesParent, false).GetComponent<VillagerTile>();
            villagerTileLite.SetValues(selectedVillager.Name, selectedVillager.FacePreview, selectedVillager.GetHashCode());
            villagerTileLite.DetachedFromBuilding.AddListener(DetachVillagerFromSelectedBuilding);
            BuildingsRegistrator.AttachVillagerToBuilding(selectedVillager.GetHashCode(), SelectedBuildingId);
            villagersTiles.Add(villagerTileLite);
            UpdateVillagersCounter();
            return true;
        }

        public void DetachVillagerFromSelectedBuilding(int villagerId)
        {
            BuildingsRegistrator.DetachVillagerFromBuilding(villagerId);

            for (int i = 0; i < villagersTiles.Count; i++)
            {
                if (villagersTiles[i].EntityID == villagerId)
                {
                    Destroy(villagersTiles[i].gameObject);
                    villagersTiles.RemoveAt(i);
                    break;
                }
            }

            UpdateVillagersCounter();
        }

        public void DrawVillagersTiles(int buildingId)
        {
            List<int> villagersInBuilding = BuildingsRegistrator.GetBuildingById(buildingId).VillagersInBuilding;

            foreach (int villagerID in villagersInBuilding)
            {
                VillagerTile villagerTile = Instantiate(villagerInBuildingTilePrefab, villagersInBuldingTilesParent, false).GetComponent<VillagerTile>();
                villagerTile.DetachedFromBuilding.AddListener(DetachVillagerFromSelectedBuilding);
                VillagerCore villager = VillagersRegistrator.GetVillagerById(villagerID);
                villagerTile.SetValues(villager.Name, villager.FacePreview, villager.GetHashCode());

                villagersTiles.Add(villagerTile);
            }

            UpdateVillagersCounter();
        }

        private void UpdateVillagersCounter()
        {
            if (SelectedBuildingId != -1)
            {
                villagersCount.text = $"{villagersTiles.Count}/{BuildingsRegistrator.GetBuildingById(SelectedBuildingId).Properties.MaximumVillagers}";
            }
            else
            {
                villagersCount.text = "0/0";
            }
        }

        public void ClearVillagersTiles()
        {
            for (int i = 0; i < villagersTiles.Count; i++)
            {
                Destroy(villagersTiles[i].gameObject);
            }

            villagersTiles.Clear();
            UpdateVillagersCounter();
        }
    }
}