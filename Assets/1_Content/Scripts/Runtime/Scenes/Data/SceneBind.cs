using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.Scriptables.Scenes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Scenes
{
    [Serializable]
    public struct SceneBind
    {
        [field: SerializeField]
        public SceneType SceneType { get; private set; }
        
        [field: SerializeField, ValueDropdown(nameof(GetAvailableScenes))]
        public string SceneName { get; private set; }

#if UNITY_EDITOR
        private IEnumerable GetAvailableScenes()
        {
            return UnityEditor.EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => System.IO.Path.GetFileNameWithoutExtension(scene.path))
                .ToList();
        }
#else
        private static IEnumerable GetAvailableScenes()
        {
            return new List<string>();
        }
#endif
    }
}