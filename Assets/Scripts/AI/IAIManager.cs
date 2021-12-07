// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace AI
{
	public interface IAIManager
	{
		bool IsAnyAIActive();

		bool IsAnyPassiveAIActive();

		CompositeNodeData GetPassiveRootNode();

		CompositeNodeData GetDefaultRootNode();
	}
}
