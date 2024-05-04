using System;
using BH.Runtime.Managers;
using UnityEngine;

namespace BH.Runtime.Scenes
{
    [Serializable]
    public struct SceneBind
    {
        [field: SerializeField]
        public SceneType SceneType { get; private set; }
        
        [field: SerializeField]
        public string SceneName { get; private set; }
    }
}