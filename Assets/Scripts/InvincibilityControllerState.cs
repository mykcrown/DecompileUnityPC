// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class InvincibilityControllerState : RollbackStateTyped<InvincibilityControllerState>
{
	public int invincibilityEndFrame;

	public int intangibileFromLedgeEndFrame;

	public int intangibleFromGrabEndFrame;

	public bool intangibileFromPlatform;

	public bool intangibileFromMove;

	public int projectileInvincibilityEndFrame;

	public bool projectileIntangibileFromMove;

	public bool dirty;

	[IgnoreCopyValidation, IsClonedManually]
	public BodyPart[] intangibileMoveBodyParts;

	[IgnoreCopyValidation, IsClonedManually]
	public BodyPart[] projectileIntangibileMoveBodyParts;

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

	public override object Clone()
	{
		InvincibilityControllerState invincibilityControllerState = new InvincibilityControllerState();
		this.CopyTo(invincibilityControllerState);
		return invincibilityControllerState;
	}

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
}
