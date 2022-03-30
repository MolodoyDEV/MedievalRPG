using Molodoy.CoreComponents.Saves;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Molodoy.CoreComponents
{
    public interface IInitializable
    {
        public interface Stage1 : IInitializable
        {
            void StartStage1();
        }

        public interface Stage2 : IInitializable
        {
            void StartStage2();
        }

        public interface Stage3 : IInitializable
        {
            void StartStage3();
        }

        public interface Stage4 : IInitializable
        {
            void StartStage4();
        }

        public interface Stage5 : IInitializable
        {
            void StartStage5();
        }

        public interface Stage6 : IInitializable
        {
            void StartStage6();
        }

        public interface AllStages : IInitializable, Stage1, Stage2, Stage3, Stage4, Stage5, Stage6
        {
        }

        bool IsInitialized { get; set; }
    }

    public class ObjectsInitializer : MonoBehaviour
    {
        private static int objectLoadingSteps = 8;
        public static bool ObjectsAreInitialized;
        [SerializeField] private GameObject initializableObjectsParent;
        private static readonly float stepDelay = 1f;
        private static ObjectsInitializer objectsInitializer;
        public static UnityEvent AllObjectsInitialized = new UnityEvent();
        public static UnityEvent ConcreteObjectInitialized = new UnityEvent();
        public static UnityEvent<int> ObjectLoadingStagePassed = new UnityEvent<int>();

        private void Awake()
        {
            objectsInitializer = this;
            ObjectsAreInitialized = false;
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 60;
        }

        public void Start()
        {
            SceneTransition.InitializeSceneProperties();
            StartCoroutine(InitializeAll());
        }

        public IEnumerator InitializeAll()
        {
            OnAllObjectsInitializationStarted();

            if (BinarySaveLoader.NeedLoadGameAtStart())
            {
                DestroyInstantiatedObjects();
                yield return BinarySaveLoader.StartLoadGame();

                BinarySaveLoader.LoadGameAtStart(false);
            }

            ObjectLoadingStagePassed?.Invoke(1);

            List<int> panelsNumbers = new List<int>();
            List<IInitializable> allInitializableObjects = FindObjectsOfType<MonoBehaviour>().OfType<IInitializable>().Where(_object => _object.IsInitialized == false).ToList();
            List<IInitializable.AllStages> allStageObjects = new List<IInitializable.AllStages>();
            List<IInitializable.Stage1> stage1Objects = new List<IInitializable.Stage1>();
            List<IInitializable.Stage2> stage2Objects = new List<IInitializable.Stage2>();
            List<IInitializable.Stage3> stage3Objects = new List<IInitializable.Stage3>();
            List<IInitializable.Stage4> stage4Objects = new List<IInitializable.Stage4>();
            List<IInitializable.Stage5> stage5Objects = new List<IInitializable.Stage5>();
            List<IInitializable.Stage6> stage6Objects = new List<IInitializable.Stage6>();


            foreach (IInitializable _object in allInitializableObjects)
            {
                if (_object is IInitializable.Stage1 || _object is IInitializable.AllStages)
                {
                    stage1Objects.Add((IInitializable.Stage1)_object);
                }
                if (_object is IInitializable.Stage2 || _object is IInitializable.AllStages)
                {
                    stage2Objects.Add((IInitializable.Stage2)_object);
                }
                if (_object is IInitializable.Stage3 || _object is IInitializable.AllStages)
                {
                    stage3Objects.Add((IInitializable.Stage3)_object);
                }
                if (_object is IInitializable.Stage4 || _object is IInitializable.AllStages)
                {
                    stage4Objects.Add((IInitializable.Stage4)_object);
                }
                if (_object is IInitializable.Stage5 || _object is IInitializable.AllStages)
                {
                    stage5Objects.Add((IInitializable.Stage5)_object);
                }
                if (_object is IInitializable.Stage6 || _object is IInitializable.AllStages)
                {
                    stage6Objects.Add((IInitializable.Stage6)_object);
                }
            }

            yield return new WaitForSeconds(0.5f);

            Debug.Log($"Global Initialization step 1. {stage1Objects.Count} scripts to initialize");
            stage1Objects.ForEach(_object => _object.StartStage1());
            ObjectLoadingStagePassed?.Invoke(2);

            Debug.Log($"Global Initialization step 2. {stage2Objects.Count} scripts to initialize");
            stage2Objects.ForEach(_object => _object.StartStage2());
            ObjectLoadingStagePassed?.Invoke(3);

            Debug.Log($"Global Initialization step 3. {stage3Objects.Count} scripts to initialize");
            stage3Objects.ForEach(_object => _object.StartStage3());
            ObjectLoadingStagePassed?.Invoke(4);

            Debug.Log($"Global Initialization step 4. {stage4Objects.Count} scripts to initialize");
            stage4Objects.ForEach(_object => _object.StartStage4());
            ObjectLoadingStagePassed?.Invoke(5);

            Debug.Log($"Global Initialization step 5. {stage5Objects.Count} scripts to initialize");
            stage5Objects.ForEach(_object => _object.StartStage5());
            ObjectLoadingStagePassed?.Invoke(6);

            Debug.Log($"Global Initialization step 6. {stage6Objects.Count} scripts to initialize");
            stage6Objects.ForEach(_object => _object.StartStage6());
            ObjectLoadingStagePassed?.Invoke(7);


            allInitializableObjects.ForEach(_object => _object.IsInitialized = true);
            ObjectLoadingStagePassed?.Invoke(8);

            OnAllObjectsInitialized(allInitializableObjects.Count);

            yield break;
        }

        private void OnAllObjectsInitializationStarted()
        {
            Debug.Log("Objects initialization started");
            GameProcess.SetDefaultGameSpeed(GetHashCode());
            GameProcess.FreezePlayer();
            GameProcess.FreezeAi();
            SceneTransition.OnObjectInitializationStarted(objectLoadingSteps);
        }

        private void OnAllObjectsInitialized(int initializedObjectsCount)
        {
            Debug.Log(initializedObjectsCount + " Objects successfuly initialized");
            ObjectsAreInitialized = true;
            AllObjectsInitialized?.Invoke();
            GameProcess.UnFreezePlayer();
            GameProcess.UnFreezeAi();
            GameProcess.ReturnPreviousSpeed(GetHashCode());
            GameProcess.UnFreezeGame(GetHashCode());
        }

        public static void InitializeObject(GameObject objectToInitialize)
        {
            List<IInitializable> initializableObjects = objectToInitialize.GetComponentsInChildren<IInitializable>().ToList();

            if (initializableObjects.Count == 0 || ObjectsAreInitialized == false) //Если в объекте нет компонентов IInitializable или все объекты еще не проинициализированы. (Будут проинициализированы позже)
            {
                return;
            }

            Debug.Log("Initialization " + objectToInitialize.name + " with " + initializableObjects.Count + " scripts");

            List<IInitializable.Stage1> stage1Objects = new List<IInitializable.Stage1>();
            List<IInitializable.Stage2> stage2Objects = new List<IInitializable.Stage2>();
            List<IInitializable.Stage3> stage3Objects = new List<IInitializable.Stage3>();
            List<IInitializable.Stage4> stage4Objects = new List<IInitializable.Stage4>();
            List<IInitializable.Stage5> stage5Objects = new List<IInitializable.Stage5>();
            List<IInitializable.Stage6> stage6Objects = new List<IInitializable.Stage6>();

            foreach (IInitializable _object in initializableObjects)
            {
                if (_object is IInitializable.Stage1 || _object is IInitializable.AllStages)
                {
                    stage1Objects.Add((IInitializable.Stage1)_object);
                }
                if (_object is IInitializable.Stage2 || _object is IInitializable.AllStages)
                {
                    stage2Objects.Add((IInitializable.Stage2)_object);
                }
                if (_object is IInitializable.Stage3 || _object is IInitializable.AllStages)
                {
                    stage3Objects.Add((IInitializable.Stage3)_object);
                }
                if (_object is IInitializable.Stage4 || _object is IInitializable.AllStages)
                {
                    stage4Objects.Add((IInitializable.Stage4)_object);
                }
                if (_object is IInitializable.Stage5 || _object is IInitializable.AllStages)
                {
                    stage5Objects.Add((IInitializable.Stage5)_object);
                }
                if (_object is IInitializable.Stage6 || _object is IInitializable.AllStages)
                {
                    stage6Objects.Add((IInitializable.Stage6)_object);
                }
            }

            Debug.Log($"Object Initialization step 1. {stage1Objects.Count} scripts to initialize");
            stage1Objects.ForEach(_object => _object.StartStage1());

            Debug.Log($"Object Initialization step 2. {stage2Objects.Count} scripts to initialize");
            stage2Objects.ForEach(_object => _object.StartStage2());

            Debug.Log($"Object Initialization step 3. {stage3Objects.Count} scripts to initialize");
            stage3Objects.ForEach(_object => _object.StartStage3());

            Debug.Log($"Object Initialization step 4. {stage4Objects.Count} scripts to initialize");
            stage4Objects.ForEach(_object => _object.StartStage4());

            Debug.Log($"Object Initialization step 5. {stage5Objects.Count} scripts to initialize");
            stage5Objects.ForEach(_object => _object.StartStage5());

            Debug.Log($"Object Initialization step 6. {stage6Objects.Count} scripts to initialize");
            stage6Objects.ForEach(_object => _object.StartStage6());

            initializableObjects.ForEach(_object => _object.IsInitialized = true);
            Debug.Log("Initialization " + objectToInitialize.name + " completed");
            ConcreteObjectInitialized?.Invoke();
        }

        private static void DestroyInstantiatedObjects()
        {
            List<GameObject> instantiatedObjects;
            instantiatedObjects = FindObjectsOfType<GameObject>().Where(_object => _object.layer == GameConstants.Layer_InstantiatedNumber).ToList();
            instantiatedObjects.ForEach(_object => Destroy(_object));
        }
    }
}