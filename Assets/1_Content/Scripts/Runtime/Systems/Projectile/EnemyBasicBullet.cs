using BH.Scriptables;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class EnemyBasicBullet : Projectile
    {
        private EnemyBasicProjectileDataSO _basicProjData;
        
        protected override void SetUpInternal(ProjectileDataSO projectileData)
        {
            if (projectileData is EnemyBasicProjectileDataSO basicData)
            {
                _basicProjData = basicData;
            }
            else
            {
                Debug.LogError("[EnemyBasicBullet] EnemyBasicProjectileDataSo is not set for EnemyBasicBullet");
            }
        }
        
        protected override void HandleEvolution()
        {
            ReturnToPool();
        }

        protected override void HandleActivation()
        {
            ReturnToPool();
        }
    }
}