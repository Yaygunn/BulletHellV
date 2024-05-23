using System.Collections.Generic;
using BH.Scriptables;
using DP.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class PlayerBasicBullet : Projectile
    {
        [SerializeField, ValueDropdown("GetLayerNames")]
        private string _layerAfterBounce;

        private int _originalLayer;
        private PlayerBasicProjectileDataSO _basicProjData;
        
        protected override void SetUpInternal(ProjectileDataSO projectileData)
        {
            if (projectileData is PlayerBasicProjectileDataSO basicData)
            {
                _basicProjData = basicData;
                _originalLayer = gameObject.layer;
            }
            else
            {
                Debug.LogError("[PlayerBasicBullet] PlayerBasicProjectileDataSO is not set for PlayerBasicBullet");
            }
        }
        
        protected override void HandleEvolution()
        {
            _currentSpeed *= _basicProjData.SpeedMultiAfterEvolution;
            
            int layer = LayerMask.NameToLayer(_layerAfterBounce);
            if (layer != -1)
            {
                gameObject.layer = layer;
            }
        }

        protected override void HandleActivation()
        {
            ReturnToPool();
        }
        
        protected override void ResetProperties()
        {
            base.ResetProperties();
            
            gameObject.layer = _originalLayer;
        }
        
        private IEnumerable<string> GetLayerNames()
        {
            return Tools.GetLayerNames();
        }
    }
}