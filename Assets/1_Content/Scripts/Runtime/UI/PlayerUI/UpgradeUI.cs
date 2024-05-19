using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BH.Scripts.Runtime.UI
{
    public class UpgradeUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _descriptionText;
        [SerializeField]
        private TMP_Text _advantageText;
        [SerializeField]
        private TMP_Text _disAdvantageText;

        private Button _button;
        private int _buttonIndex;

        public event Action<int> ButtonClickedEvent;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        public void SetUp(int index)
        {
            _buttonIndex = index;
        }

        public void SetDescription(string description)
        {
            _descriptionText.text = description;
        }


        public void SetAdvantage(string description)
        {
            _advantageText.text = description;
        }

        public void SetDisAdvantage(string description)
        {
            _disAdvantageText.text = description;
        }


        private void OnButtonClicked()
        {
            ButtonClickedEvent?.Invoke(_buttonIndex);
        }
    }
}