using System;

// Token: 0x020009B0 RID: 2480
public class NuxPopup : BaseWindow
{
	// Token: 0x1700102E RID: 4142
	// (get) Token: 0x06004480 RID: 17536 RVA: 0x0012D9F8 File Offset: 0x0012BDF8
	// (set) Token: 0x06004481 RID: 17537 RVA: 0x0012DA00 File Offset: 0x0012BE00
	[Inject]
	public IStoreAPI storeAPI { get; set; }

	// Token: 0x1700102F RID: 4143
	// (get) Token: 0x06004482 RID: 17538 RVA: 0x0012DA09 File Offset: 0x0012BE09
	// (set) Token: 0x06004483 RID: 17539 RVA: 0x0012DA11 File Offset: 0x0012BE11
	[Inject]
	public IStoreTabsModel storeTabsModel { get; set; }

	// Token: 0x17001030 RID: 4144
	// (get) Token: 0x06004484 RID: 17540 RVA: 0x0012DA1A File Offset: 0x0012BE1A
	// (set) Token: 0x06004485 RID: 17541 RVA: 0x0012DA22 File Offset: 0x0012BE22
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x06004486 RID: 17542 RVA: 0x0012DA2C File Offset: 0x0012BE2C
	[PostConstruct]
	public void Init()
	{
		this.mainMenu = base.injector.GetInstance<MenuItemList>();
		this.mainMenu.AddButton(this.DoNothing, new Action(this.Nothing));
		this.mainMenu.AddButton(this.CloseButton, new Action(this.Close));
		this.mainMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 1);
		this.mainMenu.Initialize();
		this.setDefaultItem();
	}

	// Token: 0x06004487 RID: 17543 RVA: 0x0012DAA3 File Offset: 0x0012BEA3
	public void Nothing()
	{
	}

	// Token: 0x06004488 RID: 17544 RVA: 0x0012DAA5 File Offset: 0x0012BEA5
	private void setDefaultItem()
	{
		this.FirstSelected = this.DoNothing.InteractableButton.gameObject;
		this.mainMenu.AutoSelect(this.DoNothing);
	}

	// Token: 0x06004489 RID: 17545 RVA: 0x0012DAD0 File Offset: 0x0012BED0
	public override void Close()
	{
		base.Close();
		if (this.GoToGallery)
		{
			base.audioManager.StopMusic(null, 0.5f);
			this.storeAPI.Mode = StoreMode.NORMAL;
			this.storeTabsModel.Current = StoreTab.CHARACTERS;
			this.events.Broadcast(new LoadScreenCommand(ScreenType.StoreScreen, null, ScreenUpdateType.Next));
		}
	}

	// Token: 0x0600448A RID: 17546 RVA: 0x0012DB2B File Offset: 0x0012BF2B
	public override void OnStartPressed(IPlayerCursor cursor)
	{
		base.OnStartPressed(cursor);
	}

	// Token: 0x0600448B RID: 17547 RVA: 0x0012DB34 File Offset: 0x0012BF34
	public void OnCancel()
	{
		this.Close();
	}

	// Token: 0x0600448C RID: 17548 RVA: 0x0012DB3C File Offset: 0x0012BF3C
	public override void OnCancelPressed()
	{
		this.OnCancel();
	}

	// Token: 0x0600448D RID: 17549 RVA: 0x0012DB44 File Offset: 0x0012BF44
	public override void OnCancelPressed(IPlayerCursor cursor)
	{
		this.OnCancel();
	}

	// Token: 0x04002D96 RID: 11670
	public MenuItemButton CloseButton;

	// Token: 0x04002D97 RID: 11671
	public bool GoToGallery;

	// Token: 0x04002D98 RID: 11672
	public MenuItemButton DoNothing;

	// Token: 0x04002D99 RID: 11673
	private MenuItemList mainMenu;

	// Token: 0x04002D9A RID: 11674
	private MenuItemButton primaryButton;
}
