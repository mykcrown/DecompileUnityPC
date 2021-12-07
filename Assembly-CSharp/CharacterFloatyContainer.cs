using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008BC RID: 2236
public class CharacterFloatyContainer : GameBehavior
{
	// Token: 0x17000DA3 RID: 3491
	// (get) Token: 0x06003849 RID: 14409 RVA: 0x00107FC5 File Offset: 0x001063C5
	// (set) Token: 0x0600384A RID: 14410 RVA: 0x00107FCD File Offset: 0x001063CD
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000DA4 RID: 3492
	// (get) Token: 0x0600384B RID: 14411 RVA: 0x00107FD6 File Offset: 0x001063D6
	// (set) Token: 0x0600384C RID: 14412 RVA: 0x00107FDE File Offset: 0x001063DE
	[Inject]
	public IUITextHelper textHelper { get; set; }

	// Token: 0x0600384D RID: 14413 RVA: 0x00107FE8 File Offset: 0x001063E8
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

	// Token: 0x17000DA5 RID: 3493
	// (get) Token: 0x0600384E RID: 14414 RVA: 0x00108095 File Offset: 0x00106495
	// (set) Token: 0x0600384F RID: 14415 RVA: 0x001080A0 File Offset: 0x001064A0
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

	// Token: 0x06003850 RID: 14416 RVA: 0x001081C4 File Offset: 0x001065C4
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

	// Token: 0x06003851 RID: 14417 RVA: 0x00108244 File Offset: 0x00106644
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

	// Token: 0x06003852 RID: 14418 RVA: 0x001082C1 File Offset: 0x001066C1
	private bool customNameOverride()
	{
		return base.config.uiuxSettings.alwaysShowCustomNames && PlayerUtil.IsNametag(this.playerController);
	}

	// Token: 0x06003853 RID: 14419 RVA: 0x001082EB File Offset: 0x001066EB
	private bool isNametagMode()
	{
		return PlayerUtil.IsNametag(this.playerController) || this.isSingleLocalPlayer();
	}

	// Token: 0x06003854 RID: 14420 RVA: 0x0010830D File Offset: 0x0010670D
	private bool isSingleLocalPlayer()
	{
		return base.battleServerAPI.IsSinglePlayerNetworkGame || (base.gameController.currentGame.IsTrainingMode && this.getHumanCount() <= 1);
	}

	// Token: 0x06003855 RID: 14421 RVA: 0x00108348 File Offset: 0x00106748
	private int getHumanCount()
	{
		int num = 0;
		foreach (PlayerReference playerReference in base.gameManager.PlayerReferences)
		{
			if (playerReference.Type == PlayerType.Human)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06003856 RID: 14422 RVA: 0x001083B4 File Offset: 0x001067B4
	private void setNametagBackground()
	{
		GameModeSettings settings = base.gameController.currentGame.ModeData.settings;
		if (settings.usesTeamBarUI || settings.usesTeams)
		{
			UIColor uicolor = PlayerUtil.GetUIColor(this.playerController, settings.usesTeams);
			Debug.Log(uicolor);
			Sprite overrideSprite;
			Sprite overrideSprite2;
			if (uicolor == UIColor.Red)
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

	// Token: 0x06003857 RID: 14423 RVA: 0x00108458 File Offset: 0x00106858
	private void onNameTagTextSized()
	{
		this.textHelper.UntrackText(this.NameTagText);
		RectTransform component = this.NameTagBackground.GetComponent<RectTransform>();
		Vector2 sizeDelta = component.sizeDelta;
		sizeDelta.x = this.NameTagText.renderedWidth / 1.4f + this.NameTagBounds * 2f;
		component.sizeDelta = sizeDelta;
	}

	// Token: 0x06003858 RID: 14424 RVA: 0x001084B8 File Offset: 0x001068B8
	private void Start()
	{
		bool active = this.isShowFloatyName();
		this.nameDisplayState = active;
		this.NameContainer.gameObject.SetActive(active);
	}

	// Token: 0x06003859 RID: 14425 RVA: 0x001084E4 File Offset: 0x001068E4
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

	// Token: 0x0600385A RID: 14426 RVA: 0x001086CA File Offset: 0x00106ACA
	private bool showArrow()
	{
		return !this.playerController.State.IsDead && !this.playerController.IsActive;
	}

	// Token: 0x0600385B RID: 14427 RVA: 0x001086F4 File Offset: 0x00106AF4
	private void setArrowDisplayState(bool state)
	{
		if (this.arrowDisplayState != state)
		{
			this.killArrowAlphaTween();
			this.arrowDisplayState = state;
			if (state)
			{
				this.ArrowMode.gameObject.SetActive(true);
				this.arrowAlphaTween = DOTween.To(() => this.ArrowMode.alpha, delegate(float valueIn)
				{
					this.ArrowMode.alpha = valueIn;
				}, 1f, 0.1f).SetEase(Ease.InSine).OnComplete(new TweenCallback(this.killArrowAlphaTween));
			}
			else
			{
				this.arrowAlphaTween = DOTween.To(() => this.ArrowMode.alpha, delegate(float valueIn)
				{
					this.ArrowMode.alpha = valueIn;
				}, 0f, 0.1f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.onArrowAlphaTweenOut));
			}
		}
	}

	// Token: 0x0600385C RID: 14428 RVA: 0x001087BE File Offset: 0x00106BBE
	private void onArrowAlphaTweenOut()
	{
		this.ArrowMode.gameObject.SetActive(false);
		this.killArrowAlphaTween();
	}

	// Token: 0x0600385D RID: 14429 RVA: 0x001087D7 File Offset: 0x00106BD7
	private void killArrowAlphaTween()
	{
		TweenUtil.Destroy(ref this.arrowAlphaTween);
	}

	// Token: 0x0600385E RID: 14430 RVA: 0x001087E4 File Offset: 0x00106BE4
	private void setNameDisplayState(bool state)
	{
		if (this.nameDisplayState != state)
		{
			this.killNameAlphaTween();
			this.nameDisplayState = state;
			if (state)
			{
				this.NameContainer.gameObject.SetActive(true);
				this.nameAlphaTween = DOTween.To(() => this.NameContainer.alpha, delegate(float valueIn)
				{
					this.NameContainer.alpha = valueIn;
				}, 1f, 0.1f).SetEase(Ease.InSine).OnComplete(new TweenCallback(this.killNameAlphaTween));
			}
			else
			{
				this.nameAlphaTween = DOTween.To(() => this.NameContainer.alpha, delegate(float valueIn)
				{
					this.NameContainer.alpha = valueIn;
				}, 0f, 0.1f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this.onNameAlphaTweenOut));
			}
		}
	}

	// Token: 0x0600385F RID: 14431 RVA: 0x001088AE File Offset: 0x00106CAE
	private void onNameAlphaTweenOut()
	{
		this.NameContainer.gameObject.SetActive(false);
		this.killNameAlphaTween();
	}

	// Token: 0x06003860 RID: 14432 RVA: 0x001088C7 File Offset: 0x00106CC7
	private void killNameAlphaTween()
	{
		TweenUtil.Destroy(ref this.nameAlphaTween);
	}

	// Token: 0x06003861 RID: 14433 RVA: 0x001088D4 File Offset: 0x00106CD4
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

	// Token: 0x06003862 RID: 14434 RVA: 0x00108AA4 File Offset: 0x00106EA4
	public void UpdateDisplayState()
	{
		this.FloatyName.UpdateDisplayState();
	}

	// Token: 0x06003863 RID: 14435 RVA: 0x00108AB1 File Offset: 0x00106EB1
	public void OnPaused(bool isPaused)
	{
		this.isPaused = isPaused;
		this.UpdateDisplayState();
	}

	// Token: 0x06003864 RID: 14436 RVA: 0x00108AC0 File Offset: 0x00106EC0
	private Vector3 calculateTargetPosition()
	{
		Vector3 a = (Vector3)this.playerController.Position;
		return base.gameManager.Camera.current.WorldToScreenPoint(a + this.heightOffset + this.Offset + this.playerController.CharacterMenusData.bounds.floatyUIOffset);
	}

	// Token: 0x040026A1 RID: 9889
	private const float SMOOTH_TIME_FACTOR_X = 0.02f;

	// Token: 0x040026A2 RID: 9890
	private const float SMOOTH_TIME_FACTOR_Y = 0.05f;

	// Token: 0x040026A3 RID: 9891
	private const float MAX_SMOOTH_DISTANCE = 200f;

	// Token: 0x040026A4 RID: 9892
	private const float MAX_SMOOTH_DISTANCE_SQR = 40000f;

	// Token: 0x040026A5 RID: 9893
	private const float OFFSET_Y = -1.27f;

	// Token: 0x040026A8 RID: 9896
	public Sprite BlueArrow;

	// Token: 0x040026A9 RID: 9897
	public Sprite RedArrow;

	// Token: 0x040026AA RID: 9898
	public Sprite GreenArrow;

	// Token: 0x040026AB RID: 9899
	public Sprite YellowArrow;

	// Token: 0x040026AC RID: 9900
	public Sprite PurpleArrow;

	// Token: 0x040026AD RID: 9901
	public Sprite PinkArrow;

	// Token: 0x040026AE RID: 9902
	public Sprite GreyArrow;

	// Token: 0x040026AF RID: 9903
	public FloatyName FloatyName;

	// Token: 0x040026B0 RID: 9904
	public FloatyTimer FloatyTimer;

	// Token: 0x040026B1 RID: 9905
	public CanvasGroup NameContainer;

	// Token: 0x040026B2 RID: 9906
	public GameObject NameTagMode;

	// Token: 0x040026B3 RID: 9907
	public GameObject PNumberMode;

	// Token: 0x040026B4 RID: 9908
	public CanvasGroup ArrowMode;

	// Token: 0x040026B5 RID: 9909
	public Image ArrowImage;

	// Token: 0x040026B6 RID: 9910
	public TextMeshProUGUI NameTagText;

	// Token: 0x040026B7 RID: 9911
	public GameObject NameTagBackground;

	// Token: 0x040026B8 RID: 9912
	public Image NameTagBgCenter;

	// Token: 0x040026B9 RID: 9913
	public Image NameTagBgRight;

	// Token: 0x040026BA RID: 9914
	public Image NameTagBgLeft;

	// Token: 0x040026BB RID: 9915
	public Sprite NameTagBgRedCenter;

	// Token: 0x040026BC RID: 9916
	public Sprite NameTagBgRedSide;

	// Token: 0x040026BD RID: 9917
	public Sprite NameTagBgBlueCenter;

	// Token: 0x040026BE RID: 9918
	public Sprite NameTagBgBlueSide;

	// Token: 0x040026BF RID: 9919
	public float NameTagBounds = 20f;

	// Token: 0x040026C0 RID: 9920
	public Vector3 Offset = Vector3.zero;

	// Token: 0x040026C1 RID: 9921
	private bool isPaused;

	// Token: 0x040026C2 RID: 9922
	private PlayerController playerController;

	// Token: 0x040026C3 RID: 9923
	private Vector3 targetPosition;

	// Token: 0x040026C4 RID: 9924
	private float velocityX;

	// Token: 0x040026C5 RID: 9925
	private float velocityY;

	// Token: 0x040026C6 RID: 9926
	private Vector3 heightOffset;

	// Token: 0x040026C7 RID: 9927
	public IFrameOwner FrameOwner;

	// Token: 0x040026C8 RID: 9928
	private bool wasNameShownLastFrame;

	// Token: 0x040026C9 RID: 9929
	private int floatyNameShownFrame;

	// Token: 0x040026CA RID: 9930
	private bool nameDisplayState;

	// Token: 0x040026CB RID: 9931
	private Tweener nameAlphaTween;

	// Token: 0x040026CC RID: 9932
	private bool arrowDisplayState;

	// Token: 0x040026CD RID: 9933
	private Tweener arrowAlphaTween;

	// Token: 0x040026CE RID: 9934
	private Dictionary<Color, Sprite> colorMap = new Dictionary<Color, Sprite>();
}
