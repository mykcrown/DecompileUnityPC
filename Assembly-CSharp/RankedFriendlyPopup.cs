using System;
using System.Collections.Generic;
using IconsServer;

// Token: 0x020009B1 RID: 2481
public class RankedFriendlyPopup : BaseWindow
{
	// Token: 0x17001031 RID: 4145
	// (get) Token: 0x0600448F RID: 17551 RVA: 0x0012DB54 File Offset: 0x0012BF54
	// (set) Token: 0x06004490 RID: 17552 RVA: 0x0012DB5C File Offset: 0x0012BF5C
	[Inject]
	public IStoreAPI storeAPI { get; set; }

	// Token: 0x17001032 RID: 4146
	// (get) Token: 0x06004491 RID: 17553 RVA: 0x0012DB65 File Offset: 0x0012BF65
	// (set) Token: 0x06004492 RID: 17554 RVA: 0x0012DB6D File Offset: 0x0012BF6D
	[Inject]
	public IStoreTabsModel storeTabsModel { get; set; }

	// Token: 0x17001033 RID: 4147
	// (get) Token: 0x06004493 RID: 17555 RVA: 0x0012DB76 File Offset: 0x0012BF76
	// (set) Token: 0x06004494 RID: 17556 RVA: 0x0012DB7E File Offset: 0x0012BF7E
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x06004495 RID: 17557 RVA: 0x0012DB87 File Offset: 0x0012BF87
	public void SetQueueWindowCreator(List<Func<BaseWindow>> createWindowQueue)
	{
		this.addToQueue = true;
		this.createWindowQueue = createWindowQueue;
	}

	// Token: 0x06004496 RID: 17558 RVA: 0x0012DB98 File Offset: 0x0012BF98
	[PostConstruct]
	public void Init()
	{
		this.mainMenu = base.injector.GetInstance<MenuItemList>();
		this.mainMenu.AddButton(this.selectRanked, new Action(this.SelectRanked));
		this.mainMenu.AddButton(this.selectFriendly, new Action(this.SelectFriendly));
		this.mainMenu.AddButton(this.DoNothing, new Action(this.Nothing));
		this.mainMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 1);
		this.mainMenu.Initialize();
		this.setDefaultItem();
	}

	// Token: 0x06004497 RID: 17559 RVA: 0x0012DC2B File Offset: 0x0012C02B
	public void Nothing()
	{
	}

	// Token: 0x06004498 RID: 17560 RVA: 0x0012DC30 File Offset: 0x0012C030
	public void SelectRanked()
	{
		if (this.addToQueue)
		{
			this.createWindowQueue.Add(() => base.dialogController.ShowNuxRankConfirmedDialog());
		}
		this.Close();
		if (!this.addToQueue)
		{
			base.dialogController.ShowNuxRankConfirmedDialog();
		}
		this.addToQueue = false;
	}

	// Token: 0x06004499 RID: 17561 RVA: 0x0012DC83 File Offset: 0x0012C083
	public void SelectFriendly()
	{
		this.Close();
	}

	// Token: 0x0600449A RID: 17562 RVA: 0x0012DC8B File Offset: 0x0012C08B
	private void setDefaultItem()
	{
		this.FirstSelected = this.DoNothing.InteractableButton.gameObject;
		this.mainMenu.AutoSelect(this.DoNothing);
	}

	// Token: 0x04002D9E RID: 11678
	public bool GoToGallery;

	// Token: 0x04002D9F RID: 11679
	public MenuItemButton DoNothing;

	// Token: 0x04002DA0 RID: 11680
	public MenuItemButton selectRanked;

	// Token: 0x04002DA1 RID: 11681
	public MenuItemButton selectFriendly;

	// Token: 0x04002DA2 RID: 11682
	private MenuItemList mainMenu;

	// Token: 0x04002DA6 RID: 11686
	private bool addToQueue;

	// Token: 0x04002DA7 RID: 11687
	private List<Func<BaseWindow>> createWindowQueue;
}
