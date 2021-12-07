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

public class CharacterFloatyContainer : GameBehavior
{
	private const float SMOOTH_TIME_FACTOR_X = 0.02f;

	private const float SMOOTH_TIME_FACTOR_Y = 0.05f;

	private const float MAX_SMOOTH_DISTANCE = 200f;

	private const float MAX_SMOOTH_DISTANCE_SQR = 40000f;

	private const float OFFSET_Y = -1.27f;

	public Sprite BlueArrow;

	public Sprite RedArrow;

	public Sprite GreenArrow;

	public Sprite YellowArrow;

	public Sprite PurpleArrow;

	public Sprite PinkArrow;

	public Sprite GreyArrow;

	public FloatyName FloatyName;

	public FloatyTimer FloatyTimer;

	public CanvasGroup NameContainer;

	public GameObject NameTagMode;

	public GameObject PNumberMode;

	public CanvasGroup ArrowMode;

	public Image ArrowImage;

	public TextMeshProUGUI NameTagText;

	public GameObject NameTagBackground;

	public Image NameTagBgCenter;

	public Image NameTagBgRight;

	public Image NameTagBgLeft;

	public Sprite NameTagBgRedCenter;

	public Sprite NameTagBgRedSide;

	public Sprite NameTagBgBlueCenter;

	public Sprite NameTagBgBlueSide;

	public float NameTagBounds = 20f;

	public Vector3 Offset = Vector3.zero;

	private bool isPaused;

	private PlayerController playerController;

	private Vector3 targetPosition;

	private float velocityX;

	private float velocityY;

	private Vector3 heightOffset;

	public IFrameOwner FrameOwner;

	private bool wasNameShownLastFrame;

	private int floatyNameShownFrame;

	private bool nameDisplayState;

	private Tweener nameAlphaTween;

	private bool arrowDisplayState;

	private Tweener arrowAlphaTween;

	private Dictionary<Color, Sprite> colorMap = new Dictionary<Color, Sprite>();

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public IUITextHelper textHelper
	{
		get;
		set;
	}

	public PlayerController PlayerController
	{
		get
		{
			return this.playerController;
		}
		set
		{
			this.playerController = value;
			this.NameTagMode.gameObject.SetActive(false);
			this.PNumberMode.gameObject.SetActive(false);
			this.ArrowMode.gameObject.SetActive(false);
			Color iconColor = this.playerController.iconColor;
			this.ArrowImage.sprite = this.colorMap[iconColor];
			if (!this.isAlwaysHidden())
			{
				if (this.isNametagMode())
				{
					this.textHelper.TrackText(this.NameTagText, new Action(this.onNameTagTextSized));
					this.NameTagMode.gameObject.SetActive(true);
					this.setNametagBackground();
					this.textHelper.UpdateText(this.NameTagText, this.getPlayerTagText());
				}
				else
				{
					this.PNumberMode.gameObject.SetActive(true);
					this.FloatyName.Init(this.playerController);
				}
			}
			this.heightOffset = (Vector3)this.playerController.CharacterMenusData.bounds.rotationCenterOffset;
			base.transform.position = this.calculateTargetPosition();
		}
	}

	public override void Awake()
	{
		base.Awake();
		this.colorMap[WColor.UIBlue] = this.BlueArrow;
		this.colorMap[WColor.UIRed] = this.RedArrow;
		this.colorMap[WColor.UIGreen] = this.GreenArrow;
		this.colorMap[WColor.UIYellow] = this.YellowArrow;
		this.colorMap[WColor.UIPurple] = this.PurpleArrow;
		this.colorMap[WColor.UIPink] = this.PinkArrow;
		this.colorMap[WColor.UIGrey] = this.GreyArrow;
	}

	private string getPlayerTagText()
	{
		if (this.isSingleLocalPlayer() && !PlayerUtil.IsNametag(this.playerController))
		{
			return this.localization.GetText("ui.characterSelect.you");
		}
		string text = PlayerUtil.GetPlayerNametag(this.localization, this.playerController);
		if (text.Length > base.config.uiuxSettings.floatyNameMaxSize)
		{
			text = text.Substring(0, base.config.uiuxSettings.floatyNameMaxSize);
		}
		return text;
	}

	private bool isAlwaysHidden()
	{
		if (!this.customNameOverride() && this.isSingleLocalPlayer())
		{
			if (base.battleServerAPI.IsSinglePlayerNetworkGame && !base.battleServerAPI.IsLocalPlayer(this.playerController.PlayerNum))
			{
				return true;
			}
			if (base.gameController.currentGame.IsTrainingMode && this.playerController.Reference.Type != PlayerType.Human)
			{
				return true;
			}
		}
		return false;
	}

	private bool customNameOverride()
	{
		return base.config.uiuxSettings.alwaysShowCustomNames && PlayerUtil.IsNametag(this.playerController);
	}

	private bool isNametagMode()
	{
		return PlayerUtil.IsNametag(this.playerController) || this.isSingleLocalPlayer();
	}

	private bool isSingleLocalPlayer()
	{
		return base.battleServerAPI.IsSinglePlayerNetworkGame || (base.gameController.currentGame.IsTrainingMode && this.getHumanCount() <= 1);
	}

	private int getHumanCount()
	{
		int num = 0;
		foreach (PlayerReference current in base.gameManager.PlayerReferences)
		{
			if (current.Type == PlayerType.Human)
			{
				num++;
			}
		}
		return num;
	}

	private void setNametagBackground()
	{
		GameModeSettings settings = base.gameController.currentGame.ModeData.settings;
		if (settings.usesTeamBarUI || settings.usesTeams)
		{
			UIColor uIColor = PlayerUtil.GetUIColor(this.playerController, settings.usesTeams);
			UnityEngine.Debug.Log(uIColor);
			Sprite overrideSprite;
			Sprite overrideSprite2;
			if (uIColor == UIColor.Red)
			{
				overrideSprite = this.NameTagBgRedCenter;
				overrideSprite2 = this.NameTagBgRedSide;
			}
			else
			{
				overrideSprite = this.NameTagBgBlueCenter;
				overrideSprite2 = this.NameTagBgBlueSide;
			}
			this.NameTagBgCenter.overrideSprite = overrideSprite;
			this.NameTagBgRight.overrideSprite = overrideSprite2;
			this.NameTagBgLeft.overrideSprite = overrideSprite2;
		}
	}

	private void onNameTagTextSized()
	{
		this.textHelper.UntrackText(this.NameTagText);
		RectTransform component = this.NameTagBackground.GetComponent<RectTransform>();
		Vector2 sizeDelta = component.sizeDelta;
		sizeDelta.x = this.NameTagText.renderedWidth / 1.4f + this.NameTagBounds * 2f;
		component.sizeDelta = sizeDelta;
	}

	private void Start()
	{
		bool active = this.isShowFloatyName();
		this.nameDisplayState = active;
		this.NameContainer.gameObject.SetActive(active);
	}

	private void Update()
	{
		if (this.playerController.Config == null || base.gameController == null || base.gameController.currentGame == null)
		{
			return;
		}
		bool flag = false;
		if (this.isPaused || this.playerController.State.IsDead || this.playerController.Reference.IsDisconnected)
		{
			this.FloatyTimer.gameObject.SetActive(false);
		}
		else
		{
			this.targetPosition = this.calculateTargetPosition();
			if ((this.targetPosition - base.transform.position).sqrMagnitude > 40000f)
			{
				base.transform.position = this.targetPosition;
			}
			else if (this.PlayerController.State.ActionState == ActionState.Dash || this.PlayerController.State.ActionState == ActionState.Run)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, this.targetPosition, base.config.uiuxSettings.floatyNameLerpSpeed);
			}
			else
			{
				base.transform.position = this.targetPosition;
			}
			if (this.PlayerController.IsTemporary)
			{
				this.FloatyTimer.gameObject.SetActive(true);
				this.FloatyTimer.SetValue((float)this.PlayerController.TemporaryDurationPercent);
			}
			else
			{
				this.FloatyTimer.gameObject.SetActive(false);
			}
			flag = this.isShowFloatyName();
		}
		if (flag && !this.wasNameShownLastFrame)
		{
			this.floatyNameShownFrame = this.FrameOwner.Frame;
		}
		this.setNameDisplayState(flag);
		this.setArrowDisplayState(this.showArrow());
		this.wasNameShownLastFrame = flag;
	}

	private bool showArrow()
	{
		return !this.playerController.State.IsDead && !this.playerController.IsActive;
	}

	private void setArrowDisplayState(bool state)
	{
		if (this.arrowDisplayState != state)
		{
			this.killArrowAlphaTween();
			this.arrowDisplayState = state;
			if (state)
			{
				this.ArrowMode.gameObject.SetActive(true);
				this.arrowAlphaTween = DOTween.To(new DOGetter<float>(this._setArrowDisplayState_m__0), new DOSetter<float>(this._setArrowDisplayState_m__1), 1f, 0.1f).SetEase(Ease.InSine).OnComplete(new TweenCallback(this.killArrowAlphaTween));
			}
			else
			{
				this.arrowAlphaTween = DOTween.To(new DOGetter<float>(this._setArrowDisplayState_m__2), new DOSetter<float>(this._setArrowDisplayState_m__3), 0f, 0.1f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.onArrowAlphaTweenOut));
			}
		}
	}

	private void onArrowAlphaTweenOut()
	{
		this.ArrowMode.gameObject.SetActive(false);
		this.killArrowAlphaTween();
	}

	private void killArrowAlphaTween()
	{
		TweenUtil.Destroy(ref this.arrowAlphaTween);
	}

	private void setNameDisplayState(bool state)
	{
		if (this.nameDisplayState != state)
		{
			this.killNameAlphaTween();
			this.nameDisplayState = state;
			if (state)
			{
				this.NameContainer.gameObject.SetActive(true);
				this.nameAlphaTween = DOTween.To(new DOGetter<float>(this._setNameDisplayState_m__4), new DOSetter<float>(this._setNameDisplayState_m__5), 1f, 0.1f).SetEase(Ease.InSine).OnComplete(new TweenCallback(this.killNameAlphaTween));
			}
			else
			{
				this.nameAlphaTween = DOTween.To(new DOGetter<float>(this._setNameDisplayState_m__6), new DOSetter<float>(this._setNameDisplayState_m__7), 0f, 0.1f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.onNameAlphaTweenOut));
			}
		}
	}

	private void onNameAlphaTweenOut()
	{
		this.NameContainer.gameObject.SetActive(false);
		this.killNameAlphaTween();
	}

	private void killNameAlphaTween()
	{
		TweenUtil.Destroy(ref this.nameAlphaTween);
	}

	private bool isShowFloatyName()
	{
		if (!this.playerController.Config.uiuxSettings.floatyNameHiding)
		{
			return true;
		}
		if (this.playerController.IsActive)
		{
			if (this.playerController.Config.uiuxSettings.alwaysShowCustomNames && PlayerUtil.IsNametag(this.playerController))
			{
				return true;
			}
			if (this.FrameOwner.Frame <= this.playerController.Config.uiuxSettings.floatyNamesAtMatchBegin)
			{
				return true;
			}
			if (this.FrameOwner.Frame - this.floatyNameShownFrame <= this.playerController.Config.uiuxSettings.floatyNameMinShow)
			{
				return true;
			}
			if (this.playerController.Config.uiuxSettings.floatyNameShowRespawning && this.playerController.State.IsRespawning)
			{
				return true;
			}
			if (this.playerController.Config.uiuxSettings.floatyNamesIdleShow > 0 && this.playerController.Model.userIdleFrames >= this.playerController.Config.uiuxSettings.floatyNamesIdleShow)
			{
				return true;
			}
			if (this.playerController.Model.floatyNameStun && this.playerController.Model.hitLagFrames <= 0 && (this.playerController.Model.actionState == ActionState.DownedLoop || this.playerController.Model.actionState == ActionState.FallDown || this.playerController.Model.actionState == ActionState.Tumble || (this.playerController.Model.stunType == StunType.HitStun && this.playerController.Model.stunFrames > 0)))
			{
				return true;
			}
		}
		return false;
	}

	public void UpdateDisplayState()
	{
		this.FloatyName.UpdateDisplayState();
	}

	public void OnPaused(bool isPaused)
	{
		this.isPaused = isPaused;
		this.UpdateDisplayState();
	}

	private Vector3 calculateTargetPosition()
	{
		Vector3 a = (Vector3)this.playerController.Position;
		return base.gameManager.Camera.current.WorldToScreenPoint(a + this.heightOffset + this.Offset + this.playerController.CharacterMenusData.bounds.floatyUIOffset);
	}

	private float _setArrowDisplayState_m__0()
	{
		return this.ArrowMode.alpha;
	}

	private void _setArrowDisplayState_m__1(float valueIn)
	{
		this.ArrowMode.alpha = valueIn;
	}

	private float _setArrowDisplayState_m__2()
	{
		return this.ArrowMode.alpha;
	}

	private void _setArrowDisplayState_m__3(float valueIn)
	{
		this.ArrowMode.alpha = valueIn;
	}

	private float _setNameDisplayState_m__4()
	{
		return this.NameContainer.alpha;
	}

	private void _setNameDisplayState_m__5(float valueIn)
	{
		this.NameContainer.alpha = valueIn;
	}

	private float _setNameDisplayState_m__6()
	{
		return this.NameContainer.alpha;
	}

	private void _setNameDisplayState_m__7(float valueIn)
	{
		this.NameContainer.alpha = valueIn;
	}
}
