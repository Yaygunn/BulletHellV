using System;
using TMPro;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BH.Runtime.UI
{
    public class MenuPanel : PanelView
    {
        [SerializeField]
        private TMP_Text _versionText;

        private void Start()
        {
            _versionText.text = $"Version: {Application.version}";
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