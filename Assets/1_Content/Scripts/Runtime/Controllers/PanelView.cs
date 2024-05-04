using DP.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PanelView : MonoBehaviour
{
    private CanvasGroup view;

    private void Awake()
    {
        view = GetComponent<CanvasGroup>();
    }

    public void SetVisibility(bool isVisible)
    {
        Tools.ToggleVisibility(view, isVisible);
    }
}
