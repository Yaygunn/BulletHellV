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
            
            _wwiseEventHandler.SetMusicVolume(80f);
            _musicSlider.maxValue = 100f;
            _musicSlider.value = 80f;
            _wwiseEventHandler.SetSFXVolume(80f);
            _sfxSlider.maxValue = 100f;
            _sfxSlider.value = 80f;
            _wwiseEventHandler.SetMasterVolume(80f);
            _masterSlider.maxValue = 100f;
            _masterSlider.value = 80f;
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