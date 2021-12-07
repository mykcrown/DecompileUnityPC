using System;

// Token: 0x020009F1 RID: 2545
public class CollectiblesTabAPI : ICollectiblesTabAPI
{
	// Token: 0x17001160 RID: 4448
	// (get) Token: 0x060048AF RID: 18607 RVA: 0x0013A6EE File Offset: 0x00138AEE
	// (set) Token: 0x060048B0 RID: 18608 RVA: 0x0013A6F6 File Offset: 0x00138AF6
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17001161 RID: 4449
	// (get) Token: 0x060048B1 RID: 18609 RVA: 0x0013A6FF File Offset: 0x00138AFF
	// (set) Token: 0x060048B2 RID: 18610 RVA: 0x0013A707 File Offset: 0x00138B07
	[Inject]
	public IStoreTabsModel storeTabsModel { get; set; }

	// Token: 0x17001162 RID: 4450
	// (get) Token: 0x060048B3 RID: 18611 RVA: 0x0013A710 File Offset: 0x00138B10
	// (set) Token: 0x060048B4 RID: 18612 RVA: 0x0013A718 File Offset: 0x00138B18
	[Inject]
	public IUIAdapter uiScreenAdapter { get; set; }

	// Token: 0x17001163 RID: 4451
	// (get) Token: 0x060048B5 RID: 18613 RVA: 0x0013A721 File Offset: 0x00138B21
	// (set) Token: 0x060048B6 RID: 18614 RVA: 0x0013A729 File Offset: 0x00138B29
	public bool SkipAnimation { get; private set; }

	// Token: 0x060048B7 RID: 18615 RVA: 0x0013A732 File Offset: 0x00138B32
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UIManager.SCREEN_CLOSED, new Action(this.resetOnLeave));
	}

	// Token: 0x060048B8 RID: 18616 RVA: 0x0013A750 File Offset: 0x00138B50
	private void storeTabUpdated()
	{
		if (this.storeTabsModel.Current == StoreTab.COLLECTIBLES)
		{
			this.resetState();
		}
	}

	// Token: 0x060048B9 RID: 18617 RVA: 0x0013A769 File Offset: 0x00138B69
	private void resetState()
	{
		this.SetState(CollectiblesTabState.IntroView, false);
	}

	// Token: 0x060048BA RID: 18618 RVA: 0x0013A773 File Offset: 0x00138B73
	private void resetOnLeave()
	{
		if (this.uiScreenAdapter.PreviousScreen == ScreenType.StoreScreen)
		{
			this.resetState();
		}
	}

	// Token: 0x060048BB RID: 18619 RVA: 0x0013A78D File Offset: 0x00138B8D
	public void SetState(CollectiblesTabState state, bool skipAnimation = false)
	{
		if (this.state != state)
		{
			this.state = state;
			this.SkipAnimation = skipAnimation;
			this.signalBus.Dispatch("CollectiblesTabAPI.UPDATED");
			this.SkipAnimation = false;
		}
	}

	// Token: 0x060048BC RID: 18620 RVA: 0x0013A7C0 File Offset: 0x00138BC0
	public CollectiblesTabState GetState()
	{
		return this.state;
	}

	// Token: 0x04003005 RID: 12293
	public const string UPDATED = "CollectiblesTabAPI.UPDATED";

	// Token: 0x0400300A RID: 12298
	private CollectiblesTabState state;
}
