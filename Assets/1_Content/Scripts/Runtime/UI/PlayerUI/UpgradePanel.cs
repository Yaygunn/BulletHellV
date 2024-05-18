using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.Runtime.Managers;
using DP.Utilities;
using MEC;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace BH.Scripts.Runtime.UI
{
    public class UpgradePanel : MonoBehaviour
    {
        private List<UpgradeUI> _upgradeUIs;
        private CanvasGroup _canvasGroup;
        private SignalBus _signalBus;
        
        private void Awake()
        {
            _upgradeUIs = GetComponentsInChildren<UpgradeUI>().ToList();
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            Tools.ToggleVisibility(_canvasGroup, false);
            
            for (int i = 0; i < _upgradeUIs.Count; i++)
            {
                _upgradeUIs[i].SetUp(i);
                _upgradeUIs[i].ButtonClickedEvent += OnUpgradeButtonClicked;
            }
            
            _signalBus.Subscribe<UpgradesShowSignal>(OnShowUpgrades);
        }

        private void OnDestroy()
        {
            foreach (UpgradeUI upgrade in _upgradeUIs)
            {
                upgrade.ButtonClickedEvent -= OnUpgradeButtonClicked;
            }
            
            _signalBus.TryUnsubscribe<UpgradesShowSignal>(OnShowUpgrades);
        }
        
        private void OnShowUpgrades(UpgradesShowSignal signal)
        {
            for (int i = 0; i < _upgradeUIs.Count; i++)
            {
                _upgradeUIs[i].SetDescription(signal.UpgradeOptions[i].Description);
            }

            StartCoroutine(ShowUpgradeOptionsDelayCoroutine());
        }

        private void OnUpgradeButtonClicked(int buttonIndex)
        {
            _signalBus.Fire(new UpgradeSelectedSignal(buttonIndex));
            Tools.ToggleVisibility(_canvasGroup, false);
        }
        
        private IEnumerator ShowUpgradeOptionsDelayCoroutine()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            Tools.ToggleVisibility(_canvasGroup, true);
        }
    }
}