using System;
using System.Collections.Generic;
using System.Linq;
using BH.Runtime.Entities;
using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BH.Runtime.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        [BoxGroup("UI Elements"), SerializeField]
        private Slider _healthSlider;
        [BoxGroup("UI Elements"), SerializeField]
        private Slider _shieldSlider;
        [BoxGroup("UI Elements"), SerializeField]
        private TMP_Text _waveText;
        [BoxGroup("UI Elements"), SerializeField]
        private TMP_Text _enemiesText;

        private List<BulletVisual> _bulletVisuals;

        private SignalBus _signalBus;



        private void Awake()
        {
            _bulletVisuals = GetComponentsInChildren<BulletVisual>().ToList();
        }

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<PlayerHealthChangedSignal>(OnHealthChanged);
            _signalBus.Subscribe<PlayerShieldChangedSignal>(OnShieldChanged);
            _signalBus.Subscribe<PlayerBulletsChangedSignal>(OnBulletsChanged);
            _signalBus.Subscribe<EnemiesUpdatedSignal>(OnEnemiesUpdated);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<PlayerHealthChangedSignal>(OnHealthChanged);
            _signalBus.TryUnsubscribe<PlayerShieldChangedSignal>(OnShieldChanged);
            _signalBus.TryUnsubscribe<PlayerBulletsChangedSignal>(OnBulletsChanged);
            _signalBus.TryUnsubscribe<EnemiesUpdatedSignal>(OnEnemiesUpdated);
        }

        private void OnHealthChanged(PlayerHealthChangedSignal signal)
        {
            _healthSlider.maxValue = signal.MaxHealth;
            _healthSlider.value = signal.CurrentHealth;
        }

        private void OnShieldChanged(PlayerShieldChangedSignal signal)
        {
            _shieldSlider.maxValue = signal.MaxShield;
            _shieldSlider.value = signal.CurrentShield;
        }

        private void OnEnemiesUpdated(EnemiesUpdatedSignal signal)
        {
            if (signal.RemainingEnemies == 0)
            {
                _waveText.text = "";
                _enemiesText.text = "";
            }

            _waveText.text = $"Wave: {signal.Wave} / {signal.TotalWaves}";
            _enemiesText.text = $"Enemies Remaining: {signal.RemainingEnemies}";
        }

        private void OnBulletsChanged(PlayerBulletsChangedSignal signal)
        {
            for (int i = 0; i < _bulletVisuals.Count; i++)
            {
                _bulletVisuals[i].SetBulletVisual(signal.EvolutionIcons[i], signal.EvolutionLevels[i]);
            }
        }
    }
}