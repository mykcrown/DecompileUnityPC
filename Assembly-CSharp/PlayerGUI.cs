using System;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008CD RID: 2253
public class PlayerGUI : GameBehavior, IPlayerGUI, IGUIBarElement, ITickable
{
	// Token: 0x060038C4 RID: 14532 RVA: 0x0010A6D8 File Offset: 0x00108AD8
	public PlayerGUI()
	{
		this.killAlphaTween = delegate()
		{
			this.killAlphaTweenFn();
		};
		this.tweenAlphaGetter = new DOGetter<float>(this.get_Alpha);
		this.tweenAlphaSetter = delegate(float value)
		{
			this.Alpha = value;
		};
		this.onBounceGetBigComplete = delegate()
		{
			this.onBounceGetBigCompleteFn();
		};
		this.killDamageTextTween = delegate()
		{
			this.killDamageTextTweenFn();
		};
		this.tweenDamageTextGetter = new DOGetter<float>(this.get_tweenedFontSize);
		this.tweenDamageTextSetter = delegate(float value)
		{
			this.tweenedFontSize = value;
		};
		this.flashRedPart2 = delegate()
		{
			this.flashRedPart2Fn();
		};
		this.killFlashTween = delegate()
		{
			this.killFlashTweenFn();
		};
		this.tweenFlashGetter = new DOGetter<float>(this.get_FlashAmount);
		this.tweenFlashSetter = delegate(float value)
		{
			this.FlashAmount = value;
		};
		this.killSlideTween = delegate()
		{
			this.killSlideTweenFn();
		};
		this.tweenPositionGetter = (() => base.transform.localPosition);
		this.tweenPositionSetter = delegate(Vector3 value)
		{
			base.transform.localPosition = value;
		};
	}

	// Token: 0x17000DB0 RID: 3504
	// (get) Token: 0x060038C5 RID: 14533 RVA: 0x0010A854 File Offset: 0x00108C54
	// (set) Token: 0x060038C6 RID: 14534 RVA: 0x0010A85C File Offset: 0x00108C5C
	[Inject]
	public GameDataManager gameData { private get; set; }

	// Token: 0x17000DB1 RID: 3505
	// (get) Token: 0x060038C7 RID: 14535 RVA: 0x0010A865 File Offset: 0x00108C65
	// (set) Token: 0x060038C8 RID: 14536 RVA: 0x0010A86D File Offset: 0x00108C6D
	[Inject]
	public ICharacterDataHelper characterDataHelper { private get; set; }

	// Token: 0x17000DB2 RID: 3506
	// (get) Token: 0x060038C9 RID: 14537 RVA: 0x0010A876 File Offset: 0x00108C76
	// (set) Token: 0x060038CA RID: 14538 RVA: 0x0010A87E File Offset: 0x00108C7E
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x17000DB3 RID: 3507
	// (get) Token: 0x060038CB RID: 14539 RVA: 0x0010A887 File Offset: 0x00108C87
	// (set) Token: 0x060038CC RID: 14540 RVA: 0x0010A88F File Offset: 0x00108C8F
	[Inject]
	public ICharacterDataLoader characterDataLoader { private get; set; }

	// Token: 0x17000DB4 RID: 3508
	// (get) Token: 0x060038CD RID: 14541 RVA: 0x0010A898 File Offset: 0x00108C98
	// (set) Token: 0x060038CE RID: 14542 RVA: 0x0010A8A0 File Offset: 0x00108CA0
	[Inject]
	public ILocalization localization { private get; set; }

	// Token: 0x17000DB5 RID: 3509
	// (get) Token: 0x060038CF RID: 14543 RVA: 0x0010A8A9 File Offset: 0x00108CA9
	public PlayerNum PlayerNum
	{
		get
		{
			return (this.playerInfo == null) ? PlayerNum.None : this.playerInfo.playerNum;
		}
	}

	// Token: 0x17000DB6 RID: 3510
	// (get) Token: 0x060038D0 RID: 14544 RVA: 0x0010A8C8 File Offset: 0x00108CC8
	private PlayerType type
	{
		get
		{
			return (this.playerInfo == null) ? PlayerType.None : this.playerInfo.type;
		}
	}

	// Token: 0x17000DB7 RID: 3511
	// (get) Token: 0x060038D1 RID: 14545 RVA: 0x0010A8E6 File Offset: 0x00108CE6
	public TeamNum Team
	{
		get
		{
			return (this.playerInfo == null) ? TeamNum.None : this.playerInfo.team;
		}
	}

	// Token: 0x17000DB8 RID: 3512
	// (get) Token: 0x060038D2 RID: 14546 RVA: 0x0010A905 File Offset: 0x00108D05
	public bool Visible
	{
		get
		{
			return this.displayData.player.IsInBattle;
		}
	}

	// Token: 0x060038D3 RID: 14547 RVA: 0x0010A918 File Offset: 0x00108D18
	public override void Awake()
	{
		base.Awake();
		this._canvasGroup = base.GetComponent<CanvasGroup>();
		if (base.events != null)
		{
			base.events.Subscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
		}
		base.signalBus.GetSignal<ShowEmoteCooldownSignal>().AddListener(new Action<PlayerNum>(this.showEmoteIndicator));
		base.signalBus.GetSignal<HideEmoteCooldownSignal>().AddListener(new Action<PlayerNum>(this.hideEmoteIndicator));
		this.Portrait.material = UnityEngine.Object.Instantiate<Material>(this.Portrait.material);
		this.PortraitBackground.material = UnityEngine.Object.Instantiate<Material>(this.PortraitBackground.material);
		this.FlashAmount = 0f;
	}

	// Token: 0x060038D4 RID: 14548 RVA: 0x0010A9DC File Offset: 0x00108DDC
	private void Start()
	{
		this.initScreenSpaceCache();
	}

	// Token: 0x060038D5 RID: 14549 RVA: 0x0010A9E4 File Offset: 0x00108DE4
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (base.events != null)
		{
			base.events.Unsubscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
		}
		base.signalBus.GetSignal<ShowEmoteCooldownSignal>().RemoveListener(new Action<PlayerNum>(this.showEmoteIndicator));
		base.signalBus.GetSignal<HideEmoteCooldownSignal>().RemoveListener(new Action<PlayerNum>(this.hideEmoteIndicator));
		this.killDamageTextTween();
		this.killAlphaTween();
		this.killSlideTween();
		this.killFlashTween();
	}

	// Token: 0x060038D6 RID: 14550 RVA: 0x0010AA88 File Offset: 0x00108E88
	public void Initialize(BattleSettings config, PlayerSelectionInfo playerInfo)
	{
		this.playerInfo = playerInfo;
		CharacterData data = this.characterDataLoader.GetData(playerInfo.characterID);
		if (base.battleServerAPI.IsConnected)
		{
			this.NameText.text = PlayerUtil.GetPlayerNametag(this.localization, playerInfo, true).ToUpper();
		}
		else if (playerInfo.curProfile != null && !string.IsNullOrEmpty(playerInfo.curProfile.localName))
		{
			this.NameText.text = PlayerUtil.GetPlayerNametag(this.localization, playerInfo, true).ToUpper();
		}
		else
		{
			this.NameText.text = this.characterDataHelper.GetDisplayName(playerInfo.characterID);
		}
		GameModeData dataByType = this.gameData.GameModeData.GetDataByType(config.mode);
		this.color = PlayerUtil.GetUIColor(playerInfo, dataByType.settings.usesTeams);
		this.lives.Initialize(config);
		this.initialDamageFontSize = (int)this.DamageText.fontSize;
		this.setupPlayers(base.gameController.currentGame.PlayerReferences);
		SkinData preloadedSkinData = this.skinDataManager.GetPreloadedSkinData(this.characterDataHelper.GetSkinDefinition(playerInfo.characterID, playerInfo.skinKey));
		this.Portrait.sprite = preloadedSkinData.battlePortrait;
		this.PortraitBackground.sprite = this.PortraitBackgroundSprites.GetSprite(this.color);
		this.DamageText.text = "0%";
		this.FlashAmount = 0f;
		if (data.playerGUIComponent != null)
		{
			this.setupComponent(data.playerGUIComponent);
		}
		Texture2D texture = this.createCombinedPortraitImage(preloadedSkinData, this.PortraitBackground.mainTexture as Texture2D);
		Vector2 offset = Vector2.zero;
		if (preloadedSkinData.overridePortraitOffset)
		{
			offset = preloadedSkinData.portraitOffset;
		}
		this.positionRawPortrait(offset);
		this.RawImagePortrait.texture = texture;
		this.Portrait.gameObject.SetActive(false);
		this.PortraitBackground.gameObject.SetActive(false);
		this.RawImagePortrait.gameObject.SetActive(true);
	}

	// Token: 0x060038D7 RID: 14551 RVA: 0x0010ACA8 File Offset: 0x001090A8
	private void positionRawPortrait(Vector2 offset)
	{
		int num = Mathf.Max(this.PortraitBackground.mainTexture.height, this.Portrait.mainTexture.height);
		int num2 = Mathf.Max(this.PortraitBackground.mainTexture.width, this.Portrait.mainTexture.width);
		int num3 = num - this.PortraitBackground.mainTexture.height;
		int num4 = num2 - this.PortraitBackground.mainTexture.width;
		RectTransform component = this.RawImagePortrait.GetComponent<RectTransform>();
		Vector2 sizeDelta = component.sizeDelta;
		sizeDelta.x = (float)this.Portrait.mainTexture.width + offset.x;
		sizeDelta.y = (float)this.Portrait.mainTexture.height + offset.y;
		component.sizeDelta = sizeDelta;
		Vector3 localPosition = component.localPosition;
		localPosition.x -= (float)(num4 / 2) - offset.x / 2f;
		localPosition.y += (float)(num3 / 2) - offset.y / 2f;
		component.localPosition = localPosition;
	}

	// Token: 0x060038D8 RID: 14552 RVA: 0x0010ADD8 File Offset: 0x001091D8
	private Texture2D createCombinedPortraitImage(SkinData skin, Texture2D background)
	{
		Texture2D result = null;
		if (skin.overridePortraitOffset)
		{
			result = this.combineImagesWithOffset(background, skin.battlePortrait.texture, skin.portraitOffset);
		}
		else if (skin.battlePortrait == null)
		{
			Debug.LogError("Missing battle portrait for " + skin.skinName);
		}
		else
		{
			result = this.combineImages(background, skin.battlePortrait.texture);
		}
		return result;
	}

	// Token: 0x060038D9 RID: 14553 RVA: 0x0010AE50 File Offset: 0x00109250
	private Texture2D combineImagesWithOffset(Texture2D BackgroundTex, Texture2D Overlay, Vector2Int overlayOffset)
	{
		Rect rect = new Rect(0f, 0f, (float)BackgroundTex.width, (float)BackgroundTex.height);
		Rect rect2 = new Rect((float)overlayOffset.x, (float)overlayOffset.y, (float)Overlay.width, (float)Overlay.height);
		int num = (int)Mathf.Min(rect.xMin, rect2.xMin);
		int num2 = (int)Mathf.Max(rect.xMax, rect2.xMax);
		int num3 = (int)Mathf.Min(rect.yMin, rect2.yMin);
		int num4 = (int)Mathf.Max(rect.yMax, rect2.yMax);
		int num5 = Math.Abs(num2 - num);
		int num6 = Math.Abs(num4 - num3);
		rect.x = (float)(num5 / 2) - rect.width / 2f;
		rect.y = (float)(num6 / 2) - rect.height / 2f;
		Texture2D texture2D = new Texture2D(num5, num6, TextureFormat.ARGB32, false);
		for (int i = 0; i < num5; i++)
		{
			for (int j = 0; j < num6; j++)
			{
				Color a = Color.clear;
				Color a2 = Color.clear;
				if (this.rectContains(rect2, i, j))
				{
					int num7 = (int)rect2.xMin;
					int num8 = (int)rect2.yMin;
					a = Overlay.GetPixel(i - num7, j - num8);
				}
				if (this.rectContains(rect, i, j))
				{
					int num9 = (int)rect.xMin;
					int num10 = (int)rect.yMin;
					a2 = BackgroundTex.GetPixel(i - num9, j - num10);
				}
				Color a3 = a * a.a;
				Color b = a2 * (1f - a.a);
				Color color = a3 + b;
				texture2D.SetPixel(i, j, color);
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x060038DA RID: 14554 RVA: 0x0010B040 File Offset: 0x00109440
	private Texture2D combineImages(Texture2D BackgroundTex, Texture2D Overlay)
	{
		int num = Mathf.Max(BackgroundTex.height, Overlay.height);
		int num2 = Mathf.Max(BackgroundTex.width, Overlay.width);
		int num3 = num2 - BackgroundTex.width;
		Texture2D texture2D = new Texture2D(num2, num, TextureFormat.ARGB32, false);
		texture2D.SetPixels(num3, 0, BackgroundTex.width, BackgroundTex.height, BackgroundTex.GetPixels());
		Color color = default(Color);
		for (int i = BackgroundTex.height; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				texture2D.SetPixel(j, i, color);
			}
		}
		for (int k = 0; k < num; k++)
		{
			for (int l = 0; l < num3; l++)
			{
				texture2D.SetPixel(l, k, color);
			}
		}
		for (int m = 0; m < Overlay.height; m++)
		{
			for (int n = 0; n < Overlay.width; n++)
			{
				Color b = Overlay.GetPixel(n, m) * Overlay.GetPixel(n, m).a;
				Color a = texture2D.GetPixel(n, m) * (1f - b.a);
				texture2D.SetPixel(n, m, a + b);
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x060038DB RID: 14555 RVA: 0x0010B1AD File Offset: 0x001095AD
	private bool rectContains(Rect rect, int x, int y)
	{
		return (float)x >= rect.xMin && (float)x <= rect.xMax && (float)y >= rect.yMin && (float)y <= rect.yMax;
	}

	// Token: 0x060038DC RID: 14556 RVA: 0x0010B1EC File Offset: 0x001095EC
	private void initScreenSpaceCache()
	{
		this.screenSpaceCachedAtPoint = this.OverlapBottomLeft.position;
		this.screenSpaceCache = new Rect(this.OverlapBottomLeft.position, this.OverlapTopRight.position - this.OverlapBottomLeft.position);
	}

	// Token: 0x060038DD RID: 14557 RVA: 0x0010B24C File Offset: 0x0010964C
	private void setupPlayers(List<PlayerReference> players)
	{
		for (int i = 0; i < players.Count; i++)
		{
			if (!players[i].IsSpectating)
			{
				if (players[i].PlayerNum == this.PlayerNum)
				{
					PlayerGUI.PlayerDisplayData playerDisplayData = new PlayerGUI.PlayerDisplayData();
					playerDisplayData.player = players[i];
					playerDisplayData.displayedDamage = playerDisplayData.player.Controller.CurrentDamage;
					this.displayData = playerDisplayData;
					this.lives.PlayerRef = players[i];
					break;
				}
			}
		}
		this.updateEngagementState(this.displayData.player.EngagementState);
		if (!this.Visible)
		{
			this.hide(0.1f);
		}
	}

	// Token: 0x060038DE RID: 14558 RVA: 0x0010B310 File Offset: 0x00109710
	private void onEngagementStateChanged(GameEvent message)
	{
		PlayerEngagementStateChangedEvent playerEngagementStateChangedEvent = message as PlayerEngagementStateChangedEvent;
		if (playerEngagementStateChangedEvent.playerNum == this.PlayerNum)
		{
			this.updateEngagementState(playerEngagementStateChangedEvent.engagement);
		}
	}

	// Token: 0x060038DF RID: 14559 RVA: 0x0010B344 File Offset: 0x00109744
	private void updateEngagementState(PlayerEngagementState state)
	{
		if (this.engagementState != state)
		{
			this.engagementState = state;
			this.TimerContainer.SetActive(false);
			this.FlashAmount = 0f;
			switch (state)
			{
			case PlayerEngagementState.Primary:
				this.show();
				this.lives.gameObject.SetActive(true);
				break;
			case PlayerEngagementState.Benched:
				this.hide(0.1f);
				break;
			case PlayerEngagementState.Temporary:
				this.show();
				this.lives.gameObject.SetActive(false);
				this.TimerContainer.SetActive(true);
				this.displayData.displayedDamage = this.displayData.player.Controller.CurrentDamage;
				this.updateDamageText();
				break;
			case PlayerEngagementState.Disconnected:
				this.hide(0.1f);
				break;
			}
		}
	}

	// Token: 0x060038E0 RID: 14560 RVA: 0x0010B428 File Offset: 0x00109828
	public void TickFrame()
	{
		if (!this.Visible)
		{
			return;
		}
		bool flag = false;
		if (this.displayData.displayedDamage > this.displayData.player.Controller.CurrentDamage)
		{
			this.displayData.displayedDamage = Mathf.Max(this.displayData.displayedDamage - this.damageDownSpeed * WTime.frameTime, this.displayData.player.Controller.CurrentDamage);
			flag = true;
		}
		else if (this.displayData.displayedDamage < this.displayData.player.Controller.CurrentDamage)
		{
			this.displayData.displayedDamage = Mathf.Min(this.displayData.displayedDamage + this.damageUpSpeed * WTime.frameTime, this.displayData.player.Controller.CurrentDamage);
			flag = true;
		}
		if (flag)
		{
			this.updateDamageText();
		}
		if (this._targetDamageDisplay != (int)this.displayData.player.Controller.CurrentDamage)
		{
			this._targetDamageDisplay = (int)this.displayData.player.Controller.CurrentDamage;
			this.bounceDamage();
		}
		if (this.engagementState == PlayerEngagementState.Temporary)
		{
			if (this.TimerContainer.activeInHierarchy)
			{
				this.stringBuilder.Remove(0, this.stringBuilder.Length);
				TimeUtil.FormatTimeByFrames(this.assistFramesRemaining, this.stringBuilder);
				this.TimerText.text = this.stringBuilder.ToString();
			}
			if ((float)this.assistFramesRemaining <= this.RedFlashAtFrames)
			{
				this.flashRed();
			}
		}
		this.lives.TickFrame();
	}

	// Token: 0x17000DB9 RID: 3513
	// (get) Token: 0x060038E1 RID: 14561 RVA: 0x0010B5DD File Offset: 0x001099DD
	private int assistFramesRemaining
	{
		get
		{
			return this.displayData.player.Controller.TemporaryDurationFrames;
		}
	}

	// Token: 0x060038E2 RID: 14562 RVA: 0x0010B5F4 File Offset: 0x001099F4
	private void updateDamageText()
	{
		if (this.displayData.player.Controller.IsEliminated && this.engagementState != PlayerEngagementState.Temporary)
		{
			this.DamageText.text = string.Empty;
		}
		else
		{
			this.DamageText.text = (int)this.displayData.displayedDamage + "%";
			float num = Mathf.Pow(this.displayData.displayedDamage / (float)this.fullRedDamage, 0.5f);
			if (num > 1f)
			{
				num = 1f;
			}
			this.DamageText.color = new Color(1f, 1f - num, 1f - num);
		}
	}

	// Token: 0x17000DBA RID: 3514
	// (get) Token: 0x060038E3 RID: 14563 RVA: 0x0010B6B4 File Offset: 0x00109AB4
	// (set) Token: 0x060038E4 RID: 14564 RVA: 0x0010B6BC File Offset: 0x00109ABC
	private float tweenedFontSize
	{
		get
		{
			return this._tweenedFontSize;
		}
		set
		{
			this._tweenedFontSize = value;
			this.DamageText.fontSize = (float)((int)value);
		}
	}

	// Token: 0x060038E5 RID: 14565 RVA: 0x0010B6D4 File Offset: 0x00109AD4
	private void bounceDamage()
	{
		this.killDamageTextTween();
		int num = this.initialDamageFontSize + this.LargeFontSizeIncrease;
		this._tweenedFontSize = this.DamageText.fontSize;
		this._damageTextTween = DOTween.To(this.tweenDamageTextGetter, this.tweenDamageTextSetter, (float)num, 0.08f).SetEase(Ease.OutSine).OnComplete(this.onBounceGetBigComplete);
	}

	// Token: 0x060038E6 RID: 14566 RVA: 0x0010B73A File Offset: 0x00109B3A
	private void killDamageTextTweenFn()
	{
		TweenUtil.Destroy(ref this._damageTextTween);
	}

	// Token: 0x060038E7 RID: 14567 RVA: 0x0010B748 File Offset: 0x00109B48
	private void onBounceGetBigCompleteFn()
	{
		this.killDamageTextTween();
		int num = this.initialDamageFontSize;
		this._damageTextTween = DOTween.To(this.tweenDamageTextGetter, this.tweenDamageTextSetter, (float)num, 0.08f).SetEase(Ease.InSine).SetDelay(0.05f).OnComplete(this.killDamageTextTween);
	}

	// Token: 0x060038E8 RID: 14568 RVA: 0x0010B7A0 File Offset: 0x00109BA0
	public void setPosition(float x, float y)
	{
		base.transform.localPosition = new Vector3(x, y);
	}

	// Token: 0x060038E9 RID: 14569 RVA: 0x0010B7B4 File Offset: 0x00109BB4
	private void setupComponent(PlayerGUIComponent component)
	{
		if (component is ListPlayerGUIComponent)
		{
			ListPlayerGUIComponent listPlayerGUIComponent = UnityEngine.Object.Instantiate<PlayerGUIComponent>(component) as ListPlayerGUIComponent;
			listPlayerGUIComponent.MaxDisplayAnchor = this.MaxListDisplayAnchor;
			listPlayerGUIComponent.Initialize(this.PlayerNum, this.ListComponentAnchor);
		}
		else
		{
			Debug.LogWarning("Player gui component type is not supported");
		}
	}

	// Token: 0x060038EA RID: 14570 RVA: 0x0010B808 File Offset: 0x00109C08
	private void flashRed()
	{
		if (this._flashTween == null)
		{
			this._flashTweenDuration = this.RedFlashDuration / 2f;
			this._flashTween = DOTween.To(this.tweenFlashGetter, this.tweenFlashSetter, this.RedFlashIntensity, this._flashTweenDuration).SetEase(Ease.Linear).OnComplete(this.flashRedPart2);
		}
	}

	// Token: 0x060038EB RID: 14571 RVA: 0x0010B866 File Offset: 0x00109C66
	private void flashRedPart2Fn()
	{
		this._flashTween = DOTween.To(this.tweenFlashGetter, this.tweenFlashSetter, 0f, this._flashTweenDuration).SetEase(Ease.Linear).OnComplete(this.killFlashTween);
	}

	// Token: 0x060038EC RID: 14572 RVA: 0x0010B89B File Offset: 0x00109C9B
	private void killFlashTweenFn()
	{
		TweenUtil.Destroy(ref this._flashTween);
	}

	// Token: 0x17000DBB RID: 3515
	// (get) Token: 0x060038ED RID: 14573 RVA: 0x0010B8A8 File Offset: 0x00109CA8
	// (set) Token: 0x060038EE RID: 14574 RVA: 0x0010B8B0 File Offset: 0x00109CB0
	public float FlashAmount
	{
		get
		{
			return this._flashAmount;
		}
		set
		{
			this._flashAmount = value;
			this.Portrait.material.SetFloat("_FlashAmount", this._flashAmount);
			this.PortraitBackground.material.SetFloat("_FlashAmount", this._flashAmount);
		}
	}

	// Token: 0x060038EF RID: 14575 RVA: 0x0010B8EF File Offset: 0x00109CEF
	private void killSlideTweenFn()
	{
		TweenUtil.Destroy(ref this._slideTween);
	}

	// Token: 0x060038F0 RID: 14576 RVA: 0x0010B8FC File Offset: 0x00109CFC
	public void SlideToPosition(Vector3 position, float duration, float delay)
	{
		this.killSlideTween();
		this._slideTween = DOTween.To(this.tweenPositionGetter, this.tweenPositionSetter, position, duration).SetEase(Ease.OutCirc).SetDelay(delay).OnComplete(this.killSlideTween);
	}

	// Token: 0x060038F1 RID: 14577 RVA: 0x0010B93A File Offset: 0x00109D3A
	private void killAlphaTweenFn()
	{
		this.isTweeningToStandard = false;
		this.isTweeningForOverlapClear = false;
		this.isTweeningHide = false;
		TweenUtil.Destroy(ref this._alphaTween);
	}

	// Token: 0x060038F2 RID: 14578 RVA: 0x0010B95C File Offset: 0x00109D5C
	public void hide(float duration = 0.1f)
	{
		if (!this.isHidden)
		{
			this.isHidden = true;
			this.updateAlphaTween(duration);
		}
	}

	// Token: 0x060038F3 RID: 14579 RVA: 0x0010B977 File Offset: 0x00109D77
	private void show()
	{
		if (this.isHidden)
		{
			this.isHidden = false;
			this.updateAlphaTween(0f);
		}
	}

	// Token: 0x060038F4 RID: 14580 RVA: 0x0010B998 File Offset: 0x00109D98
	private void updateAlphaTween(float duration = 0f)
	{
		if (this.isHidden)
		{
			if (!this.isTweeningHide)
			{
				this.killAlphaTween();
				this.isTweeningHide = true;
				this._alphaTween = DOTween.To(this.tweenAlphaGetter, this.tweenAlphaSetter, 0f, duration).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.onFadeOutComplete));
			}
		}
		else
		{
			base.gameObject.SetActive(true);
			if (this.clearForWorldOverlap)
			{
				if (!this.isTweeningForOverlapClear)
				{
					this.killAlphaTween();
					this.isTweeningForOverlapClear = true;
					this._alphaTween = DOTween.To(this.tweenAlphaGetter, this.tweenAlphaSetter, base.gameManager.Camera.cameraOptions.guiOverlapAlpha, duration).SetEase(Ease.Linear).OnComplete(this.killAlphaTween);
				}
			}
			else if (duration == 0f)
			{
				this.killAlphaTween();
				this.Alpha = 1f;
			}
			else if (!this.isTweeningToStandard)
			{
				this.killAlphaTween();
				this.isTweeningToStandard = true;
				this._alphaTween = DOTween.To(this.tweenAlphaGetter, this.tweenAlphaSetter, 1f, duration).SetEase(Ease.Linear).OnComplete(this.killAlphaTween);
			}
		}
	}

	// Token: 0x060038F5 RID: 14581 RVA: 0x0010BAEE File Offset: 0x00109EEE
	private void onFadeOutComplete()
	{
		this.killAlphaTween();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060038F6 RID: 14582 RVA: 0x0010BB08 File Offset: 0x00109F08
	public void ClearScreenSpace(List<Rect> list)
	{
		if (this.screenSpaceCachedAtPoint.x != this.OverlapBottomLeft.position.x || this.screenSpaceCachedAtPoint.y != this.OverlapBottomLeft.position.y)
		{
			this.initScreenSpaceCache();
		}
		this.clearForWorldOverlap = this.isOverlap(list);
		this.updateAlphaTween(0.12f);
	}

	// Token: 0x060038F7 RID: 14583 RVA: 0x0010BB7C File Offset: 0x00109F7C
	private bool isOverlap(List<Rect> list)
	{
		foreach (Rect rect in list)
		{
			if (rect.Overlaps(this.screenSpaceCache))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000DBC RID: 3516
	// (get) Token: 0x060038F8 RID: 14584 RVA: 0x0010BBE8 File Offset: 0x00109FE8
	// (set) Token: 0x060038F9 RID: 14585 RVA: 0x0010BBF0 File Offset: 0x00109FF0
	public float Alpha
	{
		get
		{
			return this._alpha;
		}
		set
		{
			this._alpha = value;
			this._canvasGroup.alpha = this._alpha;
		}
	}

	// Token: 0x060038FA RID: 14586 RVA: 0x0010BC0A File Offset: 0x0010A00A
	private void showEmoteIndicator(PlayerNum playerNum)
	{
		if (playerNum == this.PlayerNum)
		{
			this.EmoteCooldownIndicator.gameObject.SetActive(true);
			this.EmoteCooldownIndicatorGlow.gameObject.SetActive(true);
		}
	}

	// Token: 0x060038FB RID: 14587 RVA: 0x0010BC3A File Offset: 0x0010A03A
	private void hideEmoteIndicator(PlayerNum playerNum)
	{
		if (playerNum == this.PlayerNum)
		{
			this.EmoteCooldownIndicator.gameObject.SetActive(false);
			this.EmoteCooldownIndicatorGlow.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003907 RID: 14599 RVA: 0x0010BCD0 File Offset: 0x0010A0D0
	Transform IGUIBarElement.get_transform()
	{
		return base.transform;
	}

	// Token: 0x04002719 RID: 10009
	public TextMeshProUGUI NameText;

	// Token: 0x0400271A RID: 10010
	public TextMeshProUGUI DamageText;

	// Token: 0x0400271B RID: 10011
	public Image Portrait;

	// Token: 0x0400271C RID: 10012
	public Image PortraitBackground;

	// Token: 0x0400271D RID: 10013
	public int LargeFontSizeIncrease = 30;

	// Token: 0x0400271E RID: 10014
	public GameObject TimerContainer;

	// Token: 0x0400271F RID: 10015
	public TextMeshProUGUI TimerText;

	// Token: 0x04002720 RID: 10016
	public Transform OverlapBottomLeft;

	// Token: 0x04002721 RID: 10017
	public Transform OverlapTopRight;

	// Token: 0x04002722 RID: 10018
	public RawImage RawImagePortrait;

	// Token: 0x04002723 RID: 10019
	public Image EmoteCooldownIndicator;

	// Token: 0x04002724 RID: 10020
	public Image EmoteCooldownIndicatorGlow;

	// Token: 0x04002725 RID: 10021
	public ColorSpriteContainer PortraitBackgroundSprites;

	// Token: 0x04002726 RID: 10022
	public Transform ListComponentAnchor;

	// Token: 0x04002727 RID: 10023
	public Transform MaxListDisplayAnchor;

	// Token: 0x04002728 RID: 10024
	public float damageDownSpeed = 300f;

	// Token: 0x04002729 RID: 10025
	public float damageUpSpeed = 100f;

	// Token: 0x0400272A RID: 10026
	public int fullRedDamage = 300;

	// Token: 0x0400272B RID: 10027
	public float RedFlashIntensity = 0.35f;

	// Token: 0x0400272C RID: 10028
	public float RedFlashDuration = 1f;

	// Token: 0x0400272D RID: 10029
	public float RedFlashAtFrames = 300f;

	// Token: 0x0400272E RID: 10030
	public LifePipDisplay lives;

	// Token: 0x0400272F RID: 10031
	private int initialDamageFontSize;

	// Token: 0x04002730 RID: 10032
	private PlayerGUI.PlayerDisplayData displayData;

	// Token: 0x04002731 RID: 10033
	private UIColor color;

	// Token: 0x04002732 RID: 10034
	private PlayerSelectionInfo playerInfo;

	// Token: 0x04002733 RID: 10035
	private PlayerEngagementState engagementState;

	// Token: 0x04002734 RID: 10036
	private Tweener _damageTextTween;

	// Token: 0x04002735 RID: 10037
	private int _targetDamageDisplay;

	// Token: 0x04002736 RID: 10038
	private float _tweenedFontSize;

	// Token: 0x04002737 RID: 10039
	private Tweener _flashTween;

	// Token: 0x04002738 RID: 10040
	private float _flashTweenDuration;

	// Token: 0x04002739 RID: 10041
	private float _flashAmount;

	// Token: 0x0400273A RID: 10042
	private Tweener _slideTween;

	// Token: 0x0400273B RID: 10043
	private Tweener _alphaTween;

	// Token: 0x0400273C RID: 10044
	private float _alpha = 1f;

	// Token: 0x0400273D RID: 10045
	private CanvasGroup _canvasGroup;

	// Token: 0x0400273E RID: 10046
	private bool isHidden;

	// Token: 0x0400273F RID: 10047
	private bool isTweeningHide;

	// Token: 0x04002740 RID: 10048
	private bool clearForWorldOverlap;

	// Token: 0x04002741 RID: 10049
	private bool isTweeningForOverlapClear;

	// Token: 0x04002742 RID: 10050
	private bool isTweeningToStandard;

	// Token: 0x04002743 RID: 10051
	private Vector2 screenSpaceCachedAtPoint = Vector2.zero;

	// Token: 0x04002744 RID: 10052
	private Rect screenSpaceCache;

	// Token: 0x04002745 RID: 10053
	private StringBuilder stringBuilder = new StringBuilder(64);

	// Token: 0x04002746 RID: 10054
	private TweenCallback killAlphaTween;

	// Token: 0x04002747 RID: 10055
	private DOGetter<float> tweenAlphaGetter;

	// Token: 0x04002748 RID: 10056
	private DOSetter<float> tweenAlphaSetter;

	// Token: 0x04002749 RID: 10057
	private TweenCallback killDamageTextTween;

	// Token: 0x0400274A RID: 10058
	private TweenCallback onBounceGetBigComplete;

	// Token: 0x0400274B RID: 10059
	private DOGetter<float> tweenDamageTextGetter;

	// Token: 0x0400274C RID: 10060
	private DOSetter<float> tweenDamageTextSetter;

	// Token: 0x0400274D RID: 10061
	private TweenCallback flashRedPart2;

	// Token: 0x0400274E RID: 10062
	private TweenCallback killFlashTween;

	// Token: 0x0400274F RID: 10063
	private DOGetter<float> tweenFlashGetter;

	// Token: 0x04002750 RID: 10064
	private DOSetter<float> tweenFlashSetter;

	// Token: 0x04002751 RID: 10065
	private TweenCallback killSlideTween;

	// Token: 0x04002752 RID: 10066
	private DOGetter<Vector3> tweenPositionGetter;

	// Token: 0x04002753 RID: 10067
	private DOSetter<Vector3> tweenPositionSetter;

	// Token: 0x020008CE RID: 2254
	public class PlayerDisplayData
	{
		// Token: 0x04002754 RID: 10068
		public PlayerReference player;

		// Token: 0x04002755 RID: 10069
		public float displayedDamage;
	}
}
