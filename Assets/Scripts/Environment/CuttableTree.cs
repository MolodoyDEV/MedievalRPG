using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public abstract class CuttableTree : MonoBehaviour
    {
        [SerializeField] protected Transform logsParent;
        protected Rigidbody myRigidbody;
        protected bool isCutted;

        protected virtual void Awake()
        {
            myRigidbody = GetComponent<Rigidbody>();
            myRigidbody.isKinematic = true;
            isCutted = false;
        }

        public abstract void CutTree();
    }
}