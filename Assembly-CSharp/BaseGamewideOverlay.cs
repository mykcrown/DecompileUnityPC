using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// Token: 0x0200093B RID: 2363
public class BaseGamewideOverlay : MonoBehaviour, IGlobalInputDelegate
{
	// Token: 0x17000ED6 RID: 3798
	// (get) Token: 0x06003E67 RID: 15975 RVA: 0x0011AF30 File Offset: 0x00119330
	// (set) Token: 0x06003E68 RID: 15976 RVA: 0x0011AF38 File Offset: 0x00119338
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000ED7 RID: 3799
	// (get) Token: 0x06003E69 RID: 15977 RVA: 0x0011AF41 File Offset: 0x00119341
	// (set) Token: 0x06003E6A RID: 15978 RVA: 0x0011AF49 File Offset: 0x00119349
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000ED8 RID: 3800
	// (get) Token: 0x06003E6B RID: 15979 RVA: 0x0011AF52 File Offset: 0x00119352
	// (set) Token: 0x06003E6C RID: 15980 RVA: 0x0011AF5A File Offset: 0x0011935A
	public GamewideOverlayRequest Request { get; set; }

	// Token: 0x17000ED9 RID: 3801
	// (get) Token: 0x06003E6D RID: 15981 RVA: 0x0011AF63 File Offset: 0x00119363
	// (set) Token: 0x06003E6E RID: 15982 RVA: 0x0011AF6B File Offset: 0x0011936B
	public bool IsRemoving { get; set; }

	// Token: 0x17000EDA RID: 3802
	// (get) Token: 0x06003E6F RID: 15983 RVA: 0x0011AF74 File Offset: 0x00119374
	// (set) Token: 0x06003E70 RID: 15984 RVA: 0x0011AF7C File Offset: 0x0011937C
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

	// Token: 0x17000EDB RID: 3803
	// (get) Token: 0x06003E71 RID: 15985 RVA: 0x0011AF85 File Offset: 0x00119385
	// (set) Token: 0x06003E72 RID: 15986 RVA: 0x0011AF92 File Offset: 0x00119392
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

	// Token: 0x06003E73 RID: 15987 RVA: 0x0011AFA0 File Offset: 0x001193A0
	public virtual void Awake()
	{
		this._canvasGroup = base.GetComponent<CanvasGroup>();
	}

	// Token: 0x06003E74 RID: 15988 RVA: 0x0011AFAE File Offset: 0x001193AE
	public virtual void ReadyForSelections()
	{
	}

	// Token: 0x06003E75 RID: 15989 RVA: 0x0011AFB0 File Offset: 0x001193B0
	public virtual void OnOpen()
	{
	}

	// Token: 0x06003E76 RID: 15990 RVA: 0x0011AFB2 File Offset: 0x001193B2
	public void KillTween()
	{
		TweenUtil.Destroy(ref this._transitionTween);
	}

	// Token: 0x06003E77 RID: 15991 RVA: 0x0011AFC0 File Offset: 0x001193C0
	protected void removeAllListeners()
	{
		for (int i = this.signalListeners.Count - 1; i >= 0; i--)
		{
			this.signalBus.RemoveListener(this.signalListeners[i].name, this.signalListeners[i].theFunction);
		}
		this.signalListeners.Clear();
	}

	// Token: 0x06003E78 RID: 15992 RVA: 0x0011B023 File Offset: 0x00119423
	public virtual void Close()
	{
		this.removeAllListeners();
		this.CloseRequest(this);
	}

	// Token: 0x06003E79 RID: 15993 RVA: 0x0011B037 File Offset: 0x00119437
	public virtual void OnXButtonPressed()
	{
	}

	// Token: 0x04002A64 RID: 10852
	public GameObject FirstSelected;

	// Token: 0x04002A67 RID: 10855
	public Action<BaseGamewideOverlay> CloseRequest;

	// Token: 0x04002A68 RID: 10856
	private List<SignalListenerRecord> signalListeners = new List<SignalListenerRecord>();

	// Token: 0x04002A69 RID: 10857
	private Tweener _transitionTween;

	// Token: 0x04002A6A RID: 10858
	private CanvasGroup _canvasGroup;
}
