// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootBoxBuyList : MonoBehaviour
{
	private sealed class _addItem_c__AnonStorey0
	{
		internal LootBoxPackage package;

		internal LootBoxBuyList _this;

		internal void __m__0()
		{
			this._this.buyItemClicked(this.package);
		}
	}

	public HorizontalLayoutGroup MainList;

	public LootBoxItem LootBoxItemPrefab;

	public Action<UserPurchaseResult> BuyResultCallback;

	private MenuItemList itemMenu;

	private bool isDirty;

	private MenuItemButton pendingSelection;

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public IBuyLootboxFlow buyLootBoxFlow
	{
		get;
		set;
	}

	[Inject]
	public ILootBoxesModel lootBoxesModel
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public UIManager uiManager
	{
		get;
		set;
	}

	public MenuItemList Menu
	{
		get
		{
			return this.itemMenu;
		}
	}

	[PostConstruct]
	public void Init()
	{
		this.itemMenu = this.injector.GetInstance<MenuItemList>();
		this.addLootBoxes();
		this.itemMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.itemMenu.Initialize();
	}

	private void addLootBoxes()
	{
		LootBoxPackage[] packages = this.lootBoxesModel.GetPackages();
		for (int i = 0; i < packages.Length; i++)
		{
			LootBoxPackage package = packages[i];
			this.addItem(package);
		}
		this.itemMenu.OnSelected = new Action<MenuItemButton, BaseEventData>(this.onButtonSelected);
		this.isDirty = true;
	}

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

	private void addItem(LootBoxPackage package)
	{
		LootBoxBuyList._addItem_c__AnonStorey0 _addItem_c__AnonStorey = new LootBoxBuyList._addItem_c__AnonStorey0();
		_addItem_c__AnonStorey.package = package;
		_addItem_c__AnonStorey._this = this;
		LootBoxItem lootBoxItem = UnityEngine.Object.Instantiate<LootBoxItem>(this.LootBoxItemPrefab);
		lootBoxItem.TitleText.text = this.localization.GetText("ui.store.lootBox.title", _addItem_c__AnonStorey.package.quantity + string.Empty);
		lootBoxItem.SubText.text = this.localization.GetHardPriceString(_addItem_c__AnonStorey.package.price);
		lootBoxItem.transform.SetParent(this.MainList.transform, false);
		this.itemMenu.AddButton(lootBoxItem.Button, new Action(_addItem_c__AnonStorey.__m__0));
	}

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

	private void buyItemClicked(LootBoxPackage package)
	{
		if (this.uiManager.CurrentInputModule.CurrentMode == ControlMode.MouseMode)
		{
			this.itemMenu.RemoveSelection();
		}
		this.buyLootBoxFlow.Start(package.packageId, new Action<UserPurchaseResult>(this.onBuyResult));
	}

	private void onBuyResult(UserPurchaseResult result)
	{
		if (this.BuyResultCallback != null)
		{
			this.BuyResultCallback(result);
		}
	}
}
