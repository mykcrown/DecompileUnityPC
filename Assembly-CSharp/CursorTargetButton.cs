using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000934 RID: 2356
public class CursorTargetButton : Button, ISubmitHandler, IAltSubmitHandler, IFaceButton3Handler, IAnimatableButton, IEventSystemHandler
{
	// Token: 0x17000EC5 RID: 3781
	// (get) Token: 0x06003DEF RID: 15855 RVA: 0x0011B7ED File Offset: 0x00119BED
	// (set) Token: 0x06003DF0 RID: 15856 RVA: 0x0011B7F5 File Offset: 0x00119BF5
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17000EC6 RID: 3782
	// (get) Token: 0x06003DF1 RID: 15857 RVA: 0x0011B7FE File Offset: 0x00119BFE
	// (set) Token: 0x06003DF2 RID: 15858 RVA: 0x0011B806 File Offset: 0x00119C06
	public bool RepeatOnHold { get; set; }

	// Token: 0x17000EC7 RID: 3783
	// (get) Token: 0x06003DF3 RID: 15859 RVA: 0x0011B80F File Offset: 0x00119C0F
	public Image ButtonBackgroundGet
	{
		get
		{
			return this.CursorHoverImage;
		}
	}

	// Token: 0x17000EC8 RID: 3784
	// (get) Token: 0x06003DF4 RID: 15860 RVA: 0x0011B817 File Offset: 0x00119C17
	public List<Image> AdditionalImagesGet
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000EC9 RID: 3785
	// (get) Token: 0x06003DF5 RID: 15861 RVA: 0x0011B81A File Offset: 0x00119C1A
	public TextMeshProUGUI TextFieldGet
	{
		get
		{
			return this.TextField;
		}
	}

	// Token: 0x17000ECA RID: 3786
	// (get) Token: 0x06003DF6 RID: 15862 RVA: 0x0011B822 File Offset: 0x00119C22
	public CanvasGroup FadeCanvasGet
	{
		get
		{
			return this.CanvasGroup;
		}
	}

	// Token: 0x17000ECB RID: 3787
	// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x0011B82A File Offset: 0x00119C2A
	// (set) Token: 0x06003DF8 RID: 15864 RVA: 0x0011B832 File Offset: 0x00119C32
	public bool UseOverrideHighlightSound { get; set; }

	// Token: 0x17000ECC RID: 3788
	// (get) Token: 0x06003DF9 RID: 15865 RVA: 0x0011B83B File Offset: 0x00119C3B
	// (set) Token: 0x06003DFA RID: 15866 RVA: 0x0011B843 File Offset: 0x00119C43
	public AudioData OverrideHighlightSound { get; set; }

	// Token: 0x17000ECD RID: 3789
	// (get) Token: 0x06003DFB RID: 15867 RVA: 0x0011B84C File Offset: 0x00119C4C
	// (set) Token: 0x06003DFC RID: 15868 RVA: 0x0011B859 File Offset: 0x00119C59
	public ButtonAnimator.VisualDisableType DisableType
	{
		get
		{
			return this.buttonAnimator.DisableType;
		}
		set
		{
			this.buttonAnimator.DisableType = value;
		}
	}

	// Token: 0x17000ECE RID: 3790
	// (get) Token: 0x06003DFD RID: 15869 RVA: 0x0011B867 File Offset: 0x00119C67
	// (set) Token: 0x06003DFE RID: 15870 RVA: 0x0011B874 File Offset: 0x00119C74
	public float DisableDuration
	{
		get
		{
			return this.buttonAnimator.DisableDuration;
		}
		set
		{
			this.buttonAnimator.DisableDuration = value;
		}
	}

	// Token: 0x17000ECF RID: 3791
	// (get) Token: 0x06003DFF RID: 15871 RVA: 0x0011B882 File Offset: 0x00119C82
	private ButtonAnimator buttonAnimator
	{
		get
		{
			if (this._buttonAnimator == null)
			{
				this._buttonAnimator = new ButtonAnimator(this);
			}
			return this._buttonAnimator;
		}
	}

	// Token: 0x06003E00 RID: 15872 RVA: 0x0011B8A1 File Offset: 0x00119CA1
	protected override void Awake()
	{
		base.Awake();
		StaticInject.Inject(this);
		if (this.UseTextColorHighlight && this.TextHighlightField != null)
		{
			this.originalTextColor = this.TextHighlightField.color;
		}
	}

	// Token: 0x06003E01 RID: 15873 RVA: 0x0011B8DC File Offset: 0x00119CDC
	public void SetInteractable(bool value)
	{
		base.gameObject.SetActive(value);
		if (!value)
		{
			this.buttonAnimator.PlayDisable();
		}
		else
		{
			this.buttonAnimator.PlayEnable();
		}
	}

	// Token: 0x06003E02 RID: 15874 RVA: 0x0011B90B File Offset: 0x00119D0B
	public bool IsAuthorized(BaseEventData eventData)
	{
		return this.isAuthorized(eventData);
	}

	// Token: 0x06003E03 RID: 15875 RVA: 0x0011B914 File Offset: 0x00119D14
	public bool IsAuthorized(PlayerNum player)
	{
		return this.isAuthorized(player);
	}

	// Token: 0x06003E04 RID: 15876 RVA: 0x0011B91D File Offset: 0x00119D1D
	private bool isAuthorized(BaseEventData eventData)
	{
		return !(eventData is PointerEventData) || this.isAuthorized(this.getClickingPlayer(eventData));
	}

	// Token: 0x06003E05 RID: 15877 RVA: 0x0011B939 File Offset: 0x00119D39
	private PlayerNum getClickingPlayer(BaseEventData eventData)
	{
		if (!(eventData is PointerEventData))
		{
			return PlayerNum.None;
		}
		return PlayerUtil.GetPointerEventOwner(eventData as PointerEventData);
	}

	// Token: 0x06003E06 RID: 15878 RVA: 0x0011B954 File Offset: 0x00119D54
	private bool isAuthorized(PlayerNum clickingPlayer)
	{
		return !this.requireAuthorization || this.authorizedPlayers.Contains(clickingPlayer);
	}

	// Token: 0x06003E07 RID: 15879 RVA: 0x0011B978 File Offset: 0x00119D78
	public void RequireAuthorization(PlayerNum owner)
	{
		this.requireAuthorization = true;
		if (!this.authorizedPlayers.Contains(owner))
		{
			this.authorizedPlayers.Add(owner);
		}
		int count = this.selectionList.Count;
		for (int i = this.selectionList.Count - 1; i >= 0; i--)
		{
			PlayerNum item = this.selectionList[i];
			if (!this.authorizedPlayers.Contains(item))
			{
				this.selectionList.RemoveAt(i);
			}
		}
		this.updateAfterSelectionListRemoval(count);
	}

	// Token: 0x06003E08 RID: 15880 RVA: 0x0011BA08 File Offset: 0x00119E08
	public void RemovePlayerSelect(PlayerNum removePlayer)
	{
		int count = this.selectionList.Count;
		for (int i = this.selectionList.Count - 1; i >= 0; i--)
		{
			if (this.selectionList[i] == removePlayer)
			{
				this.selectionList.RemoveAt(i);
			}
		}
		this.updateAfterSelectionListRemoval(count);
	}

	// Token: 0x06003E09 RID: 15881 RVA: 0x0011BA64 File Offset: 0x00119E64
	public void DisableAuthorization()
	{
		this.requireAuthorization = false;
	}

	// Token: 0x06003E0A RID: 15882 RVA: 0x0011BA6D File Offset: 0x00119E6D
	private void updateAfterSelectionListRemoval(int prevCount)
	{
		if (this.selectionList.Count <= 0 && prevCount > 0)
		{
			this.hideHighlight();
		}
	}

	// Token: 0x06003E0B RID: 15883 RVA: 0x0011BA8D File Offset: 0x00119E8D
	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		if (this.ClickCallback != null)
		{
			this.ClickCallback(this);
		}
	}

	// Token: 0x06003E0C RID: 15884 RVA: 0x0011BAB0 File Offset: 0x00119EB0
	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		if (this.UseOverrideHighlightSound)
		{
			this.audioManager.PlayMenuSound(this.OverrideHighlightSound, 0f);
		}
		else
		{
			this.audioManager.PlayMenuSound(SoundKey.generic_buttonHighlight, 0f);
		}
		if (!(eventData is PointerEventData))
		{
			this.showHighlight();
		}
		if (this.BasicSelectCallback != null)
		{
			this.BasicSelectCallback(this);
		}
	}

	// Token: 0x06003E0D RID: 15885 RVA: 0x0011BB23 File Offset: 0x00119F23
	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		if (!(eventData is PointerEventData))
		{
			this.hideHighlight();
		}
		if (this.BasicDeselectCallback != null)
		{
			this.BasicDeselectCallback(this);
		}
	}

	// Token: 0x06003E0E RID: 15886 RVA: 0x0011BB54 File Offset: 0x00119F54
	public override void OnSubmit(BaseEventData eventData)
	{
		if (!this.isAuthorized(eventData))
		{
			return;
		}
		base.OnSubmit(eventData);
		if (this.SubmitCallback != null)
		{
			this.SubmitCallback(this, eventData as PointerEventData);
		}
	}

	// Token: 0x06003E0F RID: 15887 RVA: 0x0011BB87 File Offset: 0x00119F87
	public void OnAltSubmit(BaseEventData eventData)
	{
		if (!this.isAuthorized(eventData))
		{
			return;
		}
		if (this.AltSubmitCallback != null)
		{
			this.AltSubmitCallback(this, eventData as PointerEventData);
		}
	}

	// Token: 0x06003E10 RID: 15888 RVA: 0x0011BBB3 File Offset: 0x00119FB3
	public void OnFaceButton3(BaseEventData eventData)
	{
		if (!this.isAuthorized(eventData))
		{
			return;
		}
		if (this.FaceButton3Callback != null)
		{
			this.FaceButton3Callback(this, eventData as PointerEventData);
		}
	}

	// Token: 0x06003E11 RID: 15889 RVA: 0x0011BBE0 File Offset: 0x00119FE0
	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (!this.isAuthorized(eventData))
		{
			return;
		}
		PlayerNum clickingPlayer = this.getClickingPlayer(eventData);
		int count = this.selectionList.Count;
		if (!this.selectionList.Contains(clickingPlayer))
		{
			this.selectionList.Add(clickingPlayer);
		}
		if (count == 0 && this.selectionList.Count == 1)
		{
			this.showHighlight();
		}
		if (this.SelectCallback != null)
		{
			this.SelectCallback(this, eventData);
		}
	}

	// Token: 0x06003E12 RID: 15890 RVA: 0x0011BC60 File Offset: 0x0011A060
	public override void OnPointerExit(PointerEventData eventData)
	{
		if (!this.isAuthorized(eventData))
		{
			return;
		}
		PlayerNum clickingPlayer = this.getClickingPlayer(eventData);
		int count = this.selectionList.Count;
		this.selectionList.Remove(clickingPlayer);
		this.updateAfterSelectionListRemoval(count);
		if (this.DeselectCallback != null)
		{
			this.DeselectCallback(this, eventData);
		}
	}

	// Token: 0x06003E13 RID: 15891 RVA: 0x0011BCBC File Offset: 0x0011A0BC
	private void showHighlight()
	{
		if (this.CursorHoverImage != null && this.CursorHoverSprite != null)
		{
			this.CursorHoverImage.overrideSprite = this.CursorHoverSprite;
		}
		if (this.ScaleUpOnMouseover != 1f)
		{
			this.killScaleTween();
			Vector3 endValue = new Vector3(this.ScaleUpOnMouseover, this.ScaleUpOnMouseover, this.ScaleUpOnMouseover);
			this.scaleTween = DOTween.To(() => this.ScaleUpMouseoverContainer.transform.localScale, delegate(Vector3 x)
			{
				this.ScaleUpMouseoverContainer.transform.localScale = x;
			}, endValue, 0.07f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killScaleTween));
		}
		if (this.UseTextColorHighlight)
		{
			if (this.TextHighlightField.color != this.TextColorHighlight)
			{
				this.originalTextColor = this.TextHighlightField.color;
			}
			this.TextHighlightField.color = this.TextColorHighlight;
		}
		this.isManualHighlight = true;
		if (this.HighlightCallback != null)
		{
			this.HighlightCallback(this);
		}
	}

	// Token: 0x06003E14 RID: 15892 RVA: 0x0011BDD4 File Offset: 0x0011A1D4
	private void hideHighlight()
	{
		if (this.CursorHoverImage != null && this.CursorHoverSprite != null)
		{
			this.CursorHoverImage.overrideSprite = null;
		}
		if (this.ScaleUpOnMouseover != 1f)
		{
			this.killScaleTween();
			Vector3 endValue = new Vector3(1f, 1f, 1f);
			this.scaleTween = DOTween.To(() => this.ScaleUpMouseoverContainer.transform.localScale, delegate(Vector3 x)
			{
				this.ScaleUpMouseoverContainer.transform.localScale = x;
			}, endValue, 0.07f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killScaleTween));
		}
		if (this.UseTextColorHighlight)
		{
			this.TextHighlightField.color = this.originalTextColor;
		}
		this.isManualHighlight = false;
		if (this.UnhighlightCallback != null)
		{
			this.UnhighlightCallback(this);
		}
	}

	// Token: 0x17000ED0 RID: 3792
	// (get) Token: 0x06003E15 RID: 15893 RVA: 0x0011BEB5 File Offset: 0x0011A2B5
	public bool IsManualHighlight
	{
		get
		{
			return this.isManualHighlight;
		}
	}

	// Token: 0x06003E16 RID: 15894 RVA: 0x0011BEBD File Offset: 0x0011A2BD
	private void killScaleTween()
	{
		if (this.scaleTween != null)
		{
			if (this.scaleTween.IsPlaying())
			{
				this.scaleTween.Kill(false);
			}
			this.scaleTween = null;
		}
	}

	// Token: 0x06003E17 RID: 15895 RVA: 0x0011BEED File Offset: 0x0011A2ED
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (!this.isAuthorized(eventData))
		{
			return;
		}
		base.OnPointerDown(eventData);
		this.pointerDownEventData = eventData;
		this.pointerDownTime = WTime.currentTimeMs;
	}

	// Token: 0x06003E18 RID: 15896 RVA: 0x0011BF15 File Offset: 0x0011A315
	public override void OnPointerUp(PointerEventData eventData)
	{
		if (!this.isAuthorized(eventData))
		{
			return;
		}
		base.OnPointerUp(eventData);
		this.pointerDownTime = 0L;
	}

	// Token: 0x06003E19 RID: 15897 RVA: 0x0011BF34 File Offset: 0x0011A334
	private void Update()
	{
		if (this.RepeatOnHold)
		{
			if (this.selectionList.Count == 0)
			{
				this.pointerDownTime = 0L;
			}
			if (this.pointerDownTime != 0L)
			{
				long num = WTime.currentTimeMs - this.pointerDownTime;
				if (num > (long)this.RepeatBeginMs)
				{
					long num2 = WTime.currentTimeMs - this.lastRepeatAt;
					if (num2 > (long)this.RepeatFrequency)
					{
						this.lastRepeatAt = WTime.currentTimeMs;
						this.OnSubmit(this.pointerDownEventData);
					}
				}
			}
		}
	}

	// Token: 0x06003E1A RID: 15898 RVA: 0x0011BFBC File Offset: 0x0011A3BC
	public void Removed()
	{
		if (this.selectionList.Count > 0)
		{
			this.selectionList.Clear();
			this.hideHighlight();
			if (this.DeselectCallback != null)
			{
				this.DeselectCallback(this, null);
			}
		}
	}

	// Token: 0x06003E1B RID: 15899 RVA: 0x0011BFF8 File Offset: 0x0011A3F8
	protected override void OnDestroy()
	{
		this.buttonAnimator.OnDestroy();
		this.SubmitCallback = null;
		this.AltSubmitCallback = null;
		this.SelectCallback = null;
		this.DeselectCallback = null;
		this.FaceButton3Callback = null;
		this.BasicSelectCallback = null;
		this.BasicDeselectCallback = null;
	}

	// Token: 0x04002A16 RID: 10774
	public Action<CursorTargetButton, PointerEventData> SubmitCallback;

	// Token: 0x04002A17 RID: 10775
	public Action<CursorTargetButton, PointerEventData> AltSubmitCallback;

	// Token: 0x04002A18 RID: 10776
	public Action<CursorTargetButton, PointerEventData> SelectCallback;

	// Token: 0x04002A19 RID: 10777
	public Action<CursorTargetButton, PointerEventData> DeselectCallback;

	// Token: 0x04002A1A RID: 10778
	public Action<CursorTargetButton, PointerEventData> FaceButton3Callback;

	// Token: 0x04002A1B RID: 10779
	public Action<CursorTargetButton> HighlightCallback;

	// Token: 0x04002A1C RID: 10780
	public Action<CursorTargetButton> UnhighlightCallback;

	// Token: 0x04002A1D RID: 10781
	public Action<CursorTargetButton> BasicSelectCallback;

	// Token: 0x04002A1E RID: 10782
	public Action<CursorTargetButton> BasicDeselectCallback;

	// Token: 0x04002A1F RID: 10783
	public Action<CursorTargetButton> ClickCallback;

	// Token: 0x04002A20 RID: 10784
	public Image CursorHoverImage;

	// Token: 0x04002A21 RID: 10785
	public Sprite CursorHoverSprite;

	// Token: 0x04002A23 RID: 10787
	public int RepeatBeginMs = 500;

	// Token: 0x04002A24 RID: 10788
	public int RepeatFrequency = 50;

	// Token: 0x04002A25 RID: 10789
	public GameObject ScaleUpMouseoverContainer;

	// Token: 0x04002A26 RID: 10790
	public float ScaleUpOnMouseover = 1f;

	// Token: 0x04002A27 RID: 10791
	public bool UseTextColorHighlight;

	// Token: 0x04002A28 RID: 10792
	public Color TextColorHighlight;

	// Token: 0x04002A29 RID: 10793
	public TextMeshProUGUI TextHighlightField;

	// Token: 0x04002A2A RID: 10794
	public TextMeshProUGUI TextField;

	// Token: 0x04002A2D RID: 10797
	private List<PlayerNum> selectionList = new List<PlayerNum>();

	// Token: 0x04002A2E RID: 10798
	private Color originalTextColor;

	// Token: 0x04002A2F RID: 10799
	private long lastRepeatAt;

	// Token: 0x04002A30 RID: 10800
	private long pointerDownTime;

	// Token: 0x04002A31 RID: 10801
	private PointerEventData pointerDownEventData;

	// Token: 0x04002A32 RID: 10802
	private Tweener scaleTween;

	// Token: 0x04002A33 RID: 10803
	private bool isManualHighlight;

	// Token: 0x04002A34 RID: 10804
	public CanvasGroup CanvasGroup;

	// Token: 0x04002A35 RID: 10805
	private bool requireAuthorization;

	// Token: 0x04002A36 RID: 10806
	private HashSet<PlayerNum> authorizedPlayers = new HashSet<PlayerNum>(default(PlayerNumComparer));

	// Token: 0x04002A37 RID: 10807
	private ButtonAnimator _buttonAnimator;
}
