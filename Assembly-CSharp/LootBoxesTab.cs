using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000A10 RID: 2576
public class LootBoxesTab : StoreTabElement
{
	// Token: 0x170011D4 RID: 4564
	// (get) Token: 0x06004ACB RID: 19147 RVA: 0x001401E4 File Offset: 0x0013E5E4
	// (set) Token: 0x06004ACC RID: 19148 RVA: 0x001401EC File Offset: 0x0013E5EC
	[Inject]
	public IUserLootboxesModel userLootBoxesModel { get; set; }

	// Token: 0x170011D5 RID: 4565
	// (get) Token: 0x06004ACD RID: 19149 RVA: 0x001401F5 File Offset: 0x0013E5F5
	// (set) Token: 0x06004ACE RID: 19150 RVA: 0x001401FD File Offset: 0x0013E5FD
	[Inject]
	public IWindowDisplay windowDisplay { get; set; }

	// Token: 0x06004ACF RID: 19151 RVA: 0x00140208 File Offset: 0x0013E608
	[PostConstruct]
	public void Init()
	{
		this.mainMenu = base.injector.GetInstance<MenuItemList>();
		this.mainMenu.AddButton(this.OpenButton, new Action(this.openClicked));
		this.mainMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.mainMenu.Initialize();
		this.buyList = UnityEngine.Object.Instantiate<LootBoxBuyList>(this.LootBoxBuyListPrefab).GetComponent<LootBoxBuyList>();
		this.buyList.transform.SetParent(this.BuyListAnchor.transform);
		base.injector.Inject(this.buyList);
		this.buyList.Menu.AddEdgeNavigation(MoveDirection.Down, this.mainMenu);
		this.buyList.Menu.AddEdgeNavigation(MoveDirection.Up, this.mainMenu);
		this.mainMenu.LandingPoint = this.OpenButton;
		this.mainMenu.AddEdgeNavigation(MoveDirection.Up, this.buyList.Menu);
		this.mainMenu.AddEdgeNavigation(MoveDirection.Down, this.buyList.Menu);
		base.listen(StoreAPI.UPDATE, new Action(this.updateMode));
		base.listen(UserLootboxesModel.UPDATED, new Action(this.updateData));
		this.updateData();
	}

	// Token: 0x06004AD0 RID: 19152 RVA: 0x0014033E File Offset: 0x0013E73E
	private void Start()
	{
		this.updateMode();
	}

	// Token: 0x06004AD1 RID: 19153 RVA: 0x00140346 File Offset: 0x0013E746
	private void updateMode()
	{
		if (base.storeAPI.Mode != StoreMode.NORMAL)
		{
			this.mainMenu.RemoveSelection();
		}
	}

	// Token: 0x06004AD2 RID: 19154 RVA: 0x00140363 File Offset: 0x0013E763
	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		if (base.allowInteraction())
		{
			this.syncButtonNavigation();
		}
	}

	// Token: 0x06004AD3 RID: 19155 RVA: 0x0014037C File Offset: 0x0013E77C
	public override void OnActivate()
	{
		base.OnActivate();
		if (base.allowInteraction())
		{
			this.syncButtonNavigation();
		}
	}

	// Token: 0x06004AD4 RID: 19156 RVA: 0x00140398 File Offset: 0x0013E798
	private void syncButtonNavigation()
	{
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode && this.windowDisplay.GetWindowCount() == 0 && this.buyList.Menu.CurrentSelection == null && this.mainMenu.CurrentSelection == null)
		{
			this.buyList.Menu.AutoSelect(this.buyList.Menu.GetButtons()[0]);
		}
	}

	// Token: 0x06004AD5 RID: 19157 RVA: 0x00140422 File Offset: 0x0013E822
	private void openClicked()
	{
		base.storeAPI.Mode = StoreMode.UNBOXING;
	}

	// Token: 0x06004AD6 RID: 19158 RVA: 0x00140430 File Offset: 0x0013E830
	private void updateData()
	{
		this.updateCount();
	}

	// Token: 0x06004AD7 RID: 19159 RVA: 0x00140438 File Offset: 0x0013E838
	private void updateCount()
	{
		int totalQuantity = this.userLootBoxesModel.GetTotalQuantity();
		this.QuantityText.gameObject.SetActive(true);
		this.QuantityText.text = base.localization.GetText("ui.store.unboxing.quantity", totalQuantity + string.Empty);
	}

	// Token: 0x04003131 RID: 12593
	public MenuItemButton OpenButton;

	// Token: 0x04003132 RID: 12594
	public TextMeshProUGUI QuantityText;

	// Token: 0x04003133 RID: 12595
	public Transform BuyListAnchor;

	// Token: 0x04003134 RID: 12596
	public LootBoxBuyList LootBoxBuyListPrefab;

	// Token: 0x04003135 RID: 12597
	private MenuItemList mainMenu;

	// Token: 0x04003136 RID: 12598
	private LootBoxBuyList buyList;
}
