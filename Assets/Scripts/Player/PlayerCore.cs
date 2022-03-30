using Assets.Scripts.BuildingSystem;
using Molodoy.Characters.Player.States;
using Molodoy.CoreComponents;
using Molodoy.CoreComponents.StateMachine;
using Molodoy.Interfaces;
using System.Collections.Generic;
using UnityEngine;


namespace Molodoy.Characters.Player
{
    //TODO: Пересмотреть класс. Убрать статику?
    [RequireComponent(typeof(PlayerMovementControl))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerCore : BaseCharacter
    {
        [SerializeField] private Hud hud;
        [SerializeField] private Camera playerCamera;
        public Hud HUD { get => hud; }
        public CharacterController CharacterController { get; private set; }
        public static PlayerMovementControl MovementControl { get; private set; }
        public static PlayerCore instance;
        public static Camera PlayerCamera { get => instance.playerCamera; }


        private void Awake()
        {
            instance = this;
            CharacterController = GetComponent<CharacterController>();
            MovementControl = GetComponent<PlayerMovementControl>();

            allStates = new List<BaseState>()
        {
            new PlayerDefaultState(this),
            new PlayerFreezeState(this),
            new PlayerBuildingState(this, FindObjectOfType<BuildingSystem>())
        };

            currentState = allStates[0];

            GameProcess.PlayerFreezed += Freeze;
            GameProcess.PlayerUnFreezed += UnFreeze;
        }

        private void OnDestroy()
        {
            GameProcess.PlayerFreezed -= Freeze;
            GameProcess.PlayerUnFreezed -= UnFreeze;
        }

        private void Update()
        {
            currentState?.Update();
        }

        private void Freeze()
        {
            SwitchState<PlayerFreezeState>();
        }

        private void UnFreeze()
        {
            ReturnToPreviousState();
        }
    }
}