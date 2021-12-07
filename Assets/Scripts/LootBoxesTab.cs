// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LootBoxesTab : StoreTabElement
{
	public MenuItemButton OpenButton;

	public TextMeshProUGUI QuantityText;

	public Transform BuyListAnchor;

	public LootBoxBuyList LootBoxBuyListPrefab;

	private MenuItemList mainMenu;

	private LootBoxBuyList buyList;

	[Inject]
	public IUserLootboxesModel userLootBoxesModel
	{
		get;
		set;
	}

	[Inject]
	public IWindowDisplay windowDisplay
	{
		get;
		set;
	}

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

	private void Start()
	{
		this.updateMode();
	}

	private void updateMode()
	{
		if (base.storeAPI.Mode != StoreMode.NORMAL)
		{
			this.mainMenu.RemoveSelection();
		}
	}

	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		if (base.allowInteraction())
		{
			this.syncButtonNavigation();
		}
	}

	public override void OnActivate()
	{
		base.OnActivate();
		if (base.allowInteraction())
		{
			this.syncButtonNavigation();
		}
	}

	private void syncButtonNavigation()
	{
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode && this.windowDisplay.GetWindowCount() == 0 && this.buyList.Menu.CurrentSelection == null && this.mainMenu.CurrentSelection == null)
		{
			this.buyList.Menu.AutoSelect(this.buyList.Menu.GetButtons()[0]);
		}
	}

	private void openClicked()
	{
		base.storeAPI.Mode = StoreMode.UNBOXING;
	}

	private void updateData()
	{
		this.updateCount();
	}

	private void updateCount()
	{
		int totalQuantity = this.userLootBoxesModel.GetTotalQuantity();
		this.QuantityText.gameObject.SetActive(true);
		this.QuantityText.text = base.localization.GetText("ui.store.unboxing.quantity", totalQuantity + string.Empty);
	}
}
