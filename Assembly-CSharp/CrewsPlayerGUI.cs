using System;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008B4 RID: 2228
public class CrewsPlayerGUI : GameBehavior, ICrewsPlayerUI
{
	// Token: 0x17000D93 RID: 3475
	// (get) Token: 0x060037FB RID: 14331 RVA: 0x001067F3 File Offset: 0x00104BF3
	// (set) Token: 0x060037FC RID: 14332 RVA: 0x001067FB File Offset: 0x00104BFB
	[Inject]
	public GameDataManager gameData { private get; set; }

	// Token: 0x17000D94 RID: 3476
	// (get) Token: 0x060037FD RID: 14333 RVA: 0x00106804 File Offset: 0x00104C04
	// (set) Token: 0x060037FE RID: 14334 RVA: 0x0010680C File Offset: 0x00104C0C
	[Inject]
	public ILocalization localization { private get; set; }

	// Token: 0x17000D95 RID: 3477
	// (get) Token: 0x060037FF RID: 14335 RVA: 0x00106815 File Offset: 0x00104C15
	// (set) Token: 0x06003800 RID: 14336 RVA: 0x0010681D File Offset: 0x00104C1D
	[Inject]
	public ICharacterDataHelper characterDataHelper { private get; set; }

	// Token: 0x17000D96 RID: 3478
	// (get) Token: 0x06003801 RID: 14337 RVA: 0x00106826 File Offset: 0x00104C26
	// (set) Token: 0x06003802 RID: 14338 RVA: 0x0010682E File Offset: 0x00104C2E
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x17000D97 RID: 3479
	// (get) Token: 0x06003803 RID: 14339 RVA: 0x00106837 File Offset: 0x00104C37
	private PlayerNum playerNum
	{
		get
		{
			return (this.playerInfo == null) ? PlayerNum.None : this.playerInfo.playerNum;
		}
	}

	// Token: 0x17000D98 RID: 3480
	// (get) Token: 0x06003804 RID: 14340 RVA: 0x00106856 File Offset: 0x00104C56
	private PlayerType type
	{
		get
		{
			return (this.playerInfo == null) ? PlayerType.None : this.playerInfo.type;
		}
	}

	// Token: 0x17000D99 RID: 3481
	// (get) Token: 0x06003805 RID: 14341 RVA: 0x00106874 File Offset: 0x00104C74
	private TeamNum team
	{
		get
		{
			return (this.playerInfo == null) ? TeamNum.None : this.playerInfo.team;
		}
	}

	// Token: 0x17000D9A RID: 3482
	// (get) Token: 0x06003806 RID: 14342 RVA: 0x00106893 File Offset: 0x00104C93
	public bool Visible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003807 RID: 14343 RVA: 0x00106898 File Offset: 0x00104C98
	public override void Awake()
	{
		base.Awake();
		if (base.events != null)
		{
			this.didSubscribe = true;
			base.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
			base.events.Subscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
			base.events.Subscribe(typeof(TagInPlayerEvent), new Events.EventHandler(this.onTagIn));
			base.events.Subscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		}
	}

	// Token: 0x06003808 RID: 14344 RVA: 0x00106944 File Offset: 0x00104D44
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.didSubscribe)
		{
			base.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
			base.events.Unsubscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
			base.events.Unsubscribe(typeof(TagInPlayerEvent), new Events.EventHandler(this.onTagIn));
			base.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		}
	}

	// Token: 0x06003809 RID: 14345 RVA: 0x001069E8 File Offset: 0x00104DE8
	public void Initialize(BattleSettings config, PlayerSelectionInfo playerInfo, TeamNum team, CrewsGUISide side)
	{
		this.side = side;
		this.playerInfo = playerInfo;
		this.assignTagAssistButtons();
		this.assistButtonPosition = this.assistButton.transform.localPosition;
		this.tagButtonPosition = this.tagButton.transform.localPosition;
		this.tagButton.gameObject.SetActive(false);
		this.assistButton.gameObject.SetActive(false);
		GameModeData dataByType = this.gameData.GameModeData.GetDataByType(config.mode);
		this.teamColor = PlayerUtil.GetUIColor(playerInfo, dataByType.settings.usesTeams);
		this.AssistActiveText.text = this.localization.GetText("ui.crewsHud.assistActive");
		this.alignSideContainer();
		this.playersRequestHandler(base.gameController.currentGame.PlayerReferences);
		this.playerSpawnerHandler(base.gameController.currentGame.PlayerSpawner);
		this.redraw();
		this.updateAssistActive();
		this.createPortrait(PortraitMode.GREY, PortraitBgMode.BLUE);
		this.createPortrait(PortraitMode.GREY, PortraitBgMode.INACTIVE);
		this.createPortrait(PortraitMode.GREY, PortraitBgMode.RED);
		this.createPortrait(PortraitMode.NORMAL, PortraitBgMode.BLUE);
		this.createPortrait(PortraitMode.NORMAL, PortraitBgMode.INACTIVE);
		this.createPortrait(PortraitMode.NORMAL, PortraitBgMode.RED);
		this.Portrait.gameObject.SetActive(false);
		this.PortraitBackground.gameObject.SetActive(false);
		this.updatePortrait();
		this.RawImagePortrait.gameObject.SetActive(true);
	}

	// Token: 0x0600380A RID: 14346 RVA: 0x00106B48 File Offset: 0x00104F48
	private void createPortrait(PortraitMode mode, PortraitBgMode bgMode)
	{
		PortraitDefinition portraitDefinition = new PortraitDefinition(mode, bgMode);
		this.updatePortraitDisplay(portraitDefinition);
		portraitDefinition.texture = this.combineImages(this.PortraitBackground.mainTexture as Texture2D, this.Portrait.mainTexture as Texture2D);
		this.portraitTextures.Add(portraitDefinition);
	}

	// Token: 0x0600380B RID: 14347 RVA: 0x00106B9C File Offset: 0x00104F9C
	private Texture2D combineImages(Texture2D BackgroundTex, Texture2D Overlay)
	{
		Texture2D texture2D = new Texture2D(BackgroundTex.width, BackgroundTex.height, TextureFormat.ARGB32, false);
		Vector2 vector = new Vector2((float)((texture2D.width - Overlay.width) / 2), (float)((texture2D.height - Overlay.height) / 2));
		texture2D.SetPixels(BackgroundTex.GetPixels());
		for (int i = 0; i < Overlay.height; i++)
		{
			for (int j = 0; j < Overlay.width; j++)
			{
				Color b = Overlay.GetPixel(j, i) * Overlay.GetPixel(j, i).a;
				Color a = texture2D.GetPixel(j + (int)vector.x, i + (int)vector.y) * (1f - b.a);
				texture2D.SetPixel(j + (int)vector.x, i + (int)vector.y, a + b);
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x0600380C RID: 14348 RVA: 0x00106C98 File Offset: 0x00105098
	private void assignTagAssistButtons()
	{
		if (this.gameData.ConfigData.uiuxSettings.crewsGuiType == CrewBattleGuiType.VERSION1)
		{
			this.tagButton = this.TagButton;
			this.assistButton = this.AssistButton;
		}
		else if (this.side == CrewsGUISide.LEFT)
		{
			this.tagButton = this.TagButtonRight;
			this.assistButton = this.AssistButtonRight;
		}
		else
		{
			this.tagButton = this.TagButton;
			this.assistButton = this.AssistButton;
		}
	}

	// Token: 0x0600380D RID: 14349 RVA: 0x00106D1C File Offset: 0x0010511C
	private void alignSideContainer()
	{
		if (this.gameData.ConfigData.uiuxSettings.crewsGuiType == CrewBattleGuiType.VERSION1)
		{
			float num = -Mathf.Abs(this.SideContainer.transform.position.x);
			float num2 = Mathf.Abs(this.SideContainerBackground.transform.localScale.x);
			if (this.side == CrewsGUISide.RIGHT)
			{
				num = -num;
				num2 = -num2;
			}
			Vector3 position = this.SideContainer.transform.position;
			position.x = num;
			this.SideContainer.transform.position = position;
			Vector3 localScale = this.SideContainerBackground.transform.localScale;
			localScale.x = num2;
			this.SideContainerBackground.transform.localScale = localScale;
		}
		else if (this.gameData.ConfigData.uiuxSettings.crewsGuiType == CrewBattleGuiType.VERSION2)
		{
			this.TagInLeft.gameObject.SetActive(false);
			this.AssistLeft.gameObject.SetActive(false);
			this.TagInRight.gameObject.SetActive(false);
			this.AssistRight.gameObject.SetActive(false);
			float num3 = -Mathf.Abs(this.SideContainer.transform.position.x);
			float num4 = Mathf.Abs(this.SideContainerBackground.transform.localScale.x);
			if (this.side == CrewsGUISide.LEFT)
			{
				num3 = -num3;
				num4 = -num4;
				this.TagInRight.gameObject.SetActive(true);
				this.AssistRight.gameObject.SetActive(true);
			}
			else
			{
				this.TagInLeft.gameObject.SetActive(true);
				this.AssistLeft.gameObject.SetActive(true);
			}
			Vector3 position2 = this.SideContainer.transform.position;
			position2.x = num3;
			this.SideContainer.transform.position = position2;
			Vector3 localScale2 = this.SideContainer.transform.localScale;
			localScale2.x = num4;
			this.SideContainer.transform.localScale = localScale2;
			localScale2 = this.FlipContainer.transform.localScale;
			localScale2.x = num4;
			this.FlipContainer.transform.localScale = localScale2;
		}
	}

	// Token: 0x0600380E RID: 14350 RVA: 0x00106F74 File Offset: 0x00105374
	private void onGameInit(GameEvent message)
	{
		this.redraw();
	}

	// Token: 0x0600380F RID: 14351 RVA: 0x00106F7C File Offset: 0x0010537C
	public void TickFrame()
	{
		bool flag = false;
		if (this.player.IsTemporary)
		{
			this.stringBuilder.Remove(0, this.stringBuilder.Length);
			TimeUtil.FormatTimeByFrames(this.player.Controller.TemporaryDurationFrames, this.stringBuilder);
			this.TimerDisplay.text = this.stringBuilder.ToString();
			flag |= true;
		}
		if (this.spawner.IsRespawning(this.playerNum))
		{
			flag |= true;
			int num = Mathf.CeilToInt((float)this.spawner.GetRespawnDelayFrames(this.player.PlayerNum) / 60f);
			if (num != this.cachedLastRespawnTime)
			{
				this.TimerDisplay.text = this.localization.GetText("ui.crewsHud.secondsTime", num.ToString());
				this.cachedLastRespawnTime = num;
			}
		}
		if (this.spawner.CanTagIn(this.playerNum))
		{
			this.updateButtonMode(CrewsPlayerGUIButtonMode.TAG);
		}
		else if (this.spawner.CanAssist(this.player))
		{
			this.updateButtonMode(CrewsPlayerGUIButtonMode.ASSIST);
		}
		else
		{
			this.updateButtonMode(CrewsPlayerGUIButtonMode.NONE);
		}
		this.TimerDisplay.gameObject.SetActive(false);
		this.updatePortrait();
	}

	// Token: 0x06003810 RID: 14352 RVA: 0x001070C4 File Offset: 0x001054C4
	private void updateButtonMode(CrewsPlayerGUIButtonMode mode)
	{
		if (this.currentButtonMode != mode)
		{
			CrewsPlayerGUIButtonMode crewsPlayerGUIButtonMode = this.currentButtonMode;
			this.currentButtonMode = mode;
			if (crewsPlayerGUIButtonMode == CrewsPlayerGUIButtonMode.NEEDS_INIT)
			{
				this.killTagTween();
				this.tagButton.gameObject.SetActive(false);
				this.killAssistTween();
				this.assistButton.gameObject.SetActive(false);
			}
			else if (crewsPlayerGUIButtonMode == CrewsPlayerGUIButtonMode.ASSIST)
			{
				this.animateOutAssist();
			}
			else if (crewsPlayerGUIButtonMode == CrewsPlayerGUIButtonMode.TAG)
			{
				this.animateOutTagIn();
			}
			if (this.currentButtonMode == CrewsPlayerGUIButtonMode.ASSIST)
			{
				this.animateInAssistButton();
			}
			else if (this.currentButtonMode == CrewsPlayerGUIButtonMode.TAG)
			{
				this.animateInTagButton();
			}
			if (this.currentButtonMode == CrewsPlayerGUIButtonMode.NONE)
			{
				this.PortraitGroup.alpha = 0.5f;
			}
			else
			{
				this.PortraitGroup.alpha = 1f;
			}
		}
	}

	// Token: 0x06003811 RID: 14353 RVA: 0x001071A0 File Offset: 0x001055A0
	private void animateOutTagIn()
	{
		this.killTagTween();
		this._tagTween = DOTween.To(() => this.tagButton.alpha, delegate(float valueIn)
		{
			this.tagButton.alpha = valueIn;
		}, 0f, 0.1f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killTagTween));
	}

	// Token: 0x06003812 RID: 14354 RVA: 0x001071F8 File Offset: 0x001055F8
	private void animateOutAssist()
	{
		this.killAssistTween();
		this._assistTween = DOTween.To(() => this.assistButton.alpha, delegate(float valueIn)
		{
			this.assistButton.alpha = valueIn;
		}, 0f, 0.1f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killAssistTween));
	}

	// Token: 0x06003813 RID: 14355 RVA: 0x0010724F File Offset: 0x0010564F
	private bool reverseButtonAnimate()
	{
		return this.gameData.ConfigData.uiuxSettings.crewsGuiType == CrewBattleGuiType.VERSION2 && this.side == CrewsGUISide.RIGHT;
	}

	// Token: 0x06003814 RID: 14356 RVA: 0x00107280 File Offset: 0x00105680
	private void animateInTagButton()
	{
		this.killTagTween();
		this.tagButton.gameObject.SetActive(true);
		Vector3 localPosition = this.tagButtonPosition;
		if (this.reverseButtonAnimate())
		{
			localPosition.x += this.ButtonWidth;
		}
		else
		{
			localPosition.x -= this.ButtonWidth;
		}
		this.tagButton.transform.localPosition = localPosition;
		this.tagButton.alpha = 1f;
		this._tagTween = DOTween.To(() => this.tagButton.transform.localPosition, delegate(Vector3 valueIn)
		{
			this.tagButton.transform.localPosition = valueIn;
		}, this.tagButtonPosition, 0.35f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killTagTween));
	}

	// Token: 0x06003815 RID: 14357 RVA: 0x0010734C File Offset: 0x0010574C
	private void animateInAssistButton()
	{
		this.killAssistTween();
		this.assistButton.gameObject.SetActive(true);
		Vector3 localPosition = this.assistButtonPosition;
		if (this.reverseButtonAnimate())
		{
			localPosition.x += this.ButtonWidth;
		}
		else
		{
			localPosition.x -= this.ButtonWidth;
		}
		this.assistButton.transform.localPosition = localPosition;
		this.assistButton.alpha = 1f;
		this._assistTween = DOTween.To(() => this.assistButton.transform.localPosition, delegate(Vector3 valueIn)
		{
			this.assistButton.transform.localPosition = valueIn;
		}, this.assistButtonPosition, 0.35f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killAssistTween));
	}

	// Token: 0x06003816 RID: 14358 RVA: 0x00107415 File Offset: 0x00105815
	private void killAssistTween()
	{
		TweenUtil.Destroy(ref this._assistTween);
	}

	// Token: 0x06003817 RID: 14359 RVA: 0x00107422 File Offset: 0x00105822
	private void killTagTween()
	{
		TweenUtil.Destroy(ref this._tagTween);
	}

	// Token: 0x06003818 RID: 14360 RVA: 0x0010742F File Offset: 0x0010582F
	private PortraitMode getPortraitMode()
	{
		if (this.player.IsEliminated)
		{
			return PortraitMode.GREY;
		}
		return PortraitMode.NORMAL;
	}

	// Token: 0x06003819 RID: 14361 RVA: 0x00107444 File Offset: 0x00105844
	private PortraitBgMode getPortraitBgMode()
	{
		PlayerNum primaryPlayer = this.spawner.GetPrimaryPlayer(this.team);
		if (primaryPlayer != this.playerNum)
		{
			return PortraitBgMode.INACTIVE;
		}
		if (this.teamColor == UIColor.Blue)
		{
			return PortraitBgMode.BLUE;
		}
		return PortraitBgMode.RED;
	}

	// Token: 0x0600381A RID: 14362 RVA: 0x00107480 File Offset: 0x00105880
	private void updatePortraitDisplay(PortraitDefinition def)
	{
		SkinData preloadedSkinData = this.skinDataManager.GetPreloadedSkinData(this.characterDataHelper.GetSkinDefinition(this.playerInfo.characterID, this.playerInfo.skinKey));
		PortraitMode mode = def.mode;
		if (mode != PortraitMode.GREY)
		{
			if (mode == PortraitMode.NORMAL)
			{
				this.Portrait.sprite = preloadedSkinData.battlePortrait;
			}
		}
		else
		{
			this.Portrait.sprite = preloadedSkinData.battlePortraitGrey;
		}
		PortraitBgMode bgMode = def.bgMode;
		if (bgMode != PortraitBgMode.BLUE)
		{
			if (bgMode != PortraitBgMode.RED)
			{
				if (bgMode == PortraitBgMode.INACTIVE)
				{
					this.PortraitBackground.sprite = this.InactiveSprite;
				}
			}
			else
			{
				this.PortraitBackground.sprite = this.RedTeamSprite;
			}
		}
		else
		{
			this.PortraitBackground.sprite = this.BlueTeamSprite;
		}
	}

	// Token: 0x0600381B RID: 14363 RVA: 0x00107561 File Offset: 0x00105961
	public void setPosition(float x, float y)
	{
		base.transform.localPosition = new Vector3(x, y);
	}

	// Token: 0x0600381C RID: 14364 RVA: 0x00107578 File Offset: 0x00105978
	private void onEngagementStateChanged(GameEvent message)
	{
		PlayerEngagementStateChangedEvent playerEngagementStateChangedEvent = message as PlayerEngagementStateChangedEvent;
		if (playerEngagementStateChangedEvent.playerNum == this.playerNum)
		{
			this.redraw();
			this.updateAssistActive();
		}
	}

	// Token: 0x0600381D RID: 14365 RVA: 0x001075AC File Offset: 0x001059AC
	private void updateAssistActive()
	{
		bool active = false;
		switch (this.player.EngagementState)
		{
		case PlayerEngagementState.Primary:
			active = false;
			break;
		case PlayerEngagementState.Benched:
			active = false;
			break;
		case PlayerEngagementState.Temporary:
			active = true;
			break;
		case PlayerEngagementState.Disconnected:
			active = false;
			break;
		}
		this.AssistActiveContainer.SetActive(active);
	}

	// Token: 0x0600381E RID: 14366 RVA: 0x00107610 File Offset: 0x00105A10
	private void onTagIn(GameEvent message)
	{
		TagInPlayerEvent tagInPlayerEvent = message as TagInPlayerEvent;
		if (tagInPlayerEvent.taggedPlayerNum == this.playerNum)
		{
			this.redraw();
		}
	}

	// Token: 0x0600381F RID: 14367 RVA: 0x0010763B File Offset: 0x00105A3B
	private void onCharacterDeath(GameEvent message)
	{
		this.redraw();
	}

	// Token: 0x06003820 RID: 14368 RVA: 0x00107644 File Offset: 0x00105A44
	private void redraw()
	{
		this.AssistText.text = this.spawner.GetAssistsRemaining(this.playerNum).ToString();
		this.StockText.text = this.player.Lives.ToString();
		this.updatePortrait();
	}

	// Token: 0x06003821 RID: 14369 RVA: 0x001076A8 File Offset: 0x00105AA8
	private void updatePortrait()
	{
		PortraitBgMode portraitBgMode = this.getPortraitBgMode();
		PortraitMode portraitMode = this.getPortraitMode();
		foreach (PortraitDefinition portraitDefinition in this.portraitTextures)
		{
			if (portraitDefinition.mode == portraitMode && portraitDefinition.bgMode == portraitBgMode)
			{
				this.RawImagePortrait.texture = portraitDefinition.texture;
			}
		}
	}

	// Token: 0x06003822 RID: 14370 RVA: 0x00107734 File Offset: 0x00105B34
	private void playerSpawnerHandler(PlayerSpawner playerSpawner)
	{
		this.spawner = (playerSpawner as IBenchedPlayerSpawner);
	}

	// Token: 0x06003823 RID: 14371 RVA: 0x00107744 File Offset: 0x00105B44
	private void playersRequestHandler(List<PlayerReference> players)
	{
		for (int i = 0; i < players.Count; i++)
		{
			if (players[i].PlayerNum == this.playerNum)
			{
				this.player = players[i];
				break;
			}
		}
		if (!this.Visible)
		{
			base.gameObject.SetActive(false);
		}
		this.PlayerName.text = PlayerUtil.GetPlayerNametag(this.localization, this.player.Controller);
	}

	// Token: 0x17000D9B RID: 3483
	// (get) Token: 0x06003824 RID: 14372 RVA: 0x001077C9 File Offset: 0x00105BC9
	public Transform Transform
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x04002636 RID: 9782
	public TextMeshProUGUI PlayerName;

	// Token: 0x04002637 RID: 9783
	public Image Portrait;

	// Token: 0x04002638 RID: 9784
	public Image PortraitBackground;

	// Token: 0x04002639 RID: 9785
	public CanvasGroup PortraitGroup;

	// Token: 0x0400263A RID: 9786
	public TextMeshProUGUI StockText;

	// Token: 0x0400263B RID: 9787
	public GameObject AssistIcon;

	// Token: 0x0400263C RID: 9788
	public TextMeshProUGUI AssistText;

	// Token: 0x0400263D RID: 9789
	public GameObject SideContainer;

	// Token: 0x0400263E RID: 9790
	public GameObject SideContainerBackground;

	// Token: 0x0400263F RID: 9791
	public Sprite RedTeamSprite;

	// Token: 0x04002640 RID: 9792
	public Sprite BlueTeamSprite;

	// Token: 0x04002641 RID: 9793
	public Sprite InactiveSprite;

	// Token: 0x04002642 RID: 9794
	public GameObject FlipContainer;

	// Token: 0x04002643 RID: 9795
	public RawImage RawImagePortrait;

	// Token: 0x04002644 RID: 9796
	public TextMeshProUGUI TimerDisplay;

	// Token: 0x04002645 RID: 9797
	public CanvasGroup TagButton;

	// Token: 0x04002646 RID: 9798
	public CanvasGroup AssistButton;

	// Token: 0x04002647 RID: 9799
	public CanvasGroup TagButtonRight;

	// Token: 0x04002648 RID: 9800
	public CanvasGroup AssistButtonRight;

	// Token: 0x04002649 RID: 9801
	public GameObject TagInLeft;

	// Token: 0x0400264A RID: 9802
	public GameObject AssistLeft;

	// Token: 0x0400264B RID: 9803
	public GameObject TagInRight;

	// Token: 0x0400264C RID: 9804
	public GameObject AssistRight;

	// Token: 0x0400264D RID: 9805
	public float ButtonWidth = 300f;

	// Token: 0x0400264E RID: 9806
	public TextMeshProUGUI AssistActiveText;

	// Token: 0x0400264F RID: 9807
	public GameObject AssistActiveContainer;

	// Token: 0x04002650 RID: 9808
	private CanvasGroup tagButton;

	// Token: 0x04002651 RID: 9809
	private CanvasGroup assistButton;

	// Token: 0x04002652 RID: 9810
	private PlayerSelectionInfo playerInfo;

	// Token: 0x04002653 RID: 9811
	private PlayerReference player;

	// Token: 0x04002654 RID: 9812
	private IBenchedPlayerSpawner spawner;

	// Token: 0x04002655 RID: 9813
	private UIColor teamColor;

	// Token: 0x04002656 RID: 9814
	private CrewsGUISide side;

	// Token: 0x04002657 RID: 9815
	private int cachedLastRespawnTime;

	// Token: 0x04002658 RID: 9816
	private bool didSubscribe;

	// Token: 0x04002659 RID: 9817
	private Vector3 assistButtonPosition;

	// Token: 0x0400265A RID: 9818
	private Vector3 tagButtonPosition;

	// Token: 0x0400265B RID: 9819
	private Tweener _assistTween;

	// Token: 0x0400265C RID: 9820
	private Tweener _tagTween;

	// Token: 0x0400265D RID: 9821
	private List<PortraitDefinition> portraitTextures = new List<PortraitDefinition>();

	// Token: 0x0400265E RID: 9822
	private CrewsPlayerGUIButtonMode currentButtonMode;

	// Token: 0x0400265F RID: 9823
	private StringBuilder stringBuilder = new StringBuilder(64);
}
