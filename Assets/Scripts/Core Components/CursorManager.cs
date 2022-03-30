using Molodoy.Extensions;
using UnityEngine;

namespace Molodoy.CoreComponents
{
    public struct CursorProperties : IQueueable
    {
        public bool IsVisible;
        public CursorLockMode LockMode;
        public int hash;
        int IQueueable.Hash { get => hash; }

        public CursorProperties(int _hash, bool _isVisible, CursorLockMode _lockMode)
        {
            hash = _hash;
            IsVisible = _isVisible;
            LockMode = _lockMode;
        }

        public void TurnHasCome(int _hash)
        {
            Cursor.lockState = LockMode;
            Cursor.visible = IsVisible;

            Debug.Log($"Cursos mode {LockMode}, visible {IsVisible}, added by {hash}, applied by {_hash}");
        }
    }

    public static class CursorManager
    {
        private static CustomQueue cursorStateQueue = new CustomQueue();

        public static void SetCursorState(int _hash, bool _isVisible, CursorLockMode _lockMode)
        {
            cursorStateQueue.AddToQueueAndApply(new CursorProperties(_hash, _isVisible, _lockMode));
        }

        public static void SetBaseCursorState(int _hash, bool _isVisible, CursorLockMode _lockMode)
        {
            cursorStateQueue.InitializeQueue(new CursorProperties(_hash, _isVisible, _lockMode));
        }

        public static void ForgetCursorState(int _hash)
        {
            cursorStateQueue.RemoveStateFromQueueAll(_hash);
        }
    }
}
