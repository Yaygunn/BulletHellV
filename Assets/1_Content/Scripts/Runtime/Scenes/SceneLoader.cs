using System.Collections.Generic;
using BH.Runtime.Managers;
using BH.Scriptables.Scenes;
using MEC;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace BH.Runtime.Scenes
{
    public class SceneLoader
    {
        private readonly IGameStateHandler _gameState;
        private readonly SceneSettingsSO _sceneSettings;
        
        public SceneLoader(IGameStateHandler gameState, SceneSettingsSO sceneSettings)
        {
            _gameState = gameState;
            _sceneSettings = sceneSettings;
        }
        
        public void LoadSceneAsync(SceneType targetSceneType, bool useLoadingScreen = true)
        {
            if (IsLoadingInProgress())
                return;
            
            if (!TryGetSceneBind(targetSceneType, out SceneBind targetSceneBind))
                return;
            
            StartSceneLoad(targetSceneBind, useLoadingScreen);
        }
        
        private bool IsLoadingInProgress()
        {
            if (_gameState.CurrentGameState != GameState.Loading) return false;
            
            Debug.LogError("[SceneLoader] Attempted to load a scene while another scene load is in progress.");
            return true;
        }

        private bool TryGetSceneBind(SceneType sceneType, out SceneBind sceneBind)
        {
            sceneBind = _sceneSettings.SceneBinds.Find(bind => bind.SceneType == sceneType);
            if (!string.IsNullOrEmpty(sceneBind.SceneName)) 
                return true;
            
            Debug.LogError($"[SceneLoader] SceneBind for {sceneType} not found. Check SceneSettings ScriptableObject...");
            return false;
        }
        
        private void StartSceneLoad(SceneBind targetSceneBind, bool useLoadingScreen)
        {
            _gameState.SetGameState(GameState.Loading);
            
            if (useLoadingScreen && TryGetSceneBind(SceneType.Loader, out SceneBind loadingSceneBind))
                Timing.RunCoroutine(LoadSceneWithLoadingScreen(loadingSceneBind.SceneName, targetSceneBind.SceneName));
            else
                SceneManager.LoadSceneAsync(targetSceneBind.SceneName);
        }

        private IEnumerator<float> LoadSceneWithLoadingScreen(string loadingSceneName, string targetSceneName)
        {
            yield return Timing.WaitUntilDone(SceneManager.LoadSceneAsync(loadingSceneName));

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
            asyncLoad.allowSceneActivation = false;
            
            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                    break;
                }
                
                yield return Timing.WaitForOneFrame;
            }
        }
    }
}