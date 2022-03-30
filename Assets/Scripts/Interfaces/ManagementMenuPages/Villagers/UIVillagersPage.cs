using Assets.Scripts.Management.Registrators;
using Assets.Scripts.Units;
using Molodoy.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interfaces.ManagementMenuPages
{
    public class UIVillagersPage : MonoBehaviour
    {
        private List<VillagerCore> villagers = new List<VillagerCore>();
        [SerializeField] private Transform villagerTilesParent;
        [SerializeField] private GameObject villagerTilePrefab;

        private void OnEnable()
        {
            villagers = VillagersRegistrator.GetRegisteredVillagers();
            DrawTiles();
        }

        private void DrawTiles()
        {
            foreach (VillagerCore villager in villagers)
            {
                string inBuildingTitle = "";

                if (villager.InBuldingID != -1)
                {
                    inBuildingTitle = "\tIn " + BuildingsRegistrator.GetBuildingById(villager.InBuldingID).Properties.Title;
                }

                BaseInterfaceTile tile = Instantiate(villagerTilePrefab, villagerTilesParent, false).GetComponent<BaseInterfaceTile>();
                tile.SetValues($"{villager.Grade} {villager.Name}\n{villager.Profession}\n{inBuildingTitle}",
                    villager.FacePreview,
                    villager.GetHashCode());
            }
        }

        public void DestroyAllTiles()
        {
            for (int i = 0; i < villagerTilesParent.childCount; i++)
            {
                Destroy(villagerTilesParent.GetChild(i).gameObject);
            }
        }

        private void OnDisable()
        {
            DestroyAllTiles();
        }
    }
}