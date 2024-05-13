#if !DISABLE_GIT_LOCKS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using Unity.EditorCoroutines.Editor;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace KreliStudio
{
    public static class GitCommands
    {
        /// <summary>Cached git username.</summary>
        public static string SelfGitUsername { get; private set; }
        
        /// <summary>Git process name depends on platform</summary>
        public static string GitProcessName
        {
#if UNITY_EDITOR_WIN
            get => "git.exe"; // Windows
#else
            get => "git"; // macOS
#endif
        }

        /// <summary>
        /// It is url to repository history and it is used to open history directly from unity.
        /// Example URL "https://github.com/GitUser/RepositoryName/commits/main/".
        /// </summary>
        public static string RepositoryURL 
        {
            get
            {
                return EditorPrefs.GetString($"repository_url{UnityEngine.Application.productName}");
            }
            set
            {
                EditorPrefs.SetString($"repository_url{UnityEngine.Application.productName}", value);
            }
        }



        internal static readonly Dictionary<int, GitLocksDataItem> lockedAssetsCacheById = new Dictionary<int, GitLocksDataItem>(capacity: 16);
        internal static readonly Dictionary<string, GitLocksDataItem> lockedAssetsCacheByPath = new Dictionary<string, GitLocksDataItem>(capacity: 16);
        static Process gitProcess;



        static GitCommands()
        {
            // delete lockscache.db (the file gets corrupted often which breaks git lfs operations)
            // this should fix git locks operations after editor restart
            try
            {
                const string key = "LocksCacheDeleted";

                if (SessionState.GetBool(key, false))
                    return;

                DeleteLocksCache();
                SessionState.SetBool(key, true);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }

            try
            {
                void UpdateUsername(string username)
                    => EditorPrefs.SetString("git_config_user_name", SelfGitUsername = username.Trim());

                // load cached username from editor prefs
                SelfGitUsername = EditorPrefs.GetString("git_config_user_name");

                // start coroutine to update username
                ExecuteGitCommandCoroutine("config --get user.name", UpdateUsername)
                .StartEditorCoroutine();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }


        /// <summary>Get cached git lock data for asset with instanceId.</summary>
        public static GitLocksDataItem? GetLockDataForAsset(int instanceId)
            => lockedAssetsCacheById.TryGetValue(instanceId, out var result) ? new Nullable<GitLocksDataItem>(result) : new Nullable<GitLocksDataItem>();

        /// <summary>Get cached git lock data for asset with assetPath.</summary>
        public static GitLocksDataItem? GetLockDataForAsset(string assetPath)
            => lockedAssetsCacheByPath.TryGetValue(assetPath, out var result) ? new Nullable<GitLocksDataItem>(result) : new Nullable<GitLocksDataItem>();

        /// <summary>Lock target asset.</summary>
        public static void GitLockAsset(string assetPath)
        {
            if (string.IsNullOrWhiteSpace(assetPath))
                return;

            GitLocksWindow.FreezeWindowUntilNextRefresh();

            ExecuteGitCommandCoroutine($"lfs lock \"{assetPath}\"", Debug.Log)
                .Then(GitLocksWindow.RefreshCurrentGitLocks)
                .Then(AssetDatabase.Refresh)
                .WithProgress("Git", $"Locking asset {Path.GetFileName(assetPath)}...")
                .StartEditorCoroutine();
        }

        /// <summary>Lock group of assets.</summary>
        public static void GitLockAllAssets(IEnumerable<string> assetPaths)
        {
            assetPaths = assetPaths
                .Where(x => !string.IsNullOrWhiteSpace(x));

            var assetCount = assetPaths.Count();

            if (assetCount == 0)
                return;

            GitLocksWindow.FreezeWindowUntilNextRefresh();

            var coroutines = assetPaths
                .Select(x => ExecuteGitCommandCoroutine($"lfs lock  \"{x}\"", Debug.Log))
                .ToArray();

            string s = assetCount > 1 ? "s" : "";

            // synchronously wait for coroutines to complete
            foreach (var coroutine in coroutines)
                coroutine.ExecuteEditorCoroutineSynchronously("Git", $"Locking {assetCount} asset{s}...");

            GitLocksWindow.RefreshCurrentGitLocks();
            AssetDatabase.Refresh();
        }
        
        /// <summary>Unlock target asset. If your git user has permission you can force unlock.</summary>
        public static void GitUnlockAsset(string assetPath, bool force = false)
        {
            if (string.IsNullOrWhiteSpace(assetPath))
                return;

            var lockData = GetLockDataForAsset(assetPath);

            if (lockData == null)
                return;

            bool ok = !force || EditorUtility.DisplayDialog(
                title: "Unlock git file",
                message: $"Do you really want to forcefully unlock?\n{assetPath}\n" +
                         $"Currently locked by: {lockData.Value.owner.name}",
                ok: "Unlock",
                cancel: "Cancel"
            );

            if (!ok)
                return;

            GitLocksWindow.FreezeWindowUntilNextRefresh();

            ExecuteGitCommandCoroutine($"lfs unlock {(force ? "--force" : "")}  \"{assetPath}\"", Debug.Log)
                .Then(GitLocksWindow.RefreshCurrentGitLocks)
                .Then(AssetDatabase.Refresh)
                .Then(() => ClearReadOnly(assetPath))
                .WithProgress("Git", $"Unlocking asset {Path.GetFileName(assetPath)}...")
                .StartEditorCoroutine();
        }

        /// <summary>Unlock group of assets. If your git user has permission you can force unlock.</summary>
        public static void GitUnlockAllAssets(IEnumerable<string> assetPaths, bool force = false)
        {
            assetPaths = assetPaths
                .Where(x => !string.IsNullOrWhiteSpace(x));

            var assetCount = assetPaths.Count();

            if (assetCount == 0)
                return;

            GitLocksWindow.FreezeWindowUntilNextRefresh();

            var coroutines = assetPaths
                .Select(x => ExecuteGitCommandCoroutine($"lfs unlock {(force ? "--force" : "")}  \"{x}\"", Debug.Log))
                .ToArray();

            string s = assetCount > 1 ? "s" : "";

            // synchronously wait for coroutines to complete
            foreach (var coroutine in coroutines)
                coroutine.ExecuteEditorCoroutineSynchronously("Git", $"Unlocking {assetCount} asset{s}...");

            foreach (var assetPath in assetPaths)
                ClearReadOnly(assetPath);

            GitLocksWindow.RefreshCurrentGitLocks();
            AssetDatabase.Refresh();
        }

        /// <summary>Open target asset history on project repository in web browser. It needs set repository url to work correctly.</summary>
        public static void OpenFileHistory(string assetPath)
        {
            assetPath = assetPath.Replace('\\', '/');
            Process.Start(RepositoryURL + assetPath);
        }

        /// <summary>Checkout target asset.</summary>
        public static void GitLfsCheckout(string assetPath)
        {
            if (string.IsNullOrWhiteSpace(assetPath))
                return;

            ExecuteGitCommandCoroutine($"lfs checkout -- \"{assetPath}\" \"{assetPath}.meta\"", Debug.Log)
                .Then(AssetDatabase.Refresh)
                .WithProgress("Git", "Checking out LFS object...")
                .StartEditorCoroutine();
        }

        /// <summary>Discard target asset changes.</summary>
        public static void GitDiscardChanges(string assetPath)
        {
            bool ok = EditorUtility.DisplayDialog(
                "Git",
                message: $"Really discard all changes to file?\n{assetPath}",
                ok: "Discard changes",
                cancel: "Cancel"
            );

            if (!ok)
                return;

            GitLocksWindow.FreezeWindowUntilNextRefresh();

            ExecuteGitCommandCoroutine($"checkout -- \"{assetPath}\" \"{assetPath}.meta\"", Debug.Log)
                .Then(GitLocksWindow.RefreshCurrentGitLocks)
                .Then(AssetDatabase.Refresh)
                .WithProgress("Git", $"Discarding changes to {Path.GetFileName(assetPath)}...")
                .StartEditorCoroutine();
        }

        /// <summary>Stash target asset changes.</summary>
        public static void GitStashChanges(string assetPath)
        {
            bool ok = EditorUtility.DisplayDialog(
                "Git",
                message: $"Really stash changes to file?\n{assetPath}",
                ok: "Stash changes",
                cancel: "Cancel"
            );

            if (!ok)
                return;

            GitLocksWindow.FreezeWindowUntilNextRefresh();

            ExecuteGitCommandCoroutine($"stash push --include-untracked -m \"{Path.GetFileName(assetPath)}\" {assetPath}", Debug.Log)
                .Then(GitLocksWindow.RefreshCurrentGitLocks)
                .Then(AssetDatabase.Refresh)
                .WithProgress("Git", $"Stashing changes to {Path.GetFileName(assetPath)}...")
                .StartEditorCoroutine();
        }



        /// <summary>Fetch repository and fix git login.</summary>
        [MenuItem("Tools/Git/Fix Git login")]
        public static void FixGitLogin()
            => new Process
            {
                StartInfo =
                {
                    FileName = GitProcessName,
                    Arguments = $"fetch",
                    WorkingDirectory = Environment.CurrentDirectory,
                    UseShellExecute = true,
                    CreateNoWindow = false,
                },
            }.TryStartOrLogError();

        /// <summary>Print console log with you git username.</summary>
        [MenuItem("Tools/Git/Show Git username")]
        public static void ShowGitUsername()
            => ExecuteGitCommandCoroutine("config --get user.name", x => Debug.Log($"Your git username is: <i><b>{x.Trim()}</b></i>"))
                .StartEditorCoroutine();


        [MenuItem("Tools/Git/Lock Suggestions Popup",priority = 2)]
        static void EnableLockSuggestions()
        {
            var enable = EditorPrefs.GetBool("EnableLockSuggestions", true);
            EditorPrefs.SetBool("EnableLockSuggestions", !enable);
            Menu.SetChecked("Tools/Git/Lock Suggestions Popup", enable);
        }

        [MenuItem("Tools/Git/Lock Suggestions Popup", true)]
        static bool EnableLockSuggestionsValidate()
        {
            Menu.SetChecked("Tools/Git/Lock Suggestions Popup", EditorPrefs.GetBool("EnableLockSuggestions", true));
            return true;
        }

        /// <summary>Execute command on git process and get back results.</summary>
        public static IEnumerator ExecuteGitCommandCoroutine(string command, Action<string> stdoutCallback = null, Action<string> stderrCallback = null, bool alwaysRunStdoutCallback = false)
        {
            gitProcess = new Process()
            {
                StartInfo =
                {
                    FileName = GitProcessName,
                    Arguments = command,
                    WorkingDirectory = Environment.CurrentDirectory,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                },
            };

            gitProcess.TryStartOrLogError();

            if (gitProcess.Handle == default)
                yield break;

            while (gitProcess is { HasExited: false })
                yield return null;

            string stdout = gitProcess?.StandardOutput?.ReadToEnd();
            string stderr = gitProcess?.StandardError?.ReadToEnd();

            if (!string.IsNullOrWhiteSpace(stderr))
            {
                if (stderrCallback != null)
                    stderrCallback(stderr);
                else
                    Debug.LogError($"{stderr}\nCommand: '{command}'");

                if (stderr.StartsWith("Logon failed,"))
                    FixGitLogin();

                if (stderr.StartsWith("Error: unknown flag: -- "))
                    Debug.LogError("Your Git version is outdated. Please install Git from https://git-scm.com");
            }

            if (!string.IsNullOrWhiteSpace(stdout) || alwaysRunStdoutCallback)
                stdoutCallback?.Invoke(stdout);
        }



        internal static void VerifyModifiedAssetsAsync(string[] assetPaths)
        {            
            if (Progress.EnumerateItems().Any(x => string.Equals(x.name, "Git", StringComparison.Ordinal)))
                return;

            ReadGitLocksCoroutine(cached: false)
                .Then(VerifyLocksAndWarnOfIssues)
                .WithProgress("Git", "Verifying asset locks...")
                .StartEditorCoroutine();

            void VerifyLocksAndWarnOfIssues()
            {
                var collisions = new List<(string path, string owner)>();
                var lockSuggestions = new List<(string path, string owner)>();

                foreach (var path in assetPaths)
                {
                    var lockData = GetLockDataForAsset(path);

                    if (lockData != null && !lockData.Value.isLocalOwner)
                        collisions.Add((path, lockData.Value.owner.name));
                    else if (lockData == null && !SessionState.GetBool($"LockSuggestion{path}", false))
                        lockSuggestions.Add((path, string.Empty));
                }

                if (collisions.Count > 0)
                    GitLocksWarningPopup.OpenCollisionsWindow(collisions.ToList());

                if (lockSuggestions.Count > 0 && EditorPrefs.GetBool("EnableLockSuggestions", true))
                    GitLocksWarningPopup.OpenLockSuggestionWindow(lockSuggestions.ToList());
            }

            
        }
        internal static IEnumerator ReadGitLocksCoroutine(bool cached, Action<GitLocksDataItem[]> callback = null, bool tryFixIssues = true)
        {
            if (!gitProcess?.HasExited ?? false)
                yield break;

            using var process = gitProcess = new Process
            {
                StartInfo =
                {
                    FileName = GitProcessName,
                    Arguments = $"lfs locks --json --verify {(cached ? "--cached" : "")}",
                    WorkingDirectory = Environment.CurrentDirectory,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                },
            };

            process.TryStartOrLogError();

            if (process.Handle == default)
                yield break;

            while (!process.HasExited)
                yield return null;

            string stdout = process.StandardOutput.ReadToEnd();
            string stderr = process.StandardError.ReadToEnd();

            gitProcess = null;

            if (!string.IsNullOrWhiteSpace(stderr) && !stderr.StartsWith("info: detecting host provider for"))
            {
                if (stderr.StartsWith("Unable to create lock system", StringComparison.Ordinal) && tryFixIssues)
                {
                    // fix git cache error
                    DeleteLocksCache();
                    
                    // repeat command
                    yield return ReadGitLocksCoroutine(cached, callback, tryFixIssues: false);
                    yield break;
                }

                if (stderr.StartsWith("Logon failed,", StringComparison.Ordinal) && tryFixIssues)
                {
                    FixGitLogin();

                    // repeat command
                    yield return ReadGitLocksCoroutine(cached, callback, tryFixIssues: false);
                    yield break;
                }

                if (stderr.StartsWith("Error: unknown flag: -- ", StringComparison.Ordinal))
                    Debug.LogError("Your Git version is outdated. Please install Git from https://git-scm.com");

                Debug.LogError(stderr);
                yield break;
            }

            var locksData = JsonConvert.DeserializeObject<GitLocksData>(stdout);

            UpdateLockedAssetsCache(locksData.All);
            callback?.Invoke(locksData.All);
        }
        internal static void GitUpdateAssetAuthor(string assetPath, int assetInstanceId)
        {
            char[] splitCharsNewline = { '\n' };
            char[] splitCharsSpace = { ' ', '\t' };

            ExecuteGitCommandCoroutine(
                    command: $"shortlog HEAD -n -s --since=\"08 Aug 2020\" -- \"{assetPath}\"",
                    stdout =>
                    {
                        if (string.IsNullOrWhiteSpace(stdout))
                        {
                            GitAuthors.AddAuthor(assetInstanceId, "Authors: <i><b>(unknown)</b></i>");
                            return;
                        }

                        var authors = stdout
                            .Split(splitCharsNewline)
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .Select(line => string.Join(" ", line.Split(splitCharsSpace, StringSplitOptions.RemoveEmptyEntries).Skip(1)));

                        var authorLine = $"Authors: <i><b>{string.Join(", ", authors)}</b></i>";

                        GitAuthors.AddAuthor(assetInstanceId, authorLine);
                    },
                    stderr => GitAuthors.AddAuthor(assetInstanceId, "Authors: <i><b>(unknown)</b></i>"),
                    alwaysRunStdoutCallback: true)
                .StartEditorCoroutine();
        }

        [MenuItem("Assets/Git/Lock file", isValidateFunction: false, priority: 40)]
        static void GitLockSelection() => GitLockAsset(AssetDatabase.GetAssetPath(Selection.activeObject));
        
        [MenuItem("Assets/Git/Unlock file", isValidateFunction: false, priority: 40)]
        static void GitUnlockSelection() => GitUnlockAsset(AssetDatabase.GetAssetPath(Selection.activeObject));
        
        [MenuItem("Assets/Git/Show file history", isValidateFunction: false, priority: 40)]
        static void OpenFileHistory() => OpenFileHistory(AssetDatabase.GetAssetPath(Selection.activeObject));
        
        [MenuItem("Assets/Git/LFS Checkout", isValidateFunction: false, priority: 40)]
        static void GitLfsCheckoutSelection() => GitLfsCheckout(AssetDatabase.GetAssetPath(Selection.activeObject));

        [MenuItem("Assets/Git/Discard changes", isValidateFunction: false, priority: 40)]
        static void GitDiscardChanges() => GitDiscardChanges(AssetDatabase.GetAssetPath(Selection.activeObject));

        [MenuItem("Assets/Git/Stash changes", isValidateFunction: false, priority: 40)]
        static void GitStashChanges() => GitStashChanges(AssetDatabase.GetAssetPath(Selection.activeObject));

        [MenuItem("Assets/Git/Show file history", isValidateFunction: true, priority: 40)]
        [MenuItem("Assets/Git/LFS Checkout", isValidateFunction: true, priority: 40)]
        [MenuItem("Assets/Git/Discard changes", isValidateFunction: true, priority: 40)]
        [MenuItem("Assets/Git/Stash changes", isValidateFunction: true, priority: 40)]
        static bool ValidateSingleAssetSelected()
            => Selection.objects.Length == 1 && Selection.activeObject.GetType() != typeof(DefaultAsset);

        [MenuItem("Assets/Git/Lock file", isValidateFunction: true, priority: 40)]
        static bool ValidateIsNotLocked()
            => ValidateSingleAssetSelected()
               && GetLockDataForAsset(Selection.activeObject.GetHashCode()) is var lockData
               && lockData == null;

        [MenuItem("Assets/Git/Unlock file", isValidateFunction: true, priority: 40)]
        static bool ValidateIsLockedByLocalUser()
            => ValidateSingleAssetSelected()
               && GetLockDataForAsset(Selection.activeObject.GetHashCode()) is var lockData
               && lockData.HasValue
               && lockData.Value.isLocalOwner;

        static void DeleteLocksCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), ".git\\lfs\\lockcache.db");

            if (File.Exists(path))
                File.Delete(path);
        }

        static void UpdateLockedAssetsCache(GitLocksDataItem[] locks)
        {
            lockedAssetsCacheById.Clear();
            lockedAssetsCacheByPath.Clear();

            foreach (var lockData in locks)
            {
                if (string.IsNullOrWhiteSpace(lockData.FileName))
                    continue;

                var asset = AssetDatabase.LoadAssetAtPath(lockData.path, typeof(Object));
                var hashCode = asset?.GetHashCode() ?? lockData.GetHashCode();

                lockedAssetsCacheById[hashCode] = lockData;
                lockedAssetsCacheByPath[lockData.path] = lockData;
            }
        }

        static void TryStartOrLogError(this Process process)
        {
            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                Debug.LogError("<b>Unable to find git</b>! Is it correctly installed on your system?\nInstall it from git-scm.com\n\n" + ex);
            }
        }

        static void ClearReadOnly(string assetPath)
        { 
            if (File.Exists(assetPath))
                File.SetAttributes(assetPath, File.GetAttributes(assetPath) & ~FileAttributes.ReadOnly);
        }
    }
}
#endif