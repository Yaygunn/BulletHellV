using System;
using System.Collections.Generic;
using BH.Runtime.Entities;
using BH.Runtime.Factories;
using BH.Runtime.Scenes;
using BH.Scriptables;
using BH.Scriptables.Scenes;
using BH.Utilities.ImprovedTimers;
using MEC;
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
        Upgrading,
        GameOver,
        GameWon
    }

    // TODO: Break up level manager, it's doing too much already...
    public class LevelManager : IInitializable, IDisposable, ILevelStateHandler
    {
        private LevelSettingsSO _levelSettings;
        private IGameStateHandler _gameState;
        private IPLayerFactory _playerFactory;
        private SignalBus _signalBus;
        private SceneLoader _sceneLoader;


        private Vector3 _playerSpawnPosition;
        private CountdownTimer _respawnTimer;

        private LevelState _previousLevelState;
        public LevelState CurrentLevelState { get; private set; }

        public PlayerController Player { get; private set; }

        public event Action<LevelState> OnLevelStateChanged;

        private LevelManager(LevelSettingsSO levelSettings, IGameStateHandler gameState, IPLayerFactory playerFactory,
            SignalBus signalBus, SceneLoader sceneLoader)
        {
            _levelSettings = levelSettings;
            _gameState = gameState;
            _playerFactory = playerFactory;
            _signalBus = signalBus;
            _sceneLoader = sceneLoader;
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

            if (_respawnTimer != null)
                _respawnTimer.OnTimerStop -= HandleRespawnTimerStop;
        }

        public void TogglePause(bool pause)
        {
            if (pause)
            {
                _gameState.SetGameState(GameState.Paused);
                Time.timeScale = 0f;
            }
            else
            {
                _gameState.SetGameState(GameState.Playing);
                Time.timeScale = 1f;
            }
        }

        private void SpawnPlayer()
        {
            SetLevelState(LevelState.SpawningPlayer);

            Player = _playerFactory.CreatePlayer();
            if (Player == null)
            {
                Debug.LogError("Failed to spawn player.");
                return;
            }

            if (_levelSettings.UseSpawnTransform && _levelSettings.SpawnTransform != null)
                _playerSpawnPosition = _levelSettings.SpawnTransform.position;
            else
                _playerSpawnPosition = _levelSettings.SpawnPosition;

            // TODO: Any setup on player spawn...
            Player.transform.position = _playerSpawnPosition;
            // TODO: We need to request state, not directly change it like this...
            Player.Activate(false);
        }

        private void RespawnPlayer()
        {
            SetLevelState(LevelState.SpawningPlayer);
            Player.gameObject.SetActive(false);

            _respawnTimer = new CountdownTimer(_levelSettings.RespawnDelay);
            _respawnTimer.OnTimerStop += HandleRespawnTimerStop;
            _respawnTimer.Start();
        }

        private void HandleRespawnTimerStop()
        {
            _respawnTimer.OnTimerStop -= HandleRespawnTimerStop;
            Player.transform.position = _playerSpawnPosition;
            Player.gameObject.SetActive(true);
            SetLevelState(_previousLevelState);
            // TODO: Need to request state change here
            Player.Activate(true);
        }

        public void SetLevelState(LevelState newState)
        {
            if (CurrentLevelState == newState)
                return;

            _previousLevelState = CurrentLevelState;
            CurrentLevelState = newState;
            OnLevelStateChanged?.Invoke(CurrentLevelState);
            _signalBus.Fire(new LevelStateChangedSignal(CurrentLevelState));
            Debug.Log($"[LevelManager] State changed to: {CurrentLevelState}");

            if (CurrentLevelState == LevelState.GameOver)
            {
                TogglePause(false);
                Timing.RunCoroutine(GameOverCoroutine());
                _gameState.SetGameState(GameState.Paused);
            }

            if (CurrentLevelState == LevelState.GameWon)
            {
                TogglePause(false);
                Timing.RunCoroutine(GameWonCoroutine());
                _gameState.SetGameState(GameState.Paused);
            }
        }

        private void OnPlayerDied(PlayerDiedSignal signal)
        {
            if (_levelSettings.RespawnOnDeath)
                RespawnPlayer();
            else
                SetLevelState(LevelState.GameOver);
        }

        private IEnumerator<float> GameOverCoroutine()
        {
            yield return Timing.WaitForSeconds(2f);
            _sceneLoader.LoadSceneAsync(SceneType.GameOver);
        }

        private IEnumerator<float> GameWonCoroutine()
        {
            yield return Timing.WaitForSeconds(2f);
            _sceneLoader.LoadSceneAsync(SceneType.GameOver);
        }
    }
}