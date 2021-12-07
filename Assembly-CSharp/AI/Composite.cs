using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	// Token: 0x02000326 RID: 806
	public class Composite : INode
	{
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001141 RID: 4417 RVA: 0x000648FD File Offset: 0x00062CFD
		// (set) Token: 0x06001142 RID: 4418 RVA: 0x00064905 File Offset: 0x00062D05
		public int shuffleWeight { get; set; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06001143 RID: 4419 RVA: 0x0006490E File Offset: 0x00062D0E
		// (set) Token: 0x06001144 RID: 4420 RVA: 0x00064916 File Offset: 0x00062D16
		public List<INode> children { get; set; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x0006491F File Offset: 0x00062D1F
		protected PlayerReference playerRef
		{
			get
			{
				return this.context.playerRef;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x0006492C File Offset: 0x00062D2C
		protected PlayerController player
		{
			get
			{
				return this.playerRef.Controller;
			}
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x0006493C File Offset: 0x00062D3C
		public void Init(BehaviorTree context)
		{
			this.context = context;
			if (this.children != null)
			{
				foreach (INode node in this.children)
				{
					node.Init(context);
				}
			}
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x000649AC File Offset: 0x00062DAC
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

		// Token: 0x06001149 RID: 4425 RVA: 0x00064BF2 File Offset: 0x00062FF2
		private void debugResult(NodeResult result, INode node)
		{
			if (result != NodeResult.Running || node is Leaf)
			{
			}
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x00064C08 File Offset: 0x00063008
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

		// Token: 0x0600114B RID: 4427 RVA: 0x00064C58 File Offset: 0x00063058
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

		// Token: 0x04000AF3 RID: 2803
		public Composite.Method method;

		// Token: 0x04000AF4 RID: 2804
		public bool shuffle;

		// Token: 0x04000AF5 RID: 2805
		protected int currentIndex = -1;

		// Token: 0x04000AF6 RID: 2806
		protected BehaviorTree context;

		// Token: 0x02000327 RID: 807
		public enum Method
		{
			// Token: 0x04000AF8 RID: 2808
			Sequence,
			// Token: 0x04000AF9 RID: 2809
			SelectOne,
			// Token: 0x04000AFA RID: 2810
			FirstOnly,
			// Token: 0x04000AFB RID: 2811
			DoAll
		}
	}
}
