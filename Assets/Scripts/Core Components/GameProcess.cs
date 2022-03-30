using Molodoy.Extensions;
using System;
using UnityEngine;

namespace Molodoy.CoreComponents
{
    public struct GameSpeedProperties : IQueueable
    {
        public int hash;
        public float Speed;
        int IQueueable.Hash { get => hash; }

        public GameSpeedProperties(int _hash, float _speed)
        {
            hash = _hash;
            Speed = _speed;
        }

        public void TurnHasCome(int _hash)
        {
            if (Speed < 0 || Speed > GameConstants.MaximumGameSpeed)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (Speed == Time.timeScale)
            {
                Debug.LogWarning($"Game speed already is {Speed}, added by {hash}, applied by {_hash}");
            }
            else
            {
                Time.timeScale = Speed;
                Time.timeScale = (float)Math.Round(Time.timeScale, 2);
                Debug.LogWarning($"Game speed set to {Time.timeScale }, added by {hash}, applied by {_hash}");
            }
        }
    }

    [DisallowMultipleComponent]
    public static class GameProcess
    {
        public delegate void GameIsEndingDelegate();
        public static event GameIsEndingDelegate PlayerFreezed;
        public static event GameIsEndingDelegate PlayerUnFreezed;
        public static event GameIsEndingDelegate AiFreezed;
        public static event GameIsEndingDelegate AiUnFreezed;
        private static bool gameIsEnding;
        private static CustomQueue gameSpeedQueue = new CustomQueue();

        public static void FreezePlayer()
        {
            PlayerInputHandler.CanProcessInput = false;
            PlayerFreezed?.Invoke();
        }

        public static void UnFreezePlayer()
        {
            PlayerInputHandler.CanProcessInput = true;
            PlayerUnFreezed?.Invoke();
        }

        public static void FreezeAi()
        {
            AiFreezed?.Invoke();
        }

        public static void UnFreezeAi()
        {
            AiUnFreezed?.Invoke();
        }

        public static bool GameIsEnding() => gameIsEnding;

        public static void FreezeGame(int _hash)
        {
            gameIsEnding = false;
            gameSpeedQueue.AddToQueueAndApply(new GameSpeedProperties(_hash, 0f));
        }

        public static void UnFreezeGame(int _hash)
        {
            gameSpeedQueue.RemoveStateFromQueueAll(_hash);
            gameIsEnding = true;
        }

        public static void SetDefaultGameSpeed(int _hash)
        {
            SetGameSpeed(GameConstants.DefaultGameSpeed, _hash);
        }

        public static void SetMaximumGameSpeed(int _hash)
        {
            SetGameSpeed(GameConstants.MaximumGameSpeed, _hash);
        }

        public static void ModifyGameSpeed(float _addSpeed, int _hash)
        {
            if (Time.timeScale <= 1f && Mathf.Sign(_addSpeed) == -1 || Time.timeScale < 1f && Mathf.Sign(_addSpeed) == 1)
            {
                _addSpeed = _addSpeed / 10f;
            }

            if (Time.timeScale + _addSpeed <= 0)
            {
                SetGameSpeed(0f, _hash);
            }
            else if (Time.timeScale + _addSpeed >= GameConstants.MaximumGameSpeed)
            {
                SetGameSpeed(GameConstants.MaximumGameSpeed, _hash);
            }
            else
            {
                SetGameSpeed(Time.timeScale + _addSpeed, _hash);
            }
        }

        public static void SetBaseGameSpeed(float _speed, int _hash)
        {
            gameSpeedQueue.InitializeQueue(new GameSpeedProperties(_hash, _speed));
        }

        public static void SetGameSpeed(float _speed, int _hash)
        {
            gameSpeedQueue.AddToQueueAndApply(new GameSpeedProperties(_hash, _speed));
        }

        public static void ReturnPreviousSpeed(int _hash)
        {
            gameSpeedQueue.RemoveStateFromQueueAll(_hash);
        }

        public static bool ThisSpeedIsAvailable(Vector3 currentVelocity)
        {
            bool isAvailable = (currentVelocity.x <= GameConstants.MaxAvailableSpeed &&
                currentVelocity.y <= GameConstants.MaxAvailableSpeed &&
                currentVelocity.z <= GameConstants.MaxAvailableSpeed);

            if (isAvailable == false)
            {
                Debug.LogWarning("Object speed is more than available!");
            }

            return isAvailable;
        }

        public static bool ThisFallenOutInWorld(Transform _transform)
        {
            bool isFalen = _transform.position.y < GameConstants.MaxDownPositionInWorld;

            if (isFalen)
            {
                Debug.LogWarning(_transform.name + " falen out in world!");
            }

            return isFalen;
        }
    }
}