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

        private AudioState _currentAudioState = AudioState.GamePaused;
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
            PostAudioEvent(Music.Play);

            _signalBus.Subscribe<AudioStateSignal>(x => ChangeAudioState(x.AudioState));
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
            _signalBus.Subscribe<LevelStateChangedSignal>(OnLevelStateChanged);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<AudioStateSignal>(x => ChangeAudioState(x.AudioState));
            _signalBus.TryUnsubscribe<GameStateChangedSignal>(OnGameStateChanged);
            _signalBus.TryUnsubscribe<LevelStateChangedSignal>(OnLevelStateChanged);
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

        // GAME STATE SIGNALS
        private void OnGameStateChanged(GameStateChangedSignal signal)
        {
            switch (signal.GameState)
            {
                case GameState.Menu:

                    /*PostAudioEvent(Music.Play);*/
                    ChangeAudioState(AudioState.GamePaused);
                    break;
                case GameState.Playing:
                    ChangeAudioState(AudioState.GameActive);
                    break;
                case GameState.Paused:
                    ChangeAudioState(AudioState.GamePaused);
                    break;
                case GameState.GameOver:
                    ChangeAudioState(AudioState.GamePaused);
                    break;
            }
        }

        // LEVEL STATE SIGNALS
        private void OnLevelStateChanged(LevelStateChangedSignal signal)
        {
            switch (signal.LevelState)
            {
                case LevelState.NormalRound:
                    ChangeAudioState(AudioState.GameActive);
                    break;
                case LevelState.BossRound:
                    ChangeAudioState(AudioState.GameActive);
                    break;
            }
        }

        public void SetMusicVolume(float value)
        {
            _audioSettings.MusicVolume.SetGlobalValue(value);
        }

        public void SetSFXVolume(float value)
        {
            _audioSettings.SFXVolume.SetGlobalValue(value);
        }

        public void SetMasterVolume(float value)
        {
            _audioSettings.MasterVolume.SetGlobalValue(value);
        }
    }
}