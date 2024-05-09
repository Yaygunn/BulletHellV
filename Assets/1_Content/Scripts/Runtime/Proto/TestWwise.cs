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
        
        private IWwiseEventHandler _wwiseEventHandler;
        private SignalBus _signalBus;
        
        [Inject]
        private void Construct(IWwiseEventHandler wwiseEventHandler, SignalBus signalBus)
        {
            _wwiseEventHandler = wwiseEventHandler;
            _signalBus = signalBus;
        }
        
        [Button(ButtonSizes.Large)]
        public void SetTestState()
        {
            _signalBus.Fire(new AudioStateSignal(_stateToTest));
        }
        
        [Button(ButtonSizes.Large)]
        public void PlayTestSFX()
        {
            _wwiseEventHandler.PostAudioEvent(_playerSFXToTest);
        }
    }
}