// Decompile from assembly: Assembly-CSharp.dll

using System;

public class NuxPopup : BaseWindow
{
	public MenuItemButton CloseButton;

	public bool GoToGallery;

	public MenuItemButton DoNothing;

	private MenuItemList mainMenu;

	private MenuItemButton primaryButton;

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
	public IEvents events
	{
		get;
		set;
	}

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

	public void Nothing()
	{
	}

	private void setDefaultItem()
	{
		this.FirstSelected = this.DoNothing.InteractableButton.gameObject;
		this.mainMenu.AutoSelect(this.DoNothing);
	}

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

	public override void OnStartPressed(IPlayerCursor cursor)
	{
		base.OnStartPressed(cursor);
	}

	public void OnCancel()
	{
		this.Close();
	}

	public override void OnCancelPressed()
	{
		this.OnCancel();
	}

	public override void OnCancelPressed(IPlayerCursor cursor)
	{
		this.OnCancel();
	}
}
