using Assets.Scripts.Management.Registrators;
using Assets.Scripts.Units.States;
using Assets.Scripts.Villagers;
using Molodoy.CoreComponents;
using Molodoy.CoreComponents.StateMachine;
using Molodoy.Inspector.Extentions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(UnitMovementController))]
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(NavMeshObstacle))]
    [RequireComponent(typeof(UnitPresenter))]
    [RequireComponent(typeof(UnitHealth))]
    public class VillagerCore : BaseCharacter, IStationSwitcher
    {
        [ReadOnlyInspector] [SerializeField] private bool isWorking;
        [ReadOnlyInspector] [SerializeField] private string myName;
        [ReadOnlyInspector] [SerializeField] private Sprite facePreview;
        [ReadOnlyInspector] [SerializeField] private int inBuldingID = -1;
        [SerializeField] private int teamID;
        [SerializeField] private List<int> enemyTeamIDs = new List<int>();
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float runSpeedMultiplier;
        [SerializeField] private VillagerProfession profession;
        [SerializeField] private VillagerPropertiesSO propertiesSO;
        private Transform myTransform;
        private UnitPresenter presenter;
        private UnitMovementController myMovementController;
        private NavMeshAgent agent;
        private NavMeshObstacle obstacle;
        private UnitHealth health;

        public VillagerProfession Profession { get => profession; }
        public VillagerGrade Grade { get => propertiesSO.Grade; }

        public int TeamID { get => teamID; private set => teamID = value; }
        public List<int> EnemyTeamIDs { get => enemyTeamIDs; private set => enemyTeamIDs = value; }
        public bool IsMoving { get => myMovementController.IsMoving; }
        public string Name { get => myName; }
        public bool IsWorking { get => isWorking; private set => isWorking = value; }
        public Sprite FacePreview { get => facePreview; }
        public int InBuldingID { get => inBuldingID; set => inBuldingID = value; }

        private void Awake()
        {
            health = GetComponent<UnitHealth>();
            myTransform = transform;
            agent = GetComponent<NavMeshAgent>();
            obstacle = GetComponent<NavMeshObstacle>();
            presenter = GetComponent<UnitPresenter>();
            myMovementController = GetComponent<UnitMovementController>();
            VillagersRegistrator.RegisterVillager(this);

            allStates = new List<BaseState>
            {
                new UnitIdleState(this),
                new UnitFreezeState(this)
            };

            obstacle.enabled = false;
            currentState = allStates[0];

            GameProcess.AiFreezed += Freeze;
            GameProcess.AiUnFreezed += UnFreeze;
        }

        private void Start()
        {
            if (myName == "")
            {
                myName = propertiesSO.GetRandomName();
            }

            if (facePreview == null)
            {
                facePreview = propertiesSO.GetRandomFace();
            }

            presenter.SetHealth(health.CurrentHealth.ToString());
            presenter.SetName($"{profession} {myName}");

            health.HealthChange.AddListener(presenter.SetHealth);
            health.VillagerDeath.AddListener(OnVillagerDeath);
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

        public void OnAttachedToBuilding(int buildingId)
        {
            isWorking = true;
            InBuldingID = buildingId;
        }

        public void OnDetachedFromBuilding()
        {
            isWorking = false;
            InBuldingID = -1;
        }

        private void OnVillagerDeath()
        {
            myMovementController.StopMoving();
            myMovementController.StopRotation();
            StopAllCoroutines();
            myMovementController.MovingComplete.RemoveListener(OnMovementComplete);
            agent.enabled = false;
            //obstacle.enabled = true;
            presenter.OnUnitDeath();
            myTransform.rotation = Quaternion.Euler(new Vector3(90f, myTransform.rotation.eulerAngles.y, myTransform.rotation.eulerAngles.z));
            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y - 1f, myTransform.position.z);
            SwitchState<UnitDeathState>();
        }

        public UnityEvent MoveTo(Vector3 toPosition, float minDistance, bool run)
        {
            if (run)
            {
                myMovementController.SetMovementSpeed(movementSpeed * runSpeedMultiplier);
            }
            else
            {
                myMovementController.SetMovementSpeed(movementSpeed);
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
                myMovementController.SetMovementSpeed(movementSpeed * movementSpeed);
            }
            else
            {
                myMovementController.SetMovementSpeed(movementSpeed);
            }

            //SwitchState<UnitMoveState>();
            myMovementController.MovingComplete.AddListener(OnMovementComplete);
            myMovementController.MoveTo(toTransform, minDistance);
            return myMovementController.MovingComplete;
        }

        public UnityEvent LookAt(Vector3 atPosition)
        {
            myMovementController.SetRotationSpeed(rotationSpeed);
            myMovementController.LookAt(atPosition);
            return myMovementController.RotationComplete;
        }

        public UnityEvent LookAt(Transform atTransform, bool toFollow)
        {
            myMovementController.SetRotationSpeed(rotationSpeed);
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