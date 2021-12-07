// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IExitGame
{
	void InstantTerminate();

	void DestroyGameManager();

	void ExitGameMode(Action callback);
}
