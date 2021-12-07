// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace AI
{
	public interface INode
	{
		List<INode> children
		{
			get;
			set;
		}

		int shuffleWeight
		{
			get;
			set;
		}

		void Init(BehaviorTree context);

		NodeResult TickFrame();
	}
}
