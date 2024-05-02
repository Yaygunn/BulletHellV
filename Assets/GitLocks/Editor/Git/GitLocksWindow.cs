#if !DISABLE_GIT_LOCKS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using KreliStudio.TreeView;


namespace KreliStudio
{
    public class GitLocksWindow : EditorWindow
    {
        AssetTree assetTreeData;
        GitLocksTreeGUI assetTreeGUI;
        Vector2 scrollPosition;
        string searchText = string.Empty;
        bool filterByOwner;
        static bool windowFrozenUntilNextRefresh;
        static GitLocksWindow instance;


        [Shortcut("Open Git Locks Window", KeyCode.L, ShortcutModifiers.Action | ShortcutModifiers.Shift)]
        [MenuItem("Tools/Git/Git Locks Window", false, 1)]
        public static void OpenWindow()
        {
            instance = GetWindow<GitLocksWindow>(
                        title: "Git Locks",
                        focus: true,
                        desiredDockNextTo: TypeCache.GetTypesDerivedFrom<EditorWindow>().Single(x => x.Name == "SceneHierarchyWindow")
                        );
                    
            instance.Show();
        }

        public static void FreezeWindowUntilNextRefresh()
            => windowFrozenUntilNextRefresh = true;

        public static void RefreshCurrentGitLocks()
        {
            if (instance == null)
                GitCommands.ReadGitLocksCoroutine(cached: false).StartEditorCoroutine();
            else
                instance.RefreshCurrentGitLocksWithCallback(
                    callback: () => windowFrozenUntilNextRefresh = false
                );
        }



        void OnEnable()
        {
            titleContent = new GUIContent("Git Locks", EditorGUIUtility.IconContent("d_InspectorLock").image);
            assetTreeData = new AssetTree();
            assetTreeGUI = new GitLocksTreeGUI(assetTreeData.Root);
            RefreshWindow();

            minSize = new Vector2(300.0f, 300.0f);

            instance = this;
        }

        void OnGUI()
        {
            DrawToolbar();

            GUI.enabled = !windowFrozenUntilNextRefresh;

            DrawSearch();            

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            {
                if (windowFrozenUntilNextRefresh && assetTreeData.Root.Count == 0)
                    EditorGUILayout.LabelField("Loading locks...");
                else
                    assetTreeGUI.DrawTreeLayout();
            }
            EditorGUILayout.EndScrollView();

            DrawInfo();

            GUI.enabled = true;
        }

        void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                if (GUILayout.Button(GitGUIStyle.settingsLabel, EditorStyles.toolbarButton, GUILayout.Width(25)))
                {
                    PopupWindow.Show(new Rect(0,0,1,25),new GitRepoLinkPopup());
                }

                EditorGUI.BeginChangeCheck();
                {
                    filterByOwner = DrawToggle(GitGUIStyle.filterByOwnerLabel, filterByOwner, GUILayout.Width(25));
                }
                if (EditorGUI.EndChangeCheck())
                    RefreshWindow();

                assetTreeGUI.FlatHierarchy = DrawToggle(GitGUIStyle.flatHierarchyLabel, assetTreeGUI.FlatHierarchy, GUILayout.Width(25));

                if (position.width > 150.0f)
                    using (new EditorGUI.DisabledScope(disabled: assetTreeGUI.FlatHierarchy))
                    {
                        if (GUILayout.Button(GitGUIStyle.expandLabel, EditorStyles.toolbarButton, GUILayout.Width(25)))
                            assetTreeGUI.ExpandAll();

                        if (GUILayout.Button(GitGUIStyle.collapseLabel, EditorStyles.toolbarButton, GUILayout.Width(25)))
                            assetTreeGUI.CollapseAll();
                    }

                GUILayout.FlexibleSpace();

                if (GUILayout.Button(GitGUIStyle.refreshLabel, EditorStyles.toolbarButton, GUILayout.Width(25)))
                {
                    FreezeWindowUntilNextRefresh();
                    RefreshCurrentGitLocks();
                }
            }
            EditorGUILayout.EndHorizontal();


            bool DrawToggle(GUIContent content, bool value, params GUILayoutOption[] options)
            {
                var defaultColor = GUI.contentColor;

                if (value)
                    GUI.contentColor = Color.green;

                value = GUILayout.Toggle(value, content, EditorStyles.toolbarButton, options);

                GUI.contentColor = defaultColor;

                return value;
            }
        }

        void DrawInfo()
        {
            var selected = assetTreeGUI.Selected;

            if (selected == null)
                return;

            if (selected.Data == null)
                return;

            if (string.IsNullOrWhiteSpace(selected.Data.fullPath))
                return;

            var lockData = GitCommands.GetLockDataForAsset(selected.Data.fullPath);

            if (lockData == null)
                return;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                DrawLabelWithToolTip(GitGUIStyle.ownerLabel, lockData.Value.owner.name);
                DrawLabelWithToolTip(GitGUIStyle.dateLabel, $"{lockData.Value.lockedAt:M}, {lockData.Value.lockedAt:t}");
                DrawLabelWithToolTip(GitGUIStyle.pathLabel, lockData.Value.path);

                GitGUIStyle.referenceLabel.text = position.width > 200f ? "Asset" : string.Empty;
                EditorGUILayout.ObjectField(GitGUIStyle.referenceLabel, lockData.Value.Object, lockData.Value.Object?.GetType() ?? typeof(UnityEngine.Object), false);
            }
            EditorGUILayout.EndVertical();

            void DrawLabelWithToolTip(GUIContent label, string text)
            {
                label.tooltip = $"{text}\r";
                EditorGUILayout.LabelField(label, new GUIContent(text, $"{text}\r"));
            }
        }
        void DrawSearch()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                EditorGUI.BeginChangeCheck();
                searchText = EditorGUILayout.DelayedTextField(searchText, EditorStyles.toolbarSearchField, GUILayout.ExpandWidth(true));

                if (!string.IsNullOrWhiteSpace(searchText) && GUILayout.Button(GitGUIStyle.closeLabel, EditorStyles.boldLabel, GUILayout.Width(20)))
                    searchText = string.Empty;

                if (EditorGUI.EndChangeCheck())
                    RefreshWindow();
            }
            EditorGUILayout.EndHorizontal();
        }


        void RefreshWindow()
        {
            assetTreeData.Clear();

            var searchProjectAsset = string.IsNullOrWhiteSpace(searchText) ? 
                new string[0] :
                AssetDatabase.FindAssets(searchText)
                .Select(x => AssetDatabase.GUIDToAssetPath(x));

            var searchLockedAssetNames = GitCommands.lockedAssetsCacheByPath
                .Where(x => x.Value.FileName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(x => x.Key);

            var paths = GitCommands.lockedAssetsCacheByPath
                .Where(x => !filterByOwner || x.Value.isLocalOwner)  
                .Select(x => x.Key)
                .Where(x => (searchProjectAsset.Contains(x) || searchLockedAssetNames.Contains(x)))
                .ToList();

            foreach (var item in paths)
                assetTreeData.AddAsset(item);
            

            assetTreeGUI.ExpandAll();
            Repaint();
        }


        void RefreshCurrentGitLocksWithCallback(Action callback)
            => RefreshMenuTreeCoroutine()
                .Then(callback)
                .WithProgress("Git", "Refreshing locks...")
                .StartEditorCoroutine();

        IEnumerator RefreshMenuTreeCoroutine()
        {
             IEnumerator Refresh(bool cached)
                 => GitCommands.ReadGitLocksCoroutine(
                     cached: cached,
                     callback: locksData =>
                     {
                         RefreshWindow();
                     }
                 );

             GitLocksAutoRefresh.ResetCooldown();
             yield return Refresh(cached: false);
        }
    }
}
#endif