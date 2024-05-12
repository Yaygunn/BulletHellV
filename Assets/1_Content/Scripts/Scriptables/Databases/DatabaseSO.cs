using System;
using System.Collections.Generic;
using System.Linq;
using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables.Databases
{
    [CreateAssetMenu(fileName = "GameDatabase", menuName = "BH/Databases/New Game Database")]
    public class DatabaseSO : ScriptableObject
    {
        [field: BoxGroup("Projectiles"), SerializeField]
        public List<AttractorBulletDataSO> AttractorBulletData { get; private set; }
        
        [field: BoxGroup("Projectiles"), SerializeField]
        public List<BasicBulletDataSO> BasicBulletData { get; private set; }
        
        [field: BoxGroup("Projectiles"), SerializeField]
        public List<ChainReactionBulletDataSO> ChainReactionBulletData { get; private set; }
        
        [field: BoxGroup("Projectiles"), SerializeField]
        public List<ExplodingBulletDataSO> ExplodingBulletData { get; private set; }
        
        [field: BoxGroup("Projectiles"), SerializeField]
        public List<HealingBulletDataSO> HealingBulletData { get; private set; }
        
        [field: BoxGroup("Projectiles"), SerializeField]
        public List<HomingBulletDataSO> HomingBulletData { get; private set; }

        private Dictionary<ProjectileType, Func<List<ProjectileDataSO>>> _dataAccessors;

        private void OnEnable()
        {
            _dataAccessors = new Dictionary<ProjectileType, Func<List<ProjectileDataSO>>>
            {
                { ProjectileType.AttractorBullet, () => AttractorBulletData.Cast<ProjectileDataSO>().ToList() },
                { ProjectileType.PlayerBasicBullet, () => BasicBulletData.Where(x => x.Type == ProjectileType.PlayerBasicBullet).Cast<ProjectileDataSO>().ToList() },
                { ProjectileType.EnemyBasicBullet, () => BasicBulletData.Where(x => x.Type == ProjectileType.EnemyBasicBullet).Cast<ProjectileDataSO>().ToList() },
                { ProjectileType.ChainReactionBullet, () => ChainReactionBulletData.Cast<ProjectileDataSO>().ToList() },
                { ProjectileType.ExplodingBullet, () => ExplodingBulletData.Cast<ProjectileDataSO>().ToList() },
                { ProjectileType.HealingBullet, () => HealingBulletData.Cast<ProjectileDataSO>().ToList() },
                { ProjectileType.HomingBullet, () => HomingBulletData.Cast<ProjectileDataSO>().ToList() },
            };
        }

        public ProjectileDataSO GetProjectileData(ProjectileType type, int projectileLevel)
        {
            return _dataAccessors.TryGetValue(type, out Func<List<ProjectileDataSO>> getList) 
                ? getList().FirstOrDefault(x => x.ProjectileLevel == projectileLevel) : null;
        }
    }
}