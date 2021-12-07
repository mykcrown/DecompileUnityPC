// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IPlayerDataOwner : IFacing
{
	IPlayerState State
	{
		get;
	}

	IAnimationPlayer AnimationPlayer
	{
		get;
	}

	AudioManager Audio
	{
		get;
	}

	IPlayerOrientation Orientation
	{
		get;
	}

	IRespawnController RespawnController
	{
		get;
	}

	IBoneController Bones
	{
		get;
	}

	ICharacterRenderer Renderer
	{
		get;
	}

	MoveController ActiveMove
	{
		get;
	}

	CharacterActionData ActionData
	{
		get;
	}

	PlayerPhysicsController Physics
	{
		get;
	}

	IShield Shield
	{
		get;
	}

	IGrabController GrabController
	{
		get;
	}

	CharacterData CharacterData
	{
		get;
	}

	CharacterMenusData CharacterMenusData
	{
		get;
	}

	PlayerModel Model
	{
		get;
	}

	IGameVFX GameVFX
	{
		get;
	}

	IGameInput GameInput
	{
		get;
	}

	ILedgeGrabController LedgeGrabController
	{
		get;
	}

	bool AreInputsLocked
	{
		get;
	}
}
