using System.Collections.Generic;
using UnityEngine;

namespace Molodoy.CoreComponents
{
    public struct SceneProperties
    {
        public readonly float GameSpeed;
        public readonly CursorProperties CursorProperties;

        public SceneProperties(float _gameSpeed, CursorProperties _cursorProperties)
        {
            GameSpeed = _gameSpeed;
            CursorProperties = _cursorProperties;
        }
    }

    public static class GamePropertiesConstants
    {
        private static readonly Dictionary<string, SceneProperties> PropertiesBySceneName = new Dictionary<string, SceneProperties>()
        {
            { GameConstants.Scene_Main, new SceneProperties(GameConstants.DefaultGameSpeed, new CursorProperties(-1, false, CursorLockMode.Locked)) },
            { GameConstants.Scene_MainMenuName, new SceneProperties(GameConstants.DefaultGameSpeed, new CursorProperties(-1, true, CursorLockMode.Confined)) },
            { GameConstants.Scene_BattleName, new SceneProperties(GameConstants.DefaultGameSpeed, new CursorProperties(-1, true, CursorLockMode.Confined)) }
        };

        public static SceneProperties GetScenePropertiesOrDefault(string sceneName)
        {
            if (PropertiesBySceneName.ContainsKey(sceneName))
            {
                return PropertiesBySceneName[sceneName];
            }
            else
            {
                return new SceneProperties(GameConstants.DefaultGameSpeed, new CursorProperties(-1, false, CursorLockMode.Locked));
            }
        }
    }
}