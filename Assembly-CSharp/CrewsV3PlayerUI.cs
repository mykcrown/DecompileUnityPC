using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008BA RID: 2234
public class CrewsV3PlayerUI : GameBehavior, ICrewsPlayerUI
{
	// Token: 0x17000D9C RID: 3484
	// (get) Token: 0x0600382F RID: 14383 RVA: 0x0010787A File Offset: 0x00105C7A
	// (set) Token: 0x06003830 RID: 14384 RVA: 0x00107882 File Offset: 0x00105C82
	[Inject]
	public GameDataManager gameData { private get; set; }

	// Token: 0x17000D9D RID: 3485
	// (get) Token: 0x06003831 RID: 14385 RVA: 0x0010788B File Offset: 0x00105C8B
	// (set) Token: 0x06003832 RID: 14386 RVA: 0x00107893 File Offset: 0x00105C93
	[Inject]
	public ILocalization localization { private get; set; }

	// Token: 0x17000D9E RID: 3486
	// (get) Token: 0x06003833 RID: 14387 RVA: 0x0010789C File Offset: 0x00105C9C
	// (set) Token: 0x06003834 RID: 14388 RVA: 0x001078A4 File Offset: 0x00105CA4
	[Inject]
	public ICharacterDataHelper characterDataHelper { private get; set; }

	// Token: 0x17000D9F RID: 3487
	// (get) Token: 0x06003835 RID: 14389 RVA: 0x001078AD File Offset: 0x00105CAD
	// (set) Token: 0x06003836 RID: 14390 RVA: 0x001078B5 File Offset: 0x00105CB5
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x17000DA0 RID: 3488
	// (get) Token: 0x06003837 RID: 14391 RVA: 0x001078BE File Offset: 0x00105CBE
	private PlayerNum playerNum
	{
		get
		{
			return (this.playerInfo == null) ? PlayerNum.None : this.playerInfo.playerNum;
		}
	}

	// Token: 0x06003838 RID: 14392 RVA: 0x001078E0 File Offset: 0x00105CE0
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

	// Token: 0x06003839 RID: 14393 RVA: 0x0010798C File Offset: 0x00105D8C
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

	// Token: 0x0600383A RID: 14394 RVA: 0x00107A30 File Offset: 0x00105E30
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

	// Token: 0x0600383B RID: 14395 RVA: 0x00107C16 File Offset: 0x00106016
	private void playerSpawnerHandler(PlayerSpawner playerSpawner)
	{
		this.spawner = (playerSpawner as IBenchedPlayerSpawner);
	}

	// Token: 0x0600383C RID: 14396 RVA: 0x00107C24 File Offset: 0x00106024
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

	// Token: 0x0600383D RID: 14397 RVA: 0x00107C71 File Offset: 0x00106071
	private void onGameInit(GameEvent message)
	{
		this.redraw();
	}

	// Token: 0x0600383E RID: 14398 RVA: 0x00107C7C File Offset: 0x0010607C
	private void onEngagementStateChanged(GameEvent message)
	{
		PlayerEngagementStateChangedEvent playerEngagementStateChangedEvent = message as PlayerEngagementStateChangedEvent;
		if (playerEngagementStateChangedEvent.playerNum == this.playerNum)
		{
			this.redraw();
		}
	}

	// Token: 0x0600383F RID: 14399 RVA: 0x00107CA8 File Offset: 0x001060A8
	private void redraw()
	{
		this.assistText.text = this.spawner.GetAssistsRemaining(this.playerNum).ToString();
		this.stockText.text = this.player.Lives.ToString();
		this.updatePortrait();
	}

	// Token: 0x06003840 RID: 14400 RVA: 0x00107D0C File Offset: 0x0010610C
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

	// Token: 0x06003841 RID: 14401 RVA: 0x00107D8C File Offset: 0x0010618C
	private void onTagIn(GameEvent message)
	{
		TagInPlayerEvent tagInPlayerEvent = message as TagInPlayerEvent;
		if (tagInPlayerEvent.taggedPlayerNum == this.playerNum)
		{
			this.redraw();
		}
	}

	// Token: 0x06003842 RID: 14402 RVA: 0x00107DB7 File Offset: 0x001061B7
	private void onCharacterDeath(GameEvent message)
	{
		this.redraw();
	}

	// Token: 0x06003843 RID: 14403 RVA: 0x00107DC0 File Offset: 0x001061C0
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

	// Token: 0x17000DA1 RID: 3489
	// (get) Token: 0x06003844 RID: 14404 RVA: 0x00107F94 File Offset: 0x00106394
	public Transform Transform
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x04002676 RID: 9846
	public GameObject LeftMode;

	// Token: 0x04002677 RID: 9847
	public GameObject RightMode;

	// Token: 0x04002678 RID: 9848
	public Image PortraitLeft;

	// Token: 0x04002679 RID: 9849
	public Image PortraitRight;

	// Token: 0x0400267A RID: 9850
	public TextMeshProUGUI NameLeft;

	// Token: 0x0400267B RID: 9851
	public TextMeshProUGUI NameRight;

	// Token: 0x0400267C RID: 9852
	public Image StripeLeft;

	// Token: 0x0400267D RID: 9853
	public Image AssistTimerLeft;

	// Token: 0x0400267E RID: 9854
	public Image StripeRight;

	// Token: 0x0400267F RID: 9855
	public Image AssistTimerRight;

	// Token: 0x04002680 RID: 9856
	public Sprite AssistInactiveSprite;

	// Token: 0x04002681 RID: 9857
	public Sprite AssistNoSprite;

	// Token: 0x04002682 RID: 9858
	public CrewAssistButton AssistButtonLeft;

	// Token: 0x04002683 RID: 9859
	public CrewAssistButton TagInLeft;

	// Token: 0x04002684 RID: 9860
	public CrewAssistButton DynamicAbilityLeft;

	// Token: 0x04002685 RID: 9861
	public CrewAssistButton PowerAbilityLeft;

	// Token: 0x04002686 RID: 9862
	public CrewAssistButton AssistButtonRight;

	// Token: 0x04002687 RID: 9863
	public CrewAssistButton TagInRight;

	// Token: 0x04002688 RID: 9864
	public CrewAssistButton DynamicAbilityRight;

	// Token: 0x04002689 RID: 9865
	public CrewAssistButton PowerAbilityRight;

	// Token: 0x0400268A RID: 9866
	public TextMeshProUGUI StockTextLeft;

	// Token: 0x0400268B RID: 9867
	public TextMeshProUGUI AssistTextLeft;

	// Token: 0x0400268C RID: 9868
	public TextMeshProUGUI StockTextRight;

	// Token: 0x0400268D RID: 9869
	public TextMeshProUGUI AssistTextRight;

	// Token: 0x0400268E RID: 9870
	public float ButtonWidth = 300f;

	// Token: 0x0400268F RID: 9871
	private Image portrait;

	// Token: 0x04002690 RID: 9872
	private CanvasGroup portraitGroup;

	// Token: 0x04002691 RID: 9873
	private bool didSubscribe;

	// Token: 0x04002692 RID: 9874
	private PlayerSelectionInfo playerInfo;

	// Token: 0x04002693 RID: 9875
	private PlayerReference player;

	// Token: 0x04002694 RID: 9876
	private IBenchedPlayerSpawner spawner;

	// Token: 0x04002695 RID: 9877
	private CrewAssistButton tagButton;

	// Token: 0x04002696 RID: 9878
	private CrewAssistButton assistButton;

	// Token: 0x04002697 RID: 9879
	private CrewAssistButton dynamicButton;

	// Token: 0x04002698 RID: 9880
	private CrewAssistButton powerButton;

	// Token: 0x04002699 RID: 9881
	private Image stripe;

	// Token: 0x0400269A RID: 9882
	private Image assistTimer;

	// Token: 0x0400269B RID: 9883
	private TextMeshProUGUI stockText;

	// Token: 0x0400269C RID: 9884
	private TextMeshProUGUI assistText;

	// Token: 0x0400269D RID: 9885
	private TextMeshProUGUI nameText;

	// Token: 0x0400269E RID: 9886
	private Tweener _assistTween;

	// Token: 0x0400269F RID: 9887
	private Tweener _tagTween;

	// Token: 0x040026A0 RID: 9888
	private TeamNum _team;
}
