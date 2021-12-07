// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ICollectiblesTabAPI
{
	bool SkipAnimation
	{
		get;
	}

	void SetState(CollectiblesTabState state, bool skipAnimation = false);

	CollectiblesTabState GetState();
}
