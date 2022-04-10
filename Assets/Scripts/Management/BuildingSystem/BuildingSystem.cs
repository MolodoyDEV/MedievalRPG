using Assets.Scripts.Buildings;
using Assets.Scripts.Interfaces;
using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Management.BuildingSystem
{
    [DisallowMultipleComponent]
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] private List<BaseBuilding> availableBuildings = new List<BaseBuilding>();
        private UIBuildingSystemMenu buildingSystemMenu;
        private BuildableObject buildingGhost;
        private static BuildingSystem instance;

        public bool IsBuildingSelected { get => buildingGhost != null; }
        public static List<BaseBuilding> AvailableBuildings { get => instance.availableBuildings; }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            buildingSystemMenu = UIWindowsManager.GetWindow<UIBuildingSystemMenu>();
            //buildingSystemMenu = FindObjectOfType<UIBuildingSystemMenu>();

            //foreach (BaseBuilding building in availableBuildings)
            //{
            //    buildingSystemMenu.OnBuldingAdded(building);
            //}

            //OnExitBildingMode();
        }

        public void AddAvailableBuilding(BaseBuilding _building)
        {
            if (availableBuildings.Contains(_building))
            {
                throw new Exception("Bilding already added!");
            }

            availableBuildings.Add(_building);
            buildingSystemMenu.OnBuldingAdded(_building);
        }

        public void OnEnterBildingMode()
        {
            UIWindowsManager.CloseAllOpenedWindows();
            UIWindowsManager.OpenWindow<UIBuildingSystemMenu>();
            UIWindowsManager.OpenWindow<UIBuildingSystemHint>();
            UIWindowsManager.SetWindowsOnTop<UIBuildingSystemMenu>();
        }

        public void OnExitBildingMode()
        {
            //DeselectBuilding();
            DestroyBuldingGhost();
            UIWindowsManager.CloseWindow<UIBuildingSystemMenu>();
            UIWindowsManager.CloseWindow<UIBuildingSystemHint>();
        }

        public void RemoveAvailableBuilding(BaseBuilding _building)
        {
            availableBuildings.Remove(_building);
            buildingSystemMenu.OnBuldingRemoved(_building);
        }

        //public void SelectBulding(int number)
        //{

        //    DestroyBuldingGhost();

        //    if (availableBuildings.Count >= number)
        //    {
        //        BaseBuilding selectedBulding = availableBuildings[number - 1];
        //        buildingSystemMenu.OnBuildingSelected(selectedBulding);
        //        buildingGhost = MonoBehaviour.Instantiate(selectedBulding.gameObject).GetComponent<BuildableObject>();
        //    }
        //    else
        //    {
        //        DeselectBuilding();
        //    }
        //}

        public static void SelectBulding(BaseBuilding building)
        {
            instance.DestroyBuldingGhost();
            instance.buildingGhost = MonoBehaviour.Instantiate(building.gameObject).GetComponent<BuildableObject>();
        }

        private void DestroyBuldingGhost()
        {
            if (buildingGhost)
            {
                MonoBehaviour.Destroy(buildingGhost.gameObject);
                buildingGhost = null;
            }
        }

        //private void DeselectBuilding()
        //{
        //    buildingSystemMenu.DeselectAllTiles();
        //}

        public void RotateBulding(float delta)
        {
            Vector3 currentRotation = buildingGhost.transform.rotation.eulerAngles;
            buildingGhost.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y + delta, currentRotation.z);
        }

        public void SetBuildingPosition(Vector3 position)
        {
            buildingGhost.transform.position = position;
        }

        public bool TryPlaceBulding()
        {
            if (buildingGhost.IsAllowToBuild)
            {
                buildingGhost.OnUnfinishedBuildingPlaced();
                buildingGhost = null;
                //DeselectBuilding();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CancelBulding()
        {
            DestroyBuldingGhost();
            UIWindowsManager.OpenWindow<UIBuildingSystemMenu>();
            //DeselectBuilding();
        }
    }
}