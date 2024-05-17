using System;
using System.Collections.Generic;
using BH.Runtime.Factories;
using BH.Runtime.Managers;
using MEC;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace BH.Runtime.Entities
{
    public class EnemySpawner : Entity
    {
        [SerializeField]
        private bool _loopWaves = true;
        [SerializeField]
        private Wave[] _waves;
        private int _currentWaveIndex = 0;
        
        private IAIFactory _aiFactory;
        private ILevelStateHandler _levelStateHandler;
        private List<Entity> _spawnedEnemies = new ();
        
        [Inject(Id = "MainCamera")]
        private Camera _mainCamera;

        public event Action WaveFinishedEvent;
        public event Action AllWavesCompletedEvent;
        
        [Inject]
        public void Construct(IAIFactory aiFactory, ILevelStateHandler levelStateHandler)
        {
            _aiFactory = aiFactory;
            _levelStateHandler = levelStateHandler;
            _levelStateHandler.OnLevelStateChanged += OnLevelStateChanged;
        }

        private void OnDestroy()
        {
            _levelStateHandler.OnLevelStateChanged -= OnLevelStateChanged;
        }

        private void SpawnMeleeAI()
        {
            AIMeleeController enemy = _aiFactory.CreateAIMelee();
            enemy.SetUp(this);
            enemy.EnemyHFSM.ChangeState(enemy.IdleState);

            Vector2 spawnPosition = GetRandomSpawnOffCamera();
            enemy.transform.position = spawnPosition;

            _spawnedEnemies.Add(enemy);
        }
        
        public void EntityDied(Entity entity)
        {
            _spawnedEnemies.Remove(entity);
        }
        
        private void OnLevelStateChanged(LevelState levelState)
        {
            if (levelState == LevelState.NormalRound)
            {
                Timing.RunCoroutine(StartNextWaveCoroutine());
            }
        }
        
        private IEnumerator<float> StartNextWaveCoroutine()
        {
            if (_currentWaveIndex >= _waves.Length)
            {
                if (_waves.Length > 0 && _loopWaves)
                {
                    _currentWaveIndex = 0;
                }
                else
                {
                    AllWavesCompletedEvent?.Invoke();
                    yield break;
                }
            }

            Wave currentWave = _waves[_currentWaveIndex];
            
            for (int i = 0; i < currentWave.MeleeAICount; i++)
            {
                if (_spawnedEnemies.Count < currentWave.MeleeAICount)
                {
                    SpawnMeleeAI();
                    float interval = Random.Range(currentWave.MinSpawnInterval, currentWave.MaxSpawnInterval);
                    yield return Timing.WaitForSeconds(interval);
                }
            }

            yield return Timing.WaitUntilDone(WaitForAllEnemiesDefeated());

            WaveFinishedEvent?.Invoke();
            _levelStateHandler.SetLevelState(LevelState.Upgrading);

            _currentWaveIndex++;
            if (_currentWaveIndex < _waves.Length && _loopWaves)
            {
                _currentWaveIndex = 0;
            }
            else
            {
                AllWavesCompletedEvent?.Invoke();
            }
        }
        
        private IEnumerator<float> WaitForAllEnemiesDefeated()
        {
            while (_spawnedEnemies.Count > 0)
            {
                yield return Timing.WaitForOneFrame;
            }
        }
        
        private Vector2 GetRandomSpawnOffCamera()
        {
            Vector3 cameraPos = _mainCamera.transform.position;
            float cameraHeight = 2f * _mainCamera.orthographicSize;
            float cameraWidth = cameraHeight * _mainCamera.aspect;

            float margin = 1f;

            float minX = cameraPos.x - cameraWidth / 2 - margin;
            float maxX = cameraPos.x + cameraWidth / 2 + margin;
            float minY = cameraPos.y - cameraHeight / 2 - margin;
            float maxY = cameraPos.y + cameraHeight / 2 + margin;

            int edge = Random.Range(0, 4);

            Vector2 spawnPoint = edge switch
            {
                0 => new Vector2(minX, Random.Range(minY, maxY)),
                1 => new Vector2(maxX, Random.Range(minY, maxY)),
                2 => new Vector2(Random.Range(minX, maxX), minY),
                3 => new Vector2(Random.Range(minX, maxX), maxY),
                _ => Vector2.zero
            };

            return spawnPoint;
        }
    }
}