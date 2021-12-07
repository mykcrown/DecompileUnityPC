using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using IconsServer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009BC RID: 2492
public class PlayerCard : BaseGamewideOverlay
{
	// Token: 0x1700106F RID: 4207
	// (get) Token: 0x06004563 RID: 17763 RVA: 0x00130C6F File Offset: 0x0012F06F
	// (set) Token: 0x06004564 RID: 17764 RVA: 0x00130C77 File Offset: 0x0012F077
	[Inject]
	public IIconsServerAPI serverAPI { get; set; }

	// Token: 0x17001070 RID: 4208
	// (get) Token: 0x06004565 RID: 17765 RVA: 0x00130C80 File Offset: 0x0012F080
	// (set) Token: 0x06004566 RID: 17766 RVA: 0x00130C88 File Offset: 0x0012F088
	[Inject]
	public IUIAdapter uiAdapter { get; set; }

	// Token: 0x17001071 RID: 4209
	// (get) Token: 0x06004567 RID: 17767 RVA: 0x00130C91 File Offset: 0x0012F091
	// (set) Token: 0x06004568 RID: 17768 RVA: 0x00130C99 File Offset: 0x0012F099
	[Inject]
	public IUserCurrencyModel userCurrencyModel { get; set; }

	// Token: 0x17001072 RID: 4210
	// (get) Token: 0x06004569 RID: 17769 RVA: 0x00130CA2 File Offset: 0x0012F0A2
	// (set) Token: 0x0600456A RID: 17770 RVA: 0x00130CAA File Offset: 0x0012F0AA
	[Inject]
	public IUserGlobalEquippedModel userGlobalModel { get; set; }

	// Token: 0x17001073 RID: 4211
	// (get) Token: 0x0600456B RID: 17771 RVA: 0x00130CB3 File Offset: 0x0012F0B3
	// (set) Token: 0x0600456C RID: 17772 RVA: 0x00130CBB File Offset: 0x0012F0BB
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17001074 RID: 4212
	// (get) Token: 0x0600456D RID: 17773 RVA: 0x00130CC4 File Offset: 0x0012F0C4
	// (set) Token: 0x0600456E RID: 17774 RVA: 0x00130CCC File Offset: 0x0012F0CC
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x0600456F RID: 17775 RVA: 0x00130CD8 File Offset: 0x0012F0D8
	[PostConstruct]
	public void Init()
	{
		this.canvas = base.GetComponent<CanvasGroup>();
		this.canvas.alpha = 0f;
		this.currentVisibility = false;
		base.signalBus.AddListener(UIScreenAdapter.SCREEN_CHANGED, new Action(this.onScreenChanged));
		base.signalBus.AddListener("AccountAPI.UPDATE", new Action(this.onUpdateName));
		base.signalBus.AddListener(UserCurrencyModel.UPDATED, new Action(this.onUpdateCurrency));
		base.signalBus.AddListener(UserGlobalEquippedModel.UPDATED, new Action(this.onUserEquippedUpdated));
		base.signalBus.AddListener(EquipmentModel.UPDATED, new Action(this.onUpdate));
		base.signalBus.AddListener(SteamManager.STEAM_INITIALIZED, new Action(this.onUpdateName));
		this.onUpdate();
	}

	// Token: 0x06004570 RID: 17776 RVA: 0x00130DB6 File Offset: 0x0012F1B6
	private void Start()
	{
		this.onUpdate();
	}

	// Token: 0x06004571 RID: 17777 RVA: 0x00130DBE File Offset: 0x0012F1BE
	private void onUpdate()
	{
		this.onScreenChanged();
		this.onUpdateName();
		this.onUpdateLevel();
		this.onUpdateCurrency();
		this.onUserEquippedUpdated();
	}

	// Token: 0x06004572 RID: 17778 RVA: 0x00130DDE File Offset: 0x0012F1DE
	private void onUpdateName()
	{
		this.NameText.text = this.serverAPI.Username;
	}

	// Token: 0x06004573 RID: 17779 RVA: 0x00130DF6 File Offset: 0x0012F1F6
	private void onUpdateLevel()
	{
		this.LevelText.text = string.Empty;
	}

	// Token: 0x06004574 RID: 17780 RVA: 0x00130E08 File Offset: 0x0012F208
	private void onUpdateCurrency()
	{
		this.CoinText.text = this.userCurrencyModel.Spectra.ToString();
	}

	// Token: 0x06004575 RID: 17781 RVA: 0x00130E3C File Offset: 0x0012F23C
	private void onUserEquippedUpdated()
	{
		EquipmentID equippedByType = this.userGlobalModel.GetEquippedByType(EquipmentTypes.PLAYER_ICON, 100);
		PlayerCardIconData playerIconFromItem = this.equipmentModel.GetPlayerIconFromItem(equippedByType);
		if (playerIconFromItem != null)
		{
			this.IconImage.sprite = playerIconFromItem.sprite;
		}
	}

	// Token: 0x06004576 RID: 17782 RVA: 0x00130E82 File Offset: 0x0012F282
	private void onScreenChanged()
	{
		if (this.shouldHide())
		{
			this.setVisibleState(false);
		}
		else
		{
			this.setVisibleState(true);
		}
	}

	// Token: 0x06004577 RID: 17783 RVA: 0x00130EA4 File Offset: 0x0012F2A4
	private void setVisibleState(bool state)
	{
		if (state != this.currentVisibility || this.alphaTween == null)
		{
			this.currentVisibility = state;
			float duration = 0.2f;
			float endValue = (float)((!this.currentVisibility) ? 0 : 1);
			if (this.currentVisibility)
			{
				this.Container.SetActive(true);
			}
			this.killAlphaTween();
			this.alphaTween = DOTween.To(() => this.canvas.alpha, delegate(float valueIn)
			{
				this.canvas.alpha = valueIn;
			}, endValue, duration).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.onTweenComplete));
		}
	}

	// Token: 0x06004578 RID: 17784 RVA: 0x00130F42 File Offset: 0x0012F342
	private void onTweenComplete()
	{
		if (!this.currentVisibility)
		{
			this.Container.SetActive(false);
		}
		this.killAlphaTween();
	}

	// Token: 0x06004579 RID: 17785 RVA: 0x00130F61 File Offset: 0x0012F361
	private void killAlphaTween()
	{
		TweenUtil.Destroy(ref this.alphaTween);
	}

	// Token: 0x0600457A RID: 17786 RVA: 0x00130F6E File Offset: 0x0012F36E
	private bool shouldHide()
	{
		return this.hideOnScreens.Contains(this.uiAdapter.CurrentScreen);
	}

	// Token: 0x04002E28 RID: 11816
	public GameObject Container;

	// Token: 0x04002E29 RID: 11817
	public TextMeshProUGUI NameText;

	// Token: 0x04002E2A RID: 11818
	public TextMeshProUGUI LevelText;

	// Token: 0x04002E2B RID: 11819
	public TextMeshProUGUI CoinText;

	// Token: 0x04002E2C RID: 11820
	public Image IconImage;

	// Token: 0x04002E2D RID: 11821
	private CanvasGroup canvas;

	// Token: 0x04002E2E RID: 11822
	private bool currentVisibility = true;

	// Token: 0x04002E2F RID: 11823
	private Tweener alphaTween;

	// Token: 0x04002E30 RID: 11824
	private List<ScreenType> hideOnScreens = new List<ScreenType>
	{
		ScreenType.LoginScreen,
		ScreenType.LoadingBattle,
		ScreenType.BattleGUI,
		ScreenType.VictoryGUI,
		ScreenType.SelectStage,
		ScreenType.CreditsScreen
	};
}
