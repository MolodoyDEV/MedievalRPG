using Assets.Scripts.Buildings;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] private UIBuildingQuickPanel buildingQuickPanel;
        //[HideInInspector] public static UnityEvent<BaseBuilding> NewBuildingAvailable = new UnityEvent<BaseBuilding>();
        //[HideInInspector] public static UnityEvent<BaseBuilding> BuildingUnAvailable = new UnityEvent<BaseBuilding>();
        //[HideInInspector] public static UnityEvent<BaseBuilding> BuildingSelected = new UnityEvent<BaseBuilding>();
        //[HideInInspector] public static UnityEvent BuildingDeSelected = new UnityEvent();
        [SerializeField] private List<BaseBuilding> availableBuildings = new List<BaseBuilding>(10);
        private BuildableObject buildingGhost;

        public bool IsBuildingSelected { get => buildingGhost != null; }

        private void Start()
        {
            foreach (BaseBuilding building in availableBuildings)
            {
                buildingQuickPanel.OnBuldingAdded(building);
                //NewBuildingAvailable?.Invoke(building);
            }

            OnExitBildingMode();
        }

        public void AddAvailableBuilding(BaseBuilding _building)
        {
            if (availableBuildings.Contains(_building))
            {
                throw new Exception("Bilding already added!");
            }

            availableBuildings.Add(_building);
            buildingQuickPanel.OnBuldingAdded(_building);
            //NewBuildingAvailable?.Invoke(_building);
        }

        public void OnEnterBildingMode()
        {
            buildingQuickPanel.gameObject.SetActive(true);
        }

        public void OnExitBildingMode()
        {
            DeselectBuilding();
            DestroyBuldingGhost();
            buildingQuickPanel.gameObject.SetActive(false);
        }

        public void RemoveAvailableBuilding(BaseBuilding _building)
        {
            availableBuildings.Remove(_building);
            buildingQuickPanel.OnBuldingRemoved(_building);
            //BuildingUnAvailable?.Invoke(_building);
        }

        public void SelectBulding(int number)
        {
            if (number == 0)
            {
                number = 10;
            }

            DestroyBuldingGhost();

            if (availableBuildings.Count >= number)
            {
                BaseBuilding selectedBulding = availableBuildings[number - 1];
                buildingQuickPanel.OnBuildingSelected(selectedBulding);
                buildingGhost = Instantiate(selectedBulding.gameObject).GetComponent<BuildableObject>();
                //BuildingSelected?.Invoke(availableBuildings[number - 1]);
            }
            else
            {
                DeselectBuilding();
            }
        }

        private void DestroyBuldingGhost()
        {
            if (buildingGhost)
            {
                Destroy(buildingGhost.gameObject);
                buildingGhost = null;
            }
        }

        private void DeselectBuilding()
        {
            buildingQuickPanel.DeselectAllBuldings();
        }

        public void RotateBulding(float delta)
        {
            Vector3 currentRotation = buildingGhost.transform.rotation.eulerAngles;
            buildingGhost.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y + delta, currentRotation.z);
        }

        public void SetBuildingPosition(Vector3 position)
        {
            buildingGhost.transform.position = position;
        }

        public bool TryApplyBulding()
        {
            if (buildingGhost.IsAllowToBuild)
            {
                buildingGhost.OnBuilded();
                buildingGhost = null;
                DeselectBuilding();
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
            DeselectBuilding();
        }
    }
}