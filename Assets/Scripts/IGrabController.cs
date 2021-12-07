// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IGrabController
{
	bool IsReadyToRelease
	{
		get;
	}

	bool IsGrabbing
	{
		get;
	}

	PlayerNum GrabbedOpponent
	{
		get;
	}

	GrabData GrabData
	{
		get;
	}

	bool IsGrabbed
	{
		get;
	}

	PlayerNum GrabbingOpponent
	{
		get;
	}

	void ReleaseGrabbedOpponent(bool escaped);

	void ReleaseFromGrab(bool escaped);

	void OnGrabOpponent(PlayerController opponent, MoveData moveData, HitData hitData);

	void OnGrabbedBy(PlayerController opponent, GrabType grabType);

	void TickStandardGrabbed();

	bool IsThrowMove(MoveData throwMove);

	void OnBeginThrow(MoveData throwMove);
}
