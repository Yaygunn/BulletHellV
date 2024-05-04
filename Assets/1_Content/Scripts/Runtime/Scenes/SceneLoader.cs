using UnityEngine.SceneManagement;

namespace BH.Runtime.Scenes
{
    public enum SceneType
    {
        MainMenu,
        Game,
        Credits
    }
    
    public class SceneLoader
    {
        public async void LoadSceneAsync(SceneType sceneType)
        {
            //SceneManager.LoadSceneAsync()
        }
    }
}