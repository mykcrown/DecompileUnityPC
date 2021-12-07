// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseGamewideOverlay : MonoBehaviour, IGlobalInputDelegate
{
	public GameObject FirstSelected;

	public Action<BaseGamewideOverlay> CloseRequest;

	private List<SignalListenerRecord> signalListeners = new List<SignalListenerRecord>();

	private Tweener _transitionTween;

	private CanvasGroup _canvasGroup;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	public GamewideOverlayRequest Request
	{
		get;
		set;
	}

	public bool IsRemoving
	{
		get;
		set;
	}

	public Tweener TransitionTween
	{
		get
		{
			return this._transitionTween;
		}
		set
		{
			this._transitionTween = value;
		}
	}

	public float Alpha
	{
		get
		{
			return this._canvasGroup.alpha;
		}
		set
		{
			this._canvasGroup.alpha = value;
		}
	}

	public virtual void Awake()
	{
		this._canvasGroup = base.GetComponent<CanvasGroup>();
	}

	public virtual void ReadyForSelections()
	{
	}

	public virtual void OnOpen()
	{
	}

	public void KillTween()
	{
		TweenUtil.Destroy(ref this._transitionTween);
	}

	protected void removeAllListeners()
	{
		for (int i = this.signalListeners.Count - 1; i >= 0; i--)
		{
			this.signalBus.RemoveListener(this.signalListeners[i].name, this.signalListeners[i].theFunction);
		}
		this.signalListeners.Clear();
	}

	public virtual void Close()
	{
		this.removeAllListeners();
		this.CloseRequest(this);
	}

	public virtual void OnXButtonPressed()
	{
	}
}
