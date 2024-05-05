using BH.Runtime.Managers;
using BH.Runtime.Scenes;
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
        [SerializeField, Tooltip("Name of the console prefab in resources folder.")]
        private string _consoleName = "Console";
        
        public override void InstallBindings()
        {
            SceneSettingsSO sceneSettings = Resources.Load<SceneSettingsSO>(_sceneSettingsName);
            if (sceneSettings == null)
            {
                Debug.LogError($"SceneSettingsSO asset file '{_sceneSettingsName}' not found in the resources folder.");
                return;
            }
            Container.BindInstance(sceneSettings).AsSingle();
            
            GameObject consolePrefab = Resources.Load<GameObject>(_consoleName);
            if (consolePrefab == null)
            {
                Debug.LogError($"Console prefab '{_consoleName}' not found in the resources folder.");
                return;
            }
            Container.Bind<QuantumConsole>().FromComponentInNewPrefab(consolePrefab).AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
            Container.Bind<SceneLoader>().AsSingle().NonLazy();
        }
    }
}