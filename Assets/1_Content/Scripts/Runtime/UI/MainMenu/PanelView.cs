using DP.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace BH.Runtime.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class PanelView : MonoBehaviour, IPanelView
    {
        [field: SerializeField, Tooltip("Panel to show when Menu scene loads. Make sure there is only one.")]
        public bool IsDefaultPanel { get; private set; }
        
        [field: SerializeField, Tooltip("Buttons used to show this specific panel (normally from other panel(s)).")]
        public Button[] ShowPanelButtons { get; private set; }
    
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetVisibility(bool isVisible)
        {
            Tools.ToggleVisibility(_canvasGroup, isVisible);
        }
    }
}