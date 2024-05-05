using BH.Scriptables.Scenes;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Scenes
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField]
        private bool _loadSceneOnStart;

        [SerializeField]
        private SceneType _scene = SceneType.MainMenu;
        
        private SceneLoader _sceneLoader;
        
        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Start()
        {
            if (_loadSceneOnStart)
                _sceneLoader.LoadSceneAsync(_scene);
        }
        
        public void ChangeScene()
        {
            _sceneLoader.LoadSceneAsync(_scene);
        }
    }
}