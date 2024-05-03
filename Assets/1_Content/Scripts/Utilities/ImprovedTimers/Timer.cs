﻿using System;

namespace BH.Utilities.ImprovedTimers
{
    public abstract class Timer
    {
        protected float _initialTime;
        public float Time { get; set; }
        public bool IsRunning { get; protected set; }

        public float Progress => Time / _initialTime;

        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };

        protected Timer(float value) 
        {
            _initialTime = value;
            IsRunning = false;
        }

        public void Start() 
        {
            Time = _initialTime;
            if (IsRunning) return;
            
            IsRunning = true;
            TimerManager.RegisterTimer(this);
            OnTimerStart.Invoke();
        }

        public void Stop()
        {
            if (!IsRunning) return;
            
            IsRunning = false;
            TimerManager.DeregisterTimer(this);
            OnTimerStop.Invoke();
        }

        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;

        public abstract void Tick(float deltaTime);
    }
}