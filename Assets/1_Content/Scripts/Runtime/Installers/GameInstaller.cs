using BH.Runtime.Input;
using BH.Runtime.Managers;
using BH.Runtime.Scenes;
using BH.Scriptables;
using BH.Scriptables.Scenes;
using QFSW.QC;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [SerializeField, Tooltip("Name of the SceneSettingsSO asset file in the resources folder.")] 
        private string _sceneSettingsName = "SceneSettings";
        [SerializeField, Tooltip("Name of the AudioSettingsSO asset file in the resources folder.")] 
        private string _audioSettingsName = "AudioSettings";
        [SerializeField, Tooltip("Name of the console prefab in resources folder.")]
        private string _consoleName = "Console";
        
        public override void InstallBindings()
        {
            // Scenes
            if (TryLoadResource(_sceneSettingsName, out SceneSettingsSO sceneSettings))
            {
                Container.BindInstance(sceneSettings).AsSingle();
                Container.Bind<SceneLoader>().AsSingle().NonLazy();
            }
            
            // Audio
            if (TryLoadResource(_audioSettingsName, out AudioSettingsSO audioSettings))
            {
                Container.BindInstance(audioSettings).AsSingle();
                Container.Bind<GameObject>().FromInstance(gameObject).WhenInjectedInto<AudioManager>();
                Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle().NonLazy();
            }
            
            // Console
            if (TryLoadResource(_consoleName, out GameObject consolePrefab))
            {
                Container.Bind<QuantumConsole>().FromComponentInNewPrefab(consolePrefab).AsSingle().NonLazy();
            }
            
            // Input
            // TODO: Check for optimization
            InputHandler inputHandler = GetComponent<InputHandler>();
            Container.BindInterfacesTo<InputHandler>().FromInstance(inputHandler).AsSingle().NonLazy();
            
            // Other
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
        }
        
        private bool TryLoadResource<T>(string resourceName, out T asset) where T : Object
        { 
            asset = Resources.Load<T>(resourceName);
            if (asset != null) return true;
            
            Debug.LogError($"{typeof(T).Name} asset file '{resourceName}' not found in the resources folder.");
            return false;
        }
    }
}