using System;
using GH.Scriptables;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Systems
{
    [Serializable]
    public class Stats
    {
        [field: FoldoutGroup("Health"), SerializeField]
        public MMHealthBar HealthBarVisaul { get; private set; }
        [field: FoldoutGroup("Health"), SerializeField, ReadOnly]
        public int CurrentHealth { get; private set; }
        [field: FoldoutGroup("Health"), SerializeField, ReadOnly]
        public int MaxHealth { get; private set; }
        [FoldoutGroup("Health"), SerializeField]
        private int _initialHealth;
        //[FoldoutGroup("Health"), SerializeField]
        //private int _healthRecoveryRate;
        [FoldoutGroup("Health"), SerializeField]
        private bool _isInvincible;

        // TODO: Implement shield....
        [field: FoldoutGroup("Shield"), SerializeField, ReadOnly]
        public int CurrentShield { get; private set; }
        [field: FoldoutGroup("Shield"), SerializeField, ReadOnly]
        public int MaxShield { get; private set; }
        [FoldoutGroup("Shield"), SerializeField]
        private int _initialShield;
        [FoldoutGroup("Shield"), SerializeField]
        private float _shieldRecoveryRate;

        [field: FoldoutGroup("Speed"), SerializeField, ReadOnly]
        public float CurrentSpeed { get; private set; }
        [FoldoutGroup("Speed"), SerializeField]
        private int _initialSpeed;

        [field: FoldoutGroup("Modifications"), SerializeField, HideLabel]
        public GeneralStatMod StatMod { get; private set; }

        public Action<int, int> HealthChangedEvent;
        public Action<int, int> ShieldChangedEvent;
        public Action<float, float> SpeedChangedEvent;
        public Action DiedEvent;

        private float _shieldRegenTimer;

        public void Initialize()
        {
            HealthBarVisaul?.UpdateBar(CurrentHealth, 0, MaxHealth, false);
        }
        
        public void LogicUpdate(float deltaTime)
        {
            if (CurrentShield < MaxShield)
            {
                _shieldRegenTimer += deltaTime;
                if (_shieldRegenTimer >= _shieldRecoveryRate)
                {
                    CurrentShield = Math.Min(CurrentShield + 1, MaxShield);
                    ShieldChangedEvent?.Invoke(MaxShield, CurrentShield);
                    _shieldRegenTimer = 0;
                }
            }
        }
        
        public void ResetStats()
        {
            MaxHealth = _initialHealth;
            CurrentHealth = MaxHealth;
            HealthChangedEvent?.Invoke(MaxHealth, CurrentHealth);

            MaxShield = _initialShield;
            CurrentShield = MaxShield;
            ShieldChangedEvent?.Invoke(MaxShield, CurrentShield);

            CurrentSpeed = _initialSpeed;
            SpeedChangedEvent?.Invoke(_initialSpeed, CurrentSpeed);

            StatMod.Reset();
        }

        public bool TakeDamage(int amount)
        {
            if (_isInvincible || CurrentHealth <= 0)
                return false;
            
            if (CurrentShield > 0)
            {
                CurrentShield = Math.Max(0, CurrentShield - amount);
                ShieldChangedEvent?.Invoke(MaxShield, CurrentShield);
                if (CurrentShield > 0)
                    return true;
                else
                    amount = Math.Abs(CurrentShield);
            }

            CurrentHealth = Math.Max(0, CurrentHealth - amount);
            HealthChangedEvent?.Invoke(MaxHealth, CurrentHealth);
            HealthBarVisaul?.UpdateBar(CurrentHealth, 0, MaxHealth, true);

            if (CurrentHealth == 0)
            {
                DiedEvent?.Invoke();
            }

            return true;
        }

        public void SetInvincibility(bool isInvincible)
        {
            _isInvincible = isInvincible;
        }

        public void ModifyStat(StatUpgradeSO statModification)
        {
            statModification.ApplyUpgrade(StatMod);
            UpdateMaxHealth();
            UpdateMaxShield();
            UpdateSpeed();
        }

        public void ModifyStatsManual(float healthMultiplier, float shieldMultiplier, float speedMultiplier)
        {
            StatMod.HealthMultiplier *= healthMultiplier;
            StatMod.ShieldMultiplier *= shieldMultiplier;
            StatMod.SpeedMultiplier *= speedMultiplier;
            UpdateMaxHealth();
            UpdateMaxShield();
            UpdateSpeed();
        }

        private void UpdateMaxHealth()
        {
            MaxHealth = (int)((_initialHealth + StatMod.IncreasedHealth) * StatMod.HealthMultiplier);
            CurrentHealth = Math.Min(CurrentHealth, MaxHealth);
            HealthChangedEvent?.Invoke(MaxHealth, CurrentHealth);
        }

        private void UpdateMaxShield()
        {
            MaxShield = (int)((_initialShield + StatMod.IncreasedShield) * StatMod.ShieldMultiplier);
            CurrentShield = Math.Min(CurrentShield, MaxShield);
            ShieldChangedEvent?.Invoke(MaxShield, CurrentShield);
        }

        private void UpdateSpeed()
        {
            CurrentSpeed = (_initialSpeed + StatMod.IncreasedSpeed) * StatMod.SpeedMultiplier;
            SpeedChangedEvent?.Invoke(_initialSpeed, CurrentSpeed);
        }
    }
}