using UnityEngine;

namespace Molodoy.Extensions
{
    public static class TransformExtension
    {
        public static Transform SetParameters(this Transform transform, Vector3 position, Vector3 rotation, Vector3 localScale)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(rotation);
            transform.localScale = localScale;

            return transform;
        }

        public static T FindParentWithComponentOrDefault<T>(this Transform childTransform)
        {
            Transform tempParent = childTransform.parent;

            while (tempParent != null)
            {
                if (tempParent.TryGetComponent(out T findedComponent))
                {
                    return findedComponent;
                }
                else
                {
                    tempParent = tempParent.parent;
                }
            }

            return default(T);
        }

        public static Transform FindParentWithTag(this Transform childTransform, string tag)
        {
            Transform tempParent = childTransform.parent;

            while (tempParent != null)
            {
                if (tempParent.tag == tag)
                {
                    return tempParent;
                }
                else
                {
                    tempParent = tempParent.parent;
                }
            }

            return null;
        }
    }
}