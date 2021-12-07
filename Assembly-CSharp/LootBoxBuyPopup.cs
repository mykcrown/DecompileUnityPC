using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000A0F RID: 2575
public class LootBoxBuyPopup : BaseWindow
{
	// Token: 0x06004AC2 RID: 19138 RVA: 0x0013FFC4 File Offset: 0x0013E3C4
	[PostConstruct]
	public void Init()
	{
		this.mainMenu = base.injector.GetInstance<MenuItemList>();
		this.mainMenu.AddButton(this.CancelButton, new Action(this.cancelClicked));
		this.mainMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.mainMenu.Initialize();
		this.buyList = UnityEngine.Object.Instantiate<LootBoxBuyList>(this.LootBoxBuyListPrefab).GetComponent<LootBoxBuyList>();
		this.buyList.transform.SetParent(this.ListAnchor.transform, false);
		base.injector.Inject(this.buyList);
		this.buyList.Menu.AddEdgeNavigation(MoveDirection.Down, this.mainMenu);
		this.buyList.Menu.AddEdgeNavigation(MoveDirection.Up, this.mainMenu);
		this.mainMenu.LandingPoint = this.CancelButton;
		this.mainMenu.AddEdgeNavigation(MoveDirection.Up, this.buyList.Menu);
		this.mainMenu.AddEdgeNavigation(MoveDirection.Down, this.buyList.Menu);
		LootBoxBuyList lootBoxBuyList = this.buyList;
		lootBoxBuyList.BuyResultCallback = (Action<UserPurchaseResult>)Delegate.Combine(lootBoxBuyList.BuyResultCallback, new Action<UserPurchaseResult>(this.onBuyResult));
		this.syncButtonNavigation();
	}

	// Token: 0x06004AC3 RID: 19139 RVA: 0x001400F4 File Offset: 0x0013E4F4
	public override void OnCancelPressed()
	{
		this.Close();
	}

	// Token: 0x06004AC4 RID: 19140 RVA: 0x001400FC File Offset: 0x0013E4FC
	public override void OnAnyNavigationButtonPressed()
	{
		this.syncButtonNavigation();
	}

	// Token: 0x06004AC5 RID: 19141 RVA: 0x00140104 File Offset: 0x0013E504
	private void onBuyResult(UserPurchaseResult result)
	{
		if (result == UserPurchaseResult.SUCCESS)
		{
			this.Close();
		}
	}

	// Token: 0x06004AC6 RID: 19142 RVA: 0x00140114 File Offset: 0x0013E514
	private void syncButtonNavigation()
	{
		if (!base.uiManager.CurrentInputModule.IsMouseMode && this.isNullSelection())
		{
			this.buyList.Menu.AutoSelect(this.buyList.Menu.GetButtons()[0]);
		}
	}

	// Token: 0x06004AC7 RID: 19143 RVA: 0x00140163 File Offset: 0x0013E563
	private bool isNullSelection()
	{
		return this.mainMenu.CurrentSelection == null && this.buyList.Menu.CurrentSelection == null;
	}

	// Token: 0x06004AC8 RID: 19144 RVA: 0x00140194 File Offset: 0x0013E594
	private void cancelClicked()
	{
		this.Close();
	}

	// Token: 0x06004AC9 RID: 19145 RVA: 0x0014019C File Offset: 0x0013E59C
	protected override void OnDestroy()
	{
		if (this.buyList != null)
		{
			LootBoxBuyList lootBoxBuyList = this.buyList;
			lootBoxBuyList.BuyResultCallback = (Action<UserPurchaseResult>)Delegate.Remove(lootBoxBuyList.BuyResultCallback, new Action<UserPurchaseResult>(this.onBuyResult));
		}
		base.OnDestroy();
	}

	// Token: 0x0400312A RID: 12586
	public Transform ListAnchor;

	// Token: 0x0400312B RID: 12587
	public LootBoxBuyList LootBoxBuyListPrefab;

	// Token: 0x0400312C RID: 12588
	public MenuItemButton CancelButton;

	// Token: 0x0400312D RID: 12589
	private MenuItemList mainMenu;

	// Token: 0x0400312E RID: 12590
	private LootBoxBuyList buyList;
}
