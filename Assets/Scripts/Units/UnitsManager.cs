using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Units
{
    public class UnitsManager : MonoBehaviour
    {
        [SerializeField] private Material materialTemplate;
        private List<Color> teamColors = new List<Color> { Color.red, Color.green, Color.blue, Color.white, Color.black, Color.cyan };
        private List<UnitController> allUnits = new List<UnitController>();
        private List<UnitController> aliveUnits = new List<UnitController>();
        private static UnitsManager instance;

        private void Awake()
        {
            instance = this;
            allUnits = FindObjectsOfType<UnitController>().ToList();

            foreach (UnitController unit in allUnits)
            {
                Material tempMaterial = new Material(materialTemplate);
                tempMaterial.color = teamColors[unit.TeamID];
                unit.GetComponent<Renderer>().material = tempMaterial;

                if (unit.IsDeath == false)
                {
                    aliveUnits.Add(unit);
                    unit.UnitDeath.AddListener(OnUnitDeath);
                }
            }
        }

        public void OnUnitDeath(UnitController unitController)
        {
            unitController.UnitDeath.RemoveListener(OnUnitDeath);
            aliveUnits.Remove(unitController);
        }

        public static UnitController FindNearestAliveEnemyOrNull(UnitController fromUnit)
        {
            float minFoundedDistance = float.PositiveInfinity;
            Transform fromUnitTransform = fromUnit.transform;
            UnitController nearestEnemy = null;

            foreach (UnitController unit in instance.aliveUnits)
            {
                if (fromUnit.EnemyTeamIDs.Contains(unit.TeamID))
                {
                    float distance = Vector3.Distance(fromUnitTransform.position, unit.transform.position);

                    if (distance < minFoundedDistance)
                    {
                        minFoundedDistance = distance;
                        nearestEnemy = unit;
                    }
                }
            }

            return nearestEnemy;
        }
    }
}