#if !DISABLE_GIT_LOCKS
using UnityEditor;
using UnityEngine;

namespace KreliStudio
{
    public static class GitGUIStyle
    {
        // Icons & Tooltips
        public static readonly GUIContent settingsLabel = new GUIContent(EditorGUIUtility.IconContent("Settings").image, "Settings");
        public static readonly GUIContent expandLabel = new GUIContent(EditorGUIUtility.IconContent("d_animationvisibilitytoggleon").image, "Expand");
        public static readonly GUIContent collapseLabel = new GUIContent(EditorGUIUtility.IconContent("d_animationvisibilitytoggleoff").image, "Collapse");
        public static readonly GUIContent refreshLabel = new GUIContent(EditorGUIUtility.IconContent("Refresh").image, "Refresh");
        public static readonly GUIContent lockLabelRed = EditorGUIUtility.IconContent("P4_LockedRemote");
        public static readonly GUIContent lockLabelBlue = EditorGUIUtility.IconContent("P4_LockedLocal");
        public static readonly GUIContent authorLabel = EditorGUIUtility.IconContent("FilterByLabel");
        public static readonly GUIContent filterByOwnerLabel = new GUIContent(EditorGUIUtility.IconContent("FilterByLabel").image, "Filter By Owner");
        public static readonly GUIContent closeLabel = EditorGUIUtility.IconContent("winbtn_win_close");
        public static readonly GUIContent flatHierarchyLabel = new GUIContent(EditorGUIUtility.IconContent("d_UnityEditor.HierarchyWindow").image, "Flat Hierarchy");
        public static readonly GUIContent notificationLockedLocal = EditorGUIUtility.IconContent("P4_LockedRemote@2x");
        public static readonly GUIContent notificationLockedRemote = EditorGUIUtility.IconContent("P4_LockedLocal@2x");
        public static readonly GUIContent lockModifiedAssetLabel = new GUIContent(" Lock",EditorGUIUtility.IconContent("P4_LockedRemote").image);

        // Labels
        public static readonly GUIContent selectLabel = new GUIContent("Select");
        public static readonly GUIContent unlockLabel = new GUIContent("Unlock");
        public static readonly GUIContent forceUnlockLabel = new GUIContent("Force Unlock");
        public static readonly GUIContent historyLabel = new GUIContent("Show File History");
        public static readonly GUIContent lfsCheckout = new GUIContent("LFS Checkout");
        public static readonly GUIContent discardLabel = new GUIContent("Discard Changes");
        public static readonly GUIContent stashLabel = new GUIContent("Stash Changes");
        public static readonly GUIContent ownerLabel = new GUIContent("Locked by");
        public static readonly GUIContent dateLabel = new GUIContent("Locked since");
        public static readonly GUIContent pathLabel = new GUIContent("Path");
        public static readonly GUIContent referenceLabel = new GUIContent("Asset");

        // GUI Style
        public static GUIStyle richTextStyle = new GUIStyle(EditorStyles.label) { richText = true, fontStyle = FontStyle.Italic };

        /// <summary>
        /// Create GUI Content with asset icon and name. If asset is not validated add 'missing' text and change icon to 'bug'.
        /// </summary>
        public static GUIContent GetAssetGUIContent(string path, bool label = true, bool missing = false)
        {
            GUIContent content = new GUIContent();

            if (label)
                content.text = System.IO.Path.GetFileName(path);

            var image = AssetDatabase.GetCachedIcon(path);
            if (image != null)
            {
                content.image = image;

            }
            else if (!System.IO.Path.HasExtension(path))
            {
                content.image = EditorGUIUtility.IconContent("d_FolderEmpty Icon").image;

                if (label && missing)
                    content.text += " (missing)";
            }
            else if (AssetDatabase.LoadAssetAtPath(path, typeof(Object)) != null)
            {
                content.image = EditorGUIUtility.IconContent("d_PreMatCube").image;
            }
            else
            {
                content.image = EditorGUIUtility.IconContent("d_DebuggerEnabled").image;
                if (label && missing)
                    content.text += " (missing)";
            }

            return content;
        }
    }
}
#endif