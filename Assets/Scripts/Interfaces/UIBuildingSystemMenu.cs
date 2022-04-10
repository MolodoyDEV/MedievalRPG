using Assets.Scripts.Buildings;
using Assets.Scripts.Management.BuildingSystem;
using Assets.Scripts.UI;
using Molodoy.CoreComponents;
using Molodoy.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interfaces
{
    public class UIBuildingSystemMenu : MonoBehaviour, IWindow
    {
        [SerializeField] private Transform buildingTilesParent;
        [SerializeField] private Image buildingPreviewImage;
        [SerializeField] private Text buildingPreviewText;
        [SerializeField] private Transform window;
        [SerializeField] private GameObject buildingTilePrefab;
        private BaseBuilding selectedBuilding;
        private Dictionary<BaseBuilding, BaseInterfaceTile> tileByBuilding = new Dictionary<BaseBuilding, BaseInterfaceTile>();

        private void DestroyAllTiles()
        {
            BaseBuilding[] buildings = tileByBuilding.Keys.ToArray();

            for (int i = 0; i < buildings.Length; i++)
            {
                OnBuldingRemoved(buildings[i]);
            }

            tileByBuilding.Clear();
            selectedBuilding = null;
            buildingPreviewImage.sprite = null;
            buildingPreviewImage.enabled = false;
            buildingPreviewText.text = "";
        }

        private void RenewAllTiles()
        {
            DestroyAllTiles();

            foreach (BaseBuilding building in BuildingSystem.AvailableBuildings)
            {
                OnBuldingAdded(building);
            }
        }

        public void OnBuldingAdded(BaseBuilding building)
        {
            GameObject tileObject = Instantiate(buildingTilePrefab);
            tileObject.transform.SetParent(buildingTilesParent);
            BaseInterfaceTile tile = tileObject.GetComponent<BaseInterfaceTile>();
            tile.OnLeftClick.AddListener(OnTileSelected);
            tile.SetValues(building.Properties.Name, building.Properties.BuildingPreview, tileByBuilding.Count);
            tileByBuilding.Add(building, tile);
        }

        public void OnBuldingRemoved(BaseBuilding _building)
        {
            BaseInterfaceTile tile = tileByBuilding[_building];
            tile.OnLeftClick.RemoveListener(OnTileSelected);
            Destroy(tile.gameObject);
            tileByBuilding.Remove(_building);
        }

        public void CloseWindowFromButton()
        {
            UIWindowsManager.CloseWindow<UIBuildingSystemMenu>();
        }

        public void OnBuildButtonPressed()
        {
            if (selectedBuilding)
            {
                BuildingSystem.SelectBulding(selectedBuilding);
                CloseWindowFromButton();
            }
        }

        public void CloseWindow()
        {
            window.gameObject.SetActive(false);
            DestroyAllTiles();
            CursorManager.ForgetCursorState(GetHashCode());
        }

        public void OpenWindow()
        {
            RenewAllTiles();
            window.gameObject.SetActive(true);
            CursorManager.SetCursorState(GetHashCode(), true, CursorLockMode.Confined);
        }

        public void OnTileSelected(int tileID)
        {
            DeselectAllTiles();
            BaseBuilding building = tileByBuilding.Keys.ToArray()[tileID];
            selectedBuilding = building;
            BaseInterfaceTile tile = tileByBuilding[building];
            buildingPreviewImage.enabled = true;
            buildingPreviewImage.sprite = tile.Icon.sprite;
            buildingPreviewText.text = building.Properties.Description;
        }

        public void DeselectAllTiles()
        {
            //BaseInterfaceTile[] tilesImages = tileByBuilding.Values.ToArray();

            //for (int i = 0; i < tileByBuilding.Values.Count; i++)
            //{
            //    tilesImages[i].color = baseTileColor;
            //}
        }
    }
}