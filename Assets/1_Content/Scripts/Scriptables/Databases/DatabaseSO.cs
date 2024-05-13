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
        [field: BoxGroup("Projectile Evolutions"), SerializeField]
        public List<AttractorEvolutionDataSO> AttractorEvolutionData { get; private set; }
        
        [field: BoxGroup("Projectile Evolutions"), SerializeField]
        public List<ChainReactionEvolutionDataSO> ChainReactionEvolutionData { get; private set; }
        
        [field: BoxGroup("Projectile Evolutions"), SerializeField]
        public List<ExpandingEvolutionDataSO> ExpandingEvolutionData { get; private set; }
        
        [field: BoxGroup("Projectile Evolutions"), SerializeField]
        public List<ExplodingEvolutionDataSO> ExplodingEvolutionData { get; private set; }
        
        [field: BoxGroup("Projectile Evolutions"), SerializeField]
        public List<HealingEvolutionDataSO> HealingEvolutionData { get; private set; }
        
        [field: BoxGroup("Projectile Evolutions"), SerializeField]
        public List<HomingEvolutionDataSO> HomingEvolutionData { get; private set; }

        private Dictionary<ProjectileType, Func<List<EvolutionDataSO>>> _dataAccessors;

        private void OnEnable()
        {
            _dataAccessors = new Dictionary<ProjectileType, Func<List<EvolutionDataSO>>>
            {
                { ProjectileType.AttractorBullet, () => AttractorEvolutionData.Cast<EvolutionDataSO>().ToList() },
                { ProjectileType.ChainReactionBullet, () => ChainReactionEvolutionData.Cast<EvolutionDataSO>().ToList() },
                { ProjectileType.ExpandingBullet, () => ExpandingEvolutionData.Cast<EvolutionDataSO>().ToList() },
                { ProjectileType.ExplodingBullet, () => ExplodingEvolutionData.Cast<EvolutionDataSO>().ToList() },
                { ProjectileType.HealingBullet, () => HealingEvolutionData.Cast<EvolutionDataSO>().ToList() },
                { ProjectileType.HomingBullet, () => HomingEvolutionData.Cast<EvolutionDataSO>().ToList() },
            };
        }
        
        public bool TryGetNextEvolutionData(ProjectileType type, int level, out EvolutionDataSO nextEvolutionData)
        {
            int index = level - 1;
            
            if (_dataAccessors.TryGetValue(type, out Func<List<EvolutionDataSO>> getList))
            {
                List<EvolutionDataSO> list = getList();
                if (index >= 0 && index < list.Count)
                {
                    nextEvolutionData = list[index];
                    return true;
                }
            }
            nextEvolutionData = null;
            return false;
        }
    }
}