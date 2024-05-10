using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "BH/Level/New Level Settings")]
    public class LevelSettingsSO : ScriptableObject
    {
        // TODO: Intro Cutscene?
        
        [field: BoxGroup("Player Spawn"), SerializeField]
        public bool UseSpawnTransform { get; private set; }
        [field: BoxGroup("Player Spawn"), ShowIf(nameof(UseSpawnTransform)), SerializeField]
        public Transform SpawnTransform { get; private set; }
        [field: BoxGroup("Player Spawn"), HideIf(nameof(UseSpawnTransform)), SerializeField]
        public Vector2 SpawnPosition { get; private set; }
        [field: BoxGroup("Player Spawn"), SerializeField]
        public bool RespawnOnDeath { get; private set; } = true;
        [field: BoxGroup("Player Spawn"), SerializeField]
        public float RespawnDelay { get; private set; } = 3f;
    }
}