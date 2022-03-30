using Assets.Scripts.Buildings;
using Assets.Scripts.Interfaces.Tiles;
using Assets.Scripts.Management.Registrators;
using Assets.Scripts.Units;
using Molodoy.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Interfaces.ManagementMenuPages
{
    [RequireComponent(typeof(Dropdown))]
    public class UIAddVillagerDropdown : MonoBehaviour, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField] private UIBuildingsSelection buildingsSelection;
        [SerializeField] private List<VillagerCore> availableVillagers;
        private Dropdown dropdown;
        private Transform myTransform;

        private void Awake()
        {
            myTransform = transform;
            dropdown = GetComponent<Dropdown>();
            dropdown.ClearOptions();
        }

        public void OnVillagerSelected(int id)
        {
            Debug.Log($"Selected {id}");
            buildingsSelection.TryAttachVillagerToSelectedBuilding(availableVillagers[id]);
            dropdown.ClearOptions();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            dropdown.ClearOptions();
            availableVillagers = VillagersRegistrator.GetRegisteredVillagers();
            BaseBuilding selectedBuilding = BuildingsRegistrator.GetBuildingById(buildingsSelection.SelectedBuildingId);

            if (selectedBuilding.CanAttachVillager())
            {
                for (int i = 0; i < availableVillagers.Count; i++)
                {
                    VillagerCore villager = availableVillagers[i];

                    if (villager.IsWorking == false && selectedBuilding.IsAllowToWorkHere(villager.Grade))
                    {
                        dropdown.options.Add(new Dropdown.OptionData(villager.GetHashCode().ToString()));
                    }
                    else
                    {
                        availableVillagers.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            VillagerTile[] dropdownTiles = myTransform.GetChild(3).GetComponentsInChildren<VillagerTile>();
            Debug.Log(dropdownTiles.Length);
            BaseBuilding selectedBuilding = BuildingsRegistrator.GetBuildingById(buildingsSelection.SelectedBuildingId);

            for (int i = 0; i < dropdownTiles.Length; i++)
            {
                VillagerCore villager = availableVillagers[i];

                if (villager.IsWorking == false && selectedBuilding.IsAllowToWorkHere(villager.Grade))
                {
                    dropdownTiles[i].SetValues(villager.Name, villager.FacePreview, villager.GetHashCode());
                }
            }
        }
    }
}