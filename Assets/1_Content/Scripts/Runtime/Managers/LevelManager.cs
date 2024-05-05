using System;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Managers
{
    public class LevelManager : MonoBehaviour
    {
        private IGameStateHandler _gameState;
        
        [Inject]
        private void Construct(IGameStateHandler gameState)
        {
            _gameState = gameState;
        }
        
        private void Start()
        {
            _gameState.SetGameState(GameState.Playing);
        }
    }
}