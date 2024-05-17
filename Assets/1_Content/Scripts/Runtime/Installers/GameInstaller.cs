using BH.Runtime.Input;
using BH.Runtime.Managers;
using BH.Runtime.Scenes;
using BH.Scriptables;
using BH.Scriptables.Databases;
using BH.Scriptables.Scenes;
using DP.Utilities;
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
        [SerializeField, Tooltip("Name of the Game Database prefab in resources folder.")]
        private string _databaseName = "GameDatabase";
        
        public override void InstallBindings()
        {
            // Scenes
            if (Tools.TryLoadResource(_sceneSettingsName, out SceneSettingsSO sceneSettings))
            {
                Container.BindInstance(sceneSettings).AsSingle();
                Container.Bind<SceneLoader>().AsSingle().NonLazy();
            }
            
            // Audio
            if (Tools.TryLoadResource(_audioSettingsName, out AudioSettingsSO audioSettings))
            {
                Container.BindInstance(audioSettings).AsSingle();
                Container.Bind<GameObject>().FromInstance(gameObject).WhenInjectedInto<AudioManager>();
                Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle().NonLazy();
            }
            
            // Console
            if (Tools.TryLoadResource(_consoleName, out GameObject consolePrefab))
            {
                Container.Bind<QuantumConsole>().FromComponentInNewPrefab(consolePrefab).AsSingle().NonLazy();
            }
            
            // Input
            // TODO: Check for optimization
            InputHandler inputHandler = GetComponent<InputHandler>();
            Container.BindInterfacesTo<InputHandler>().FromInstance(inputHandler).AsSingle().NonLazy();
            
            // Database
            if (Tools.TryLoadResource(_databaseName, out DatabaseSO database))
            {
                Container.BindInstance(database).AsSingle();
            }
            
            // Other
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
        }
    }
}