using System;
using System.Collections.Generic;
using System.Linq;
using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BH.Runtime.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        [BoxGroup("UI Elements"), SerializeField]
        private Slider _slider;
        
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
            _signalBus.Subscribe<PlayerBulletsChangedSignal>(OnBulletsChanged);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<PlayerHealthChangedSignal>(OnHealthChanged);
            _signalBus.TryUnsubscribe<PlayerBulletsChangedSignal>(OnBulletsChanged);
        }

        private void OnHealthChanged(PlayerHealthChangedSignal signal)
        {
            _slider.maxValue = signal.MaxHealth;
            _slider.value = signal.CurrentHealth;
        }
        
        public float GetSliderValue()
        {
            return _slider.value;
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