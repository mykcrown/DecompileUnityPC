using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000A0E RID: 2574
public class LootBoxBuyList : MonoBehaviour
{
	// Token: 0x170011CE RID: 4558
	// (get) Token: 0x06004AAF RID: 19119 RVA: 0x0013FD26 File Offset: 0x0013E126
	// (set) Token: 0x06004AB0 RID: 19120 RVA: 0x0013FD2E File Offset: 0x0013E12E
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x170011CF RID: 4559
	// (get) Token: 0x06004AB1 RID: 19121 RVA: 0x0013FD37 File Offset: 0x0013E137
	// (set) Token: 0x06004AB2 RID: 19122 RVA: 0x0013FD3F File Offset: 0x0013E13F
	[Inject]
	public IBuyLootboxFlow buyLootBoxFlow { get; set; }

	// Token: 0x170011D0 RID: 4560
	// (get) Token: 0x06004AB3 RID: 19123 RVA: 0x0013FD48 File Offset: 0x0013E148
	// (set) Token: 0x06004AB4 RID: 19124 RVA: 0x0013FD50 File Offset: 0x0013E150
	[Inject]
	public ILootBoxesModel lootBoxesModel { get; set; }

	// Token: 0x170011D1 RID: 4561
	// (get) Token: 0x06004AB5 RID: 19125 RVA: 0x0013FD59 File Offset: 0x0013E159
	// (set) Token: 0x06004AB6 RID: 19126 RVA: 0x0013FD61 File Offset: 0x0013E161
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x170011D2 RID: 4562
	// (get) Token: 0x06004AB7 RID: 19127 RVA: 0x0013FD6A File Offset: 0x0013E16A
	// (set) Token: 0x06004AB8 RID: 19128 RVA: 0x0013FD72 File Offset: 0x0013E172
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x06004AB9 RID: 19129 RVA: 0x0013FD7B File Offset: 0x0013E17B
	[PostConstruct]
	public void Init()
	{
		this.itemMenu = this.injector.GetInstance<MenuItemList>();
		this.addLootBoxes();
		this.itemMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.itemMenu.Initialize();
	}

	// Token: 0x170011D3 RID: 4563
	// (get) Token: 0x06004ABA RID: 19130 RVA: 0x0013FDAC File Offset: 0x0013E1AC
	public MenuItemList Menu
	{
		get
		{
			return this.itemMenu;
		}
	}

	// Token: 0x06004ABB RID: 19131 RVA: 0x0013FDB4 File Offset: 0x0013E1B4
	private void addLootBoxes()
	{
		foreach (LootBoxPackage package in this.lootBoxesModel.GetPackages())
		{
			this.addItem(package);
		}
		this.itemMenu.OnSelected = new Action<MenuItemButton, BaseEventData>(this.onButtonSelected);
		this.isDirty = true;
	}

	// Token: 0x06004ABC RID: 19132 RVA: 0x0013FE0C File Offset: 0x0013E20C
	private void Update()
	{
		if (this.isDirty)
		{
			this.MainList.enabled = false;
			if (this.pendingSelection != null)
			{
				this.pendingSelection.transform.parent.SetAsLastSibling();
				this.pendingSelection = null;
			}
			this.isDirty = false;
		}
	}

	// Token: 0x06004ABD RID: 19133 RVA: 0x0013FE64 File Offset: 0x0013E264
	private void addItem(LootBoxPackage package)
	{
		LootBoxItem lootBoxItem = UnityEngine.Object.Instantiate<LootBoxItem>(this.LootBoxItemPrefab);
		lootBoxItem.TitleText.text = this.localization.GetText("ui.store.lootBox.title", package.quantity + string.Empty);
		lootBoxItem.SubText.text = this.localization.GetHardPriceString(package.price);
		lootBoxItem.transform.SetParent(this.MainList.transform, false);
		this.itemMenu.AddButton(lootBoxItem.Button, delegate()
		{
			this.buyItemClicked(package);
		});
	}

	// Token: 0x06004ABE RID: 19134 RVA: 0x0013FF1D File Offset: 0x0013E31D
	private void onButtonSelected(MenuItemButton button, BaseEventData eventData)
	{
		if (this.isDirty)
		{
			this.pendingSelection = button;
		}
		else
		{
			button.transform.parent.SetAsLastSibling();
		}
	}

	// Token: 0x06004ABF RID: 19135 RVA: 0x0013FF46 File Offset: 0x0013E346
	private void buyItemClicked(LootBoxPackage package)
	{
		if (this.uiManager.CurrentInputModule.CurrentMode == ControlMode.MouseMode)
		{
			this.itemMenu.RemoveSelection();
		}
		this.buyLootBoxFlow.Start(package.packageId, new Action<UserPurchaseResult>(this.onBuyResult));
	}

	// Token: 0x06004AC0 RID: 19136 RVA: 0x0013FF86 File Offset: 0x0013E386
	private void onBuyResult(UserPurchaseResult result)
	{
		if (this.BuyResultCallback != null)
		{
			this.BuyResultCallback(result);
		}
	}

	// Token: 0x04003124 RID: 12580
	public HorizontalLayoutGroup MainList;

	// Token: 0x04003125 RID: 12581
	public LootBoxItem LootBoxItemPrefab;

	// Token: 0x04003126 RID: 12582
	public Action<UserPurchaseResult> BuyResultCallback;

	// Token: 0x04003127 RID: 12583
	private MenuItemList itemMenu;

	// Token: 0x04003128 RID: 12584
	private bool isDirty;

	// Token: 0x04003129 RID: 12585
	private MenuItemButton pendingSelection;
}
