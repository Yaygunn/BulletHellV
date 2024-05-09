using System;
using System.Collections.Generic;
using BH.Runtime.Audio;
using BH.Scriptables;
using UnityEngine;
using Zenject;
using Event = AK.Wwise.Event;

namespace BH.Runtime.Managers
{
    public class AudioManager : IInitializable, IDisposable, IWwiseEventHandler
    {
        private AudioSettingsSO _audioSettings;
        private GameObject _postableObject;
        private SignalBus _signalBus;
        
        private AudioState _currentAudioState = AudioState.Loading;
        private readonly Dictionary<Enum, Event> _eventCache = new ();

        public AudioManager(GameObject postableObject, AudioSettingsSO audioSettings, SignalBus signalBus)
        {
            _postableObject = postableObject;
            _audioSettings = audioSettings;
            _signalBus = signalBus;
        }
    
        public void Initialize()
        {
            LoadSoundbanks();
            
            _signalBus.Subscribe<AudioStateSignal>(x => ChangeAudioState(x.AudioState));
        }
        
        public void Dispose()
        {
            _signalBus.TryUnsubscribe<AudioStateSignal>(x => ChangeAudioState(x.AudioState));
        }
    
        private void LoadSoundbanks()
        {
            if (_audioSettings.Soundbanks.Count < 1)
            {
                Debug.LogError("[AudioManager] Soundbanks list is Empty");
                return;
            }

            foreach (AK.Wwise.Bank bank in _audioSettings.Soundbanks)
            {
                bank.Load();
            }

            Debug.Log("[AudioManager] Sound Banks have been loaded.");
        }

        private void ChangeAudioState(AudioState newState)
        {
            if (newState == _currentAudioState)
                return;
            
            AK.Wwise.State state = _audioSettings.AudioStates.Find(x => x.Type == newState).WwiseState;
            if (state == null)
            {
                Debug.LogError($"[AudioManager] {newState} is not found, make sure it's in AudioSettings");
                return;
            }
        
            state.SetValue();
            _currentAudioState = newState;
            Debug.Log($"[AudioManager] New Audio State: {state}");
        }
        
        public void PostAudioEvent<T>(T eventType) where T : Enum
        {
            PostAudioEvent(eventType, _postableObject);
        }

        public void PostAudioEvent<T>(T eventType, GameObject postableObject) where T : Enum
        {
            if (!_eventCache.TryGetValue(eventType, out Event wwiseEvent))
            {
                List<AudioEventData<T>> audioEvents = _audioSettings.GetEventList<T>();
                if (audioEvents == null)
                {
                    Debug.LogError($"[AudioManager] No audio event data list found for type {typeof(T)}.");
                    return;
                }

                AudioEventData<T> audioEventData = audioEvents.Find(x => x.Type.Equals(eventType));
                if (audioEventData.Equals(default(AudioEventData<T>)))
                {
                    Debug.LogError($"[AudioManager] {eventType} is not found in the AudioSettingsSO for type {typeof(T)}.");
                    return;
                }

                if (!audioEventData.WwiseEvent.IsValid())
                {
                    Debug.LogError($"[AudioManager] Wwise event for {eventType} is not valid.");
                    return;
                }

                wwiseEvent = audioEventData.WwiseEvent;
                _eventCache.Add(eventType, wwiseEvent);
            }

            wwiseEvent.Post(postableObject);
            Debug.Log($"[AudioManager] Posted audio event for {eventType}.");
        }
    }
}