using System;
using System.Collections;
using BH.Runtime.Factories;
using BH.Runtime.Systems;
using BH.Utilities.ImprovedTimers;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Test
{
    public class TestSpawner : MonoBehaviour
    {
        [field: SerializeField, ReadOnly]
        public int BulletCounter { get; set; }
        
        [BoxGroup("Projectile Spawn"), SerializeField, Range(0f, 100f)]
        private float _rotationSpeed = 20f;
        [BoxGroup("Projectile Spawn"), SerializeField, Range(0f, 2f)]
        private float _spawnFrequency = 0.5f;
        [BoxGroup("Projectile Spawn"), SerializeField, Range(0, 500)]
        private int _numBullets = 100;
        [BoxGroup("Projectile Spawn"), SerializeField, Range(0f, 10f)]
        private float _spiralTurns = 5f;
        [BoxGroup("Projectile Spawn"), SerializeField, Range(0f, 10f)]
        private float _spiralRadius = 1f;
        [BoxGroup("Projectile Spawn"), SerializeField, Range(0f, 360f)]
        private float _startAngle = 0f;
        [BoxGroup("Projectile Spawn"), SerializeField, Range(0f, 360f)]
        private float _endAngle = 360f;
        
        private CountdownTimer _spawnTimer;
        private float _angleOffset = 0f;
        
        [Inject]
        private IProjectileFactory _projectileFactory;
        
        private void Start()
        {
            _spawnTimer = new CountdownTimer(_spawnFrequency);
            _spawnTimer.OnTimerStop += GeneratePattern;
            _spawnTimer.Start();
        }
        
        private void Update()
        {
            _angleOffset += _rotationSpeed * Time.deltaTime;
        }

        private void OnDestroy()
        {
            if (_spawnTimer != null)
                _spawnTimer.OnTimerStop -= GeneratePattern;
        }
        
        private void GeneratePattern()
        {
            if (BulletCounter >= 200)
            {
                _spawnTimer.Stop();
                return;
            }
            
            float angleStep = (_endAngle - _startAngle) / _numBullets;
            float currentAngle = _startAngle;
            
            for (int i = 0; i < _numBullets; i++)
            {
                float angleRadius = (currentAngle + _angleOffset) * Mathf.Deg2Rad;
                Vector2 direction = new (Mathf.Cos(angleRadius), Mathf.Sin(angleRadius));
                SpawnBullet(direction * _spiralRadius);
                currentAngle += angleStep;
            }
            
            _spawnTimer.Reset(_spawnFrequency);
            _spawnTimer.Start();
        }
        
        private void SpawnBullet(Vector3 velocity, Vector3? position = null)
        {
            BulletCounter++;
            Projectile projectile = _projectileFactory.CreateProjectile(ProjectileType.EnemyBasicBullet);
            projectile.transform.position = position ?? Vector3.zero;
            projectile.SetUp(Vector2.zero, velocity.normalized);
        }
    }
}