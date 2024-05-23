using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BH.Runtime.UI
{
    public class MenuPanel : PanelView
    {
        [SerializeField]
        private TMP_Text _versionText;

        [SerializeField]
        private Image _toggleImage;
        [SerializeField]
        private Sprite _toggleOn;
        [SerializeField]
        private Sprite _toggleOff;

        [SerializeField]
        private Button _playButton;

        public UnityEvent OnPlayButtonClickedHard;
        public UnityEvent OnPlayButtonClickedEasy;
        
        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnClick_PlayButton);
        }

        private void Start()
        {
            _versionText.text = $"Version: {Application.version}";
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnClick_PlayButton);
        }

        public void OnToggleValueChanged(bool isOn)
        {
            Debug.Log("CALLED: " + isOn);
            
            if (isOn)
            {
                _toggleImage.sprite = _toggleOn;
            }
            else
            {
                _toggleImage.sprite = _toggleOff;
            }
        }
        
        public void OnClick_PlayButton()
        {
            if (_toggleImage.sprite == _toggleOn)
            {
                OnPlayButtonClickedHard?.Invoke();
            }
            else
            {
                OnPlayButtonClickedEasy?.Invoke();
            }
        }

        public void OnClick_ExitButton()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}