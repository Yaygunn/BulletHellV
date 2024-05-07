using System.Collections.Generic;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "BH/Audio/New Audio Settings")]
    public class AudioSettingsSO : ScriptableObject
    {
        [field: Header("Startup SoundBanks"), SerializeField]
        public List<AK.Wwise.Bank> Soundbanks { get; private set; }

        [field: Header("Sfx State variables"), SerializeField]
        public AK.Wwise.State SFX_Gameplay { get; private set; }
        [field: SerializeField] 
        public AK.Wwise.State SFX_Paused { get; private set; }
        [field: SerializeField] 
        public AK.Wwise.State SFX_None { get; private set; }
        
        [field: Header("Music State variables"), SerializeField]
        public AK.Wwise.State Music_Gameplay { get; private set; }
        [field: SerializeField] 
        public AK.Wwise.State Music_Paused { get; private set; }
        [field: SerializeField] 
        public AK.Wwise.State Music_None { get; private set; }
        
        [field: Header("Wwise Music Events"), SerializeField]
        public AK.Wwise.Event MusicPlay { get; private set; }
        [field: SerializeField] 
        public AK.Wwise.Event SFXPlay { get; private set; }
    }
}