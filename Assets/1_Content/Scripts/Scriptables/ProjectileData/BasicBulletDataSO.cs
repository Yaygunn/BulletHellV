using System;
using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "BasicBullet", menuName = "BH/Projectiles/New Basic Bullet")]
    public class BasicBulletDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Basic Bullet"), SerializeField]
        public bool IsEnemyBullet { get; private set; }
        
        [field: BoxGroup("Basic Bullet"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; }

        //public override void OnHit()
        //{
        //    
        //}
        
        private void OnValidate()
        {
            if (IsEnemyBullet)
            {
                Type = ProjectileType.EnemyBasicBullet;
            }
            else
            {
                Type = ProjectileType.PlayerBasicBullet;
            }
        }

        public override ProjectileType GetProjectileType()
        {
            return Type;
        }
    }
}