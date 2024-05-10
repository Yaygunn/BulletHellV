using System;
using QFSW.QC;
using UnityEngine;
using Zenject;

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
    
    public class GameManager : IInitializable, IGameStateHandler
    {
        private QuantumConsole _console;
        
        public GameState CurrentGameState { get; private set; } = GameState.Initialization;
        public Action<GameState> OnGameStateChanged { get; }
        
        public GameManager(QuantumConsole console)
        {
            _console = console;
            
            // TODO: probably input could be injected here...
        }
        
        public void Initialize()
        {
            _console.Activate();
            _console.Toggle();
        }
        
        public void SetGameState(GameState gameState)
        {
            if (CurrentGameState == gameState)
                return;
            
            CurrentGameState = gameState;
            OnGameStateChanged?.Invoke(CurrentGameState);
            Debug.Log($"[GameManager] Game state changed to: {CurrentGameState}");
        }
    }
}