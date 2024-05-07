using BH.Scriptables;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Managers
{
    public enum WwiseSFXState { Gameplay, Paused, None };
    public enum WwiseMusicState { Gameplay, Paused, None };
    public enum WwiseEvent { MusicPlay, SFXPlay };
    
    public class AudioManager : IInitializable
    {
        private AudioSettingsSO _audioSettings;
        private GameObject _postableObject;
        private bool _isInitialized;
        private WwiseSFXState _currentSFXState;
        private WwiseMusicState _currentMusicState;

        public AudioManager(GameObject postableObject, AudioSettingsSO audioSettings)
        {
            _postableObject = postableObject;
            _audioSettings = audioSettings;
        }
    
        public void Initialize()
        {
            if (!_isInitialized)
                LoadSoundbanks();
            _isInitialized = true;
        
            _currentMusicState = WwiseMusicState.None;
            _currentSFXState = WwiseSFXState.None;
        }
    
        private void LoadSoundbanks()
        {
            if (_audioSettings.Soundbanks.Count > 0)
            {
                foreach (AK.Wwise.Bank bank in _audioSettings.Soundbanks)
                    bank.Load();
                Debug.Log("[AudioManager] Startup Soundbanks have been loaded.");
            }
            else
                Debug.LogError("[AudioManager] Soundbanks list is Empty");
        }

        public void SetWwiseSFXState(WwiseSFXState state)
        {
            if (state == _currentSFXState)
            {
                Debug.Log($"[AudioManager] SFX state is already {state}.");
                return;
            }
        
            switch (state)
            {
                default:
                case (WwiseSFXState.None):
                    _audioSettings.SFX_None.SetValue();
                    break;
                case (WwiseSFXState.Gameplay):
                    _audioSettings.SFX_Gameplay.SetValue();
                    break;
                case (WwiseSFXState.Paused):
                    _audioSettings.SFX_Paused.SetValue();
                    break;

            }
        
            Debug.Log($"[AudioManager] New Wwise SFX state: {state}");
            _currentSFXState = state;
        }
        public void SetWwiseMusicState(WwiseMusicState state)
        {
            if (state == _currentMusicState)
            {
                Debug.Log($"[AudioManager] Music state is already {state}");
                return;
            }

            switch (state)
            {
                default:
                case (WwiseMusicState.None):
                    _audioSettings.Music_None.SetValue();
                    break;
                case (WwiseMusicState.Gameplay):
                    _audioSettings.Music_Gameplay.SetValue();
                    break;
                case (WwiseMusicState.Paused):
                    _audioSettings.Music_Paused.SetValue();
                    break;

            }
        
            Debug.Log($"[AudioManager] New Wwise Music state: {state}");
            _currentMusicState = state;
        }

        public void PostWWiseEvent(AK.Wwise.Event wwiseEvent)
        {
            if (wwiseEvent == null)
            {
                Debug.LogError("[AudioManager] WWise event is null!");
                return;
            }
        
            if (wwiseEvent.IsValid())
                wwiseEvent.Post(_postableObject);
            else
                Debug.LogError($"[AudioManager] {wwiseEvent.Name} Wwise event is invalid!");
        }
        public void PostWWiseEvent(AK.Wwise.Event wwiseEvent, GameObject targetObject)
        {
            if (wwiseEvent == null || targetObject == null)
            {
                Debug.LogError("[AudioManager] WWise event or target gameobject is null!");
                return;
            }
            
            if (wwiseEvent.IsValid())
                wwiseEvent.Post(targetObject);
            else
                Debug.LogError($"[AudioManager] {wwiseEvent.Name} Wwise event is invalid!");
        }
    }
}