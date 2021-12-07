using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020008F9 RID: 2297
public class PlayerToken : MonoBehaviour
{
	// Token: 0x17000E3C RID: 3644
	// (get) Token: 0x06003B54 RID: 15188 RVA: 0x0011060F File Offset: 0x0010EA0F
	// (set) Token: 0x06003B55 RID: 15189 RVA: 0x00110617 File Offset: 0x0010EA17
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000E3D RID: 3645
	// (get) Token: 0x06003B56 RID: 15190 RVA: 0x00110620 File Offset: 0x0010EA20
	// (set) Token: 0x06003B57 RID: 15191 RVA: 0x00110628 File Offset: 0x0010EA28
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000E3E RID: 3646
	// (get) Token: 0x06003B58 RID: 15192 RVA: 0x00110631 File Offset: 0x0010EA31
	// (set) Token: 0x06003B59 RID: 15193 RVA: 0x00110639 File Offset: 0x0010EA39
	public PlayerNum PlayerNum { get; set; }

	// Token: 0x17000E3F RID: 3647
	// (get) Token: 0x06003B5A RID: 15194 RVA: 0x00110642 File Offset: 0x0010EA42
	// (set) Token: 0x06003B5B RID: 15195 RVA: 0x0011064A File Offset: 0x0010EA4A
	public float GrabbedByTime { get; set; }

	// Token: 0x06003B5C RID: 15196 RVA: 0x00110653 File Offset: 0x0010EA53
	public void Awake()
	{
		this.ButtonInteract = base.GetComponentInChildren<CursorTargetButton>();
		this.ButtonInteract.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClick);
	}

	// Token: 0x06003B5D RID: 15197 RVA: 0x00110678 File Offset: 0x0010EA78
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
				this.alphaTween = DOTween.To(new DOGetter<float>(this.get_Alpha), delegate(float valueIn)
				{
					this.Alpha = valueIn;
				}, this._targetAlpha, 0.05f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killAlphaTween));
			}
		}
	}

	// Token: 0x06003B5E RID: 15198 RVA: 0x001106FB File Offset: 0x0010EAFB
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

	// Token: 0x17000E40 RID: 3648
	// (get) Token: 0x06003B5F RID: 15199 RVA: 0x00110720 File Offset: 0x0010EB20
	// (set) Token: 0x06003B60 RID: 15200 RVA: 0x00110728 File Offset: 0x0010EB28
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

	// Token: 0x06003B61 RID: 15201 RVA: 0x00110748 File Offset: 0x0010EB48
	public virtual void UpdateText(PlayerSelectionInfo info, UIColor color)
	{
	}

	// Token: 0x06003B62 RID: 15202 RVA: 0x0011074A File Offset: 0x0010EB4A
	protected void killAlphaTween()
	{
		if (this.alphaTween != null && this.alphaTween.IsPlaying())
		{
			this.alphaTween.Kill(false);
		}
		this.alphaTween = null;
	}

	// Token: 0x06003B63 RID: 15203 RVA: 0x0011077A File Offset: 0x0010EB7A
	protected void onClick(CursorTargetButton target, PointerEventData eventData)
	{
		if (this.OnClick != null)
		{
			this.OnClick(this, eventData);
		}
	}

	// Token: 0x06003B64 RID: 15204 RVA: 0x00110794 File Offset: 0x0010EB94
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

	// Token: 0x06003B65 RID: 15205 RVA: 0x00110908 File Offset: 0x0010ED08
	protected bool isCloseEnoughToAttach(Vector3 target)
	{
		return (base.transform.position - target).magnitude <= 3f;
	}

	// Token: 0x06003B66 RID: 15206 RVA: 0x0011093C File Offset: 0x0010ED3C
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

	// Token: 0x06003B67 RID: 15207 RVA: 0x0011098B File Offset: 0x0010ED8B
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

	// Token: 0x06003B68 RID: 15208 RVA: 0x001109B9 File Offset: 0x0010EDB9
	public void SnapToCursor(IPlayerCursor cursor, bool instant = false)
	{
		this.SnapToPoint(this.getTargetFromCursor(cursor), instant);
	}

	// Token: 0x06003B69 RID: 15209 RVA: 0x001109C9 File Offset: 0x0010EDC9
	protected Vector3 getTargetFromCursor(IPlayerCursor cursor)
	{
		return (cursor as MonoBehaviour).transform.position;
	}

	// Token: 0x06003B6A RID: 15210 RVA: 0x001109DC File Offset: 0x0010EDDC
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

	// Token: 0x17000E41 RID: 3649
	// (get) Token: 0x06003B6B RID: 15211 RVA: 0x00110A56 File Offset: 0x0010EE56
	public IPlayerCursor AttachToCursor
	{
		get
		{
			return this.attachToCursor;
		}
	}

	// Token: 0x17000E42 RID: 3650
	// (get) Token: 0x06003B6C RID: 15212 RVA: 0x00110A5E File Offset: 0x0010EE5E
	public TokenState State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x06003B6D RID: 15213 RVA: 0x00110A66 File Offset: 0x0010EE66
	public void OnDestroy()
	{
		this.OnClick = null;
	}

	// Token: 0x06003B6E RID: 15214 RVA: 0x00110A6F File Offset: 0x0010EE6F
	public virtual Sprite GetSpriteForColor(UIColor color)
	{
		return this.Image.sprite;
	}

	// Token: 0x040028E0 RID: 10464
	public Image Image;

	// Token: 0x040028E1 RID: 10465
	public CursorTargetButton ButtonInteract;

	// Token: 0x040028E2 RID: 10466
	public CharacterID AttachedToCharacter;

	// Token: 0x040028E3 RID: 10467
	protected float _alpha = 1f;

	// Token: 0x040028E4 RID: 10468
	protected float _targetAlpha = 1f;

	// Token: 0x040028E5 RID: 10469
	protected Tweener alphaTween;

	// Token: 0x040028E6 RID: 10470
	public Action<PlayerToken, PointerEventData> OnClick;

	// Token: 0x040028E7 RID: 10471
	protected TokenState state = TokenState.STATIC;

	// Token: 0x040028E8 RID: 10472
	protected IPlayerCursor attachToCursor;

	// Token: 0x040028E9 RID: 10473
	protected Vector3 staticPointTarget;

	// Token: 0x040028EB RID: 10475
	public float ScaleUp = 1.3f;
}
