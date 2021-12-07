// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ITokenManager
{
	void Init(Func<PlayerNum, IPlayerCursor> findPlayerCursor);

	void Reset();

	void ReleaseFunctions();

	PlayerToken GetCurrentlyGrabbing(PlayerNum playerNum);

	void ReleaseAnyGrabbers(PlayerToken token);

	void ReleaseToken(PlayerNum player);

	void GrabToken(PlayerNum player, PlayerToken token, float clickTime = 0f);

	PlayerNum IsBeingGrabbedByPlayer(PlayerToken token);

	PlayerToken GetPlayerToken(PlayerNum playerNum);

	PlayerToken[] GetAll();

	void AddToken(PlayerNum owner, PlayerToken token);

	void RemoveToken(PlayerNum owner, PlayerToken token);
}
