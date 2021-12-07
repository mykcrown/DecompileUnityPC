// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ICharactersTabAPI
{
	bool SkipAnimation
	{
		get;
	}

	void SetState(CharactersTabState state, bool skipAnimation = false);

	CharactersTabState GetState();
}
