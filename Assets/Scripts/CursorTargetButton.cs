// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorTargetButton : Button, ISubmitHandler, IAltSubmitHandler, IFaceButton3Handler, IAnimatableButton, IEventSystemHandler
{
	public Action<CursorTargetButton, PointerEventData> SubmitCallback;

	public Action<CursorTargetButton, PointerEventData> AltSubmitCallback;

	public Action<CursorTargetButton, PointerEventData> SelectCallback;

	public Action<CursorTargetButton, PointerEventData> DeselectCallback;

	public Action<CursorTargetButton, PointerEventData> FaceButton3Callback;

	public Action<CursorTargetButton> HighlightCallback;

	public Action<CursorTargetButton> UnhighlightCallback;

	public Action<CursorTargetButton> BasicSelectCallback;

	public Action<CursorTargetButton> BasicDeselectCallback;

	public Action<CursorTargetButton> ClickCallback;

	public Image CursorHoverImage;

	public Sprite CursorHoverSprite;

	public int RepeatBeginMs = 500;

	public int RepeatFrequency = 50;

	public GameObject ScaleUpMouseoverContainer;

	public float ScaleUpOnMouseover = 1f;

	public bool UseTextColorHighlight;

	public Color TextColorHighlight;

	public TextMeshProUGUI TextHighlightField;

	public TextMeshProUGUI TextField;

	private List<PlayerNum> selectionList = new List<PlayerNum>();

	private Color originalTextColor;

	private long lastRepeatAt;

	private long pointerDownTime;

	private PointerEventData pointerDownEventData;

	private Tweener scaleTween;

	private bool isManualHighlight;

	public CanvasGroup CanvasGroup;

	private bool requireAuthorization;

	private HashSet<PlayerNum> authorizedPlayers = new HashSet<PlayerNum>(default(PlayerNumComparer));

	private ButtonAnimator _buttonAnimator;

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	public bool RepeatOnHold
	{
		get;
		set;
	}

	public Image ButtonBackgroundGet
	{
		get
		{
			return this.CursorHoverImage;
		}
	}

	public List<Image> AdditionalImagesGet
	{
		get
		{
			return null;
		}
	}

	public TextMeshProUGUI TextFieldGet
	{
		get
		{
			return this.TextField;
		}
	}

	public CanvasGroup FadeCanvasGet
	{
		get
		{
			return this.CanvasGroup;
		}
	}

	public bool UseOverrideHighlightSound
	{
		get;
		set;
	}

	public AudioData OverrideHighlightSound
	{
		get;
		set;
	}

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

	public bool IsManualHighlight
	{
		get
		{
			return this.isManualHighlight;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		StaticInject.Inject(this);
		if (this.UseTextColorHighlight && this.TextHighlightField != null)
		{
			this.originalTextColor = this.TextHighlightField.color;
		}
	}

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

	public bool IsAuthorized(BaseEventData eventData)
	{
		return this.isAuthorized(eventData);
	}

	public bool IsAuthorized(PlayerNum player)
	{
		return this.isAuthorized(player);
	}

	private bool isAuthorized(BaseEventData eventData)
	{
		return !(eventData is PointerEventData) || this.isAuthorized(this.getClickingPlayer(eventData));
	}

	private PlayerNum getClickingPlayer(BaseEventData eventData)
	{
		if (!(eventData is PointerEventData))
		{
			return PlayerNum.None;
		}
		return PlayerUtil.GetPointerEventOwner(eventData as PointerEventData);
	}

	private bool isAuthorized(PlayerNum clickingPlayer)
	{
		return !this.requireAuthorization || this.authorizedPlayers.Contains(clickingPlayer);
	}

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

	public void DisableAuthorization()
	{
		this.requireAuthorization = false;
	}

	private void updateAfterSelectionListRemoval(int prevCount)
	{
		if (this.selectionList.Count <= 0 && prevCount > 0)
		{
			this.hideHighlight();
		}
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		if (this.ClickCallback != null)
		{
			this.ClickCallback(this);
		}
	}

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
			this.scaleTween = DOTween.To(new DOGetter<Vector3>(this._showHighlight_m__0), new DOSetter<Vector3>(this._showHighlight_m__1), endValue, 0.07f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killScaleTween));
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
			this.scaleTween = DOTween.To(new DOGetter<Vector3>(this._hideHighlight_m__2), new DOSetter<Vector3>(this._hideHighlight_m__3), endValue, 0.07f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killScaleTween));
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

	public override void OnPointerUp(PointerEventData eventData)
	{
		if (!this.isAuthorized(eventData))
		{
			return;
		}
		base.OnPointerUp(eventData);
		this.pointerDownTime = 0L;
	}

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

	private Vector3 _showHighlight_m__0()
	{
		return this.ScaleUpMouseoverContainer.transform.localScale;
	}

	private void _showHighlight_m__1(Vector3 x)
	{
		this.ScaleUpMouseoverContainer.transform.localScale = x;
	}

	private Vector3 _hideHighlight_m__2()
	{
		return this.ScaleUpMouseoverContainer.transform.localScale;
	}

	private void _hideHighlight_m__3(Vector3 x)
	{
		this.ScaleUpMouseoverContainer.transform.localScale = x;
	}
}
