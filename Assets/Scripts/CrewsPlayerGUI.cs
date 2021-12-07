// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrewsPlayerGUI : GameBehavior, ICrewsPlayerUI
{
	public TextMeshProUGUI PlayerName;

	public Image Portrait;

	public Image PortraitBackground;

	public CanvasGroup PortraitGroup;

	public TextMeshProUGUI StockText;

	public GameObject AssistIcon;

	public TextMeshProUGUI AssistText;

	public GameObject SideContainer;

	public GameObject SideContainerBackground;

	public Sprite RedTeamSprite;

	public Sprite BlueTeamSprite;

	public Sprite InactiveSprite;

	public GameObject FlipContainer;

	public RawImage RawImagePortrait;

	public TextMeshProUGUI TimerDisplay;

	public CanvasGroup TagButton;

	public CanvasGroup AssistButton;

	public CanvasGroup TagButtonRight;

	public CanvasGroup AssistButtonRight;

	public GameObject TagInLeft;

	public GameObject AssistLeft;

	public GameObject TagInRight;

	public GameObject AssistRight;

	public float ButtonWidth = 300f;

	public TextMeshProUGUI AssistActiveText;

	public GameObject AssistActiveContainer;

	private CanvasGroup tagButton;

	private CanvasGroup assistButton;

	private PlayerSelectionInfo playerInfo;

	private PlayerReference player;

	private IBenchedPlayerSpawner spawner;

	private UIColor teamColor;

	private CrewsGUISide side;

	private int cachedLastRespawnTime;

	private bool didSubscribe;

	private Vector3 assistButtonPosition;

	private Vector3 tagButtonPosition;

	private Tweener _assistTween;

	private Tweener _tagTween;

	private List<PortraitDefinition> portraitTextures = new List<PortraitDefinition>();

	private CrewsPlayerGUIButtonMode currentButtonMode;

	private StringBuilder stringBuilder = new StringBuilder(64);

	[Inject]
	public GameDataManager gameData
	{
		private get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		private get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	private PlayerNum playerNum
	{
		get
		{
			return (this.playerInfo == null) ? PlayerNum.None : this.playerInfo.playerNum;
		}
	}

	private PlayerType type
	{
		get
		{
			return (this.playerInfo == null) ? PlayerType.None : this.playerInfo.type;
		}
	}

	private TeamNum team
	{
		get
		{
			return (this.playerInfo == null) ? TeamNum.None : this.playerInfo.team;
		}
	}

	public bool Visible
	{
		get
		{
			return true;
		}
	}

	public Transform Transform
	{
		get
		{
			return base.transform;
		}
	}

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

	private void createPortrait(PortraitMode mode, PortraitBgMode bgMode)
	{
		PortraitDefinition portraitDefinition = new PortraitDefinition(mode, bgMode);
		this.updatePortraitDisplay(portraitDefinition);
		portraitDefinition.texture = this.combineImages(this.PortraitBackground.mainTexture as Texture2D, this.Portrait.mainTexture as Texture2D);
		this.portraitTextures.Add(portraitDefinition);
	}

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

	private void onGameInit(GameEvent message)
	{
		this.redraw();
	}

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

	private void animateOutTagIn()
	{
		this.killTagTween();
		this._tagTween = DOTween.To(new DOGetter<float>(this._animateOutTagIn_m__0), new DOSetter<float>(this._animateOutTagIn_m__1), 0f, 0.1f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killTagTween));
	}

	private void animateOutAssist()
	{
		this.killAssistTween();
		this._assistTween = DOTween.To(new DOGetter<float>(this._animateOutAssist_m__2), new DOSetter<float>(this._animateOutAssist_m__3), 0f, 0.1f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killAssistTween));
	}

	private bool reverseButtonAnimate()
	{
		return this.gameData.ConfigData.uiuxSettings.crewsGuiType == CrewBattleGuiType.VERSION2 && this.side == CrewsGUISide.RIGHT;
	}

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
		this._tagTween = DOTween.To(new DOGetter<Vector3>(this._animateInTagButton_m__4), new DOSetter<Vector3>(this._animateInTagButton_m__5), this.tagButtonPosition, 0.35f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killTagTween));
	}

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
		this._assistTween = DOTween.To(new DOGetter<Vector3>(this._animateInAssistButton_m__6), new DOSetter<Vector3>(this._animateInAssistButton_m__7), this.assistButtonPosition, 0.35f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.killAssistTween));
	}

	private void killAssistTween()
	{
		TweenUtil.Destroy(ref this._assistTween);
	}

	private void killTagTween()
	{
		TweenUtil.Destroy(ref this._tagTween);
	}

	private PortraitMode getPortraitMode()
	{
		if (this.player.IsEliminated)
		{
			return PortraitMode.GREY;
		}
		return PortraitMode.NORMAL;
	}

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

	public void setPosition(float x, float y)
	{
		base.transform.localPosition = new Vector3(x, y);
	}

	private void onEngagementStateChanged(GameEvent message)
	{
		PlayerEngagementStateChangedEvent playerEngagementStateChangedEvent = message as PlayerEngagementStateChangedEvent;
		if (playerEngagementStateChangedEvent.playerNum == this.playerNum)
		{
			this.redraw();
			this.updateAssistActive();
		}
	}

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

	private void onTagIn(GameEvent message)
	{
		TagInPlayerEvent tagInPlayerEvent = message as TagInPlayerEvent;
		if (tagInPlayerEvent.taggedPlayerNum == this.playerNum)
		{
			this.redraw();
		}
	}

	private void onCharacterDeath(GameEvent message)
	{
		this.redraw();
	}

	private void redraw()
	{
		this.AssistText.text = this.spawner.GetAssistsRemaining(this.playerNum).ToString();
		this.StockText.text = this.player.Lives.ToString();
		this.updatePortrait();
	}

	private void updatePortrait()
	{
		PortraitBgMode portraitBgMode = this.getPortraitBgMode();
		PortraitMode portraitMode = this.getPortraitMode();
		foreach (PortraitDefinition current in this.portraitTextures)
		{
			if (current.mode == portraitMode && current.bgMode == portraitBgMode)
			{
				this.RawImagePortrait.texture = current.texture;
			}
		}
	}

	private void playerSpawnerHandler(PlayerSpawner playerSpawner)
	{
		this.spawner = (playerSpawner as IBenchedPlayerSpawner);
	}

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

	private float _animateOutTagIn_m__0()
	{
		return this.tagButton.alpha;
	}

	private void _animateOutTagIn_m__1(float valueIn)
	{
		this.tagButton.alpha = valueIn;
	}

	private float _animateOutAssist_m__2()
	{
		return this.assistButton.alpha;
	}

	private void _animateOutAssist_m__3(float valueIn)
	{
		this.assistButton.alpha = valueIn;
	}

	private Vector3 _animateInTagButton_m__4()
	{
		return this.tagButton.transform.localPosition;
	}

	private void _animateInTagButton_m__5(Vector3 valueIn)
	{
		this.tagButton.transform.localPosition = valueIn;
	}

	private Vector3 _animateInAssistButton_m__6()
	{
		return this.assistButton.transform.localPosition;
	}

	private void _animateInAssistButton_m__7(Vector3 valueIn)
	{
		this.assistButton.transform.localPosition = valueIn;
	}
}
