// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LootBoxBuyPopup : BaseWindow
{
	public Transform ListAnchor;

	public LootBoxBuyList LootBoxBuyListPrefab;

	public MenuItemButton CancelButton;

	private MenuItemList mainMenu;

	private LootBoxBuyList buyList;

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
		LootBoxBuyList expr_FC = this.buyList;
		expr_FC.BuyResultCallback = (Action<UserPurchaseResult>)Delegate.Combine(expr_FC.BuyResultCallback, new Action<UserPurchaseResult>(this.onBuyResult));
		this.syncButtonNavigation();
	}

	public override void OnCancelPressed()
	{
		this.Close();
	}

	public override void OnAnyNavigationButtonPressed()
	{
		this.syncButtonNavigation();
	}

	private void onBuyResult(UserPurchaseResult result)
	{
		if (result == UserPurchaseResult.SUCCESS)
		{
			this.Close();
		}
	}

	private void syncButtonNavigation()
	{
		if (!base.uiManager.CurrentInputModule.IsMouseMode && this.isNullSelection())
		{
			this.buyList.Menu.AutoSelect(this.buyList.Menu.GetButtons()[0]);
		}
	}

	private bool isNullSelection()
	{
		return this.mainMenu.CurrentSelection == null && this.buyList.Menu.CurrentSelection == null;
	}

	private void cancelClicked()
	{
		this.Close();
	}

	protected override void OnDestroy()
	{
		if (this.buyList != null)
		{
			LootBoxBuyList expr_17 = this.buyList;
			expr_17.BuyResultCallback = (Action<UserPurchaseResult>)Delegate.Remove(expr_17.BuyResultCallback, new Action<UserPurchaseResult>(this.onBuyResult));
		}
		base.OnDestroy();
	}
}
