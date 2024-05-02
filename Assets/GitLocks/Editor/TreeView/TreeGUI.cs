using UnityEngine;
using UnityEditor;

namespace KreliStudio.TreeView
{
	public class TreeGUI<T> where T : ITreeGUIData
	{
		readonly TreeNode<T> root;

		Rect controlRect;
		float drawY;
		float height;
		int controlID;

		public bool FlatHierarchy { get; set; }
		public TreeNode<T> Selected { get; private set; }

		public TreeGUI(TreeNode<T> root)
		{
			this.root = root;
		}

		public void DrawTreeLayout()
		{
			height = 0;
			drawY = 0;
			root.Traverse(OnGetLayoutHeight);

			controlRect = EditorGUILayout.GetControlRect(false, height);
			controlID = GUIUtility.GetControlID(FocusType.Passive, controlRect);
			root.Traverse(OnDrawRow);
		}
		public void ExpandAll() => root?.Traverse(Expand);
		public void CollapseAll() => root?.Traverse(Collapse);
        

		protected virtual bool Expand(TreeNode<T> node)
		{
			if (node.Data != null) 
				node.Data.IsExpanded = true;

			return true;
		}
		protected virtual bool Collapse(TreeNode<T> node)
		{
			if (node.Data != null) 
				node.Data.IsExpanded = false;

			return true;
		}
		protected virtual float GetRowHeight(TreeNode<T> node) => EditorGUIUtility.singleLineHeight;		
		protected virtual bool OnGetLayoutHeight(TreeNode<T> node)
		{
			if (node.Data == null) 
				return true;

			height += GetRowHeight(node);
			return node.Data.IsExpanded;
		}
		protected virtual bool OnDrawRow(TreeNode<T> node)
		{
			if (node.Data == null) 
				return true;

			if (FlatHierarchy && !node.IsLeaf)
				return true;

			var rowIndent = FlatHierarchy ? 0.0f : 14 * node.Level;
			var rowHeight = GetRowHeight(node);
			var rowRect = new Rect(0, controlRect.y + drawY, controlRect.width, rowHeight);
			var indentRect = new Rect(rowIndent, controlRect.y + drawY, controlRect.width - rowIndent, rowHeight);

			if (Selected == node)
				EditorGUI.DrawRect(rowRect, Color.gray);			

			OnDrawTreeNode(indentRect, node, Selected == node, false);


			EventType eventType = Event.current.GetTypeForControl(controlID);
			if (OnClick())
			{
				Selected = node;

				GUI.changed = true;
				Event.current.Use();
			}
			if (OnRightClick())
			{
				GenericMenu menu = new GenericMenu();
				AddItemsToMenu(menu, node);
				if (menu.GetItemCount() > 0)
					menu.ShowAsContext();
			}

			drawY += rowHeight;
			return node.Data.IsExpanded;



			bool OnRightClick() => OnClick() && Event.current.button == 1;
			bool OnClick() => eventType == EventType.MouseUp && rowRect.Contains(Event.current.mousePosition);
		}
		protected virtual void OnDrawTreeNode(Rect rect, TreeNode<T> node, bool selected, bool focus)
		{
			var labelContent = new GUIContent(node.Data.ToString());

			if (!node.IsLeaf)
				node.Data.IsExpanded = EditorGUI.Foldout(new Rect(rect.x - 12, rect.y, 12, rect.height), node.Data.IsExpanded, GUIContent.none);
			

			EditorGUI.LabelField(rect, labelContent, selected ? EditorStyles.whiteLabel : EditorStyles.label);
		}
		protected virtual void AddItemsToMenu(GenericMenu menu, TreeNode<T> node) { }
	}

	public interface ITreeGUIData
	{
		bool IsExpanded { get; set; }
	}

}