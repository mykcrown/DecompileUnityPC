// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class ActivateFloatComponent : MoveComponent, IMoveTickMoveFrameComponent
{
	public ActivateFloatComponentData configData;

	private FloatCharacterComponent floatComponent;

	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		base.Init(moveDelegate, playerDelegate, input);
		this.floatComponent = playerDelegate.GetCharacterComponent<FloatCharacterComponent>();
		if (this.floatComponent == null)
		{
			UnityEngine.Debug.LogError("Attempted to initiate a float but the character has no FloatCharacterComponent attached to them.");
		}
	}

	public void TickMoveFrame(InputButtonsData input)
	{
		if (this.floatComponent != null && this.moveDelegate.Model.internalFrame == this.configData.startAfterFrames)
		{
			this.floatComponent.BeginFloat(this.configData.frameDuration);
		}
	}
}
