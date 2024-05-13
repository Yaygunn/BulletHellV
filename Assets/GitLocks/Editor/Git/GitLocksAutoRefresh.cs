#if !DISABLE_GIT_LOCKS
using System;
using System.Linq;
using UnityEditor;

namespace KreliStudio
{
    [UnityEditor.InitializeOnLoad]
    public static class GitLocksAutoRefresh
    {
        static DateTime lastRefreshTime = DateTime.Now;

        static GitLocksAutoRefresh()
            => EditorApplication.update += () =>
            {
                // wait delay
                if (DateTime.Now - lastRefreshTime <= TimeSpan.FromSeconds(60))
                    return;

                // wait for the editor application window to be in foreground
                if (!UnityEditorInternal.InternalEditorUtility.isApplicationActive)
                    return;

                // reset delay
                ResetCooldown();

                // do not queue multiple tasks if a git command is currently executing
                if (Progress.running)
                    if (Progress.EnumerateItems().Any(x => string.Equals(x.name, "Git", StringComparison.Ordinal)))
                        return;
                                
                // call refresh command
                GitLocksWindow.RefreshCurrentGitLocks();
            };

        public static void ResetCooldown()
            => lastRefreshTime = DateTime.Now;

    }
}
#endif
