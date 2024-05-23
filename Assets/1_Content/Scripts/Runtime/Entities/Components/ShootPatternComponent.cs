using System;
using BH.Runtime.Audio;
using BH.Runtime.Factories;
using BH.Runtime.Systems;
using BH.Scriptables;
using BH.Scriptables.Databases;
using BH.Utilities.ImprovedTimers;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Entities
{
    public class ShootPatternComponent : MonoBehaviour, IEntityComponent
    {
        [SerializeField, ReadOnly]
        private int _bulletCounter;

        private CountdownTimer _spawnTimer;
        private float _angleOffset = 0f;
        private float _defaultSpawnFrequency = 0.5f;
        private ProjectilePatternDataSO _patternData;

        private IProjectileFactory _projectileFactory;
        private DatabaseSO _database;
        private IWwiseEventHandler _wwiseEventHandler;

        public event Action ShootPatternCompletedEvent;
        
        [Inject]
        private void Construct(DatabaseSO database, IProjectileFactory projectileFactory, IWwiseEventHandler wwiseEventHandler)
        {
            _database = database;
            _projectileFactory = projectileFactory;
            _wwiseEventHandler = wwiseEventHandler;
        }

        private void Start()
        {
            _spawnTimer = new CountdownTimer(_defaultSpawnFrequency);
            _spawnTimer.OnTimerStop += GeneratePattern;
        }

        private void Update()
        {
            if (_spawnTimer.IsRunning)
                _angleOffset += _patternData.RotationSpeed * Time.deltaTime;
        }

        private void OnDestroy()
        {
            if (_spawnTimer != null)
                _spawnTimer.OnTimerStop -= GeneratePattern;
        }

        public void StartPattern(ProjectilePatternDataSO patternData)
        {
            _patternData = patternData;
            _bulletCounter = 0;
            //_angleOffset = 0f;
            _spawnTimer.Reset(_patternData.SpawnFrequency);
            _spawnTimer.Start();
        }

        public void StopPattern()
        {
            _spawnTimer.Stop();
        }

        private void GeneratePattern()
        {
            if (_bulletCounter >= _patternData.BulletsPerPhase)
            {
                StopPattern();
                ShootPatternCompletedEvent?.Invoke();
                return;
            }

            float angleStep = (_patternData.EndAngle - _patternData.StartAngle) / _patternData.NumBullets;
            float currentAngle = _patternData.StartAngle;
            Vector3 spawnPosition = transform.position;

            _wwiseEventHandler.PostAudioEvent(EnemySFX.Fire, gameObject);
            for (int i = 0; i < _patternData.NumBullets; i++)
            {
                float angleRadius = (currentAngle + _angleOffset) * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(angleRadius), Mathf.Sin(angleRadius));
                SpawnBullet(direction * _patternData.SpiralRadius, spawnPosition);
                currentAngle += angleStep;
            }

            _spawnTimer.Reset(_patternData.SpawnFrequency);
            _spawnTimer.Start();
        }


        private void SpawnBullet(Vector3 velocity, Vector3 position)
        {
            _bulletCounter++;
            if (_projectileFactory == null)
                Debug.Log("PROJ FACTORY NULL");
            Projectile projectile = _projectileFactory.CreateProjectile(ProjectileType.EnemyBasicBullet);
            _database.TryGetProjectileData(ProjectileType.EnemyBasicBullet, 1, out ProjectileDataSO projectileData);
            projectile.SetUp(position, velocity.normalized, projectileData);
        }
    }
}
