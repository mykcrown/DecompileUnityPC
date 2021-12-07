// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerToken : MonoBehaviour
{
	public Image Image;

	public CursorTargetButton ButtonInteract;

	public CharacterID AttachedToCharacter;

	protected float _alpha = 1f;

	protected float _targetAlpha = 1f;

	protected Tweener alphaTween;

	public Action<PlayerToken, PointerEventData> OnClick;

	protected TokenState state = TokenState.STATIC;

	protected IPlayerCursor attachToCursor;

	protected Vector3 staticPointTarget;

	public float ScaleUp = 1.3f;

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	public PlayerNum PlayerNum
	{
		get;
		set;
	}

	public float GrabbedByTime
	{
		get;
		set;
	}

	public virtual float Alpha
	{
		get
		{
			return this._alpha;
		}
		set
		{
			this._alpha = value;
			this.Image.CrossFadeAlpha(this._alpha, 0f, false);
		}
	}

	public IPlayerCursor AttachToCursor
	{
		get
		{
			return this.attachToCursor;
		}
	}

	public TokenState State
	{
		get
		{
			return this.state;
		}
	}

	public void Awake()
	{
		this.ButtonInteract = base.GetComponentInChildren<CursorTargetButton>();
		this.ButtonInteract.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClick);
	}

	protected void setAlpha(float alpha, bool instant)
	{
		if (this._targetAlpha != alpha)
		{
			this._targetAlpha = alpha;
			this.killAlphaTween();
			if (instant)
			{
				this.Alpha = this._targetAlpha;
			}
			else
			{
				this.alphaTween = DOTween.To(new DOGetter<float>(this.get_Alpha), new DOSetter<float>(this._setAlpha_m__0), this._targetAlpha, 0.05f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killAlphaTween));
			}
		}
	}

	public void SetVisible(bool state, bool instant = false)
	{
		if (state)
		{
			this.setAlpha(1f, instant);
		}
		else
		{
			this.setAlpha(0f, instant);
		}
	}

	public virtual void UpdateText(PlayerSelectionInfo info, UIColor color)
	{
	}

	protected void killAlphaTween()
	{
		if (this.alphaTween != null && this.alphaTween.IsPlaying())
		{
			this.alphaTween.Kill(false);
		}
		this.alphaTween = null;
	}

	protected void onClick(CursorTargetButton target, PointerEventData eventData)
	{
		if (this.OnClick != null)
		{
			this.OnClick(this, eventData);
		}
	}

	public void Update()
	{
		if (this.state == TokenState.ATTACHED || this.state == TokenState.TRANSITION_TO_ATTACHED)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(this.ScaleUp, this.ScaleUp, this.ScaleUp), 0.5f);
		}
		else
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1f, 1f, 1f), 0.5f);
		}
		if (this.state == TokenState.ATTACHED)
		{
			base.transform.position = (this.attachToCursor as MonoBehaviour).transform.position;
		}
		else if (this.state == TokenState.TRANSITION_TO_ATTACHED)
		{
			Vector3 position = (this.attachToCursor as MonoBehaviour).transform.position;
			base.transform.position = Vector3.Lerp(base.transform.position, position, 0.2f);
			if (this.isCloseEnoughToAttach(position))
			{
				this.setState(TokenState.ATTACHED);
			}
		}
		else if (this.state == TokenState.TRANSITION_TO_STATIC)
		{
			Vector3 vector = this.staticPointTarget;
			base.transform.position = Vector3.Lerp(base.transform.position, vector, 0.2f);
			if (this.isCloseEnoughToAttach(vector))
			{
				base.transform.position = vector;
				this.setState(TokenState.STATIC);
			}
		}
	}

	protected bool isCloseEnoughToAttach(Vector3 target)
	{
		return (base.transform.position - target).magnitude <= 3f;
	}

	protected void setState(TokenState state)
	{
		this.state = state;
		if (state == TokenState.STATIC)
		{
			this.Image.raycastTarget = true;
		}
		else
		{
			this.Image.raycastTarget = false;
			this.AttachedToCharacter = CharacterID.None;
			this.signalBus.Dispatch(UIManager.RAYCAST_UPDATE);
		}
	}

	public void SnapToPoint(Vector3 target, bool instant = false)
	{
		if (instant)
		{
			this.setState(TokenState.STATIC);
			base.transform.position = target;
		}
		else
		{
			this.setState(TokenState.TRANSITION_TO_STATIC);
			this.staticPointTarget = target;
		}
	}

	public void SnapToCursor(IPlayerCursor cursor, bool instant = false)
	{
		this.SnapToPoint(this.getTargetFromCursor(cursor), instant);
	}

	protected Vector3 getTargetFromCursor(IPlayerCursor cursor)
	{
		return (cursor as MonoBehaviour).transform.position;
	}

	public void Attach(IPlayerCursor cursor, bool instant = false)
	{
		if (cursor == null)
		{
			if (this.state == TokenState.ATTACHED || this.state == TokenState.TRANSITION_TO_ATTACHED)
			{
				this.setState(TokenState.STATIC);
			}
			this.attachToCursor = null;
		}
		else
		{
			if (instant || this.isCloseEnoughToAttach(this.getTargetFromCursor(cursor)))
			{
				this.setState(TokenState.ATTACHED);
			}
			else
			{
				this.setState(TokenState.TRANSITION_TO_ATTACHED);
			}
			this.attachToCursor = cursor;
			base.transform.SetAsLastSibling();
		}
	}

	public void OnDestroy()
	{
		this.OnClick = null;
	}

	public virtual Sprite GetSpriteForColor(UIColor color)
	{
		return this.Image.sprite;
	}

	private void _setAlpha_m__0(float valueIn)
	{
		this.Alpha = valueIn;
	}
}
