using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "NewProjectilePattern", menuName = "BH/Patterns/New Projectile Pattern")]
    public class ProjectilePatternDataSO : ScriptableObject
    {
        [field: BoxGroup("General"), SerializeField]
        public int BulletsPerPhase { get; private set; } = 50;

        [field: BoxGroup("Pattern Settings"), SerializeField, Range(0f, 100f)]
        public float RotationSpeed { get; private set; } = 20f;
        [field: BoxGroup("Pattern Settings"), SerializeField, Range(0f, 2f)]
        public float SpawnFrequency { get; private set; } = 0.5f;
        [field: BoxGroup("Pattern Settings"), SerializeField, Range(0, 500)]
        public int NumBullets { get; private set; } = 20;
        [field: BoxGroup("Pattern Settings"), SerializeField, Range(0f, 10f)]
        public float SpiralTurns { get; private set; } = 5f;
        [field: BoxGroup("Pattern Settings"), SerializeField, Range(0f, 10f)]
        public float SpiralRadius { get; private set; } = 1f;
        [field: BoxGroup("Pattern Settings"), SerializeField, Range(0f, 360f)]
        public float StartAngle { get; private set; } = 0f;
        [field: BoxGroup("Pattern Settings"), SerializeField, Range(0f, 360f)]
        public float EndAngle { get; private set; } = 360f;
    }
}