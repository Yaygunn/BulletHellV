using BH.Scriptables;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class PlayerBasicBullet : Projectile
    {
        private PlayerBasicProjectileDataSO _basicProjData;
        
        protected override void SetUpInternal(ProjectileDataSO projectileData)
        {
            if (projectileData is PlayerBasicProjectileDataSO basicData)
            {
                _basicProjData = basicData;
            }
            else
            {
                Debug.LogError("[PlayerBasicBullet] PlayerBasicProjectileDataSO is not set for PlayerBasicBullet");
            }
        }
        
        protected override void HandleEvolution()
        {
            _currentSpeed *= _basicProjData.SpeedMultiAfterEvolution;
        }

        protected override void HandleActivation()
        {
            ReturnToPool();
        }
    }
}