using System;

namespace Molodoy.CoreComponents.StateMachine
{
    [Serializable]
    public abstract class BaseState
    {
        protected IStationSwitcher stationSwitcher;

        public BaseState(IStationSwitcher _stationSwitcher)
        {
            stationSwitcher = _stationSwitcher;
        }

        public abstract void Update();

        public abstract void ActivityUpdate();

        public abstract void FixedUpdate();

        public abstract void StartState();

        public abstract void StopState();
    }
}