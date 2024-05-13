#if !DISABLE_GIT_LOCKS
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace KreliStudio
{

    [InitializeOnLoad]
    public sealed class GitAssetModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        static GitAssetModificationProcessor()
        {
            EditorSceneManager.sceneOpened += OnSceneOpened;

            void OnSceneOpened(Scene scene, OpenSceneMode mode)
            {
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);

                if (sceneAsset == null)
                    return;

                if (GitCommands.GetLockDataForAsset(sceneAsset.GetInstanceID()) == null)
                    GitCommands.ReadGitLocksCoroutine(cached: false)
                        .Then(() => ShowSceneNotification(sceneAsset))
                        .StartEditorCoroutine();
                else
                    ShowSceneNotification(sceneAsset);
            }
        }

        static void ShowSceneNotification(SceneAsset sceneAsset)
        {
            var lockData = GitCommands.GetLockDataForAsset(sceneAsset.GetInstanceID());

            if (lockData == null)
                return;

            var (owner, isLocalOwner) = (lockData.Value.owner.name, lockData.Value.isLocalOwner);

            var notification = isLocalOwner ? GitGUIStyle.notificationLockedLocal : GitGUIStyle.notificationLockedRemote;
            notification.text = isLocalOwner ? $"You have an active lock on '{sceneAsset.name}' scene." : $"Warning: Scene '{sceneAsset.name}' is locked by user {owner}.";
            double fadeoutWait = isLocalOwner ? 2f : 20f;

            if (SceneView.lastActiveSceneView is { } lastActiveSceneView && lastActiveSceneView)
                lastActiveSceneView.ShowNotification(notification, fadeoutWait);
        }
        
        static string[] OnWillSaveAssets(string[] paths)
        {
            GitCommands.VerifyModifiedAssetsAsync(paths);
            return paths;
        }

        static bool IsOpenForEdit(string[] assetOrMetaFilePaths, List<string> outNotEditablePaths, StatusQueryOptions statusQueryOptions)
        {
            bool result = true;

            foreach (var assetPath in assetOrMetaFilePaths)
            {
                var lockData = GitCommands.GetLockDataForAsset(assetPath);

                if (lockData == null)
                    continue;

                if (lockData.Value.isLocalOwner)
                    continue;

                result = false;
                outNotEditablePaths.Add(assetPath);
            }

            return result;
        }
    }
}
#endif
