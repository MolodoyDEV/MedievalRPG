namespace Molodoy.CoreComponents.StateMachine
{
    public interface IStationSwitcher
    {
        public void SwitchState<T>() where T : BaseState;

        public void SwitchState(BaseState toState);

        public void ReturnToPreviousState();
    }
}