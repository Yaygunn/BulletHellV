using System;
using BH.Runtime.Entities;
using BH.Runtime.Factories;
using BH.Scriptables;
using BH.Utilities.ImprovedTimers;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Managers
{
    public enum LevelState
    {
        Loading,
        Cutscene,
        SpawningPlayer,
        NormalRound,
        BossRound,
        GameOver
    }
    
    // TODO: Break up level manager, it's doing too much already...
    public class LevelManager : IInitializable, IDisposable, ILevelStateHandler
    {
        private LevelSettingsSO _levelSettings;
        private IGameStateHandler _gameState;
        private IPLayerFactory _playerFactory;
        private SignalBus _signalBus;
        
        private PlayerController _player;
        private Vector3 _playerSpawnPosition;
        private CountdownTimer _respawnTimer;
        
        private LevelState _previousLevelState;
        public LevelState CurrentLevelState { get; private set; }
        public Action<LevelState> OnLevelStateChanged { get; }
        
        private LevelManager(LevelSettingsSO levelSettings, IGameStateHandler gameState, IPLayerFactory playerFactory, SignalBus signalBus)
        {
            _levelSettings = levelSettings;
            _gameState = gameState;
            _playerFactory = playerFactory;
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            // States
            _gameState.SetGameState(GameState.Playing);
            SetLevelState(LevelState.Loading);
            
            // Signals
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
            
            // TODO: set up level, cutscene if any, etc...
            
            SpawnPlayer();
            SetLevelState(LevelState.NormalRound);
        }
        
        public void Dispose()
        {
            // TODO: clean up level, etc...
            
            // Signals
            _signalBus.TryUnsubscribe<PlayerDiedSignal>(OnPlayerDied);
        }
        
        private void SpawnPlayer()
        {
            SetLevelState(LevelState.SpawningPlayer);
            
            _player = _playerFactory.CreatePlayer();
            if (_player == null)
            {
                Debug.LogError("Failed to spawn player.");
                return;
            }

            if (_levelSettings.UseSpawnTransform && _levelSettings.SpawnTransform != null)
                _playerSpawnPosition = _levelSettings.SpawnTransform.position;
            else
                _playerSpawnPosition = _levelSettings.SpawnPosition;

            // TODO: Any setup on player spawn...
            _player.transform.position = _playerSpawnPosition;
            // TODO: We need to request state, not directly change it like this...
            _player.Activate();
        }
        
        private void RespawnPlayer()
        {
            SetLevelState(LevelState.SpawningPlayer);
            _player.gameObject.SetActive(false);
            
            _respawnTimer = new CountdownTimer(_levelSettings.RespawnDelay);
            _respawnTimer.OnTimerStop += HandleRespawnTimerStop;
            _respawnTimer.Start();
        }
        
        private void HandleRespawnTimerStop()
        {
            _respawnTimer.OnTimerStop -= HandleRespawnTimerStop;
            _player.transform.position = _playerSpawnPosition;
            _player.gameObject.SetActive(true);
            SetLevelState(_previousLevelState);
            // TODO: Need to request state change here
            _player.Activate();
        }

        public void SetLevelState(LevelState newState)
        {
            if (CurrentLevelState == newState)
                return;
            
            _previousLevelState = CurrentLevelState;
            CurrentLevelState = newState;
            OnLevelStateChanged?.Invoke(CurrentLevelState);
            Debug.Log($"[LevelManager] State changed to: {CurrentLevelState}");
        }
        
        private void OnPlayerDied(PlayerDiedSignal signal)
        {
            if (_levelSettings.RespawnOnDeath)
                RespawnPlayer();
            else
                SetLevelState(LevelState.GameOver);
        }
    }
}