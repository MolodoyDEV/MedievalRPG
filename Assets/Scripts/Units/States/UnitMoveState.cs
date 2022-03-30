using Molodoy.CoreComponents.StateMachine;
using UnityEngine;

namespace Assets.Scripts.Units.States
{
    public class UnitMoveState : BaseState
    {
        public UnitMoveState(IStationSwitcher _stationSwitcher) : base(_stationSwitcher)
        {
        }

        public override void Update()
        {
        }

        public override void ActivityUpdate()
        {
        }

        public override void FixedUpdate()
        {
            Debug.Log("Movement");
        }

        public override void StartState()
        {
        }

        public override void StopState()
        {
        }
    }
}