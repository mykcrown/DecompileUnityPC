// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class RankedFriendlyPopup : BaseWindow
{
	public bool GoToGallery;

	public MenuItemButton DoNothing;

	public MenuItemButton selectRanked;

	public MenuItemButton selectFriendly;

	private MenuItemList mainMenu;

	private bool addToQueue;

	private List<Func<BaseWindow>> createWindowQueue;

	[Inject]
	public IStoreAPI storeAPI
	{
		get;
		set;
	}

	[Inject]
	public IStoreTabsModel storeTabsModel
	{
		get;
		set;
	}

	[Inject]
	public IIconsServerAPI iconsServerAPI
	{
		get;
		set;
	}

	public void SetQueueWindowCreator(List<Func<BaseWindow>> createWindowQueue)
	{
		this.addToQueue = true;
		this.createWindowQueue = createWindowQueue;
	}

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

	public void Nothing()
	{
	}

	public void SelectRanked()
	{
		if (this.addToQueue)
		{
			this.createWindowQueue.Add(new Func<BaseWindow>(this._SelectRanked_m__0));
		}
		this.Close();
		if (!this.addToQueue)
		{
			base.dialogController.ShowNuxRankConfirmedDialog();
		}
		this.addToQueue = false;
	}

	public void SelectFriendly()
	{
		this.Close();
	}

	private void setDefaultItem()
	{
		this.FirstSelected = this.DoNothing.InteractableButton.gameObject;
		this.mainMenu.AutoSelect(this.DoNothing);
	}

	private BaseWindow _SelectRanked_m__0()
	{
		return base.dialogController.ShowNuxRankConfirmedDialog();
	}
}
