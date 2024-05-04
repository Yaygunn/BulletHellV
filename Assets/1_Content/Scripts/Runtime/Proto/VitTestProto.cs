using System;
using System.Collections;
using BH.Runtime.Managers;
using BH.Runtime.Scenes;
using BH.Scriptables.Scenes;
using BH.Utilities;
using UnityEngine;
using Zenject;

namespace BH.Runtime
{
    public class VitTestProto : MonoBehaviour
    {
        [Inject]
        private GameManager _gameManager;

        [Inject]
        private SceneLoader _sceneLoader;

        private void Start()
        {
            _sceneLoader.LoadSceneAsync(SceneType.MainMenu);
        }
    }
}