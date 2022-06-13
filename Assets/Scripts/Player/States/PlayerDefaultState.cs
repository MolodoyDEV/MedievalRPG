using Assets.Scripts.Environment;
using Molodoy.CoreComponents;
using Molodoy.CoreComponents.StateMachine;
using UnityEngine;

namespace Molodoy.Characters.Player.States
{
    public class PlayerDefaultState : PlayerBaseState
    {
        private bool objectPlacesHighlihts;

        public override void Update()
        {
            base.Update();

            ProcessInput();
            PlayerCore.MovementControl.ProcessMovement();
        }

        private void ProcessInput()
        {
            if (PlayerInputHandler.GetEscapeInputDown())
            {
                hud.ToggleInGameMenu();
            }
            else if (PlayerInputHandler.GetManagementInputDown())
            {
                hud.ToggleManagementMenu();
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                stationSwitcher.SwitchState<PlayerBuildingState>();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                CuttableTree cuttableTree;
                if (playerRaycast.HitedObject && playerRaycast.HitedObject.TryGetComponent(out cuttableTree))
                {
                    cuttableTree.CutTree();
                }
            }
        }

        public PlayerDefaultState(IStationSwitcher _stationSwitcher) : base(_stationSwitcher)
        {
        }

        public override void StartState()
        {
        }

        public override void StopState()
        {
        }

        public override void ActivityUpdate()
        {
        }

        public override void FixedUpdate()
        {
        }
    }
}