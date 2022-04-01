using Assets.Scripts.Management.BuildingSystem;
using Molodoy.CoreComponents;
using Molodoy.CoreComponents.StateMachine;
using UnityEngine;

namespace Molodoy.Characters.Player.States
{
    internal class PlayerBuildingState : PlayerBaseState
    {
        private BuildingSystem buildingSystem;
        private Transform playerCameraTransform;
        private Ray ray;
        private RaycastHit hit;
        private float maxBuldingDistance = 25f;
        private LayerMask layerMask = new LayerMask();
        private float buildingRotationSpeed = 180;

        public override void Update()
        {
            PlayerCore.MovementControl.ProcessMovement();

            int numericInput = PlayerInputHandler.GetNumericInputDown();

            if (numericInput != -1)
            {
                buildingSystem.SelectBulding(numericInput);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                stationSwitcher.ReturnToPreviousState();
            }
            else if (PlayerInputHandler.GetCancelInputDown())
            {
                buildingSystem.CancelBulding();
            }
            else if (PlayerInputHandler.GetLeftMouseInputDown())
            {
                buildingSystem.TryApplyBulding();
            }

            if (buildingSystem.IsBuildingSelected)
            {
                ray = new Ray(playerCameraTransform.position, playerCameraTransform.forward * maxBuldingDistance);
                Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * maxBuldingDistance, Color.yellow);

                if (Physics.Raycast(ray, out hit, maxBuldingDistance, layerMask))
                {
                    //if (hit.transform.gameObject.layer == GameConstants.Layer_GroundNumber)
                    //{
                    buildingSystem.RotateBulding(PlayerInputHandler.GetMouseScrollDeltaY() * buildingRotationSpeed * Time.deltaTime);
                    buildingSystem.SetBuildingPosition(hit.point);
                    //}
                }
            }
        }

        public override void ActivityUpdate()
        {

        }

        public override void FixedUpdate()
        {

        }

        public override void StartState()
        {
            buildingSystem.OnEnterBildingMode();
        }

        public override void StopState()
        {
            buildingSystem.OnExitBildingMode();
        }

        public PlayerBuildingState(IStationSwitcher _stationSwitcher, BuildingSystem _buildingSystem) : base(_stationSwitcher)
        {
            buildingSystem = _buildingSystem;
            playerCameraTransform = PlayerCore.PlayerCamera.transform;
            layerMask.value ^= 1 << GameConstants.Layer_GroundNumber;
            //layerMask.value = ~0;
            //layerMask.value &= ~(1 << GameConstants.Layer_PlayerNumber);
            //layerMask.value &= ~(1 << GameConstants.Layer_AiNumber);
        }
    }
}