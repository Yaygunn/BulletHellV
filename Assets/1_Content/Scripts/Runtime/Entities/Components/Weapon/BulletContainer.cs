using System;
using BH.Runtime.Systems;
using BH.Scriptables;
using UnityEngine;

namespace BH.Runtime.Entities
{
    [Serializable]
    public class BulletContainer
    {
        [field: SerializeField]
        public ProjectileDataSO BulletData { get; private set; }
        
        // [field: SerializeField]
        // public ProjectileType BulletType { get; private set; } = ProjectileType.PlayerBasicBullet;
        //
        // [field: SerializeField]
        // public int UpgradeLevel { get; private set; } = 0;
        
        public void SetBulletData(ProjectileDataSO data)
        {
            BulletData = data;
        }
    }
}