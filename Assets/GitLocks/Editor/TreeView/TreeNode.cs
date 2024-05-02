using System.Collections.Generic;

namespace KreliStudio.TreeView
{
	public class TreeNode<T>
	{
		readonly T data;
		readonly TreeNode<T> parent;
		readonly int level;
		readonly List<TreeNode<T>> children;

		public int Level => level;
		public int Count =>  children.Count;
		public bool IsRoot => parent == null;
		public bool IsLeaf => children.Count == 0;
		public T Data => data;
		public TreeNode<T> Parent => parent;
		public TreeNode<T> this[int key] => children[key];
		

		public TreeNode(T data)
		{
			this.data = data;
			children = new List<TreeNode<T>>();
			level = 0;
		}
		public TreeNode(T data, TreeNode<T> parent) : this(data)
		{
			this.parent = parent;
            level = this.parent != null ? this.parent.Level + 1 : 0;
		}


		public TreeNode<T> AddChild(T value)
		{
			var node = new TreeNode<T>(value, this);
			children.Add(node);

			return node;
		}
		public bool RemoveChild(TreeNode<T> node) => children.Remove(node);
		public void Clear() => children.Clear();		
		public bool HasChild(T data) => FindInChildren(data) != null;		
		public TreeNode<T> FindInChildren(T data)
		{
			for (int i = 0; i < Count; ++i)
			{ 
				var child = children[i];
				if (child.Data.Equals(data))
					return child;
			}

			return null;
		}
		public void Traverse(System.Func<T, bool> handler)
		{
			if (handler != null)
				if (handler.Invoke(data))
					for (int i = 0; i < Count; ++i)
						children[i].Traverse(handler);
		}
		public void Traverse(System.Func<TreeNode<T>, bool> handler)
		{
			if (handler != null)
				if (handler.Invoke(this))
					for (int i = 0; i < Count; ++i)
						children[i].Traverse(handler);
		}
	}
}