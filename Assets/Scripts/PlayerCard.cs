// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using IconsServer;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : BaseGamewideOverlay
{
	public GameObject Container;

	public TextMeshProUGUI NameText;

	public TextMeshProUGUI LevelText;

	public TextMeshProUGUI CoinText;

	public Image IconImage;

	private CanvasGroup canvas;

	private bool currentVisibility = true;

	private Tweener alphaTween;

	private List<ScreenType> hideOnScreens = new List<ScreenType>
	{
		ScreenType.LoginScreen,
		ScreenType.LoadingBattle,
		ScreenType.BattleGUI,
		ScreenType.VictoryGUI,
		ScreenType.SelectStage,
		ScreenType.CreditsScreen
	};

	[Inject]
	public IIconsServerAPI serverAPI
	{
		get;
		set;
	}

	[Inject]
	public IUIAdapter uiAdapter
	{
		get;
		set;
	}

	[Inject]
	public IUserCurrencyModel userCurrencyModel
	{
		get;
		set;
	}

	[Inject]
	public IUserGlobalEquippedModel userGlobalModel
	{
		get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

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

	private void Start()
	{
		this.onUpdate();
	}

	private void onUpdate()
	{
		this.onScreenChanged();
		this.onUpdateName();
		this.onUpdateLevel();
		this.onUpdateCurrency();
		this.onUserEquippedUpdated();
	}

	private void onUpdateName()
	{
		this.NameText.text = this.serverAPI.Username;
	}

	private void onUpdateLevel()
	{
		this.LevelText.text = string.Empty;
	}

	private void onUpdateCurrency()
	{
		this.CoinText.text = this.userCurrencyModel.Spectra.ToString();
	}

	private void onUserEquippedUpdated()
	{
		EquipmentID equippedByType = this.userGlobalModel.GetEquippedByType(EquipmentTypes.PLAYER_ICON, 100);
		PlayerCardIconData playerIconFromItem = this.equipmentModel.GetPlayerIconFromItem(equippedByType);
		if (playerIconFromItem != null)
		{
			this.IconImage.sprite = playerIconFromItem.sprite;
		}
	}

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
			this.alphaTween = DOTween.To(new DOGetter<float>(this._setVisibleState_m__0), new DOSetter<float>(this._setVisibleState_m__1), endValue, duration).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.onTweenComplete));
		}
	}

	private void onTweenComplete()
	{
		if (!this.currentVisibility)
		{
			this.Container.SetActive(false);
		}
		this.killAlphaTween();
	}

	private void killAlphaTween()
	{
		TweenUtil.Destroy(ref this.alphaTween);
	}

	private bool shouldHide()
	{
		return this.hideOnScreens.Contains(this.uiAdapter.CurrentScreen);
	}

	private float _setVisibleState_m__0()
	{
		return this.canvas.alpha;
	}

	private void _setVisibleState_m__1(float valueIn)
	{
		this.canvas.alpha = valueIn;
	}
}
