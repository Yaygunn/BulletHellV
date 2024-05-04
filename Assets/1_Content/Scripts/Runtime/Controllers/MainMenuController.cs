using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private PanelView _settingsPanelView;

    private void Awake()
    {
        _settingsPanelView = GetComponentInChildren<PanelView>();
    }

    private void Start()
    {
        _settingsPanelView.SetVisibility(false);
    }

    public void OnClick_PlayButton()
    {
        Debug.Log("PlayButton Clicked!");
    }

    public void OnClick_SettingsButton()
    {
        _settingsPanelView.SetVisibility(true);
    }

    public void OnClick_SettingsBackButton()
    {
        _settingsPanelView.SetVisibility(false);
    }
}