using System;

// Token: 0x020005D5 RID: 1493
public class InvincibilityController : IInvincibilityController, IRollbackStateOwner, ITickable
{
	// Token: 0x1700076C RID: 1900
	// (get) Token: 0x06002179 RID: 8569 RVA: 0x000A759A File Offset: 0x000A599A
	// (set) Token: 0x0600217A RID: 8570 RVA: 0x000A75A2 File Offset: 0x000A59A2
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x0600217B RID: 8571 RVA: 0x000A75AB File Offset: 0x000A59AB
	public void Init(ICharacterRenderer renderer, IFrameOwner frameOwner, IHurtBoxOwner hurtBoxes)
	{
		this.renderer = renderer;
		this.frameOwner = frameOwner;
		this.hurtBoxes = hurtBoxes;
		this.state = new InvincibilityControllerState();
	}

	// Token: 0x0600217C RID: 8572 RVA: 0x000A75D0 File Offset: 0x000A59D0
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

	// Token: 0x0600217D RID: 8573 RVA: 0x000A7678 File Offset: 0x000A5A78
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

	// Token: 0x0600217E RID: 8574 RVA: 0x000A77E7 File Offset: 0x000A5BE7
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<InvincibilityControllerState>(this.state));
		return true;
	}

	// Token: 0x0600217F RID: 8575 RVA: 0x000A7803 File Offset: 0x000A5C03
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<InvincibilityControllerState>(ref this.state);
		return true;
	}

	// Token: 0x1700076D RID: 1901
	// (get) Token: 0x06002180 RID: 8576 RVA: 0x000A7813 File Offset: 0x000A5C13
	private bool intangibleFromLedge
	{
		get
		{
			return this.frameOwner.Frame < this.state.intangibileFromLedgeEndFrame;
		}
	}

	// Token: 0x1700076E RID: 1902
	// (get) Token: 0x06002181 RID: 8577 RVA: 0x000A782D File Offset: 0x000A5C2D
	public bool IsInvincible
	{
		get
		{
			return this.frameOwner.Frame < this.state.invincibilityEndFrame;
		}
	}

	// Token: 0x1700076F RID: 1903
	// (get) Token: 0x06002182 RID: 8578 RVA: 0x000A7847 File Offset: 0x000A5C47
	public bool IsProjectileInvincible
	{
		get
		{
			return this.isFullyProjectileInvulnerable;
		}
	}

	// Token: 0x17000770 RID: 1904
	// (get) Token: 0x06002183 RID: 8579 RVA: 0x000A784F File Offset: 0x000A5C4F
	public bool IsFullyIntangible
	{
		get
		{
			return this.state.intangibileFromMove || this.intangibleFromLedge || this.state.intangibileFromPlatform;
		}
	}

	// Token: 0x17000771 RID: 1905
	// (get) Token: 0x06002184 RID: 8580 RVA: 0x000A787A File Offset: 0x000A5C7A
	private bool isFullyProjectileInvulnerable
	{
		get
		{
			return this.frameOwner.Frame < this.state.projectileInvincibilityEndFrame || this.state.projectileIntangibileFromMove;
		}
	}

	// Token: 0x17000772 RID: 1906
	// (get) Token: 0x06002185 RID: 8581 RVA: 0x000A78A5 File Offset: 0x000A5CA5
	private bool isGrabInvulnerable
	{
		get
		{
			return this.state.intangibleFromGrabEndFrame > 0 && this.frameOwner.Frame < this.state.intangibleFromGrabEndFrame;
		}
	}

	// Token: 0x1700076A RID: 1898
	// (get) Token: 0x06002186 RID: 8582 RVA: 0x000A78D3 File Offset: 0x000A5CD3
	int IInvincibilityController.InvincibilityFramesRemaining
	{
		get
		{
			return this.state.invincibilityEndFrame - this.frameOwner.Frame;
		}
	}

	// Token: 0x1700076B RID: 1899
	// (get) Token: 0x06002187 RID: 8583 RVA: 0x000A78EC File Offset: 0x000A5CEC
	int IInvincibilityController.ProjectileInvincibilityFramesRemaining
	{
		get
		{
			return this.state.projectileInvincibilityEndFrame - this.frameOwner.Frame;
		}
	}

	// Token: 0x06002188 RID: 8584 RVA: 0x000A7905 File Offset: 0x000A5D05
	void IInvincibilityController.BeginInvincibility(int frames)
	{
		this.state.invincibilityEndFrame = this.frameOwner.Frame + frames;
		this.state.dirty = true;
	}

	// Token: 0x06002189 RID: 8585 RVA: 0x000A792B File Offset: 0x000A5D2B
	void IInvincibilityController.BeginLedgeIntangibility(int frames)
	{
		this.state.intangibileFromLedgeEndFrame = this.frameOwner.Frame + frames;
		this.state.dirty = true;
	}

	// Token: 0x0600218A RID: 8586 RVA: 0x000A7951 File Offset: 0x000A5D51
	void IInvincibilityController.BeginGrabIntangibility(int frames)
	{
		this.state.intangibleFromGrabEndFrame = this.frameOwner.Frame + frames;
		this.state.dirty = true;
	}

	// Token: 0x0600218B RID: 8587 RVA: 0x000A7977 File Offset: 0x000A5D77
	void IInvincibilityController.EndGrabInvincibility()
	{
		this.state.intangibleFromGrabEndFrame = 0;
		this.state.dirty = true;
	}

	// Token: 0x0600218C RID: 8588 RVA: 0x000A7991 File Offset: 0x000A5D91
	void IInvincibilityController.BeginPlatformIntangibility()
	{
		this.state.intangibileFromPlatform = true;
		this.state.dirty = true;
	}

	// Token: 0x0600218D RID: 8589 RVA: 0x000A79AB File Offset: 0x000A5DAB
	void IInvincibilityController.EndPlatformIntangibility()
	{
		this.state.intangibileFromPlatform = false;
		this.state.dirty = true;
	}

	// Token: 0x0600218E RID: 8590 RVA: 0x000A79C8 File Offset: 0x000A5DC8
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

	// Token: 0x0600218F RID: 8591 RVA: 0x000A7A30 File Offset: 0x000A5E30
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

	// Token: 0x06002190 RID: 8592 RVA: 0x000A7A95 File Offset: 0x000A5E95
	void IInvincibilityController.EndAllMoveIntangibility()
	{
		this.endMoveIntangibility();
		this.endMoveProjectileIntangibility();
	}

	// Token: 0x06002191 RID: 8593 RVA: 0x000A7AA3 File Offset: 0x000A5EA3
	void IInvincibilityController.EndMoveIntangibility()
	{
		this.endMoveIntangibility();
	}

	// Token: 0x06002192 RID: 8594 RVA: 0x000A7AAB File Offset: 0x000A5EAB
	void IInvincibilityController.EndMoveProjectileIntangibility()
	{
		this.endMoveProjectileIntangibility();
	}

	// Token: 0x06002193 RID: 8595 RVA: 0x000A7AB3 File Offset: 0x000A5EB3
	private void endMoveIntangibility()
	{
		this.state.intangibileFromMove = false;
		this.state.intangibileMoveBodyParts = null;
		this.state.dirty = true;
	}

	// Token: 0x06002194 RID: 8596 RVA: 0x000A7AD9 File Offset: 0x000A5ED9
	private void endMoveProjectileIntangibility()
	{
		this.state.projectileIntangibileFromMove = false;
		this.state.projectileIntangibileMoveBodyParts = null;
		this.state.dirty = true;
	}

	// Token: 0x06002195 RID: 8597 RVA: 0x000A7AFF File Offset: 0x000A5EFF
	void IInvincibilityController.Clear()
	{
		this.state.Clear();
		this.state.dirty = true;
	}

	// Token: 0x04001A5F RID: 6751
	private InvincibilityControllerState state;

	// Token: 0x04001A60 RID: 6752
	private IFrameOwner frameOwner;

	// Token: 0x04001A61 RID: 6753
	private ICharacterRenderer renderer;

	// Token: 0x04001A62 RID: 6754
	private IHurtBoxOwner hurtBoxes;
}
