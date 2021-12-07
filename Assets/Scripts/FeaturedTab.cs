// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FeaturedTab : StoreTabElement
{
	private sealed class _addMenuItem_c__AnonStorey0
	{
		internal MenuItemButton button;

		internal void __m__0(BaseEventData eventData)
		{
			this.button.transform.SetAsLastSibling();
		}
	}

	public MenuItemButton FoundersPackButton;

	public MenuItemButton ProAccountButton;

	public MenuItemButton Lootbox25BundleButton;

	public MenuItemButton Lootbox55BundleButton;

	public HorizontalLayoutGroup ProAccountGroup;

	public HorizontalLayoutGroup LootboxBundleGroup;

	public TextMeshProUGUI FoundersPrice;

	public TextMeshProUGUI ProAccountPrice;

	public TextMeshProUGUI Lootbox25BundlePrice;

	public TextMeshProUGUI Lootbox55BundlePrice;

	private MenuItemList mainMenu;

	[Inject]
	public IFeaturedTabAPI api
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

	[Inject]
	public IBuyLootboxFlow buyLootBoxFlow
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		base.listen(UserProAccountUnlockedModel.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	private void resetButtonMenu()
	{
		this.mainMenu = base.injector.GetInstance<MenuItemList>();
	}

	private void initMenu()
	{
		this.mainMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.mainMenu.Initialize();
	}

	private void initFoundersPackButtons()
	{
		this.resetButtonMenu();
		this.ProAccountGroup.gameObject.SetActive(true);
		this.LootboxBundleGroup.gameObject.SetActive(false);
		this.addMenuItem(this.FoundersPackButton, new Action(this.foundersPackClicked));
		this.addMenuItem(this.ProAccountButton, new Action(this.proAccountClicked));
		this.ProAccountPrice.text = this.api.GetProAccountPriceString();
		this.FoundersPrice.text = this.api.GetFoundersPackPriceString();
		this.initMenu();
	}

	private void initLootboxPackButtons()
	{
		this.resetButtonMenu();
		this.ProAccountGroup.gameObject.SetActive(false);
		this.LootboxBundleGroup.gameObject.SetActive(true);
		this.addMenuItem(this.Lootbox25BundleButton, new Action(this.lootbox25BundleCilcked));
		this.addMenuItem(this.Lootbox55BundleButton, new Action(this.lootbox55BundleClicked));
		this.Lootbox25BundlePrice.text = this.api.GetLootbox25BundlePriceString();
		this.Lootbox55BundlePrice.text = this.api.GetLootbox55BundlePriceString();
		this.initMenu();
	}

	private void onUpdate()
	{
		if (this.api.IsProAccountUnlocked())
		{
			this.initLootboxPackButtons();
		}
		else
		{
			this.initFoundersPackButtons();
		}
	}

	private void addMenuItem(MenuItemButton button, Action callback)
	{
		FeaturedTab._addMenuItem_c__AnonStorey0 _addMenuItem_c__AnonStorey = new FeaturedTab._addMenuItem_c__AnonStorey0();
		_addMenuItem_c__AnonStorey.button = button;
		this.mainMenu.AddButton(_addMenuItem_c__AnonStorey.button, callback);
		WavedashUIButton expr_2A = _addMenuItem_c__AnonStorey.button.InteractableButton;
		expr_2A.OnSelectEvent = (Action<BaseEventData>)Delegate.Combine(expr_2A.OnSelectEvent, new Action<BaseEventData>(_addMenuItem_c__AnonStorey.__m__0));
	}

	private void Update()
	{
		this.ProAccountGroup.enabled = false;
		this.LootboxBundleGroup.enabled = false;
	}

	private void foundersPackClicked()
	{
		if (!this.api.IsProAccountUnlocked())
		{
			this.api.BuyFoundersPack();
		}
	}

	private void proAccountClicked()
	{
		if (!this.api.IsProAccountUnlocked())
		{
			this.api.BuyProAccount();
		}
	}

	private void lootbox25BundleCilcked()
	{
		this.buyLootBoxFlow.Start(this.api.GetLootbox25BundlePackageId(), null);
	}

	private void lootbox55BundleClicked()
	{
		this.buyLootBoxFlow.Start(this.api.GetLootbox55BundlePackageId(), null);
	}

	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		if (base.allowInteraction())
		{
			this.syncButtonNavigation();
		}
	}

	private void syncButtonNavigation()
	{
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode && this.windowDisplay.GetWindowCount() == 0 && this.mainMenu.CurrentSelection == null)
		{
			this.mainMenu.AutoSelect(this.mainMenu.GetButtons()[0]);
		}
	}
}
