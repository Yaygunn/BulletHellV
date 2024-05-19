using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    public abstract class ProjectileDataSO : ScriptableObject
    {
        [field: BoxGroup("General Info"), SerializeField]
        public string ProjectileName { get; private set; }
        [field: BoxGroup("General Info"), SerializeField, TextArea]
        public string Description { get; private set; }
        [field: BoxGroup("General Info"), SerializeField, TextArea]
        public string PosativeEffect { get; private set; }
        [field: BoxGroup("General Info"), SerializeField, TextArea]
        public string NegativeEffect { get; private set; }
        [field: BoxGroup("General Info"), SerializeField]
        public Sprite Icon { get; private set; }
        [field: BoxGroup("General Info"), SerializeField]
        public Color Color { get; private set; } = Color.white;

        [field: BoxGroup("General Settings"), SerializeField]
        public int Damage { get; private set; } = 10;
        [field: BoxGroup("General Settings"), SerializeField]
        public float Speed { get; private set; } = 10f;
        [field: BoxGroup("General Settings"), SerializeField]
        public bool IsEvolution { get; private set; } = true;
        [field: BoxGroup("General Settings"), SerializeField]
        public int EvolutionBounces { get; private set; } = 1;
        [field: BoxGroup("General Settings"), SerializeField]
        public int ActivationBounces { get; private set; } = 3;

        public abstract ProjectileType GetProjectileType();
    }
}