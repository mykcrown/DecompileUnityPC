// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IMoveUseTracker
{
	bool HasMoveUsesLeft(MoveData move);

	void Grounded();

	void OnRemovedFromGame();

	void OnGrabbed();

	void OnReceiveHit();

	void OnGiveHit();

	void OnGrabLedge();

	void OnMoveStart(MoveData move);

	int GetMoveUsedCount(MoveLabel label);
}
