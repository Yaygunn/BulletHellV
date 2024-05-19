using BH.Runtime.Entities;
using BH.Runtime.Input;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Factories
{
    public class BossFactory : IBossFactory
    {
        private readonly DiContainer _container;
        private readonly GameObject _bossPrefab;
        private IInputProvider _inputProvider;
        
        public BossFactory(DiContainer container, GameObject bossPrefab)
        {
            _container = container;
            _bossPrefab = bossPrefab;
        }
        
        public AIBossController CreateBoss()
        {
            GameObject bossGO = _container.InstantiatePrefab(_bossPrefab);
            AIBossController boss = bossGO.GetComponent<AIBossController>();
            if (boss != null)
            {
                // TODO: Any boss setup here before returning...
                return boss;
            }
            
            Debug.LogError($"Boss prefab '{_bossPrefab.name}' does not have a AIBossController component.");
            return null;
        }
    }
}