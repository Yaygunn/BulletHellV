#if !DISABLE_GIT_LOCKS
using UnityEngine;
using UnityEditor;
using KreliStudio.TreeView;

namespace KreliStudio
{
	public class GitLocksTreeGUI : TreeGUI<AssetData>
	{
		public GitLocksTreeGUI(TreeNode<AssetData> root) : base(root) { }

		protected override void OnDrawTreeNode(Rect rect, TreeNode<AssetData> node, bool selected, bool focus)
		{
			if (!node.IsLeaf)
				node.Data.IsExpanded = EditorGUI.Foldout(new Rect(rect.x - 12, rect.y, 12, rect.height), node.Data.IsExpanded, GUIContent.none);
			
			EditorGUI.LabelField(rect,GitGUIStyle.GetAssetGUIContent(node.Data.fullPath, true, true), selected ? EditorStyles.whiteLabel : EditorStyles.label);
		}


        protected override void AddItemsToMenu(GenericMenu menu, TreeNode<AssetData> node)
        {
			if (node == null)
				return;

			if (!node.IsLeaf)
				return;

			if (node.Data == null)
				return;

			if (string.IsNullOrWhiteSpace(node.Data.fullPath))
				return;

			var lockData = GitCommands.GetLockDataForAsset(node.Data.fullPath);
			if (lockData == null)
				return;

			var asset = AssetDatabase.LoadAssetAtPath<Object>(node.Data.fullPath);

			menu.AddDisabledItem(new GUIContent($"Locked By {lockData.Value.owner.name}"));

			menu.AddSeparator(string.Empty);

			menu.AddItem(GitGUIStyle.selectLabel, false,
				() =>
				{
					Selection.activeObject = asset;
					EditorGUIUtility.PingObject(asset);
				});


			if (lockData.Value.isLocalOwner)
				menu.AddItem(GitGUIStyle.unlockLabel, false, () => GitCommands.GitUnlockAsset(node.Data.fullPath));
			else
				menu.AddDisabledItem(GitGUIStyle.unlockLabel);

			menu.AddItem(GitGUIStyle.forceUnlockLabel, false, () => GitCommands.GitUnlockAsset(node.Data.fullPath, true));

			menu.AddSeparator(string.Empty);

			menu.AddItem(GitGUIStyle.historyLabel, false, () => GitCommands.OpenFileHistory(node.Data.fullPath));

			menu.AddItem(GitGUIStyle.lfsCheckout, false, () => GitCommands.GitLfsCheckout(node.Data.fullPath));

			menu.AddItem(GitGUIStyle.discardLabel, false, () => GitCommands.GitDiscardChanges(node.Data.fullPath));

			menu.AddItem(GitGUIStyle.stashLabel, false, () => GitCommands.GitStashChanges(node.Data.fullPath));
		}
    }
}
#endif