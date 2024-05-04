using System.Collections.Generic;
using BH.Runtime.Scenes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables.SceneManagement
{
    [CreateAssetMenu(fileName = "SceneSettings", menuName = "BH/Scenes/SceneSettings")]
    public class SceneSettingsSO : ScriptableObject
    {
        [field: SerializeField]
        public List<SceneBind> SceneBinds { get; private set; }
    }
}

