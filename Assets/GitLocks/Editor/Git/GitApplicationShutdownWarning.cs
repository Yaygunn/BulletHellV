#if !DISABLE_GIT_LOCKS
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KreliStudio
{
    [InitializeOnLoad]
    public static class GitApplicationShutdownWarning
    {
        public static bool ShouldContinueQuitting = true;

        static GitApplicationShutdownWarning()
        {
            // Sometime Unity in batch mode can crash at OnApplicationQuitting callback.
            // Do not subscribe in batch mode to avoid potential crashes.
            if (Application.isBatchMode)
                return;

            EditorApplication.wantsToQuit += OnApplicationQuitting;
        }
        
        static bool OnApplicationQuitting()
        {
            try
            {
                ShouldContinueQuitting = true;

                GitCommands
                    .ReadGitLocksCoroutine(
                        cached: true,
                        callback: locks =>
                        {
                            var ownedLocks = locks
                                .Where(y => y.isLocalOwner)
                                .Select(y => (y.path, y.owner.name))
                                .ToArray();

                            if (ownedLocks.Any())
                                GitLocksWarningPopup.OpenShutdownWindow(ownedLocks.ToList());
                        })
                    .ExecuteEditorCoroutineSynchronously();

            }
            catch (System.Exception exception)
            {
                Debug.LogException(exception);
            }

            return ShouldContinueQuitting;
        }
    }
}
#endif
