using System.Linq;
using BH.Runtime.Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BH.Runtime.UI
{
    public class MainMenuController : MonoBehaviour
    {
        private IPanelView[] _panelViews;
        private IGameStateHandler _gameState;
        
        [Inject]
        private void Construct(IGameStateHandler gameState)
        {
            _gameState = gameState;
        }

        private void Awake()
        {
            _panelViews = GetComponentsInChildren<IPanelView>();
        }

        private void Start()
        {
            SetUpButtonEvents();
            ShowDefaultPanel();
            _gameState.SetGameState(GameState.Menu);
        }
        
        private void SetUpButtonEvents()
        {
            foreach (IPanelView panelView in _panelViews)
            {
                foreach (Button button in panelView.ShowPanelButtons)
                {
                    button.onClick.AddListener(() => SwitchPanel(panelView));
                }
            }
        }
        
        private void ShowDefaultPanel()
        {
            if (_panelViews.Length > 0)
                SwitchPanel(_panelViews.FirstOrDefault(panelView => panelView.IsDefaultPanel));
        }

        private void SwitchPanel(IPanelView panelToShow)
        {
            foreach (IPanelView panelView in _panelViews)
            {
                panelView.SetVisibility(panelView == panelToShow);
            }
        }
    }
}