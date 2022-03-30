using Molodoy.Extensions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Assets.Scripts.Units
{
    public class UnitMovementController : MonoBehaviour
    {
        //[SerializeField] private float walkingSpeed = 3.5f;
        //[SerializeField] private float runningSpeedMultiplier = 1.5f;
        private float rotationSpeed = 100f;
        private NavMeshAgent agent;
        private Transform myTransform;
        private bool isMoving;
        private bool isRotating;
        private Coroutine rotationCoroutine;
        private Coroutine movingCoroutine;
        [HideInInspector] public UnityEvent RotationComplete;
        [HideInInspector] public UnityEvent MovingComplete;

        public bool IsMoving { get => isMoving; private set => isMoving = value; }
        public bool IsRotating { get => isRotating; private set => isRotating = value; }

        private void Awake()
        {
            myTransform = transform;
            agent = GetComponent<NavMeshAgent>();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public Coroutine MoveToIfNotMoving(Vector3 moveToPosition, float minDistance)
        {
            if (isMoving == false)
            {
                return MoveTo(moveToPosition, minDistance);
            }

            return null;
        }

        public Coroutine LookAt(Vector3 atPosition)
        {
            StopRotation();
            rotationCoroutine = StartCoroutine(LookAtPositionCoroutine(atPosition));
            return rotationCoroutine;
        }

        public Coroutine LookAt(Transform atTransform, bool toFollow)
        {
            StopRotation();
            rotationCoroutine = StartCoroutine(LookAtTransformCoroutine(atTransform, toFollow));
            return rotationCoroutine;
        }

        public Coroutine MoveTo(Vector3 toPosition, float minDistance)
        {
            StopMoving();
            movingCoroutine = StartCoroutine(MoveToPositionCoroutine(toPosition, minDistance));
            return movingCoroutine;
        }

        public Coroutine MoveTo(Transform toTransform, float minDistance)
        {
            StopMoving();
            movingCoroutine = StartCoroutine(MoveToTransformCoroutine(toTransform, minDistance));
            return movingCoroutine;
        }

        private IEnumerator MoveToPositionCoroutine(Vector3 moveToPosition, float minDistance)
        {
            if (agent.SetDestination(moveToPosition) == false)
            {
                throw new Exception("Destination position is unreachable");
            }

            IsMoving = true;
            agent.isStopped = false;

            while (true)
            {
                float distance = Vector3.Distance(myTransform.position, moveToPosition);

                if (distance <= minDistance)
                {
                    IsMoving = false;
                    agent.isStopped = true;
                    MovingComplete?.Invoke();
                    MovingComplete.RemoveAllListeners();
                    yield break;
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator MoveToTransformCoroutine(Transform moveToTransform, float minDistance)
        {
            IsMoving = true;
            agent.isStopped = false;

            while (true)
            {
                if(agent.SetDestination(moveToTransform.position) == false)
                {
                    throw new Exception("Destination transform position is unreachable");
                }

                float distance = Vector3.Distance(myTransform.position, moveToTransform.position);

                if (distance <= minDistance)
                {
                    IsMoving = false;
                    agent.isStopped = true;
                    MovingComplete?.Invoke();
                    MovingComplete.RemoveAllListeners();
                    yield break;
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator LookAtPositionCoroutine(Vector3 atPosition)
        {
            IsRotating = true;

            while (true)
            {
                Vector3 direction = atPosition - transform.position;
                direction.y = 0f;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                yield return new WaitForFixedUpdate();

                if (transform.rotation == rotation)
                {
                    IsRotating = false;
                    rotationCoroutine = null;
                    RotationComplete?.Invoke();
                    RotationComplete.RemoveAllListeners();
                    yield break;
                }
            }
        }

        private IEnumerator LookAtTransformCoroutine(Transform atTransform, bool toFollow)
        {
            IsRotating = true;

            while (true)
            {
                Vector3 direction = atTransform.position - transform.position;
                direction.y = 0f;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                yield return new WaitForFixedUpdate();

                if (toFollow == false)
                {
                    if (transform.rotation == rotation)
                    {
                        IsRotating = false;
                        rotationCoroutine = null;
                        RotationComplete?.Invoke();
                        RotationComplete.RemoveAllListeners();
                        yield break;
                    }
                }
            }
        }

        //public void SetRunMode()
        //{
        //    agent.speed = walkingSpeed * runningSpeedMultiplier;
        //}

        //public void SetWalkMode()
        //{
        //    agent.speed = walkingSpeed;
        //}

        public void SetMovementSpeed(float speed)
        {
            agent.speed = speed / 10;
        }

        public void SetRotationSpeed(float speed)
        {
            rotationSpeed = speed;
        }

        public void StopMoving()
        {
            movingCoroutine?.Stop(this);
            IsMoving = false;
            movingCoroutine = null;
        }

        public void StopRotation()
        {
            rotationCoroutine?.Stop(this);
            IsRotating = false;
            rotationCoroutine = null;
        }
    }
}