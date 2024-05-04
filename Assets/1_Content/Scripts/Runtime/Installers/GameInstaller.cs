using BH.Runtime.Managers;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<GameManager>().AsSingle().NonLazy();
            //Container.Bind<SceneLoader>().AsSingle().NonLazy();
        }
    }
}