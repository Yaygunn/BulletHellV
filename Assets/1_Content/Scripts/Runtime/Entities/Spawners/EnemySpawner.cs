using System;
using System.Collections.Generic;
using BH.Runtime.Audio;
using BH.Runtime.Factories;
using BH.Runtime.Managers;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace BH.Runtime.Entities
{
    public class EnemySpawner : Entity
    {
        [BoxGroup("Waves"), SerializeField]
        private bool _loopWaves = true;
        [BoxGroup("Waves"), SerializeField]
        private Wave[] _waves;
        [BoxGroup("Waves"), SerializeField, ReadOnly]
        private int _currentWaveIndex = 0;

        [BoxGroup("Boss"), SerializeField]
        private Vector2 _bossSpawnPoint;

        private IAIFactory _aiFactory;
        private IBossFactory _bossFactory;
        private ILevelStateHandler _levelStateHandler;
        private IWwiseEventHandler _wwiseEventHandler;
        private List<Entity> _spawnedEnemies = new();
        private int _killedWaveEnemies;
        private bool _spawnerRunning;
        private SignalBus _signalBus;

        [Inject(Id = "MainCamera")]
        private Camera _mainCamera;

        public event Action WaveFinishedEvent;
        public event Action AllWavesCompletedEvent;

        [Inject]
        public void Construct(IAIFactory aiFactory, IBossFactory bossFactory, ILevelStateHandler levelStateHandler,
            SignalBus signalBus, IWwiseEventHandler wwiseEventHandler)
        {
            _aiFactory = aiFactory;
            _bossFactory = bossFactory;
            _levelStateHandler = levelStateHandler;
            _levelStateHandler.OnLevelStateChanged += OnLevelStateChanged;
            _signalBus = signalBus;
            _wwiseEventHandler = wwiseEventHandler;
        }

        private void OnDestroy()
        {
            _levelStateHandler.OnLevelStateChanged -= OnLevelStateChanged;
        }

        private void SpawnMeleeAI()
        {
            AIMeleeController enemy = _aiFactory.CreateAIMelee();
            enemy.SetUp(this);
            enemy.EnemyHFSM.ChangeState(enemy.ChaseState);

            Vector2 spawnPosition = GetRandomSpawnOffCamera();
            enemy.transform.position = spawnPosition;

            _spawnedEnemies.Add(enemy);
            UpdateEnemyCount();
        }

        private void SpawnRangedAI()
        {
            AIRangedController enemy = _aiFactory.CreateAIRanged();
            enemy.SetUp(this);
            enemy.EnemyHFSM.ChangeState(enemy.MoveState);

            Vector2 spawnPosition = GetRandomSpawnOffCamera();
            enemy.transform.position = spawnPosition;

            _spawnedEnemies.Add(enemy);
            UpdateEnemyCount();
        }

        public Wave GetCurrentWave()
        {
            return _waves[_currentWaveIndex];
        }

        public void EntityDied(Entity entity)
        {
            _spawnedEnemies.Remove(entity);
            _killedWaveEnemies++;
            UpdateEnemyCount();
        }

        public void BossDied(AIBossController boss)
        {
            Destroy(boss.gameObject);
            _levelStateHandler.SetLevelState(LevelState.GameWon);
        }

        private void OnLevelStateChanged(LevelState levelState)
        {
            if (levelState == LevelState.BossRound)
            {
                _spawnerRunning = false;
                //TODO: implement boss phase
            }
            else if (!_spawnerRunning && levelState == LevelState.NormalRound)
            {
                _spawnerRunning = true;
                Timing.RunCoroutine(StartNextWaveCoroutine());
            }
        }

        private IEnumerator<float> StartNextWaveCoroutine()
        {
            if (_currentWaveIndex >= _waves.Length)
            {
                if (_loopWaves)
                {
                    _currentWaveIndex = 0;
                }
                else
                {
                    AllWavesCompleted();
                    yield break;
                }
            }

            Wave currentWave = _waves[_currentWaveIndex];
            int meleeSpawned = 0;
            int rangedSpawned = 0;

            while (meleeSpawned < currentWave.MeleeAICount || rangedSpawned < currentWave.RangedAICount)
            {
                bool spawnMelee = Random.Range(0, 2) == 0;

                if (spawnMelee && meleeSpawned < currentWave.MeleeAICount)
                {
                    SpawnMeleeAI();
                    meleeSpawned++;
                }
                else if (!spawnMelee && rangedSpawned < currentWave.RangedAICount)
                {
                    SpawnRangedAI();
                    rangedSpawned++;
                }

                float interval = Random.Range(currentWave.MinSpawnInterval, currentWave.MaxSpawnInterval);
                yield return Timing.WaitForSeconds(interval);
            }

            yield return Timing.WaitUntilDone(WaitForAllEnemiesDefeated());

            _killedWaveEnemies = 0;
            UpdateEnemyCount(true);
            WaveFinishedEvent?.Invoke();
            _levelStateHandler.SetLevelState(LevelState.Upgrading);

            _currentWaveIndex++;
            if (_currentWaveIndex >= _waves.Length)
            {
                if (_loopWaves)
                {
                    _currentWaveIndex = 0;
                }
                else
                {
                    AllWavesCompleted();
                    yield break;
                }
            }

            _spawnerRunning = false;
        }

        private IEnumerator<float> WaitForAllEnemiesDefeated()
        {
            while (_spawnedEnemies.Count > 0)
            {
                yield return Timing.WaitForOneFrame;
            }
        }

        private void AllWavesCompleted()
        {
            AllWavesCompletedEvent?.Invoke();

            AIBossController boss = _bossFactory.CreateBoss();
            boss.transform.position = _bossSpawnPoint;
            boss.SetUp(this, _wwiseEventHandler);
        }

        private void UpdateEnemyCount(bool waveDone = false)
        {
            int total = _waves[_currentWaveIndex].MeleeAICount + _waves[_currentWaveIndex].RangedAICount;
            int remaining = total - _killedWaveEnemies;
            if (waveDone)
            {
                total = 0;
                remaining = 0;
            }
            _signalBus.Fire(new EnemiesUpdatedSignal(_currentWaveIndex + 1, total, remaining, _waves.Length));
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
