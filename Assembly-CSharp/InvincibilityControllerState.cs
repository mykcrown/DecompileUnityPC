using System;

// Token: 0x020005D4 RID: 1492
[Serializable]
public class InvincibilityControllerState : RollbackStateTyped<InvincibilityControllerState>
{
	// Token: 0x06002175 RID: 8565 RVA: 0x000A749C File Offset: 0x000A589C
	public override void CopyTo(InvincibilityControllerState target)
	{
		target.invincibilityEndFrame = this.invincibilityEndFrame;
		target.intangibileFromLedgeEndFrame = this.intangibileFromLedgeEndFrame;
		target.intangibleFromGrabEndFrame = this.intangibleFromGrabEndFrame;
		target.intangibileFromPlatform = this.intangibileFromPlatform;
		target.intangibileFromMove = this.intangibileFromMove;
		target.projectileInvincibilityEndFrame = this.projectileInvincibilityEndFrame;
		target.projectileIntangibileFromMove = this.projectileIntangibileFromMove;
		target.dirty = this.dirty;
		target.intangibileMoveBodyParts = this.intangibileMoveBodyParts;
		target.projectileIntangibileMoveBodyParts = this.projectileIntangibileMoveBodyParts;
	}

	// Token: 0x06002176 RID: 8566 RVA: 0x000A7524 File Offset: 0x000A5924
	public override object Clone()
	{
		InvincibilityControllerState invincibilityControllerState = new InvincibilityControllerState();
		this.CopyTo(invincibilityControllerState);
		return invincibilityControllerState;
	}

	// Token: 0x06002177 RID: 8567 RVA: 0x000A7540 File Offset: 0x000A5940
	public override void Clear()
	{
		base.Clear();
		this.intangibileMoveBodyParts = null;
		this.invincibilityEndFrame = 0;
		this.intangibileFromLedgeEndFrame = 0;
		this.intangibleFromGrabEndFrame = 0;
		this.intangibileFromMove = false;
		this.projectileIntangibileMoveBodyParts = null;
		this.projectileInvincibilityEndFrame = 0;
		this.projectileIntangibileFromMove = false;
		this.dirty = false;
	}

	// Token: 0x04001A54 RID: 6740
	public int invincibilityEndFrame;

	// Token: 0x04001A55 RID: 6741
	public int intangibileFromLedgeEndFrame;

	// Token: 0x04001A56 RID: 6742
	public int intangibleFromGrabEndFrame;

	// Token: 0x04001A57 RID: 6743
	public bool intangibileFromPlatform;

	// Token: 0x04001A58 RID: 6744
	public bool intangibileFromMove;

	// Token: 0x04001A59 RID: 6745
	public int projectileInvincibilityEndFrame;

	// Token: 0x04001A5A RID: 6746
	public bool projectileIntangibileFromMove;

	// Token: 0x04001A5B RID: 6747
	public bool dirty;

	// Token: 0x04001A5C RID: 6748
	[IsClonedManually]
	[IgnoreCopyValidation]
	public BodyPart[] intangibileMoveBodyParts;

	// Token: 0x04001A5D RID: 6749
	[IsClonedManually]
	[IgnoreCopyValidation]
	public BodyPart[] projectileIntangibileMoveBodyParts;
}
