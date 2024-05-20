using BH.Runtime.Audio;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BH.Runtime.UI
{
    public class SettingsPanel : PanelView
    {
        [SerializeField]
        private Slider _musicSlider;
        [SerializeField]
        private Slider _sfxSlider;
        [SerializeField]
        private Slider _masterSlider;
        
        private IWwiseEventHandler _wwiseEventHandler;
        
        [Inject]
        private void Construct(IWwiseEventHandler wwiseEventHandler)
        {
            _wwiseEventHandler = wwiseEventHandler;
        }
        
        private void Start()
        {
            _musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            _sfxSlider.onValueChanged.AddListener(OnSFXSliderValueChanged);
            _masterSlider.onValueChanged.AddListener(OnMasterSliderValueChanged);
            
            _wwiseEventHandler.SetMusicVolume(50);
            _wwiseEventHandler.SetSFXVolume(50);
            _wwiseEventHandler.SetMasterVolume(50);
        }

        private void OnDestroy()
        {
            _musicSlider.onValueChanged.RemoveListener(OnMusicSliderValueChanged);
            _sfxSlider.onValueChanged.RemoveListener(OnSFXSliderValueChanged);
            _masterSlider.onValueChanged.RemoveListener(OnMasterSliderValueChanged);
        }

        private void OnMusicSliderValueChanged(float value)
        {
            _wwiseEventHandler.SetMusicVolume(value);
        }

        private void OnSFXSliderValueChanged(float value)
        {
            _wwiseEventHandler.SetSFXVolume(value);
        }

        private void OnMasterSliderValueChanged(float value)
        {
            _wwiseEventHandler.SetMasterVolume(value);
        }
    }
}