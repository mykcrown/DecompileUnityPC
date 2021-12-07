// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrewsV3PlayerUI : GameBehavior, ICrewsPlayerUI
{
	public GameObject LeftMode;

	public GameObject RightMode;

	public Image PortraitLeft;

	public Image PortraitRight;

	public TextMeshProUGUI NameLeft;

	public TextMeshProUGUI NameRight;

	public Image StripeLeft;

	public Image AssistTimerLeft;

	public Image StripeRight;

	public Image AssistTimerRight;

	public Sprite AssistInactiveSprite;

	public Sprite AssistNoSprite;

	public CrewAssistButton AssistButtonLeft;

	public CrewAssistButton TagInLeft;

	public CrewAssistButton DynamicAbilityLeft;

	public CrewAssistButton PowerAbilityLeft;

	public CrewAssistButton AssistButtonRight;

	public CrewAssistButton TagInRight;

	public CrewAssistButton DynamicAbilityRight;

	public CrewAssistButton PowerAbilityRight;

	public TextMeshProUGUI StockTextLeft;

	public TextMeshProUGUI AssistTextLeft;

	public TextMeshProUGUI StockTextRight;

	public TextMeshProUGUI AssistTextRight;

	public float ButtonWidth = 300f;

	private Image portrait;

	private CanvasGroup portraitGroup;

	private bool didSubscribe;

	private PlayerSelectionInfo playerInfo;

	private PlayerReference player;

	private IBenchedPlayerSpawner spawner;

	private CrewAssistButton tagButton;

	private CrewAssistButton assistButton;

	private CrewAssistButton dynamicButton;

	private CrewAssistButton powerButton;

	private Image stripe;

	private Image assistTimer;

	private TextMeshProUGUI stockText;

	private TextMeshProUGUI assistText;

	private TextMeshProUGUI nameText;

	private Tweener _assistTween;

	private Tweener _tagTween;

	private TeamNum _team;

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
		this.playerInfo = playerInfo;
		this._team = team;
		this.LeftMode.SetActive(false);
		this.RightMode.SetActive(false);
		this.PortraitLeft.gameObject.SetActive(false);
		this.PortraitRight.gameObject.SetActive(false);
		if (side == CrewsGUISide.LEFT)
		{
			this.LeftMode.SetActive(true);
			this.portrait = this.PortraitLeft;
			this.portrait.gameObject.SetActive(true);
			this.assistButton = this.AssistButtonLeft;
			this.tagButton = this.TagInLeft;
			this.dynamicButton = this.DynamicAbilityLeft;
			this.powerButton = this.PowerAbilityLeft;
			this.stockText = this.StockTextLeft;
			this.assistText = this.AssistTextLeft;
			this.stripe = this.StripeLeft;
			this.assistTimer = this.AssistTimerLeft;
			this.nameText = this.NameLeft;
		}
		else
		{
			this.RightMode.SetActive(true);
			this.portrait = this.PortraitRight;
			this.portrait.gameObject.SetActive(true);
			this.assistButton = this.AssistButtonRight;
			this.tagButton = this.TagInRight;
			this.dynamicButton = this.DynamicAbilityRight;
			this.powerButton = this.PowerAbilityRight;
			this.stockText = this.StockTextRight;
			this.assistText = this.AssistTextRight;
			this.stripe = this.StripeRight;
			this.assistTimer = this.AssistTimerRight;
			this.nameText = this.NameRight;
		}
		this.portraitGroup = this.portrait.GetComponent<CanvasGroup>();
		this.nameText.text = PlayerUtil.GetPlayerNametag(this.localization, playerInfo, false);
		this.playersRequestHandler(base.gameController.currentGame.PlayerReferences);
		this.playerSpawnerHandler(base.gameController.currentGame.PlayerSpawner);
		this.redraw();
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
	}

	private void onGameInit(GameEvent message)
	{
		this.redraw();
	}

	private void onEngagementStateChanged(GameEvent message)
	{
		PlayerEngagementStateChangedEvent playerEngagementStateChangedEvent = message as PlayerEngagementStateChangedEvent;
		if (playerEngagementStateChangedEvent.playerNum == this.playerNum)
		{
			this.redraw();
		}
	}

	private void redraw()
	{
		this.assistText.text = this.spawner.GetAssistsRemaining(this.playerNum).ToString();
		this.stockText.text = this.player.Lives.ToString();
		this.updatePortrait();
	}

	private void updatePortrait()
	{
		SkinData preloadedSkinData = this.skinDataManager.GetPreloadedSkinData(this.characterDataHelper.GetSkinDefinition(this.playerInfo.characterID, this.playerInfo.skinKey));
		this.portrait.sprite = preloadedSkinData.crewsHudPortrait;
		if (this.player.IsEliminated)
		{
			this.portraitGroup.alpha = 0.5f;
		}
		else
		{
			this.portraitGroup.alpha = 1f;
		}
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

	public void TickFrame()
	{
		bool flag = false;
		if (this.player.IsTemporary)
		{
			this.assistTimer.fillAmount = (float)this.player.Controller.TemporaryDurationPercent;
			flag |= true;
		}
		if (this.spawner.DisplayTagInOptionInHUD() && this.spawner.CanTagIn(this.playerNum))
		{
			this.tagButton.SetState(true);
			this.dynamicButton.SetState(false);
			this.powerButton.SetState(false);
			this.assistButton.SetState(false);
		}
		else
		{
			this.tagButton.SetState(false);
			this.assistButton.SetState(this.spawner.CanAssist(this.player));
			bool flag2 = false;
			PlayerNum primaryPlayer = this.spawner.GetPrimaryPlayer(this._team);
			if (primaryPlayer != PlayerNum.None && primaryPlayer != this.playerNum)
			{
				PlayerReference playerRef = this.spawner.GetPlayerRef(primaryPlayer);
				flag2 = (playerRef != null && playerRef.CanHostTeamMove);
			}
			this.dynamicButton.SetState(flag2 && this.player.Controller.CanUsePowerMove);
			bool flag3 = false;
			PlayerNum primaryPlayer2 = this.spawner.GetPrimaryPlayer((this._team != TeamNum.Team1) ? TeamNum.Team1 : TeamNum.Team2);
			if (primaryPlayer2 != PlayerNum.None)
			{
				PlayerReference playerRef2 = this.spawner.GetPlayerRef(primaryPlayer);
				flag3 = (playerRef2 != null && playerRef2.Controller != null);
			}
			this.powerButton.SetState(flag3 && flag2 && this.player.Controller.CanUsePowerMove);
		}
		this.assistTimer.gameObject.SetActive(false);
		this.stripe.gameObject.SetActive(false);
		this.updatePortrait();
	}
}
