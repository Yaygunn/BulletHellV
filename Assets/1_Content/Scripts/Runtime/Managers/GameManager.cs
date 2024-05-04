using System;
using UnityEngine;

namespace BH.Runtime.Managers
{
    public enum GameState
    {
        Initialization,
        Loading,
        Menu,
        Playing,
        Paused,
        GameOver
    }
    
    public class GameManager : IGameStateHandler
    {
        public GameState CurrentGameState { get; private set; } = GameState.Initialization;

        public Action<GameState> OnGameStateChanged { get; }
        
        public GameManager()
        {
            
        }
        
        public void SetGameState(GameState gameState)
        {
            CurrentGameState = gameState;
            OnGameStateChanged?.Invoke(CurrentGameState);
            Debug.Log($"[GameManager] Game state changed to: {CurrentGameState}");
        }
    }
}