using Assets.Scripts.Buildings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.BuildingSystem
{
    public class UIBuildingQuickPanel : MonoBehaviour
    {
        [SerializeField] private Image[] buildingTile = new Image[10];
        [SerializeField] private BaseBuilding[] buildings = new BaseBuilding[10];
        [SerializeField] private Color baseTileColor = new Color();
        [SerializeField] private Color selectedTileColor = new Color();
        private int freePosition;

        private void Awake()
        {
            //BuildingSystem.NewBuildingAvailable.AddListener(OnBuldingAdded);
            //BuildingSystem.BuildingUnAvailable.AddListener(OnBuldingRemoved);
            //BuildingSystem.BuildingSelected.AddListener(OnBuildingSelected);
            //BuildingSystem.BuildingDeSelected.AddListener(DeselectAllBuldings);

            freePosition = 0;
        }

        public void OnBuldingAdded(BaseBuilding _building)
        {
            buildingTile[freePosition].sprite = _building.Properties.BuildingPreview;
            buildings[freePosition] = _building;
            freePosition += 1;
        }

        public void OnBuldingRemoved(BaseBuilding _building)
        {
            freePosition -= 1;
            buildingTile[freePosition].sprite = null;
            buildings[freePosition] = null;
        }

        public void OnBuildingSelected(BaseBuilding _building)
        {
            bool isSelected = false;

            for (int i = 0; i < buildings.Length; i++)
            {
                if (buildings[i] == _building)
                {
                    buildingTile[i].color = selectedTileColor;
                    isSelected = true;
                }
                else
                {
                    buildingTile[i].color = baseTileColor;
                }
            }

            if (isSelected == false)
            {
                throw new System.Exception("Bilding not found!");
            }
        }

        public void DeselectAllBuldings()
        {
            for (int i = 0; i < buildingTile.Length; i++)
            {
                buildingTile[i].color = baseTileColor;
            }
        }
    }
}