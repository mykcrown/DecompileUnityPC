// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IPlayerInputActor : IPlayerDataOwner, PlayerStateActor.IPlayerActorDelegate, IFacing
{
	int ButtonsPressedThisFrame
	{
		get;
		set;
	}

	int LastBackTapFrame
	{
		get;
		set;
	}

	int LastTechFrame
	{
		get;
		set;
	}

	int FallThroughPlatformHeldFrames
	{
		get;
		set;
	}

	bool TriggerHeldInputAsTaps
	{
		get;
	}

	bool ReadAnyBufferedInput
	{
		get;
	}

	bool AllowFastFall
	{
		get;
	}

	void OnDropInput();
}
