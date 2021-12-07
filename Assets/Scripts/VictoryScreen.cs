// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : GameScreen
{
	public TextMeshProUGUI TitleText;

	public TextMeshProUGUI TitleShadow;

	public MenuItemButton ContinueButton1;

	public GameObject PlacardPrefab;

	public HorizontalLayoutGroup PlacardAnchor;

	public TextMeshProUGUI SubtitleText;

	public Mask PlacardMask;

	public CanvasGroup TitleGroup;

	public CanvasGroup ButtonGroup;

	public HorizontalLayoutGroup NextButtonAligner;

	private DateTime timeOpened;

	private AnnouncementConfigData announcerData;

	private GameModeData modeData;

	private VictoryScreenPayload victoryPayload;

	private MenuItemList menuController;

	private float originalMaskHeight;

	private float _titleTransition;

	private GenericDialog spinny;

	private VictoryScene3D victoryScene;

	[Inject]
	public ICustomLobbyController lobbyController
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		get;
		set;
	}

	[Inject]
	public IRichPresence richPresence
	{
		get;
		set;
	}

	[Inject]
	public IEnterNewGame enterNewGame
	{
		get;
		set;
	}

	[Inject]
	public IPostBattleFlow postBattleFlow
	{
		get;
		set;
	}

	private bool playedOnlineGame
	{
		get
		{
			return base.battleServerAPI.IsConnected || this.victoryPayload.gamePayload.isOnlineGame;
		}
	}

	private bool isSpectator
	{
		get
		{
			foreach (PlayerStats current in this.victoryPayload.stats)
			{
				if (current.playerInfo.userID == this.lobbyController.MyUserID)
				{
					UnityEngine.Debug.Log("Found me " + current.playerInfo.isSpectator);
					return current.playerInfo.isSpectator;
				}
			}
			return false;
		}
	}

	public float TitleTransition
	{
		get
		{
			return this._titleTransition;
		}
		set
		{
			this._titleTransition = value;
			this.updateTitleTransition();
		}
	}

	public float MaskHeight
	{
		get
		{
			return this.PlacardMask.rectTransform.sizeDelta.y;
		}
		set
		{
			this.setMaskHeight(value);
		}
	}

	public override bool WaitForDrawCallback
	{
		get
		{
			return true;
		}
	}

	protected override void postStartAndPayload()
	{
		base.postStartAndPayload();
		this.victoryScene = base.uiAdapter.GetUIScene<VictoryScene3D>();
		base.lockInput();
		this.timeOpened = DateTime.Now;
		this.setupData();
		this.menuController = base.createMenuController();
		this.menuController.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.menuController.AddButton(this.ContinueButton1, new Action(this.onContinue));
		this.ContinueButton1.transform.SetParent(this.NextButtonAligner.transform);
		this.menuController.Initialize();
		this.setTitle(ScreenTextHelper.GetTitleText(this.victoryPayload, base.localization, this.modeData));
		this.SubtitleText.text = base.localization.GetText("gameRules.subtitle." + this.victoryPayload.gamePayload.battleConfig.mode);
		List<PlayerStats> list = new List<PlayerStats>();
		if (this.modeData.settings.sortVictorsByTeam)
		{
			TeamNum[] values = EnumUtil.GetValues<TeamNum>();
			TeamNum[] array = values;
			for (int i = 0; i < array.Length; i++)
			{
				TeamNum teamNum = array[i];
				if (PlayerUtil.IsValidTeam(teamNum))
				{
					foreach (PlayerStats current in this.victoryPayload.stats)
					{
						if (current.playerInfo.team == teamNum && current.playerInfo.type != PlayerType.None && !current.playerInfo.isSpectator && PlayerUtil.IsValidPlayer(current.playerInfo.playerNum))
						{
							list.Add(current);
						}
					}
				}
			}
		}
		else
		{
			foreach (PlayerStats current2 in this.victoryPayload.stats)
			{
				if (current2.playerInfo.type != PlayerType.None && !current2.playerInfo.isSpectator)
				{
					list.Add(current2);
				}
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			this.createVictoryPlacard(list[j], j, list.Count);
		}
		if (this.PlacardAnchor.transform.childCount > 4)
		{
			this.PlacardAnchor.spacing = 315f;
		}
		if (this.playedOnlineGame && this.isSpectator)
		{
			this.timer.CancelTimeout(new Action(this.autoContinue));
			this.timer.SetTimeout(5000, new Action(this.autoContinue));
		}
		this.originalMaskHeight = this.MaskHeight;
		this.setMaskHeight(0f);
		this.TitleTransition = 0f;
		this.ButtonGroup.alpha = 0f;
		base.onDrawComplete();
	}

	protected override void onTransitionBegin()
	{
		base.onTransitionBegin();
		this.playAnimation(0.07f);
	}

	private void playAnimation(float delay)
	{
		DOTween.To(new DOGetter<float>(this.get_MaskHeight), new DOSetter<float>(this._playAnimation_m__0), this.originalMaskHeight, 0.25f).SetEase(Ease.OutCirc).SetDelay(delay).OnComplete(new TweenCallback(this.maskTweenComplete));
	}

	private void maskTweenComplete()
	{
		this.TitleTransition = 1f;
		this.TitleGroup.alpha = 0f;
		DOTween.To(new DOGetter<float>(this._maskTweenComplete_m__1), new DOSetter<float>(this._maskTweenComplete_m__2), 1f, 0.5f).SetDelay(0.35f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.titleTweenComplete));
	}

	private void titleTweenComplete()
	{
		base.unlockInput();
		DOTween.To(new DOGetter<float>(this._titleTweenComplete_m__3), new DOSetter<float>(this._titleTweenComplete_m__4), 1f, 0.25f).SetEase(Ease.OutSine);
	}

	private void updateTitleTransition()
	{
		float titleTransition = this._titleTransition;
		this.TitleGroup.transform.localScale = new Vector3(titleTransition, titleTransition, titleTransition);
	}

	private void setMaskHeight(float height)
	{
		this.PlacardAnchor.transform.SetParent(base.transform);
		Vector2 sizeDelta = this.PlacardMask.rectTransform.sizeDelta;
		sizeDelta.y = height;
		this.PlacardMask.rectTransform.sizeDelta = sizeDelta;
		this.PlacardAnchor.transform.SetParent(this.PlacardMask.transform);
	}

	protected override FirstSelectionInfo getInitialSelection()
	{
		return new FirstSelectionInfo(this.menuController, this.ContinueButton1);
	}

	private void setupData()
	{
		this.announcerData = base.config.announcements;
		this.victoryPayload = (VictoryScreenPayload)this.payload;
		this.modeData = base.gameDataManager.GameModeData.GetDataByType(this.victoryPayload.gamePayload.battleConfig.mode);
	}

	private void setTitle(string text)
	{
		this.TitleText.text = text;
		this.TitleShadow.text = text;
	}

	private void createVictoryPlacard(PlayerStats player, int index, int totalPlayers)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PlacardPrefab);
		gameObject.transform.SetParent(this.PlacardAnchor.transform, false);
		CharacterVictoryDisplay component = gameObject.GetComponent<CharacterVictoryDisplay>();
		base.injector.Inject(component);
		string playerNametag = PlayerUtil.GetPlayerNametag(base.localization, player.playerInfo, this.playedOnlineGame);
		string displayName = this.characterDataHelper.GetDisplayName(player.playerInfo.characterID);
		component.Init(player, index, this.modeData, this.characterDataHelper.GetSkinDefinition(player.playerInfo.characterID, player.playerInfo.skinKey), this.victoryPayload.victors.Contains(player.playerInfo.playerNum), playerNametag, displayName, totalPlayers);
	}

	private void onInstantRematch()
	{
		int num = (this.announcerData == null) ? 1 : this.announcerData.SaltyRunbackSeconds;
		if (DateTime.Now.TimeOfDay.TotalSeconds - this.timeOpened.TimeOfDay.TotalSeconds <= (double)num)
		{
			base.events.Broadcast(new PlayAnnouncementCommand(AnnouncementType.Victory_SaltyRunback));
		}
		this.enterNewGame.InitPayload(GameStartType.FreePlay, this.victoryPayload.gamePayload);
		base.events.Broadcast(new LoadScreenCommand(ScreenType.LoadingBattle, null, ScreenUpdateType.Next));
		this.richPresence.SetPresence("Loading", null, null, null);
	}

	public override void OnRightTriggerPressed()
	{
		this.victoryScene.OnRightTriggerPressed();
	}

	public override void OnLeftTriggerPressed()
	{
		this.victoryScene.OnLeftTriggerPressed();
	}

	public override void OnXButtonPressed()
	{
		this.victoryScene.OnXPressed();
	}

	public override void OnYButtonPressed()
	{
		this.victoryScene.OnYPressed();
	}

	public override void OnZPressed()
	{
		this.victoryScene.OnZPressed();
	}

	private void autoContinue()
	{
		this.GoToNextScreen();
	}

	private void onContinue()
	{
		this.GoToNextScreen();
	}

	public override void GoToNextScreen()
	{
		this.GoToNextScreenUninterrupted();
	}

	private void GoToNextScreenUninterrupted()
	{
		this.timer.CancelTimeout(new Action(this.autoContinue));
		VictoryScreenPayload victoryScreenPayload = this.payload as VictoryScreenPayload;
		if (victoryScreenPayload.gamePayload != null)
		{
			if (victoryScreenPayload.gamePayload.isReplay || (this.playedOnlineGame && victoryScreenPayload.wasForfeited))
			{
				this.richPresence.SetPresence(null, null, null, null);
				base.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Next));
				return;
			}
			if (this.playedOnlineGame)
			{
				if (base.gameDataManager.IsFeatureEnabled(FeatureID.UnlockEverythingOnline))
				{
					this.postBattleFlow.ExitPostGame(victoryScreenPayload);
				}
				else
				{
					base.events.Broadcast(new LoadScreenCommand(ScreenType.PlayerProgression, victoryScreenPayload, ScreenUpdateType.Next));
				}
				return;
			}
		}
		this.enterNewGame.InitPayload(GameStartType.FreePlay, victoryScreenPayload.gamePayload);
		this.richPresence.SetPresence("InCharacterSelect", null, null, null);
		base.events.Broadcast(new LoadScreenCommand(ScreenType.CharacterSelect, null, ScreenUpdateType.Next));
	}

	private void _playAnimation_m__0(float x)
	{
		this.MaskHeight = x;
	}

	private float _maskTweenComplete_m__1()
	{
		return this.TitleGroup.alpha;
	}

	private void _maskTweenComplete_m__2(float x)
	{
		this.TitleGroup.alpha = x;
	}

	private float _titleTweenComplete_m__3()
	{
		return this.ButtonGroup.alpha;
	}

	private void _titleTweenComplete_m__4(float x)
	{
		this.ButtonGroup.alpha = x;
	}
}
