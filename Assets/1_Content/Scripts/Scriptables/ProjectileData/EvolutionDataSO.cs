using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    public abstract class EvolutionDataSO : ScriptableObject
    {
        [field: BoxGroup("General"), SerializeField]
        public string EvolutionName { get; private set; }
        [field: BoxGroup("General"), SerializeField, Range(1, 3)]
        public int EvolutionLevel { get; private set; }
        [field: BoxGroup("General"), SerializeField, TextArea]
        public string Description { get; private set; }
        [field: BoxGroup("General"), SerializeField]
        public Sprite Icon { get; private set; }

        [field: BoxGroup("General"), SerializeField]
        public int Damage { get; private set; } = 10;

        [field: BoxGroup("General"), SerializeField]
        public float Speed { get; private set; } = 10f;

        [field: BoxGroup("General"), SerializeField]
        public float ActivationBounces { get; private set; } = 12f;

        //public abstract void OnHit();

        public abstract ProjectileType GetProjectileType();

        public abstract void OnEvolve(Projectile projectile);
    }
}