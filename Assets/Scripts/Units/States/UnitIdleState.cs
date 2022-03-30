using Molodoy.CoreComponents.StateMachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Units.States
{
    public class UnitIdleState : BaseState
    {
        public UnitIdleState(IStationSwitcher _stationSwitcher) : base(_stationSwitcher)
        {
        }

        public override void ActivityUpdate()
        {
        }

        public override void FixedUpdate()
        {
        }
        public override void Update()
        {
        }

        public override void StartState()
        {
        }

        public override void StopState()
        {
        }
    }
}