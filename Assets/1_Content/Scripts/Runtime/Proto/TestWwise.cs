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
        [SerializeField]
        private EnemySFX _enemySFXToTest = EnemySFX.Die;
        [SerializeField]
        private ProjectileSFX _projectileSFXToTest = ProjectileSFX.Impact;
        [SerializeField]
        private UISFX _uiSFXToTest = UISFX.MenuPositive;
        [SerializeField]
        private Music _musicToTest = Music.Play;


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
        
        [Button(ButtonSizes.Large)]
        public void EnemyTestSFX()
        {
            _wwiseEventHandler.PostAudioEvent(_enemySFXToTest);
        }

        [Button(ButtonSizes.Large)]
        public void ProjectileTestSFX()
        {
            _wwiseEventHandler.PostAudioEvent(_projectileSFXToTest);
        }

        [Button(ButtonSizes.Large)]
        public void UITestSFX()
        {
            _wwiseEventHandler.PostAudioEvent(_uiSFXToTest);
        }


        [Button(ButtonSizes.Large)]
        public void MusicTest()
        {
            _wwiseEventHandler.PostAudioEvent(_musicToTest);
        }

    }
}