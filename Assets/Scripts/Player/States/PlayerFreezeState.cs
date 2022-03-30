using Molodoy.CoreComponents.StateMachine;

namespace Molodoy.Characters.Player.States
{
    public class PlayerFreezeState : PlayerBaseState
    {

        public override void StartState()
        {
        }

        public override void StopState()
        {
        }

        public PlayerFreezeState(IStationSwitcher _stationSwitcher) : base(_stationSwitcher)
        {
        }
    }
}