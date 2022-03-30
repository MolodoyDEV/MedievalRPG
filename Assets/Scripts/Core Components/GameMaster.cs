using Molodoy.Inspector.Extentions;
using UnityEngine;

namespace Molodoy.CoreComponents
{
    [RequireComponent(typeof(SceneTransition))]
    [RequireComponent(typeof(ObjectsInitializer))]
    public class GameMaster : MonoBehaviour
    {
    }
}