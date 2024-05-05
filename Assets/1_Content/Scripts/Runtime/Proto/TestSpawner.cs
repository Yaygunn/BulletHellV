using BH.Runtime.Factories;
using BH.Runtime.Systems;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Test
{
    public class TestSpawner : MonoBehaviour
    {
        [Inject]
        private IProjectileFactory _projectileFactory;

        [Inject]
        private SignalBus _signalBus;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Projectile projectile = _projectileFactory.CreateProjectile();
                projectile.SetUp();
                _signalBus.Fire(new TestSpawnSignal());
            }
        }
    }
}