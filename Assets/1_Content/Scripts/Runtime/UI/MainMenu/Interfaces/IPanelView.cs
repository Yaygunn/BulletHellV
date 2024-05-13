using UnityEngine.UI;

namespace BH.Runtime.UI
{
    public interface IPanelView
    {
        public bool IsDefaultPanel { get; }
        public Button[] ShowPanelButtons { get; }
        public void SetVisibility(bool isVisible);
    }
}