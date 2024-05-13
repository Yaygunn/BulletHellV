using System;
using System.Collections;
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
        
        [field: SerializeField, ValueDropdown(nameof(AvailableScenes))]
        public string SceneName { get; private set; }
        
#if UNITY_EDITOR
        private IEnumerable AvailableScenes()
        {
            return UnityEditor.EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => System.IO.Path.GetFileNameWithoutExtension(scene.path))
                .ToList();
        }
#endif
    }
}