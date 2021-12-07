using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A84 RID: 2692
public class VictoryScreen : GameScreen
{
	// Token: 0x170012A7 RID: 4775
	// (get) Token: 0x06004EBE RID: 20158 RVA: 0x0014A416 File Offset: 0x00148816
	// (set) Token: 0x06004EBF RID: 20159 RVA: 0x0014A41E File Offset: 0x0014881E
	[Inject]
	public ICustomLobbyController lobbyController { get; set; }

	// Token: 0x170012A8 RID: 4776
	// (get) Token: 0x06004EC0 RID: 20160 RVA: 0x0014A427 File Offset: 0x00148827
	// (set) Token: 0x06004EC1 RID: 20161 RVA: 0x0014A42F File Offset: 0x0014882F
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x170012A9 RID: 4777
	// (get) Token: 0x06004EC2 RID: 20162 RVA: 0x0014A438 File Offset: 0x00148838
	// (set) Token: 0x06004EC3 RID: 20163 RVA: 0x0014A440 File Offset: 0x00148840
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x170012AA RID: 4778
	// (get) Token: 0x06004EC4 RID: 20164 RVA: 0x0014A449 File Offset: 0x00148849
	// (set) Token: 0x06004EC5 RID: 20165 RVA: 0x0014A451 File Offset: 0x00148851
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x170012AB RID: 4779
	// (get) Token: 0x06004EC6 RID: 20166 RVA: 0x0014A45A File Offset: 0x0014885A
	// (set) Token: 0x06004EC7 RID: 20167 RVA: 0x0014A462 File Offset: 0x00148862
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x170012AC RID: 4780
	// (get) Token: 0x06004EC8 RID: 20168 RVA: 0x0014A46B File Offset: 0x0014886B
	// (set) Token: 0x06004EC9 RID: 20169 RVA: 0x0014A473 File Offset: 0x00148873
	[Inject]
	public IPostBattleFlow postBattleFlow { get; set; }

	// Token: 0x170012AD RID: 4781
	// (get) Token: 0x06004ECA RID: 20170 RVA: 0x0014A47C File Offset: 0x0014887C
	private bool playedOnlineGame
	{
		get
		{
			return base.battleServerAPI.IsConnected || this.victoryPayload.gamePayload.isOnlineGame;
		}
	}

	// Token: 0x06004ECB RID: 20171 RVA: 0x0014A4A4 File Offset: 0x001488A4
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
			foreach (TeamNum teamNum in values)
			{
				if (PlayerUtil.IsValidTeam(teamNum))
				{
					foreach (PlayerStats playerStats in this.victoryPayload.stats)
					{
						if (playerStats.playerInfo.team == teamNum && playerStats.playerInfo.type != PlayerType.None && !playerStats.playerInfo.isSpectator && PlayerUtil.IsValidPlayer(playerStats.playerInfo.playerNum))
						{
							list.Add(playerStats);
						}
					}
				}
			}
		}
		else
		{
			foreach (PlayerStats playerStats2 in this.victoryPayload.stats)
			{
				if (playerStats2.playerInfo.type != PlayerType.None && !playerStats2.playerInfo.isSpectator)
				{
					list.Add(playerStats2);
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

	// Token: 0x170012AE RID: 4782
	// (get) Token: 0x06004ECC RID: 20172 RVA: 0x0014A7D8 File Offset: 0x00148BD8
	private bool isSpectator
	{
		get
		{
			foreach (PlayerStats playerStats in this.victoryPayload.stats)
			{
				if (playerStats.playerInfo.userID == this.lobbyController.MyUserID)
				{
					Debug.Log("Found me " + playerStats.playerInfo.isSpectator);
					return playerStats.playerInfo.isSpectator;
				}
			}
			return false;
		}
	}

	// Token: 0x06004ECD RID: 20173 RVA: 0x0014A880 File Offset: 0x00148C80
	protected override void onTransitionBegin()
	{
		base.onTransitionBegin();
		this.playAnimation(0.07f);
	}

	// Token: 0x06004ECE RID: 20174 RVA: 0x0014A894 File Offset: 0x00148C94
	private void playAnimation(float delay)
	{
		DOTween.To(new DOGetter<float>(this.get_MaskHeight), delegate(float x)
		{
			this.MaskHeight = x;
		}, this.originalMaskHeight, 0.25f).SetEase(Ease.OutCirc).SetDelay(delay).OnComplete(new TweenCallback(this.maskTweenComplete));
	}

	// Token: 0x06004ECF RID: 20175 RVA: 0x0014A8E8 File Offset: 0x00148CE8
	private void maskTweenComplete()
	{
		this.TitleTransition = 1f;
		this.TitleGroup.alpha = 0f;
		DOTween.To(() => this.TitleGroup.alpha, delegate(float x)
		{
			this.TitleGroup.alpha = x;
		}, 1f, 0.5f).SetDelay(0.35f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.titleTweenComplete));
	}

	// Token: 0x06004ED0 RID: 20176 RVA: 0x0014A959 File Offset: 0x00148D59
	private void titleTweenComplete()
	{
		base.unlockInput();
		DOTween.To(() => this.ButtonGroup.alpha, delegate(float x)
		{
			this.ButtonGroup.alpha = x;
		}, 1f, 0.25f).SetEase(Ease.OutSine);
	}

	// Token: 0x170012AF RID: 4783
	// (get) Token: 0x06004ED1 RID: 20177 RVA: 0x0014A98F File Offset: 0x00148D8F
	// (set) Token: 0x06004ED2 RID: 20178 RVA: 0x0014A997 File Offset: 0x00148D97
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

	// Token: 0x170012B0 RID: 4784
	// (get) Token: 0x06004ED3 RID: 20179 RVA: 0x0014A9A8 File Offset: 0x00148DA8
	// (set) Token: 0x06004ED4 RID: 20180 RVA: 0x0014A9CD File Offset: 0x00148DCD
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

	// Token: 0x06004ED5 RID: 20181 RVA: 0x0014A9D8 File Offset: 0x00148DD8
	private void updateTitleTransition()
	{
		float titleTransition = this._titleTransition;
		this.TitleGroup.transform.localScale = new Vector3(titleTransition, titleTransition, titleTransition);
	}

	// Token: 0x06004ED6 RID: 20182 RVA: 0x0014AA04 File Offset: 0x00148E04
	private void setMaskHeight(float height)
	{
		this.PlacardAnchor.transform.SetParent(base.transform);
		Vector2 sizeDelta = this.PlacardMask.rectTransform.sizeDelta;
		sizeDelta.y = height;
		this.PlacardMask.rectTransform.sizeDelta = sizeDelta;
		this.PlacardAnchor.transform.SetParent(this.PlacardMask.transform);
	}

	// Token: 0x06004ED7 RID: 20183 RVA: 0x0014AA6C File Offset: 0x00148E6C
	protected override FirstSelectionInfo getInitialSelection()
	{
		return new FirstSelectionInfo(this.menuController, this.ContinueButton1);
	}

	// Token: 0x06004ED8 RID: 20184 RVA: 0x0014AA80 File Offset: 0x00148E80
	private void setupData()
	{
		this.announcerData = base.config.announcements;
		this.victoryPayload = (VictoryScreenPayload)this.payload;
		this.modeData = base.gameDataManager.GameModeData.GetDataByType(this.victoryPayload.gamePayload.battleConfig.mode);
	}

	// Token: 0x06004ED9 RID: 20185 RVA: 0x0014AADA File Offset: 0x00148EDA
	private void setTitle(string text)
	{
		this.TitleText.text = text;
		this.TitleShadow.text = text;
	}

	// Token: 0x06004EDA RID: 20186 RVA: 0x0014AAF4 File Offset: 0x00148EF4
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

	// Token: 0x06004EDB RID: 20187 RVA: 0x0014ABB4 File Offset: 0x00148FB4
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

	// Token: 0x06004EDC RID: 20188 RVA: 0x0014AC60 File Offset: 0x00149060
	public override void OnRightTriggerPressed()
	{
		this.victoryScene.OnRightTriggerPressed();
	}

	// Token: 0x06004EDD RID: 20189 RVA: 0x0014AC6D File Offset: 0x0014906D
	public override void OnLeftTriggerPressed()
	{
		this.victoryScene.OnLeftTriggerPressed();
	}

	// Token: 0x06004EDE RID: 20190 RVA: 0x0014AC7A File Offset: 0x0014907A
	public override void OnXButtonPressed()
	{
		this.victoryScene.OnXPressed();
	}

	// Token: 0x06004EDF RID: 20191 RVA: 0x0014AC87 File Offset: 0x00149087
	public override void OnYButtonPressed()
	{
		this.victoryScene.OnYPressed();
	}

	// Token: 0x06004EE0 RID: 20192 RVA: 0x0014AC94 File Offset: 0x00149094
	public override void OnZPressed()
	{
		this.victoryScene.OnZPressed();
	}

	// Token: 0x06004EE1 RID: 20193 RVA: 0x0014ACA1 File Offset: 0x001490A1
	private void autoContinue()
	{
		this.GoToNextScreen();
	}

	// Token: 0x06004EE2 RID: 20194 RVA: 0x0014ACA9 File Offset: 0x001490A9
	private void onContinue()
	{
		this.GoToNextScreen();
	}

	// Token: 0x06004EE3 RID: 20195 RVA: 0x0014ACB1 File Offset: 0x001490B1
	public override void GoToNextScreen()
	{
		this.GoToNextScreenUninterrupted();
	}

	// Token: 0x06004EE4 RID: 20196 RVA: 0x0014ACBC File Offset: 0x001490BC
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

	// Token: 0x170012B1 RID: 4785
	// (get) Token: 0x06004EE5 RID: 20197 RVA: 0x0014ADBB File Offset: 0x001491BB
	public override bool WaitForDrawCallback
	{
		get
		{
			return true;
		}
	}

	// Token: 0x04003366 RID: 13158
	public TextMeshProUGUI TitleText;

	// Token: 0x04003367 RID: 13159
	public TextMeshProUGUI TitleShadow;

	// Token: 0x04003368 RID: 13160
	public MenuItemButton ContinueButton1;

	// Token: 0x04003369 RID: 13161
	public GameObject PlacardPrefab;

	// Token: 0x0400336A RID: 13162
	public HorizontalLayoutGroup PlacardAnchor;

	// Token: 0x0400336B RID: 13163
	public TextMeshProUGUI SubtitleText;

	// Token: 0x0400336C RID: 13164
	public Mask PlacardMask;

	// Token: 0x0400336D RID: 13165
	public CanvasGroup TitleGroup;

	// Token: 0x0400336E RID: 13166
	public CanvasGroup ButtonGroup;

	// Token: 0x0400336F RID: 13167
	public HorizontalLayoutGroup NextButtonAligner;

	// Token: 0x04003370 RID: 13168
	private DateTime timeOpened;

	// Token: 0x04003371 RID: 13169
	private AnnouncementConfigData announcerData;

	// Token: 0x04003372 RID: 13170
	private GameModeData modeData;

	// Token: 0x04003373 RID: 13171
	private VictoryScreenPayload victoryPayload;

	// Token: 0x04003374 RID: 13172
	private MenuItemList menuController;

	// Token: 0x04003375 RID: 13173
	private float originalMaskHeight;

	// Token: 0x04003376 RID: 13174
	private float _titleTransition;

	// Token: 0x04003377 RID: 13175
	private GenericDialog spinny;

	// Token: 0x0400337E RID: 13182
	private VictoryScene3D victoryScene;
}
