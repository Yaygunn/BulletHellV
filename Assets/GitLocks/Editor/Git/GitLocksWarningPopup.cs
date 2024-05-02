#if !DISABLE_GIT_LOCKS
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KreliStudio
{
    public sealed class GitLocksWarningPopup : EditorWindow
    {
        public enum ContentType
        {
            collision,
            suggestion,
            shutdown,
        }

        List<(string path, string owner)> locks = new List<(string path, string owner)>();
        Vector2 scrollPosition;
        static GUIStyle scrollViewStyle;
        string messageText;
        ContentType contentType;
        List<string> locksToRemove = new List<string>();
        List<string> assetsToLock = new List<string>();

        public static void OpenCollisionsWindow(List<(string, string)> locks)
            => OpenWindow(locks, "Warning - the following assets that you have modified are locked by another user:", ContentType.collision);
        public static void OpenLockSuggestionWindow(List<(string, string)> paths)
            => OpenWindow(paths, "Warning - the following assets that you have modified are not locked:", ContentType.suggestion);

        public static void OpenShutdownWindow(List<(string, string)> locks)
            => OpenWindow(locks, "Are you going away? Keep in mind you still have an active lock on following assets:", ContentType.shutdown);

        static void OpenWindow(List<(string, string)> locks, string messageText, ContentType contentType)
        {
            var window = CreateInstance<GitLocksWarningPopup>();

            window.locks = locks;
            window.position = new Rect(
                x: Screen.width / 2f,
                y: Screen.height / 2f,
                width: 320,
                height: 280
            );
            window.maxSize = window.minSize = window.position.size;
            window.titleContent.text = "Git Locks";
            window.messageText = messageText;
            window.contentType = contentType;
            window.ShowModalUtility();
        }

        void OnGUI()
        {
            if (locks == null)
            {
                Close();
                return;
            }

            EditorGUILayout.Space(12);

            var message = EditorGUIUtility.IconContent(contentType == ContentType.collision ? "P4_LockedLocal@2x" : "P4_LockedRemote@2x");
            message.text = messageText;

            EditorGUILayout.LabelField(message, EditorStyles.wordWrappedLabel);

            EditorGUILayout.Space(8);

            if (contentType == ContentType.shutdown)
                ShutdownContent();

            scrollViewStyle = scrollViewStyle ?? new GUIStyle(EditorStyles.inspectorDefaultMargins) { padding = new RectOffset(12, 12, 0, 0) };

            scrollPosition = EditorGUILayout.BeginScrollView(
                scrollPosition: scrollPosition,
                style: scrollViewStyle,
                GUILayout.Width(320),
                GUILayout.Height(210)
            );

            foreach (var (path, owner) in locks)
            {
                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                var guiContent = new GUIContent(GitGUIStyle.GetAssetGUIContent(path, true, true));

                if (contentType == ContentType.collision)
                    guiContent.text = $"{guiContent.text} [{owner}]";

                if (GUILayout.Button(guiContent, EditorStyles.objectField, GUILayout.Height(18), GUILayout.Width(290)))
                {
                    Selection.activeObject = asset; 
                    EditorGUIUtility.PingObject(asset);
                }

                if (contentType == ContentType.suggestion)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button(GitGUIStyle.lockModifiedAssetLabel, EditorStyles.miniButtonLeft))
                    {
                        assetsToLock.Add(path);
                        locksToRemove.Add(path);
                    }
                    if (GUILayout.Button("Skip", EditorStyles.miniButtonMid))
                    {
                        locksToRemove.Add(path);
                    }
                    if (GUILayout.Button("Ignore", EditorStyles.miniButtonRight))
                    {
                        SessionState.SetBool($"LockSuggestion{path}", true);
                        locksToRemove.Add(path);
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Separator(); 
                }
            }

            RemoveUnusedLocks();

            EditorGUILayout.EndScrollView();

            if (locks == null || locks.Count == 0)
                Close();
        }

        void OnDestroy()
        {
            if (assetsToLock != null && assetsToLock.Count > 0)
                GitCommands.GitLockAllAssets(assetsToLock);

            assetsToLock?.Clear();
        }

        void RemoveUnusedLocks()
        {
            if (locksToRemove != null && locksToRemove.Count > 0 && locks != null && locks.Count > 0)
                locks.RemoveAll(x => locksToRemove.Contains(x.path));
            
            locksToRemove?.Clear();
        }

        void ShutdownContent()
        {
            GitApplicationShutdownWarning.ShouldContinueQuitting = false; // window "x" button cancels editor shutdown

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Shutdown"))
            {
                GitApplicationShutdownWarning.ShouldContinueQuitting = true;
                Close();
            }

            using (new EditorGUI.DisabledScope(disabled: locks.Count == 0))
            {
                if (GUILayout.Button("Unlock All"))
                {
                    GitCommands.GitUnlockAllAssets(locks.Select(x => x.path), true); // force to ensure dirty files are unlocked too
                    GitLocksWindow.RefreshCurrentGitLocks();
                    locks = new List<(string, string)>();
                }
            }

            if (GUILayout.Button("Cancel"))
                Close();

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(8);
        }
    }
}
#endif