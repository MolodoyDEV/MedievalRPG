using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public interface IWindow
    {
        public void OpenWindow();
        public void CloseWindow();
    };

    [DisallowMultipleComponent]
    public class UIWindowsManager : MonoBehaviour
    {
        [SerializeField] private Transform windowsParent;
        private static List<IWindow> allWindows = new List<IWindow>();
        private static List<IWindow> openedWindows = new List<IWindow>();

        private void Awake()
        {
            allWindows = Resources.FindObjectsOfTypeAll<MonoBehaviour>().OfType<IWindow>().ToList();

            foreach (IWindow window in allWindows)
            {
                window.CloseWindow();
            }
        }

        //private static void OnWindowOpened(IWindow window)
        //{
        //    if (openedWindows.Contains(window) == false)
        //    {
        //        openedWindows.Add(window);
        //    }
        //}

        //private static void OnWindowClosed(IWindow window)
        //{
        //    if (openedWindows.Contains(window))
        //    {
        //        openedWindows.Remove(window);
        //    }
        //}

        //temporary public!?
        public static T GetWindow<T>() where T : IWindow
        {
            IWindow[] windowsWithType = allWindows.Where(window => window is T).ToArray();

            if (windowsWithType.Length != 1)
            {
                throw new System.Exception($"Founded {windowsWithType.Length} windows of type {typeof(T)}! Must be only 1");
            }

            return (T)windowsWithType[0];
        }

        public static void OpenWindow<T>() where T : IWindow
        {
            IWindow window = GetWindow<T>();

            if (openedWindows.Contains(window))
            {
                throw new System.Exception($"Window {window} already opened!");
            }

            openedWindows.Add(window);
            MonoBehaviour windowMono = (MonoBehaviour)window;
            windowMono.transform.SetAsLastSibling();
            window.OpenWindow();
        }

        public static void SetWindowsOnTop<T>() where T : IWindow
        {
            IWindow window = GetWindow<T>();

            if (openedWindows.Contains(window) == false)
            {
                throw new System.Exception($"Window {window} not opened!");
            }

            MonoBehaviour windowMono = (MonoBehaviour)window;
            windowMono.transform.SetAsLastSibling();
        }

        public static void CloseWindow<T>() where T : IWindow
        {
            IWindow window = GetWindow<T>();
            openedWindows.Remove(window);
            MonoBehaviour windowMono = (MonoBehaviour)window;
            window.CloseWindow();
        }

        public static void CloseAllOpenedWindows()
        {
            foreach (IWindow window in openedWindows)
            {
                window.CloseWindow();
            }
        }
    }
}