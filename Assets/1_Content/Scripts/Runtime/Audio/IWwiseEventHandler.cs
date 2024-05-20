using System;
using UnityEngine;

namespace BH.Runtime.Audio
{
    public interface IWwiseEventHandler
    {
        public void PostAudioEvent<T>(T eventType) where T : Enum;
        public void PostAudioEvent<T>(T eventType, GameObject postableObject) where T : Enum;

        public void SetMusicVolume(float value);

        public void SetSFXVolume(float value);

        public void SetMasterVolume(float value);
    }
}