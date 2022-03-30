using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Molodoy.Inspector.Extentions;

namespace Molodoy.CoreComponents.StateMachine
{
    public class BaseCharacter : MonoBehaviour, IStationSwitcher
    {
        protected List<BaseState> allStates;
        private BaseState previousState;
        protected BaseState currentState;
        [ReadOnlyInspector] [SerializeField] protected string S_currentStateName;
        [ReadOnlyInspector] [SerializeField] protected string S_previousStateName;

        public BaseState GetCurrentState() => currentState;

        public void SwitchState<T>() where T : BaseState
        {
            //if (currentState is T)
            //{
            //    throw new Exception("Switching to same state" + currentState.GetType());
            //}

            previousState = currentState;
            BaseState toState = allStates.FirstOrDefault(state => state is T);
            currentState?.StopState();
            currentState = toState;
            toState.StartState();

            S_currentStateName = currentState.GetType().Name;
            S_previousStateName = previousState?.GetType().Name;
        }

        public void SwitchState(BaseState toState)
        {
            //if(currentState == toState)
            //{
            //    throw new Exception("Switching to same state" + toState);
            //}

            previousState = currentState;
            currentState?.StopState();
            currentState = toState;
            toState.StartState();

            S_currentStateName = currentState.GetType().Name;
            S_previousStateName = previousState?.GetType().Name;
        }

        public void ReturnToPreviousState()
        {
            SwitchState(previousState);
        }
    }
}