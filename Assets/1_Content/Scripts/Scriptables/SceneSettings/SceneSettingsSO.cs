using System.Collections.Generic;
using BH.Runtime.Scenes;
using UnityEngine;

namespace BH.Scriptables.Scenes
{
    public enum SceneType
    {
        Loader,
        MainMenu,
        GameEasy,
        GameHard,
        GameOver,
        GameWon
    }

    [CreateAssetMenu(fileName = "SceneSettings", menuName = "BH/Scenes/SceneSettings")]
    public class SceneSettingsSO : ScriptableObject
    {
        [field: SerializeField]
        public List<SceneBind> SceneBinds { get; private set; }
    }
}

