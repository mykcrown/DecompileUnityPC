// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace AI
{
	public class Decorator : INode
	{
		public List<INode> children
		{
			get;
			set;
		}

		public int shuffleWeight
		{
			get;
			set;
		}

		public void Init(BehaviorTree context)
		{
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
			return NodeResult.Failure;
		}
	}
}
