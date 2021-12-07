using System;

// Token: 0x02000A31 RID: 2609
public class StoreTabElement : ClientBehavior
{
	// Token: 0x1700121A RID: 4634
	// (get) Token: 0x06004C44 RID: 19524 RVA: 0x001361C3 File Offset: 0x001345C3
	// (set) Token: 0x06004C45 RID: 19525 RVA: 0x001361CB File Offset: 0x001345CB
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x1700121B RID: 4635
	// (get) Token: 0x06004C46 RID: 19526 RVA: 0x001361D4 File Offset: 0x001345D4
	// (set) Token: 0x06004C47 RID: 19527 RVA: 0x001361DC File Offset: 0x001345DC
	[Inject]
	public IStoreTabsModel storeTabsModel { get; set; }

	// Token: 0x1700121C RID: 4636
	// (get) Token: 0x06004C48 RID: 19528 RVA: 0x001361E5 File Offset: 0x001345E5
	// (set) Token: 0x06004C49 RID: 19529 RVA: 0x001361ED File Offset: 0x001345ED
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x1700121D RID: 4637
	// (get) Token: 0x06004C4A RID: 19530 RVA: 0x001361F6 File Offset: 0x001345F6
	// (set) Token: 0x06004C4B RID: 19531 RVA: 0x001361FE File Offset: 0x001345FE
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x1700121E RID: 4638
	// (get) Token: 0x06004C4C RID: 19532 RVA: 0x00136207 File Offset: 0x00134607
	// (set) Token: 0x06004C4D RID: 19533 RVA: 0x0013620F File Offset: 0x0013460F
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x1700121F RID: 4639
	// (get) Token: 0x06004C4E RID: 19534 RVA: 0x00136218 File Offset: 0x00134618
	// (set) Token: 0x06004C4F RID: 19535 RVA: 0x00136220 File Offset: 0x00134620
	[Inject]
	public IStoreAPI storeAPI { get; set; }

	// Token: 0x06004C50 RID: 19536 RVA: 0x00136229 File Offset: 0x00134629
	protected bool allowInteraction()
	{
		return this.storeAPI.Mode == StoreMode.NORMAL && this._allowInteraction((StoreTab)this.Def.id);
	}

	// Token: 0x17001220 RID: 4640
	// (set) Token: 0x06004C51 RID: 19537 RVA: 0x00136253 File Offset: 0x00134653
	public Func<StoreTab, bool> AllowInteraction
	{
		set
		{
			this._allowInteraction = value;
		}
	}

	// Token: 0x06004C52 RID: 19538 RVA: 0x0013625C File Offset: 0x0013465C
	public virtual bool OnCancelPressed()
	{
		return false;
	}

	// Token: 0x06004C53 RID: 19539 RVA: 0x0013625F File Offset: 0x0013465F
	public virtual bool OnBackButtonClicked()
	{
		return false;
	}

	// Token: 0x06004C54 RID: 19540 RVA: 0x00136262 File Offset: 0x00134662
	public virtual bool OnRightTriggerPressed()
	{
		return false;
	}

	// Token: 0x06004C55 RID: 19541 RVA: 0x00136265 File Offset: 0x00134665
	public virtual bool OnLeftTriggerPressed()
	{
		return false;
	}

	// Token: 0x06004C56 RID: 19542 RVA: 0x00136268 File Offset: 0x00134668
	public virtual bool OnRightStickLeft()
	{
		return false;
	}

	// Token: 0x06004C57 RID: 19543 RVA: 0x0013626B File Offset: 0x0013466B
	public virtual bool OnRightStickRight()
	{
		return false;
	}

	// Token: 0x06004C58 RID: 19544 RVA: 0x0013626E File Offset: 0x0013466E
	public virtual bool OnDPadLeft()
	{
		return false;
	}

	// Token: 0x06004C59 RID: 19545 RVA: 0x00136271 File Offset: 0x00134671
	public virtual bool OnDPadRight()
	{
		return false;
	}

	// Token: 0x06004C5A RID: 19546 RVA: 0x00136274 File Offset: 0x00134674
	public virtual void UpdateRightStick(float x, float y)
	{
	}

	// Token: 0x06004C5B RID: 19547 RVA: 0x00136276 File Offset: 0x00134676
	public virtual void OnSubmitPressed()
	{
	}

	// Token: 0x06004C5C RID: 19548 RVA: 0x00136278 File Offset: 0x00134678
	public virtual void OnZPressed()
	{
	}

	// Token: 0x06004C5D RID: 19549 RVA: 0x0013627A File Offset: 0x0013467A
	public virtual void OnLeftBumperPressed()
	{
	}

	// Token: 0x06004C5E RID: 19550 RVA: 0x0013627C File Offset: 0x0013467C
	public virtual void OnRightStickUp()
	{
	}

	// Token: 0x06004C5F RID: 19551 RVA: 0x0013627E File Offset: 0x0013467E
	public virtual void OnRightStickDown()
	{
	}

	// Token: 0x06004C60 RID: 19552 RVA: 0x00136280 File Offset: 0x00134680
	public virtual void OnDrawComplete()
	{
	}

	// Token: 0x06004C61 RID: 19553 RVA: 0x00136282 File Offset: 0x00134682
	public virtual void OnActivate()
	{
	}

	// Token: 0x06004C62 RID: 19554 RVA: 0x00136284 File Offset: 0x00134684
	public virtual void UpdateMouseMode()
	{
	}

	// Token: 0x06004C63 RID: 19555 RVA: 0x00136286 File Offset: 0x00134686
	public virtual void OnLeft()
	{
	}

	// Token: 0x06004C64 RID: 19556 RVA: 0x00136288 File Offset: 0x00134688
	public virtual void OnRight()
	{
	}

	// Token: 0x06004C65 RID: 19557 RVA: 0x0013628A File Offset: 0x0013468A
	public virtual void OnUp()
	{
	}

	// Token: 0x06004C66 RID: 19558 RVA: 0x0013628C File Offset: 0x0013468C
	public virtual void OnDown()
	{
	}

	// Token: 0x06004C67 RID: 19559 RVA: 0x0013628E File Offset: 0x0013468E
	public virtual void OnYButtonPressed()
	{
	}

	// Token: 0x04003218 RID: 12824
	public TabDefinition Def;

	// Token: 0x04003219 RID: 12825
	private Func<StoreTab, bool> _allowInteraction;
}
