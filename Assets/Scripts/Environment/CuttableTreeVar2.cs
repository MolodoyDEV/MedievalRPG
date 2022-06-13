using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class CuttableTreeVar2 : CuttableTree
    {
        public override void CutTree()
        {
            isCutted = true;

            for (int i = 0; i < logsParent.childCount;)
            {
                logsParent.GetChild(i).parent = null;
            }

            Destroy(gameObject);
        }
    }
}