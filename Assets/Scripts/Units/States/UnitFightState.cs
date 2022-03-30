using Molodoy.CoreComponents.StateMachine;
using UnityEngine;

namespace Assets.Scripts.Units.States
{
    public class UnitFightState : BaseState
    {
        private UnitController targetUnit;
        private UnitController myController;
        private UnitCharacteristicValues characteristict;
        private Transform myTransfrom;
        private Transform targetTransfrom;
        private float lasHitTime;

        public UnitFightState(IStationSwitcher _stationSwitcher, UnitController unitController, UnitCharacteristicValues unitCharacteristic) : base(_stationSwitcher)
        {
            myController = unitController;
            characteristict = unitCharacteristic;
            myTransfrom = unitController.transform;
        }

        public override void ActivityUpdate()
        {
        }

        public override void FixedUpdate()
        {
            if (targetUnit == null || targetUnit.IsDeath || myController.IsMoving)
            {
                targetUnit = UnitsManager.FindNearestAliveEnemyOrNull(myController);
                targetTransfrom = targetUnit?.transform;

                if (targetUnit == null)
                {
                    myController.StopMoving();
                    myController.StopLookAt();
                    stationSwitcher.ReturnToPreviousState();
                    return;
                }
            }

            if (targetUnit?.IsDeath == false)
            {
                if (characteristict.AttackRange.InRange(Vector3.Distance(myTransfrom.position, targetTransfrom.position)))
                {
                    if (Time.time - lasHitTime > characteristict.AttackSpeedSeconds)
                    {
                        targetUnit.TakeDamage(Random.Range(characteristict.Damage.Start, characteristict.Damage.End));
                        lasHitTime = Time.time;
                    }
                }
                else
                {
                    myController.StopLookAt();
                    myController.MoveTo(targetTransfrom, characteristict.AttackRange.End, false).AddListener(LookAtTarget);
                }
            }
            else
            {
                myController.StopLookAt();
                myController.StopMoving();
            }
        }

        public override void StartState()
        {
            lasHitTime = Time.time;
        }

        public override void StopState()
        {
        }

        public override void Update()
        {
        }

        private void LookAtTarget()
        {
            myController.LookAt(targetTransfrom, true);
        }
    }
}