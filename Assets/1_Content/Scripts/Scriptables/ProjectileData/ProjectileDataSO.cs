using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    public abstract class ProjectileDataSO : ScriptableObject
    {
        [field: BoxGroup("General"), SerializeField]
        public string ProjectileName { get; private set; }
        [field: BoxGroup("General"), SerializeField, Range(0, 3)]
        public int ProjectileLevel { get; private set; }
        [field: BoxGroup("General"), SerializeField, TextArea]
        public string Description { get; private set; }
        [field: BoxGroup("General"), SerializeField]
        public Sprite Icon { get; private set; }

        [field: BoxGroup("General"), SerializeField]
        public float Damage { get; private set; } = 10f;

        [field: BoxGroup("General"), SerializeField]
        public float Speed { get; private set; } = 10f;

        [field: BoxGroup("General"), SerializeField]
        public float MaxBounces { get; private set; } = 12f;

        //public abstract void OnHit();

        public abstract ProjectileType GetProjectileType();
    }
}