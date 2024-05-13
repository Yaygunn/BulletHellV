using System;
using System.Collections.Generic;
using BH.Runtime.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "BH/Audio/New Audio Settings")]
    public class AudioSettingsSO : ScriptableObject
    {
        [field: BoxGroup("Sound Banks"), SerializeField]
        public List<AK.Wwise.Bank> Soundbanks { get; private set; }
        
        [field: BoxGroup("Player SFX"), SerializeField]
        public List<AudioEventData<PlayerSFX>> PlayerSFX { get; private set; }
        
        [field: BoxGroup("Enemy SFX"), SerializeField]
        public List<AudioEventData<EnemySFX>> EnemySFX { get; private set; }
        
        [field: BoxGroup("Projectile SFX"), SerializeField]
        public List<AudioEventData<ProjectileSFX>> ProjectileSFX { get; private set; }
        
        [field: BoxGroup("UI SFX"), SerializeField]
        public List<AudioEventData<UISFX>> UISFX { get; private set; }
        
        [field: BoxGroup("Audio States"), SerializeField]
        public List<AudioStateData> AudioStates { get; private set; }
        
        public List<AudioEventData<T>> GetEventList<T>() where T : Enum
        {
            if (typeof(T) == typeof(PlayerSFX))
                return PlayerSFX as List<AudioEventData<T>>;
            if (typeof(T) == typeof(EnemySFX))
                return EnemySFX as List<AudioEventData<T>>;
            if (typeof(T) == typeof(ProjectileSFX))
                return ProjectileSFX as List<AudioEventData<T>>;
            if (typeof(T) == typeof(UISFX))
                return UISFX as List<AudioEventData<T>>;

            throw new ArgumentException("[AudioSettingsSO] Unsupported type passed...");
        }
    }
}