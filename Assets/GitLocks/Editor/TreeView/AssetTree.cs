using System;
using System.Collections.Generic;
using UnityEditor;

namespace KreliStudio.TreeView
{
	public class AssetTree
	{
		public TreeNode<AssetData> Root => root;

		TreeNode<AssetData> root = new TreeNode<AssetData>(null);

		public void Clear() => root.Clear();
		
		public void AddAsset(string assetPath)
		{
			if (string.IsNullOrWhiteSpace(assetPath)) 
				return;

			var node = root;
			var guid = AssetDatabase.AssetPathToGUID(assetPath);
			var startIndex = 0;
			var length = assetPath.Length;

			while (startIndex < length)
			{
				var endIndex = assetPath.IndexOf('/', startIndex);
				var subLength = endIndex == -1 ? length - startIndex : endIndex - startIndex;
				var directory = assetPath.Substring(startIndex, subLength);
				var pathNode = new AssetData(endIndex == -1 ? guid : null, directory, assetPath.Substring(0, endIndex == -1 ? length : endIndex), node.Level == 0);

				node = node.FindInChildren(pathNode) ?? node.AddChild(pathNode);
				startIndex += subLength + 1;
			}
		}
	}

	public class AssetData : ITreeGUIData
	{
		public readonly string guid;
		public readonly string path;
		public readonly string fullPath;
		public bool IsExpanded { get; set; }

		public AssetData(string guid, string path, string fullPath, bool isExpanded)
		{
			this.guid = guid;
			this.path = path;
			this.fullPath = fullPath;
			this.IsExpanded = isExpanded;
		}

		public override string ToString() => path;
        public override int GetHashCode() => -1757656154 + EqualityComparer<string>.Default.GetHashCode(path);
		public override bool Equals(object obj) => obj is AssetData node && Equals(node);
		public bool Equals(AssetData node) => node.path == path;
    }
}