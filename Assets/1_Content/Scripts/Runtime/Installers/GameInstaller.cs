using BH.Runtime.Managers;
using BH.Runtime.Scenes;
using BH.Scriptables.Scenes;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [SerializeField, Tooltip("Name of the SceneSettingsSO asset file in the resources folder.")] 
        private string _sceneSettings = "SceneSettings";
        
        public override void InstallBindings()
        {
            SceneSettingsSO sceneSettings = Resources.Load<SceneSettingsSO>(_sceneSettings);
            if (sceneSettings == null)
            {
                Debug.LogError($"SceneSettingsSO asset file '{_sceneSettings}' not found in the resources folder.");
                return;
            }
            Container.BindInstance(sceneSettings).AsSingle();
            
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
            Container.Bind<SceneLoader>().AsSingle().NonLazy();
        }
    }
}