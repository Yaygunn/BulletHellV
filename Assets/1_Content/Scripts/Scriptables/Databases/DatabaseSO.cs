using System;
using System.Collections.Generic;
using System.Linq;
using BH.Runtime.Systems;
using GH.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables.Databases
{
    [CreateAssetMenu(fileName = "GameDatabase", menuName = "BH/Databases/New Game Database")]
    public class DatabaseSO : ScriptableObject
    {
        [field: BoxGroup("Weapon Upgrades"), SerializeField]
        public List<WeaponUpgradeSO> WeaponUpgradeData { get; private set; }
        
        [field: BoxGroup("Stat Upgrades"), SerializeField]
        public List<StatUpgradeSO> StatUpgradeData { get; private set; }
        
        [field: BoxGroup("Projectile Evolutions"), SerializeField]
        public List<AttractorProjectileDataSO> AttractorEvolutionData { get; private set; }
        
        [field: BoxGroup("Projectile Evolutions"), SerializeField]
        public List<EnemyBasicProjectileDataSO> EnemyBasicProjectileData { get; private set; }
        
        [field: BoxGroup("Projectile Evolutions"), SerializeField]
        public List<ExpandingProjectileDataSO> ExpandingEvolutionData { get; private set; }
        
        [field: BoxGroup("Projectile Evolutions"), SerializeField]
        public List<ExplodingProjectileDataSO> ExplodingEvolutionData { get; private set; }
        
        [field: BoxGroup("Projectile Evolutions"), SerializeField]
        public List<HealingProjectileDataSO> HealingEvolutionData { get; private set; }
        
        [field: BoxGroup("Projectile Evolutions"), SerializeField]
        public List<HomingProjectileDataSO> HomingEvolutionData { get; private set; }
        
        [field: BoxGroup("Projectile Evolutions"), SerializeField]
        public List<PlayerBasicProjectileDataSO> PlayerBasicProjectileData { get; private set; }

        private Dictionary<ProjectileType, Func<List<ProjectileDataSO>>> _dataAccessors;

        private void OnEnable()
        {
            _dataAccessors = new Dictionary<ProjectileType, Func<List<ProjectileDataSO>>>
            {
                { ProjectileType.AttractorBullet, () => AttractorEvolutionData.Cast<ProjectileDataSO>().ToList() },
                { ProjectileType.EnemyBasicBullet, () => EnemyBasicProjectileData.Cast<ProjectileDataSO>().ToList() },
                { ProjectileType.ExpandingBullet, () => ExpandingEvolutionData.Cast<ProjectileDataSO>().ToList() },
                { ProjectileType.ExplodingBullet, () => ExplodingEvolutionData.Cast<ProjectileDataSO>().ToList() },
                { ProjectileType.HealingBullet, () => HealingEvolutionData.Cast<ProjectileDataSO>().ToList() },
                { ProjectileType.HomingBullet, () => HomingEvolutionData.Cast<ProjectileDataSO>().ToList() },
                { ProjectileType.PlayerBasicBullet, () => PlayerBasicProjectileData.Cast<ProjectileDataSO>().ToList() }
            };
        }
        
        public bool TryGetProjectileData(ProjectileType type, int level, out ProjectileDataSO projectileData)
        {
            int index = level - 1;
            
            if (_dataAccessors.TryGetValue(type, out Func<List<ProjectileDataSO>> getList))
            {
                List<ProjectileDataSO> list = getList();
                if (index >= 0 && index < list.Count)
                {
                    projectileData = list[index];
                    return true;
                }
            }
            projectileData = null;
            return false;
        }
    }
}