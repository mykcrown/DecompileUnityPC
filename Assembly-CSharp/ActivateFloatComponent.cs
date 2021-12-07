using System;
using UnityEngine;

// Token: 0x020004C3 RID: 1219
public class ActivateFloatComponent : MoveComponent, IMoveTickMoveFrameComponent
{
	// Token: 0x06001AFC RID: 6908 RVA: 0x00089D5D File Offset: 0x0008815D
	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		base.Init(moveDelegate, playerDelegate, input);
		this.floatComponent = playerDelegate.GetCharacterComponent<FloatCharacterComponent>();
		if (this.floatComponent == null)
		{
			Debug.LogError("Attempted to initiate a float but the character has no FloatCharacterComponent attached to them.");
		}
	}

	// Token: 0x06001AFD RID: 6909 RVA: 0x00089D90 File Offset: 0x00088190
	public void TickMoveFrame(InputButtonsData input)
	{
		if (this.floatComponent != null && this.moveDelegate.Model.internalFrame == this.configData.startAfterFrames)
		{
			this.floatComponent.BeginFloat(this.configData.frameDuration);
		}
	}

	// Token: 0x04001449 RID: 5193
	public ActivateFloatComponentData configData;

	// Token: 0x0400144A RID: 5194
	private FloatCharacterComponent floatComponent;
}
