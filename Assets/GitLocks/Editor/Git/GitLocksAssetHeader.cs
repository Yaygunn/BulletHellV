#if !DISABLE_GIT_LOCKS
using System;
using UnityEditor;
using UnityEngine;

namespace KreliStudio
{
    [InitializeOnLoad]
    public static class GitLocksAssetHeader
    {
        static GitLocksAssetHeader() => Editor.finishedDefaultHeaderGUI += OnHeaderGUI;

        static void OnHeaderGUI(Editor editor)
        {
            if (editor.targets.Length > 1)
                return;

            var asset = editor.target;

            if (asset is DefaultAsset)
                return;

            if (asset is GameObject go)
                if (go.scene.name != null)
                    return;

            int instanceId = asset.GetHashCode();

            var lockData = GitCommands.GetLockDataForAsset(instanceId);
            var authors = GitAuthors.GetAuthor(asset, instanceId);

            if (!string.IsNullOrEmpty(authors))
            {
                var controlRect = EditorGUILayout.GetControlRect(
                    hasLabel: false,
                    height: 14,
                    options: Array.Empty<GUILayoutOption>()
                );

                controlRect.x += 8;
                controlRect.height += 4;

                GitGUIStyle.authorLabel.text = authors;
                EditorGUI.LabelField(controlRect, GitGUIStyle.authorLabel, GitGUIStyle.richTextStyle);

                if (OnLeftClick(controlRect))
                    GitCommands.OpenFileHistory(AssetDatabase.GetAssetPath(asset));
            }

            if (lockData != null)
            {
                var controlRect = EditorGUILayout.GetControlRect(
                    hasLabel: false,
                    height: 14,
                    options: Array.Empty<GUILayoutOption>()
                );

                controlRect.x += 8;
                controlRect.height += 4;


                var label = new GUIContent(lockData.Value.isLocalOwner ? GitGUIStyle.lockLabelRed : GitGUIStyle.lockLabelBlue);
                label.text = $"Locked by: <b>{lockData.Value.owner.name}</b>";
                EditorGUI.LabelField(controlRect, label, GitGUIStyle.richTextStyle);

                if (OnLeftClick(controlRect))
                    GitLocksWindow.OpenWindow();
            }
        }


        static bool OnLeftClick(Rect rect)
        {
            var current = Event.current;
            bool flag = current.type == EventType.MouseDown && current.button == 0 && rect.Contains(current.mousePosition);

            if (flag)
                current.Use();

            return flag;
        }
    }
}
#endif