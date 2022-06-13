using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class CuttableTreeVar1 : CuttableTree
    {
        [SerializeField] private Vector2 minMaxForceOnCut = new Vector2(0.5f, 1.5f);
        [SerializeField] private Transform topPoint;

        protected override void Awake()
        {
            base.Awake();
            myRigidbody.centerOfMass = topPoint.position;
        }

        public override void CutTree()
        {
            if (isCutted)
            {
                for (int i = 0; i < logsParent.childCount;)
                {
                    logsParent.GetChild(i).parent = null;
                }

                Destroy(gameObject);
            }
            else
            {
                myRigidbody.isKinematic = false;
                Vector2 minMaxForce = new Vector2(myRigidbody.mass * minMaxForceOnCut.x, myRigidbody.mass * minMaxForceOnCut.y);

                float xForce = Random.Range(minMaxForce.x, minMaxForce.y);
                float yForce = Random.Range(minMaxForce.x, minMaxForce.y);

                if (Random.Range(0f, 1f) >= 0.5f)
                {
                    xForce = -xForce;
                }


                if (Random.Range(0f, 1f) >= 0.5f)
                {
                    yForce = -yForce;
                }

                //myRigidbody.centerOfMass = topPoint.position;
                myRigidbody.AddForceAtPosition(new Vector3(xForce, 0f, yForce), topPoint.position, ForceMode.Impulse);
                //myRigidbody.AddRelativeForce(new Vector3(xForce, 0f, yForce), ForceMode.Impulse);
                isCutted = true;
            }
        }
    }
}