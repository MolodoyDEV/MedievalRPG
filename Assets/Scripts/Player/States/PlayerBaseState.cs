using Molodoy.CoreComponents;
using Molodoy.CoreComponents.StateMachine;
using Molodoy.Interfaces;
using UnityEngine;

namespace Molodoy.Characters.Player.States
{
    public abstract class PlayerBaseState : BaseState
    {
        protected Hud hud;
        protected PlayerRaycast playerRaycast;

        public override void Update()
        {
            if (PlayerInputHandler.GetHelpInputDownAlways())
            {
                hud.ToggleGameHint();
            }
        }

        public override void ActivityUpdate()
        {

        }

        public override void FixedUpdate()
        {

        }

        protected PlayerBaseState(IStationSwitcher _stationSwitcher) : base(_stationSwitcher)
        {
            PlayerCore playerCore = (PlayerCore)_stationSwitcher;
            hud = playerCore.HUD;
            playerRaycast = playerCore.PlayerRaycast;
        }
    }
}