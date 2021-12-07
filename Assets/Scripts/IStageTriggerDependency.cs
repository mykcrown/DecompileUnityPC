// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IStageTriggerDependency : IFrameOwner
{
	GameModeData ModeData
	{
		get;
	}

	List<PlayerController> GetPlayers();
}
