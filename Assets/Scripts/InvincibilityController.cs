// Decompile from assembly: Assembly-CSharp.dll

using System;

public class InvincibilityController : IInvincibilityController, IRollbackStateOwner, ITickable
{
	private InvincibilityControllerState state;

	private IFrameOwner frameOwner;

	private ICharacterRenderer renderer;

	private IHurtBoxOwner hurtBoxes;

	int IInvincibilityController.InvincibilityFramesRemaining
	{
		get
		{
			return this.state.invincibilityEndFrame - this.frameOwner.Frame;
		}
	}

	int IInvincibilityController.ProjectileInvincibilityFramesRemaining
	{
		get
		{
			return this.state.projectileInvincibilityEndFrame - this.frameOwner.Frame;
		}
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	private bool intangibleFromLedge
	{
		get
		{
			return this.frameOwner.Frame < this.state.intangibileFromLedgeEndFrame;
		}
	}

	public bool IsInvincible
	{
		get
		{
			return this.frameOwner.Frame < this.state.invincibilityEndFrame;
		}
	}

	public bool IsProjectileInvincible
	{
		get
		{
			return this.isFullyProjectileInvulnerable;
		}
	}

	public bool IsFullyIntangible
	{
		get
		{
			return this.state.intangibileFromMove || this.intangibleFromLedge || this.state.intangibileFromPlatform;
		}
	}

	private bool isFullyProjectileInvulnerable
	{
		get
		{
			return this.frameOwner.Frame < this.state.projectileInvincibilityEndFrame || this.state.projectileIntangibileFromMove;
		}
	}

	private bool isGrabInvulnerable
	{
		get
		{
			return this.state.intangibleFromGrabEndFrame > 0 && this.frameOwner.Frame < this.state.intangibleFromGrabEndFrame;
		}
	}

	public void Init(ICharacterRenderer renderer, IFrameOwner frameOwner, IHurtBoxOwner hurtBoxes)
	{
		this.renderer = renderer;
		this.frameOwner = frameOwner;
		this.hurtBoxes = hurtBoxes;
		this.state = new InvincibilityControllerState();
	}

	public void TickFrame()
	{
		if (this.state.intangibileFromLedgeEndFrame == this.frameOwner.Frame || this.state.invincibilityEndFrame == this.frameOwner.Frame || this.state.projectileInvincibilityEndFrame == this.frameOwner.Frame || this.state.intangibleFromGrabEndFrame == this.frameOwner.Frame)
		{
			this.state.dirty = true;
		}
		if (this.state.dirty)
		{
			this.redraw();
			this.state.dirty = false;
		}
	}

	private void redraw()
	{
		this.hurtBoxes.ToggleHurtBoxes(HurtBoxVisibilityState.VISIBLE);
		if (this.isFullyProjectileInvulnerable)
		{
			this.hurtBoxes.ToggleHurtBoxes(HurtBoxVisibilityState.HIDDEN_FROM_PROJECTILES);
		}
		else if (this.state.projectileIntangibileMoveBodyParts != null && this.state.projectileIntangibileMoveBodyParts.Length > 0)
		{
			this.hurtBoxes.ToggleBodyParts(this.state.projectileIntangibileMoveBodyParts, HurtBoxVisibilityState.HIDDEN_FROM_PROJECTILES);
		}
		if (this.isGrabInvulnerable)
		{
			this.hurtBoxes.ToggleHurtBoxes(HurtBoxVisibilityState.HIDDEN_FROM_GRAB);
		}
		if (this.IsFullyIntangible)
		{
			this.hurtBoxes.ToggleHurtBoxes(HurtBoxVisibilityState.HIDDEN);
		}
		else if (this.state.intangibileMoveBodyParts != null && this.state.intangibileMoveBodyParts.Length > 0)
		{
			this.hurtBoxes.ToggleBodyParts(this.state.intangibileMoveBodyParts, HurtBoxVisibilityState.HIDDEN);
		}
		this.renderer.SetColorModeFlag(ColorMode.InvincibleSlow, this.state.intangibileFromPlatform || this.IsInvincible);
		this.renderer.SetColorModeFlag(ColorMode.InvincibleMed, this.intangibleFromLedge || this.state.intangibileFromPlatform || this.IsInvincible);
		this.renderer.SetColorModeFlag(ColorMode.Invincible, this.IsInvincible || this.IsFullyIntangible);
		this.renderer.SetColorModeFlag(ColorMode.RegrabPrevent, this.isGrabInvulnerable);
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<InvincibilityControllerState>(this.state));
		return true;
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<InvincibilityControllerState>(ref this.state);
		return true;
	}

	void IInvincibilityController.BeginInvincibility(int frames)
	{
		this.state.invincibilityEndFrame = this.frameOwner.Frame + frames;
		this.state.dirty = true;
	}

	void IInvincibilityController.BeginLedgeIntangibility(int frames)
	{
		this.state.intangibileFromLedgeEndFrame = this.frameOwner.Frame + frames;
		this.state.dirty = true;
	}

	void IInvincibilityController.BeginGrabIntangibility(int frames)
	{
		this.state.intangibleFromGrabEndFrame = this.frameOwner.Frame + frames;
		this.state.dirty = true;
	}

	void IInvincibilityController.EndGrabInvincibility()
	{
		this.state.intangibleFromGrabEndFrame = 0;
		this.state.dirty = true;
	}

	void IInvincibilityController.BeginPlatformIntangibility()
	{
		this.state.intangibileFromPlatform = true;
		this.state.dirty = true;
	}

	void IInvincibilityController.EndPlatformIntangibility()
	{
		this.state.intangibileFromPlatform = false;
		this.state.dirty = true;
	}

	void IInvincibilityController.BeginMoveIntangibility(BodyPart[] parts)
	{
		bool flag;
		if (parts == null)
		{
			flag = !this.state.intangibileFromMove;
			this.state.intangibileFromMove = true;
		}
		else
		{
			flag = (parts != this.state.intangibileMoveBodyParts);
			this.state.intangibileMoveBodyParts = parts;
		}
		if (flag)
		{
			this.state.dirty = true;
		}
	}

	void IInvincibilityController.BeginMoveProjectileIntangibility(BodyPart[] parts)
	{
		bool flag;
		if (parts == null)
		{
			flag = !this.state.projectileIntangibileFromMove;
			this.state.projectileIntangibileFromMove = true;
		}
		else
		{
			flag = (parts != this.state.projectileIntangibileMoveBodyParts);
			this.state.projectileIntangibileMoveBodyParts = parts;
		}
		if (flag)
		{
			this.state.dirty = true;
		}
	}

	void IInvincibilityController.EndAllMoveIntangibility()
	{
		this.endMoveIntangibility();
		this.endMoveProjectileIntangibility();
	}

	void IInvincibilityController.EndMoveIntangibility()
	{
		this.endMoveIntangibility();
	}

	void IInvincibilityController.EndMoveProjectileIntangibility()
	{
		this.endMoveProjectileIntangibility();
	}

	private void endMoveIntangibility()
	{
		this.state.intangibileFromMove = false;
		this.state.intangibileMoveBodyParts = null;
		this.state.dirty = true;
	}

	private void endMoveProjectileIntangibility()
	{
		this.state.projectileIntangibileFromMove = false;
		this.state.projectileIntangibileMoveBodyParts = null;
		this.state.dirty = true;
	}

	void IInvincibilityController.Clear()
	{
		this.state.Clear();
		this.state.dirty = true;
	}
}
