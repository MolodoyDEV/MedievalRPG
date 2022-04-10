using Assets.Scripts.Interfaces.ManagementMenuPages;
using Molodoy.Extensions;
using Molodoy.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Interfaces.Tiles
{
    public class BuildingTile : BaseInterfaceTile
    {
        private UIBuildingsSelection buildingsSelection;

        protected override void Awake()
        {
            base.Awake();
            buildingsSelection = transform.FindParentWithComponentOrDefault<UIBuildingsSelection>();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                buildingsSelection.OnBuildingSelected(EntityID);
                Select();
            }
        }

        public void Select()
        {
            background.color = colors.selectedColor;
        }

        public void Deselect()
        {
            background.color = colors.normalColor;
        }
    }
}