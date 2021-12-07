// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class Composite : INode
	{
		public enum Method
		{
			Sequence,
			SelectOne,
			FirstOnly,
			DoAll
		}

		public Composite.Method method;

		public bool shuffle;

		protected int currentIndex = -1;

		protected BehaviorTree context;

		public int shuffleWeight
		{
			get;
			set;
		}

		public List<INode> children
		{
			get;
			set;
		}

		protected PlayerReference playerRef
		{
			get
			{
				return this.context.playerRef;
			}
		}

		protected PlayerController player
		{
			get
			{
				return this.playerRef.Controller;
			}
		}

		public void Init(BehaviorTree context)
		{
			this.context = context;
			if (this.children != null)
			{
				foreach (INode current in this.children)
				{
					current.Init(context);
				}
			}
		}

		public virtual NodeResult TickFrame()
		{
			if (this.children == null)
			{
				return NodeResult.Failure;
			}
			if (this.currentIndex == -1)
			{
				if (this.shuffle)
				{
					if (this.method == Composite.Method.SelectOne || this.method == Composite.Method.FirstOnly)
					{
						this.weightedShuffle(this.children);
					}
					else
					{
						this.shuffleMethod(this.children);
					}
				}
				this.currentIndex = 0;
			}
			if (this.method == Composite.Method.DoAll)
			{
				for (int i = this.currentIndex; i < this.children.Count; i++)
				{
					NodeResult nodeResult = this.children[i].TickFrame();
					this.debugResult(nodeResult, this.children[i]);
					if (nodeResult == NodeResult.Running)
					{
						this.currentIndex = i;
						return nodeResult;
					}
				}
				this.currentIndex = -1;
				return NodeResult.Success;
			}
			if (this.method == Composite.Method.Sequence)
			{
				for (int j = this.currentIndex; j < this.children.Count; j++)
				{
					NodeResult nodeResult2 = this.children[j].TickFrame();
					this.debugResult(nodeResult2, this.children[j]);
					if (nodeResult2 == NodeResult.Running)
					{
						this.currentIndex = j;
						return nodeResult2;
					}
					if (nodeResult2 == NodeResult.Failure)
					{
						this.currentIndex = -1;
						return nodeResult2;
					}
				}
				this.currentIndex = -1;
				return NodeResult.Success;
			}
			if (this.method == Composite.Method.SelectOne)
			{
				for (int k = this.currentIndex; k < this.children.Count; k++)
				{
					NodeResult nodeResult3 = this.children[k].TickFrame();
					this.debugResult(nodeResult3, this.children[k]);
					if (nodeResult3 == NodeResult.Running)
					{
						this.currentIndex = k;
						return nodeResult3;
					}
					if (nodeResult3 == NodeResult.Success)
					{
						this.currentIndex = -1;
						return nodeResult3;
					}
				}
				this.currentIndex = -1;
				return NodeResult.Failure;
			}
			if (this.method == Composite.Method.FirstOnly)
			{
				for (int l = 0; l < this.children.Count; l++)
				{
					NodeResult nodeResult4 = this.children[l].TickFrame();
					this.debugResult(nodeResult4, this.children[l]);
					if (nodeResult4 == NodeResult.Running)
					{
						return nodeResult4;
					}
					if (nodeResult4 == NodeResult.Success)
					{
						return nodeResult4;
					}
				}
				return NodeResult.Failure;
			}
			return NodeResult.Failure;
		}

		private void debugResult(NodeResult result, INode node)
		{
			if (result != NodeResult.Running || node is Leaf)
			{
			}
		}

		private void shuffleMethod(List<INode> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				INode value = list[i];
				int index = UnityEngine.Random.Range(i, list.Count);
				list[i] = list[index];
				list[index] = value;
			}
		}

		private void weightedShuffle(List<INode> list)
		{
			List<INode> list2 = new List<INode>(list);
			list.Clear();
			int count = list2.Count;
			for (int i = 0; i < count; i++)
			{
				int num = 0;
				for (int j = 0; j < list2.Count; j++)
				{
					num += list2[j].shuffleWeight;
				}
				int num2 = UnityEngine.Random.Range(0, num);
				INode node = null;
				for (int k = 0; k < list2.Count; k++)
				{
					num2 -= list2[k].shuffleWeight;
					if (num2 <= 0)
					{
						node = list2[k];
						break;
					}
				}
				if (node == null)
				{
					node = list2[0];
				}
				list2.Remove(node);
				list.Add(node);
			}
		}
	}
}
