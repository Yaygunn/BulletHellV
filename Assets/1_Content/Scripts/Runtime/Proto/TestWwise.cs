using BH.Runtime.Audio;
using BH.Runtime.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Test
{
    public class TestWwise : MonoBehaviour
    {
        [SerializeField]
        private AudioState _stateToTest = AudioState.GameActive;

        [SerializeField]
        private PlayerSFX _playerSFXToTest = PlayerSFX.Hurt;
        
        private AudioManager _audioManager;
        
        [Inject]
        private void Construct(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        
        [Button(ButtonSizes.Large)]
        public void SetTestState()
        {
            _audioManager.ChangeAudioState(_stateToTest);
        }
        
        [Button(ButtonSizes.Large)]
        public void PlayTestSFX()
        {
            _audioManager.PostAudioEvent(_playerSFXToTest);
        }
    }
}