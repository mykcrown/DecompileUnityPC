using System;

// Token: 0x02000A36 RID: 2614
public class StoreTabWithEquipModule : ClientBehavior
{
	// Token: 0x17001229 RID: 4649
	// (get) Token: 0x06004C82 RID: 19586 RVA: 0x00133E8B File Offset: 0x0013228B
	// (set) Token: 0x06004C83 RID: 19587 RVA: 0x00133E93 File Offset: 0x00132293
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x1700122A RID: 4650
	// (get) Token: 0x06004C84 RID: 19588 RVA: 0x00133E9C File Offset: 0x0013229C
	// (set) Token: 0x06004C85 RID: 19589 RVA: 0x00133EA4 File Offset: 0x001322A4
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x1700122B RID: 4651
	// (get) Token: 0x06004C86 RID: 19590 RVA: 0x00133EAD File Offset: 0x001322AD
	// (set) Token: 0x06004C87 RID: 19591 RVA: 0x00133EB5 File Offset: 0x001322B5
	public virtual IEquipmentSelectorModule module { get; set; }

	// Token: 0x06004C88 RID: 19592 RVA: 0x00133EBE File Offset: 0x001322BE
	public virtual void Init()
	{
		base.injector.Inject(this.EquipModule);
		this.module = this.EquipModule;
		base.listen(UIManager.WINDOW_CLOSED, new Action(this.onWindowClosed));
	}

	// Token: 0x06004C89 RID: 19593 RVA: 0x00133EF4 File Offset: 0x001322F4
	protected bool allowInteraction()
	{
		return this._allowInteraction();
	}

	// Token: 0x1700122C RID: 4652
	// (set) Token: 0x06004C8A RID: 19594 RVA: 0x00133F01 File Offset: 0x00132301
	public Func<bool> AllowInteraction
	{
		set
		{
			this._allowInteraction = value;
		}
	}

	// Token: 0x06004C8B RID: 19595 RVA: 0x00133F0A File Offset: 0x0013230A
	protected virtual bool isEquipViewActive()
	{
		return true;
	}

	// Token: 0x06004C8C RID: 19596 RVA: 0x00133F0D File Offset: 0x0013230D
	public virtual void OnActivate()
	{
		this.module.ForceRedraws();
		if (this.isEquipViewActive())
		{
			this.module.BeginMenuFocus();
		}
	}

	// Token: 0x06004C8D RID: 19597 RVA: 0x00133F30 File Offset: 0x00132330
	public virtual void UpdateMouseMode()
	{
		this.module.OnMouseModeUpdate();
		if (this.isEquipModuleFocused())
		{
			this.module.SyncButtonModeSelection();
		}
	}

	// Token: 0x06004C8E RID: 19598 RVA: 0x00133F53 File Offset: 0x00132353
	protected bool isMouseMode()
	{
		return (this.uiManager.CurrentInputModule as UIInputModule).IsMouseMode;
	}

	// Token: 0x06004C8F RID: 19599 RVA: 0x00133F6A File Offset: 0x0013236A
	public void OnRight()
	{
		if (this.isEquipModuleFocused())
		{
			this.module.OnRight();
		}
	}

	// Token: 0x06004C90 RID: 19600 RVA: 0x00133F83 File Offset: 0x00132383
	public void OnYButtonPressed()
	{
		if (this.isEquipModuleFocused())
		{
			this.module.OnYButton();
		}
	}

	// Token: 0x06004C91 RID: 19601 RVA: 0x00133F9B File Offset: 0x0013239B
	public virtual void OnZPressed()
	{
	}

	// Token: 0x06004C92 RID: 19602 RVA: 0x00133F9D File Offset: 0x0013239D
	public virtual void OnRightStickUp()
	{
	}

	// Token: 0x06004C93 RID: 19603 RVA: 0x00133F9F File Offset: 0x0013239F
	public virtual void OnRightStickDown()
	{
	}

	// Token: 0x06004C94 RID: 19604 RVA: 0x00133FA1 File Offset: 0x001323A1
	public virtual void UpdateRightStick(float x, float y)
	{
	}

	// Token: 0x06004C95 RID: 19605 RVA: 0x00133FA3 File Offset: 0x001323A3
	private void onWindowClosed()
	{
		if (this.allowInteraction())
		{
			this.module.SyncButtonModeSelection();
		}
	}

	// Token: 0x06004C96 RID: 19606 RVA: 0x00133FBB File Offset: 0x001323BB
	protected virtual bool isEquipModuleFocused()
	{
		return this.allowInteraction();
	}

	// Token: 0x04003229 RID: 12841
	public EquipmentSelectorModule EquipModule;

	// Token: 0x0400322B RID: 12843
	private Func<bool> _allowInteraction;
}
