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

public class PlayerGUI : GameBehavior, IPlayerGUI, IGUIBarElement, ITickable
{
	public class PlayerDisplayData
	{
		public PlayerReference player;

		public float displayedDamage;
	}

	public TextMeshProUGUI NameText;

	public TextMeshProUGUI DamageText;

	public Image Portrait;

	public Image PortraitBackground;

	public int LargeFontSizeIncrease = 30;

	public GameObject TimerContainer;

	public TextMeshProUGUI TimerText;

	public Transform OverlapBottomLeft;

	public Transform OverlapTopRight;

	public RawImage RawImagePortrait;

	public Image EmoteCooldownIndicator;

	public Image EmoteCooldownIndicatorGlow;

	public ColorSpriteContainer PortraitBackgroundSprites;

	public Transform ListComponentAnchor;

	public Transform MaxListDisplayAnchor;

	public float damageDownSpeed = 300f;

	public float damageUpSpeed = 100f;

	public int fullRedDamage = 300;

	public float RedFlashIntensity = 0.35f;

	public float RedFlashDuration = 1f;

	public float RedFlashAtFrames = 300f;

	public LifePipDisplay lives;

	private int initialDamageFontSize;

	private PlayerGUI.PlayerDisplayData displayData;

	private UIColor color;

	private PlayerSelectionInfo playerInfo;

	private PlayerEngagementState engagementState;

	private Tweener _damageTextTween;

	private int _targetDamageDisplay;

	private float _tweenedFontSize;

	private Tweener _flashTween;

	private float _flashTweenDuration;

	private float _flashAmount;

	private Tweener _slideTween;

	private Tweener _alphaTween;

	private float _alpha = 1f;

	private CanvasGroup _canvasGroup;

	private bool isHidden;

	private bool isTweeningHide;

	private bool clearForWorldOverlap;

	private bool isTweeningForOverlapClear;

	private bool isTweeningToStandard;

	private Vector2 screenSpaceCachedAtPoint = Vector2.zero;

	private Rect screenSpaceCache;

	private StringBuilder stringBuilder = new StringBuilder(64);

	private TweenCallback killAlphaTween;

	private DOGetter<float> tweenAlphaGetter;

	private DOSetter<float> tweenAlphaSetter;

	private TweenCallback killDamageTextTween;

	private TweenCallback onBounceGetBigComplete;

	private DOGetter<float> tweenDamageTextGetter;

	private DOSetter<float> tweenDamageTextSetter;

	private TweenCallback flashRedPart2;

	private TweenCallback killFlashTween;

	private DOGetter<float> tweenFlashGetter;

	private DOSetter<float> tweenFlashSetter;

	private TweenCallback killSlideTween;

	private DOGetter<Vector3> tweenPositionGetter;

	private DOSetter<Vector3> tweenPositionSetter;

	[Inject]
	public GameDataManager gameData
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

	[Inject]
	public ICharacterDataLoader characterDataLoader
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

	public PlayerNum PlayerNum
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

	public TeamNum Team
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
			return this.displayData.player.IsInBattle;
		}
	}

	private int assistFramesRemaining
	{
		get
		{
			return this.displayData.player.Controller.TemporaryDurationFrames;
		}
	}

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

	public PlayerGUI()
	{
		this.killAlphaTween = new TweenCallback(this._PlayerGUI_m__0);
		this.tweenAlphaGetter = new DOGetter<float>(this.get_Alpha);
		this.tweenAlphaSetter = new DOSetter<float>(this._PlayerGUI_m__1);
		this.onBounceGetBigComplete = new TweenCallback(this._PlayerGUI_m__2);
		this.killDamageTextTween = new TweenCallback(this._PlayerGUI_m__3);
		this.tweenDamageTextGetter = new DOGetter<float>(this.get_tweenedFontSize);
		this.tweenDamageTextSetter = new DOSetter<float>(this._PlayerGUI_m__4);
		this.flashRedPart2 = new TweenCallback(this._PlayerGUI_m__5);
		this.killFlashTween = new TweenCallback(this._PlayerGUI_m__6);
		this.tweenFlashGetter = new DOGetter<float>(this.get_FlashAmount);
		this.tweenFlashSetter = new DOSetter<float>(this._PlayerGUI_m__7);
		this.killSlideTween = new TweenCallback(this._PlayerGUI_m__8);
		this.tweenPositionGetter = new DOGetter<Vector3>(this._PlayerGUI_m__9);
		this.tweenPositionSetter = new DOSetter<Vector3>(this._PlayerGUI_m__A);
	}

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

	private void Start()
	{
		this.initScreenSpaceCache();
	}

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

	private Texture2D createCombinedPortraitImage(SkinData skin, Texture2D background)
	{
		Texture2D result = null;
		if (skin.overridePortraitOffset)
		{
			result = this.combineImagesWithOffset(background, skin.battlePortrait.texture, skin.portraitOffset);
		}
		else if (skin.battlePortrait == null)
		{
			UnityEngine.Debug.LogError("Missing battle portrait for " + skin.skinName);
		}
		else
		{
			result = this.combineImages(background, skin.battlePortrait.texture);
		}
		return result;
	}

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

	private bool rectContains(Rect rect, int x, int y)
	{
		return (float)x >= rect.xMin && (float)x <= rect.xMax && (float)y >= rect.yMin && (float)y <= rect.yMax;
	}

	private void initScreenSpaceCache()
	{
		this.screenSpaceCachedAtPoint = this.OverlapBottomLeft.position;
		this.screenSpaceCache = new Rect(this.OverlapBottomLeft.position, this.OverlapTopRight.position - this.OverlapBottomLeft.position);
	}

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

	private void onEngagementStateChanged(GameEvent message)
	{
		PlayerEngagementStateChangedEvent playerEngagementStateChangedEvent = message as PlayerEngagementStateChangedEvent;
		if (playerEngagementStateChangedEvent.playerNum == this.PlayerNum)
		{
			this.updateEngagementState(playerEngagementStateChangedEvent.engagement);
		}
	}

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

	private void bounceDamage()
	{
		this.killDamageTextTween();
		int num = this.initialDamageFontSize + this.LargeFontSizeIncrease;
		this._tweenedFontSize = this.DamageText.fontSize;
		this._damageTextTween = DOTween.To(this.tweenDamageTextGetter, this.tweenDamageTextSetter, (float)num, 0.08f).SetEase(Ease.OutSine).OnComplete(this.onBounceGetBigComplete);
	}

	private void killDamageTextTweenFn()
	{
		TweenUtil.Destroy(ref this._damageTextTween);
	}

	private void onBounceGetBigCompleteFn()
	{
		this.killDamageTextTween();
		int num = this.initialDamageFontSize;
		this._damageTextTween = DOTween.To(this.tweenDamageTextGetter, this.tweenDamageTextSetter, (float)num, 0.08f).SetEase(Ease.InSine).SetDelay(0.05f).OnComplete(this.killDamageTextTween);
	}

	public void setPosition(float x, float y)
	{
		base.transform.localPosition = new Vector3(x, y);
	}

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
			UnityEngine.Debug.LogWarning("Player gui component type is not supported");
		}
	}

	private void flashRed()
	{
		if (this._flashTween == null)
		{
			this._flashTweenDuration = this.RedFlashDuration / 2f;
			this._flashTween = DOTween.To(this.tweenFlashGetter, this.tweenFlashSetter, this.RedFlashIntensity, this._flashTweenDuration).SetEase(Ease.Linear).OnComplete(this.flashRedPart2);
		}
	}

	private void flashRedPart2Fn()
	{
		this._flashTween = DOTween.To(this.tweenFlashGetter, this.tweenFlashSetter, 0f, this._flashTweenDuration).SetEase(Ease.Linear).OnComplete(this.killFlashTween);
	}

	private void killFlashTweenFn()
	{
		TweenUtil.Destroy(ref this._flashTween);
	}

	private void killSlideTweenFn()
	{
		TweenUtil.Destroy(ref this._slideTween);
	}

	public void SlideToPosition(Vector3 position, float duration, float delay)
	{
		this.killSlideTween();
		this._slideTween = DOTween.To(this.tweenPositionGetter, this.tweenPositionSetter, position, duration).SetEase(Ease.OutCirc).SetDelay(delay).OnComplete(this.killSlideTween);
	}

	private void killAlphaTweenFn()
	{
		this.isTweeningToStandard = false;
		this.isTweeningForOverlapClear = false;
		this.isTweeningHide = false;
		TweenUtil.Destroy(ref this._alphaTween);
	}

	public void hide(float duration = 0.1f)
	{
		if (!this.isHidden)
		{
			this.isHidden = true;
			this.updateAlphaTween(duration);
		}
	}

	private void show()
	{
		if (this.isHidden)
		{
			this.isHidden = false;
			this.updateAlphaTween(0f);
		}
	}

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

	private void onFadeOutComplete()
	{
		this.killAlphaTween();
		base.gameObject.SetActive(false);
	}

	public void ClearScreenSpace(List<Rect> list)
	{
		if (this.screenSpaceCachedAtPoint.x != this.OverlapBottomLeft.position.x || this.screenSpaceCachedAtPoint.y != this.OverlapBottomLeft.position.y)
		{
			this.initScreenSpaceCache();
		}
		this.clearForWorldOverlap = this.isOverlap(list);
		this.updateAlphaTween(0.12f);
	}

	private bool isOverlap(List<Rect> list)
	{
		foreach (Rect current in list)
		{
			if (current.Overlaps(this.screenSpaceCache))
			{
				return true;
			}
		}
		return false;
	}

	private void showEmoteIndicator(PlayerNum playerNum)
	{
		if (playerNum == this.PlayerNum)
		{
			this.EmoteCooldownIndicator.gameObject.SetActive(true);
			this.EmoteCooldownIndicatorGlow.gameObject.SetActive(true);
		}
	}

	private void hideEmoteIndicator(PlayerNum playerNum)
	{
		if (playerNum == this.PlayerNum)
		{
			this.EmoteCooldownIndicator.gameObject.SetActive(false);
			this.EmoteCooldownIndicatorGlow.gameObject.SetActive(false);
		}
	}

	private void _PlayerGUI_m__0()
	{
		this.killAlphaTweenFn();
	}

	private void _PlayerGUI_m__1(float value)
	{
		this.Alpha = value;
	}

	private void _PlayerGUI_m__2()
	{
		this.onBounceGetBigCompleteFn();
	}

	private void _PlayerGUI_m__3()
	{
		this.killDamageTextTweenFn();
	}

	private void _PlayerGUI_m__4(float value)
	{
		this.tweenedFontSize = value;
	}

	private void _PlayerGUI_m__5()
	{
		this.flashRedPart2Fn();
	}

	private void _PlayerGUI_m__6()
	{
		this.killFlashTweenFn();
	}

	private void _PlayerGUI_m__7(float value)
	{
		this.FlashAmount = value;
	}

	private void _PlayerGUI_m__8()
	{
		this.killSlideTweenFn();
	}

	private Vector3 _PlayerGUI_m__9()
	{
		return base.transform.localPosition;
	}

	private void _PlayerGUI_m__A(Vector3 value)
	{
		base.transform.localPosition = value;
	}

	Transform IGUIBarElement.get_transform()
	{
		return base.transform;
	}
}
