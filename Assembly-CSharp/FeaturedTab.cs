using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000A09 RID: 2569
public class FeaturedTab : StoreTabElement
{
	// Token: 0x170011BF RID: 4543
	// (get) Token: 0x06004A64 RID: 19044 RVA: 0x0013F509 File Offset: 0x0013D909
	// (set) Token: 0x06004A65 RID: 19045 RVA: 0x0013F511 File Offset: 0x0013D911
	[Inject]
	public IFeaturedTabAPI api { get; set; }

	// Token: 0x170011C0 RID: 4544
	// (get) Token: 0x06004A66 RID: 19046 RVA: 0x0013F51A File Offset: 0x0013D91A
	// (set) Token: 0x06004A67 RID: 19047 RVA: 0x0013F522 File Offset: 0x0013D922
	[Inject]
	public IWindowDisplay windowDisplay { get; set; }

	// Token: 0x170011C1 RID: 4545
	// (get) Token: 0x06004A68 RID: 19048 RVA: 0x0013F52B File Offset: 0x0013D92B
	// (set) Token: 0x06004A69 RID: 19049 RVA: 0x0013F533 File Offset: 0x0013D933
	[Inject]
	public IBuyLootboxFlow buyLootBoxFlow { get; set; }

	// Token: 0x06004A6A RID: 19050 RVA: 0x0013F53C File Offset: 0x0013D93C
	[PostConstruct]
	public void Init()
	{
		base.listen(UserProAccountUnlockedModel.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	// Token: 0x06004A6B RID: 19051 RVA: 0x0013F55B File Offset: 0x0013D95B
	private void resetButtonMenu()
	{
		this.mainMenu = base.injector.GetInstance<MenuItemList>();
	}

	// Token: 0x06004A6C RID: 19052 RVA: 0x0013F56E File Offset: 0x0013D96E
	private void initMenu()
	{
		this.mainMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.mainMenu.Initialize();
	}

	// Token: 0x06004A6D RID: 19053 RVA: 0x0013F588 File Offset: 0x0013D988
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

	// Token: 0x06004A6E RID: 19054 RVA: 0x0013F620 File Offset: 0x0013DA20
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

	// Token: 0x06004A6F RID: 19055 RVA: 0x0013F6B7 File Offset: 0x0013DAB7
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

	// Token: 0x06004A70 RID: 19056 RVA: 0x0013F6DC File Offset: 0x0013DADC
	private void addMenuItem(MenuItemButton button, Action callback)
	{
		this.mainMenu.AddButton(button, callback);
		WavedashUIButton interactableButton = button.InteractableButton;
		interactableButton.OnSelectEvent = (Action<BaseEventData>)Delegate.Combine(interactableButton.OnSelectEvent, new Action<BaseEventData>(delegate(BaseEventData eventData)
		{
			button.transform.SetAsLastSibling();
		}));
	}

	// Token: 0x06004A71 RID: 19057 RVA: 0x0013F734 File Offset: 0x0013DB34
	private void Update()
	{
		this.ProAccountGroup.enabled = false;
		this.LootboxBundleGroup.enabled = false;
	}

	// Token: 0x06004A72 RID: 19058 RVA: 0x0013F74E File Offset: 0x0013DB4E
	private void foundersPackClicked()
	{
		if (!this.api.IsProAccountUnlocked())
		{
			this.api.BuyFoundersPack();
		}
	}

	// Token: 0x06004A73 RID: 19059 RVA: 0x0013F76B File Offset: 0x0013DB6B
	private void proAccountClicked()
	{
		if (!this.api.IsProAccountUnlocked())
		{
			this.api.BuyProAccount();
		}
	}

	// Token: 0x06004A74 RID: 19060 RVA: 0x0013F788 File Offset: 0x0013DB88
	private void lootbox25BundleCilcked()
	{
		this.buyLootBoxFlow.Start(this.api.GetLootbox25BundlePackageId(), null);
	}

	// Token: 0x06004A75 RID: 19061 RVA: 0x0013F7A1 File Offset: 0x0013DBA1
	private void lootbox55BundleClicked()
	{
		this.buyLootBoxFlow.Start(this.api.GetLootbox55BundlePackageId(), null);
	}

	// Token: 0x06004A76 RID: 19062 RVA: 0x0013F7BA File Offset: 0x0013DBBA
	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		if (base.allowInteraction())
		{
			this.syncButtonNavigation();
		}
	}

	// Token: 0x06004A77 RID: 19063 RVA: 0x0013F7D4 File Offset: 0x0013DBD4
	private void syncButtonNavigation()
	{
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode && this.windowDisplay.GetWindowCount() == 0 && this.mainMenu.CurrentSelection == null)
		{
			this.mainMenu.AutoSelect(this.mainMenu.GetButtons()[0]);
		}
	}

	// Token: 0x04003101 RID: 12545
	public MenuItemButton FoundersPackButton;

	// Token: 0x04003102 RID: 12546
	public MenuItemButton ProAccountButton;

	// Token: 0x04003103 RID: 12547
	public MenuItemButton Lootbox25BundleButton;

	// Token: 0x04003104 RID: 12548
	public MenuItemButton Lootbox55BundleButton;

	// Token: 0x04003105 RID: 12549
	public HorizontalLayoutGroup ProAccountGroup;

	// Token: 0x04003106 RID: 12550
	public HorizontalLayoutGroup LootboxBundleGroup;

	// Token: 0x04003107 RID: 12551
	public TextMeshProUGUI FoundersPrice;

	// Token: 0x04003108 RID: 12552
	public TextMeshProUGUI ProAccountPrice;

	// Token: 0x04003109 RID: 12553
	public TextMeshProUGUI Lootbox25BundlePrice;

	// Token: 0x0400310A RID: 12554
	public TextMeshProUGUI Lootbox55BundlePrice;

	// Token: 0x0400310B RID: 12555
	private MenuItemList mainMenu;
}
