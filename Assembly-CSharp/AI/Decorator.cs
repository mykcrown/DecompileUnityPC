using System;
using System.Collections.Generic;

namespace AI
{
	// Token: 0x02000329 RID: 809
	public class Decorator : INode
	{
		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06001165 RID: 4453 RVA: 0x00064D31 File Offset: 0x00063131
		// (set) Token: 0x06001166 RID: 4454 RVA: 0x00064D39 File Offset: 0x00063139
		public List<INode> children { get; set; }

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06001167 RID: 4455 RVA: 0x00064D42 File Offset: 0x00063142
		// (set) Token: 0x06001168 RID: 4456 RVA: 0x00064D4A File Offset: 0x0006314A
		public int shuffleWeight { get; set; }

		// Token: 0x06001169 RID: 4457 RVA: 0x00064D54 File Offset: 0x00063154
		public void Init(BehaviorTree context)
		{
			if (this.children != null)
			{
				foreach (INode node in this.children)
				{
					node.Init(context);
				}
			}
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00064DBC File Offset: 0x000631BC
		public virtual NodeResult TickFrame()
		{
			return NodeResult.Failure;
		}
	}
}
