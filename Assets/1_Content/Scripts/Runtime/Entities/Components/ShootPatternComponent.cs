using System;
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
        [SerializeField]
        private int _bulletsPerPhase = 50;

        [BoxGroup("Pattern Settings"), SerializeField, Range(0f, 100f)]
        private float _rotationSpeed = 20f;
        [BoxGroup("Pattern Settings"), SerializeField, Range(0f, 2f)]
        private float _spawnFrequency = 0.5f;
        [BoxGroup("Pattern Settings"), SerializeField, Range(0, 500)]
        private int _numBullets = 100;
        [BoxGroup("Pattern Settings"), SerializeField, Range(0f, 10f)]
        private float _spiralTurns = 5f;
        [BoxGroup("Pattern Settings"), SerializeField, Range(0f, 10f)]
        private float _spiralRadius = 1f;
        [BoxGroup("Pattern Settings"), SerializeField, Range(0f, 360f)]
        private float _startAngle = 0f;
        [BoxGroup("Pattern Settings"), SerializeField, Range(0f, 360f)]
        private float _endAngle = 360f;

        private CountdownTimer _spawnTimer;
        private float _angleOffset = 0f;

        [Inject]
        private IProjectileFactory _projectileFactory;

        [Inject]
        private DatabaseSO _database;

        public event Action ShootPatternCompletedEvent;

        private void Start()
        {
            _spawnTimer = new CountdownTimer(_spawnFrequency);
            _spawnTimer.OnTimerStop += GeneratePattern;
        }

        private void Update()
        {
            if (_spawnTimer.IsRunning)
                _angleOffset += _rotationSpeed * Time.deltaTime;
        }

        private void OnDestroy()
        {
            if (_spawnTimer != null)
                _spawnTimer.OnTimerStop -= GeneratePattern;
        }

        public void StartPattern()
        {
            _bulletCounter = 0;
            _angleOffset = 0f;
            _spawnTimer.Reset(_spawnFrequency);
            _spawnTimer.Start();
        }

        public void StopPattern()
        {
            _spawnTimer.Stop();
        }

        private void GeneratePattern()
        {
            if (_bulletCounter >= _bulletsPerPhase)
            {
                StopPattern();
                ShootPatternCompletedEvent?.Invoke();
                return;
            }

            float angleStep = (_endAngle - _startAngle) / _numBullets;
            float currentAngle = _startAngle;
            Vector3 spawnPosition = transform.position;

            for (int i = 0; i < _numBullets; i++)
            {
                float angleRadius = (currentAngle + _angleOffset) * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(angleRadius), Mathf.Sin(angleRadius));
                SpawnBullet(direction * _spiralRadius, spawnPosition);
                currentAngle += angleStep;
            }

            _spawnTimer.Reset(_spawnFrequency);
            _spawnTimer.Start();
        }


        private void SpawnBullet(Vector3 velocity, Vector3 position)
        {
            _bulletCounter++;
            Projectile projectile = _projectileFactory.CreateProjectile(ProjectileType.EnemyBasicBullet);
            _database.TryGetProjectileData(ProjectileType.EnemyBasicBullet, 1, out ProjectileDataSO projectileData);
            projectile.SetUp(position, velocity.normalized, projectileData);
        }
    }
}
