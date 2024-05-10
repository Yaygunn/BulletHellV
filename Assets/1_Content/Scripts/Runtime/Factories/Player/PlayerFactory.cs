using BH.Runtime.Entities;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Factories
{
    public class PlayerFactory : IPLayerFactory
    {
        private readonly DiContainer _container;
        private readonly GameObject _playerPrefab;
        
        public PlayerFactory(DiContainer container, GameObject playerPrefab)
        {
            _container = container;
            _playerPrefab = playerPrefab;
        }
        
        public PlayerController CreatePlayer()
        {
            GameObject playerGO = _container.InstantiatePrefab(_playerPrefab);
            PlayerController player = playerGO.GetComponent<PlayerController>();
            if (player != null)
            {
                // TODO: Any player setup here before returning...
                return player;
            }
            
            Debug.LogError($"Player prefab '{_playerPrefab.name}' does not have a PlayerController component.");
            return null;
        }
    }
}