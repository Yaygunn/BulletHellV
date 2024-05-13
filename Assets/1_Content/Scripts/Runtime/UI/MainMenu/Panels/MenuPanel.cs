using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BH.Runtime.UI
{
    public class MenuPanel : PanelView
    {
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