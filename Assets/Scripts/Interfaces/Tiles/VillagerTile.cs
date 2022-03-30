using Molodoy.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interfaces.Tiles
{
    public class VillagerTile : BaseInterfaceTile
    {
        [HideInInspector] public UnityEvent<int> DetachedFromBuilding;

        protected override void Awake()
        {
            base.Awake();
        }

        public void DetachFromBuilding()
        {
            DetachedFromBuilding?.Invoke(EntityID);
            DetachedFromBuilding.RemoveAllListeners();
            //Destroy(gameObject);
        }
    }
}