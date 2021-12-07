using System;

// Token: 0x020009DC RID: 2524
public class CharactersTabAPI : ICharactersTabAPI
{
	// Token: 0x17001122 RID: 4386
	// (get) Token: 0x0600477F RID: 18303 RVA: 0x001365AA File Offset: 0x001349AA
	// (set) Token: 0x06004780 RID: 18304 RVA: 0x001365B2 File Offset: 0x001349B2
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17001123 RID: 4387
	// (get) Token: 0x06004781 RID: 18305 RVA: 0x001365BB File Offset: 0x001349BB
	// (set) Token: 0x06004782 RID: 18306 RVA: 0x001365C3 File Offset: 0x001349C3
	[Inject]
	public IStoreTabsModel storeTabsModel { get; set; }

	// Token: 0x17001124 RID: 4388
	// (get) Token: 0x06004783 RID: 18307 RVA: 0x001365CC File Offset: 0x001349CC
	// (set) Token: 0x06004784 RID: 18308 RVA: 0x001365D4 File Offset: 0x001349D4
	[Inject]
	public IUIAdapter uiScreenAdapter { get; set; }

	// Token: 0x17001125 RID: 4389
	// (get) Token: 0x06004785 RID: 18309 RVA: 0x001365DD File Offset: 0x001349DD
	// (set) Token: 0x06004786 RID: 18310 RVA: 0x001365E5 File Offset: 0x001349E5
	public bool SkipAnimation { get; private set; }

	// Token: 0x06004787 RID: 18311 RVA: 0x001365EE File Offset: 0x001349EE
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UIManager.SCREEN_CLOSED, new Action(this.resetOnLeave));
	}

	// Token: 0x06004788 RID: 18312 RVA: 0x0013660C File Offset: 0x00134A0C
	private void storeTabUpdated()
	{
		if (this.storeTabsModel.Current == StoreTab.CHARACTERS)
		{
			this.resetState();
		}
	}

	// Token: 0x06004789 RID: 18313 RVA: 0x00136625 File Offset: 0x00134A25
	private void resetState()
	{
		this.SetState(CharactersTabState.IntroView, false);
	}

	// Token: 0x0600478A RID: 18314 RVA: 0x0013662F File Offset: 0x00134A2F
	private void resetOnLeave()
	{
		if (this.uiScreenAdapter.PreviousScreen == ScreenType.StoreScreen)
		{
			this.resetState();
		}
	}

	// Token: 0x0600478B RID: 18315 RVA: 0x00136649 File Offset: 0x00134A49
	public void SetState(CharactersTabState state, bool skipAnimation = false)
	{
		if (this.state != state)
		{
			this.state = state;
			this.SkipAnimation = skipAnimation;
			this.signalBus.Dispatch("CharactersTabAPI.UPDATED");
			this.SkipAnimation = false;
		}
	}

	// Token: 0x0600478C RID: 18316 RVA: 0x0013667C File Offset: 0x00134A7C
	public CharactersTabState GetState()
	{
		return this.state;
	}

	// Token: 0x04002F2F RID: 12079
	public const string UPDATED = "CharactersTabAPI.UPDATED";

	// Token: 0x04002F34 RID: 12084
	private CharactersTabState state;
}
