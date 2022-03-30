using Assets.Scripts.Units.States;
using Molodoy.CoreComponents;
using Molodoy.CoreComponents.StateMachine;
using Molodoy.Inspector.Extentions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(UnitMovementController))]
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(UnitPresenter))]
    [RequireComponent(typeof(NavMeshObstacle))]
    public class UnitController : BaseCharacter, IStationSwitcher
    {
        [SerializeField] private int teamID;
        [SerializeField] private List<int> enemyTeamIDs = new List<int>();
        [SerializeField] private int currentHealth;
        [ReadOnlyInspector] [SerializeField] private bool isDeath;
        [SerializeField] private UnitCharacteristicValues characteristics;
        private Transform myTransform;
        private UnitPresenter presenter;
        private UnitMovementController myMovementController;
        private NavMeshAgent agent;
        private NavMeshObstacle obstacle;
        [HideInInspector] public UnityEvent<UnitController> UnitDeath;

        public bool IsDeath { get => isDeath; private set => isDeath = value; }
        public int TeamID { get => teamID; private set => teamID = value; }
        public List<int> EnemyTeamIDs { get => enemyTeamIDs; private set => enemyTeamIDs = value; }
        public bool IsMoving { get => myMovementController.IsMoving; }


        private void Awake()
        {
            myTransform = transform;
            agent = GetComponent<NavMeshAgent>();
            obstacle = GetComponent<NavMeshObstacle>();
            presenter = GetComponent<UnitPresenter>();
            myMovementController = GetComponent<UnitMovementController>();

            allStates = new List<BaseState>
            {
                new UnitIdleState(this),
                new UnitMoveState(this),
                new UnitDeathState(this),
                new UnitDisabledState(this),
                new UnitFightState(this, this, characteristics)
            };

            isDeath = false;
            obstacle.enabled = false;
            currentHealth = characteristics.MaximumHealth;
            currentState = allStates[0];

            GameProcess.AiFreezed += Freeze;
            GameProcess.AiUnFreezed += UnFreeze;
        }

        private void Start()
        {
            presenter.SetHealth(currentHealth.ToString());
            presenter.SetName($"{characteristics.Class}");
            SwitchState<UnitFightState>();
        }

        private void OnDestroy()
        {
            GameProcess.AiFreezed -= Freeze;
            GameProcess.AiUnFreezed -= UnFreeze;
        }

        private void Update()
        {
            currentState.Update();
        }

        private void FixedUpdate()
        {
            currentState.FixedUpdate();
        }

        private void OnUnitDeath()
        {
            myMovementController.StopMoving();
            myMovementController.StopRotation();
            StopAllCoroutines();
            myMovementController.MovingComplete.RemoveListener(OnMovementComplete);
            isDeath = true;
            agent.enabled = false;
            //obstacle.enabled = true;
            presenter.OnUnitDeath();
            myTransform.rotation = Quaternion.Euler(new Vector3(90f, myTransform.rotation.eulerAngles.y, myTransform.rotation.eulerAngles.z));
            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y - 1f, myTransform.position.z);
            UnitDeath?.Invoke(this);
            SwitchState<UnitDeathState>();
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= Mathf.RoundToInt(damage * (100f - characteristics.PhysicalDamageResistPercent) / 100);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnUnitDeath();
            }

            presenter.SetHealth(currentHealth.ToString());
        }

        public void TakeHeal(int heal)
        {
            currentHealth += heal;

            if (currentHealth > characteristics.MaximumHealth)
            {
                currentHealth = characteristics.MaximumHealth;
            }

            presenter.SetHealth(currentHealth.ToString());
        }

        public UnityEvent MoveTo(Vector3 toPosition, float minDistance, bool run)
        {
            if (run)
            {
                myMovementController.SetMovementSpeed(characteristics.MovementSpeed * characteristics.RunSpeedMultiplier);
            }
            else
            {
                myMovementController.SetMovementSpeed(characteristics.MovementSpeed);
            }

            //SwitchState<UnitMoveState>();
            myMovementController.MovingComplete.AddListener(OnMovementComplete);
            myMovementController.MoveTo(toPosition, minDistance);
            return myMovementController.MovingComplete;
        }

        public UnityEvent MoveTo(Transform toTransform, float minDistance, bool run)
        {
            if (run)
            {
                myMovementController.SetMovementSpeed(characteristics.MovementSpeed * characteristics.RunSpeedMultiplier);
            }
            else
            {
                myMovementController.SetMovementSpeed(characteristics.MovementSpeed);
            }

            //SwitchState<UnitMoveState>();
            myMovementController.MovingComplete.AddListener(OnMovementComplete);
            myMovementController.MoveTo(toTransform, minDistance);
            return myMovementController.MovingComplete;
        }

        public UnityEvent LookAt(Vector3 atPosition)
        {
            myMovementController.SetRotationSpeed(characteristics.RotationSpeed);
            myMovementController.LookAt(atPosition);
            return myMovementController.RotationComplete;
        }

        public UnityEvent LookAt(Transform atTransform, bool toFollow)
        {
            myMovementController.SetRotationSpeed(characteristics.RotationSpeed);
            myMovementController.LookAt(atTransform, toFollow);
            return myMovementController.RotationComplete;
        }

        public void OnMovementComplete()
        {
            myMovementController.MovingComplete.RemoveListener(OnMovementComplete);
            //ReturnToPreviousState();
        }

        public void StopLookAt()
        {
            myMovementController.StopRotation();
        }

        public void StopMoving()
        {
            myMovementController.StopMoving();
        }

        private void Freeze()
        {
            SwitchState<UnitFreezeState>();
        }

        private void UnFreeze()
        {
            ReturnToPreviousState();
        }
    }
}