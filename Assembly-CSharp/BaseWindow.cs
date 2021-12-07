using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200095E RID: 2398
public class BaseWindow : MonoBehaviour, ICursorInputDelegate, IButtonInputDelegate
{
	// Token: 0x17000F19 RID: 3865
	// (get) Token: 0x06003FC0 RID: 16320 RVA: 0x0011CCD1 File Offset: 0x0011B0D1
	// (set) Token: 0x06003FC1 RID: 16321 RVA: 0x0011CCD9 File Offset: 0x0011B0D9
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000F1A RID: 3866
	// (get) Token: 0x06003FC2 RID: 16322 RVA: 0x0011CCE2 File Offset: 0x0011B0E2
	// (set) Token: 0x06003FC3 RID: 16323 RVA: 0x0011CCEA File Offset: 0x0011B0EA
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17000F1B RID: 3867
	// (get) Token: 0x06003FC4 RID: 16324 RVA: 0x0011CCF3 File Offset: 0x0011B0F3
	// (set) Token: 0x06003FC5 RID: 16325 RVA: 0x0011CCFB File Offset: 0x0011B0FB
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17000F1C RID: 3868
	// (get) Token: 0x06003FC6 RID: 16326 RVA: 0x0011CD04 File Offset: 0x0011B104
	// (set) Token: 0x06003FC7 RID: 16327 RVA: 0x0011CD0C File Offset: 0x0011B10C
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x17000F1D RID: 3869
	// (get) Token: 0x06003FC8 RID: 16328 RVA: 0x0011CD15 File Offset: 0x0011B115
	// (set) Token: 0x06003FC9 RID: 16329 RVA: 0x0011CD1D File Offset: 0x0011B11D
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000F1E RID: 3870
	// (get) Token: 0x06003FCA RID: 16330 RVA: 0x0011CD26 File Offset: 0x0011B126
	// (set) Token: 0x06003FCB RID: 16331 RVA: 0x0011CD2E File Offset: 0x0011B12E
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000F1F RID: 3871
	// (get) Token: 0x06003FCC RID: 16332 RVA: 0x0011CD37 File Offset: 0x0011B137
	// (set) Token: 0x06003FCD RID: 16333 RVA: 0x0011CD3F File Offset: 0x0011B13F
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x17000F20 RID: 3872
	// (get) Token: 0x06003FCE RID: 16334 RVA: 0x0011CD48 File Offset: 0x0011B148
	// (set) Token: 0x06003FCF RID: 16335 RVA: 0x0011CD50 File Offset: 0x0011B150
	[Inject]
	public IGamewideOverlayController gamewideOverlayController { get; set; }

	// Token: 0x17000F21 RID: 3873
	// (get) Token: 0x06003FD0 RID: 16336 RVA: 0x0011CD59 File Offset: 0x0011B159
	// (set) Token: 0x06003FD1 RID: 16337 RVA: 0x0011CD61 File Offset: 0x0011B161
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

	// Token: 0x17000F22 RID: 3874
	// (get) Token: 0x06003FD2 RID: 16338 RVA: 0x0011CD6A File Offset: 0x0011B16A
	// (set) Token: 0x06003FD3 RID: 16339 RVA: 0x0011CD72 File Offset: 0x0011B172
	public WindowRequest Request { get; set; }

	// Token: 0x17000F23 RID: 3875
	// (get) Token: 0x06003FD4 RID: 16340 RVA: 0x0011CD7B File Offset: 0x0011B17B
	// (set) Token: 0x06003FD5 RID: 16341 RVA: 0x0011CD83 File Offset: 0x0011B183
	public ScreenType SourceScreen { get; set; }

	// Token: 0x17000F24 RID: 3876
	// (get) Token: 0x06003FD6 RID: 16342 RVA: 0x0011CD8C File Offset: 0x0011B18C
	// (set) Token: 0x06003FD7 RID: 16343 RVA: 0x0011CD94 File Offset: 0x0011B194
	public bool IsRemoving { get; set; }

	// Token: 0x17000F25 RID: 3877
	// (get) Token: 0x06003FD8 RID: 16344 RVA: 0x0011CD9D File Offset: 0x0011B19D
	// (set) Token: 0x06003FD9 RID: 16345 RVA: 0x0011CDA5 File Offset: 0x0011B1A5
	public Action CloseCallback { get; set; }

	// Token: 0x17000F26 RID: 3878
	// (get) Token: 0x06003FDA RID: 16346 RVA: 0x0011CDAE File Offset: 0x0011B1AE
	// (set) Token: 0x06003FDB RID: 16347 RVA: 0x0011CDB6 File Offset: 0x0011B1B6
	public GameObject PreviousSelection { get; set; }

	// Token: 0x17000F27 RID: 3879
	// (get) Token: 0x06003FDC RID: 16348 RVA: 0x0011CDBF File Offset: 0x0011B1BF
	// (set) Token: 0x06003FDD RID: 16349 RVA: 0x0011CDCC File Offset: 0x0011B1CC
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

	// Token: 0x17000F28 RID: 3880
	// (get) Token: 0x06003FDE RID: 16350 RVA: 0x0011CDDA File Offset: 0x0011B1DA
	// (set) Token: 0x06003FDF RID: 16351 RVA: 0x0011CDE7 File Offset: 0x0011B1E7
	public float ContainerAlpha
	{
		get
		{
			return this.Container.alpha;
		}
		set
		{
			this.Container.alpha = value;
		}
	}

	// Token: 0x17000F29 RID: 3881
	// (get) Token: 0x06003FE0 RID: 16352 RVA: 0x0011CDF5 File Offset: 0x0011B1F5
	// (set) Token: 0x06003FE1 RID: 16353 RVA: 0x0011CE02 File Offset: 0x0011B202
	public float ShroudAlpha
	{
		get
		{
			return this.Shroud.alpha;
		}
		set
		{
			this.Shroud.alpha = value;
		}
	}

	// Token: 0x17000F2A RID: 3882
	// (get) Token: 0x06003FE2 RID: 16354 RVA: 0x0011CE10 File Offset: 0x0011B210
	// (set) Token: 0x06003FE3 RID: 16355 RVA: 0x0011CE18 File Offset: 0x0011B218
	public bool UseOverrideOpenSound { get; set; }

	// Token: 0x17000F2B RID: 3883
	// (get) Token: 0x06003FE4 RID: 16356 RVA: 0x0011CE21 File Offset: 0x0011B221
	// (set) Token: 0x06003FE5 RID: 16357 RVA: 0x0011CE29 File Offset: 0x0011B229
	public AudioData OverrideOpenSound { get; set; }

	// Token: 0x17000F2C RID: 3884
	// (get) Token: 0x06003FE6 RID: 16358 RVA: 0x0011CE32 File Offset: 0x0011B232
	// (set) Token: 0x06003FE7 RID: 16359 RVA: 0x0011CE3A File Offset: 0x0011B23A
	public bool UseOverrideCloseSound { get; set; }

	// Token: 0x17000F2D RID: 3885
	// (get) Token: 0x06003FE8 RID: 16360 RVA: 0x0011CE43 File Offset: 0x0011B243
	// (set) Token: 0x06003FE9 RID: 16361 RVA: 0x0011CE4B File Offset: 0x0011B24B
	public AudioData OverrideCloseSound { get; set; }

	// Token: 0x06003FEA RID: 16362 RVA: 0x0011CE54 File Offset: 0x0011B254
	public virtual void Awake()
	{
		this._canvasGroup = base.GetComponent<CanvasGroup>();
	}

	// Token: 0x06003FEB RID: 16363 RVA: 0x0011CE62 File Offset: 0x0011B262
	public virtual void Open()
	{
		if (this.UseOverrideOpenSound)
		{
			this.audioManager.PlayMenuSound(this.OverrideOpenSound, 0f);
		}
		else
		{
			this.audioManager.PlayMenuSound(SoundKey.generic_dialogOpen, 0f);
		}
	}

	// Token: 0x06003FEC RID: 16364 RVA: 0x0011CE9B File Offset: 0x0011B29B
	protected void listen(string signalName, Action theFunction)
	{
		this.signalBus.AddListener(signalName, theFunction);
		this.signalListeners.Add(new SignalListenerRecord(signalName, theFunction));
	}

	// Token: 0x06003FED RID: 16365 RVA: 0x0011CEBC File Offset: 0x0011B2BC
	protected void removeAllListeners()
	{
		for (int i = this.signalListeners.Count - 1; i >= 0; i--)
		{
			this.signalBus.RemoveListener(this.signalListeners[i].name, this.signalListeners[i].theFunction);
		}
		this.signalListeners.Clear();
	}

	// Token: 0x06003FEE RID: 16366 RVA: 0x0011CF1F File Offset: 0x0011B31F
	public virtual void ReadyForSelections()
	{
	}

	// Token: 0x06003FEF RID: 16367 RVA: 0x0011CF21 File Offset: 0x0011B321
	public void KillTween()
	{
		TweenUtil.Destroy(ref this._transitionTween);
	}

	// Token: 0x06003FF0 RID: 16368 RVA: 0x0011CF2E File Offset: 0x0011B32E
	protected void selectTextField(WavedashTMProInput field)
	{
		this.uiManager.CurrentInputModule.SetSelectedInputField(field);
	}

	// Token: 0x06003FF1 RID: 16369 RVA: 0x0011CF41 File Offset: 0x0011B341
	public virtual void OnControllerMode()
	{
	}

	// Token: 0x06003FF2 RID: 16370 RVA: 0x0011CF43 File Offset: 0x0011B343
	public virtual void OnMouseMode()
	{
	}

	// Token: 0x06003FF3 RID: 16371 RVA: 0x0011CF45 File Offset: 0x0011B345
	public virtual void OnAdvance1Pressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003FF4 RID: 16372 RVA: 0x0011CF47 File Offset: 0x0011B347
	public virtual void OnAdvance2Pressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003FF5 RID: 16373 RVA: 0x0011CF49 File Offset: 0x0011B349
	public virtual void OnRightStickUpPressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003FF6 RID: 16374 RVA: 0x0011CF4B File Offset: 0x0011B34B
	public virtual void OnRightStickDownPressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003FF7 RID: 16375 RVA: 0x0011CF4D File Offset: 0x0011B34D
	public virtual void OnAltCancelPressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003FF8 RID: 16376 RVA: 0x0011CF4F File Offset: 0x0011B34F
	public virtual void OnAltSubmitPressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003FF9 RID: 16377 RVA: 0x0011CF51 File Offset: 0x0011B351
	public virtual void OnCancelPressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003FFA RID: 16378 RVA: 0x0011CF53 File Offset: 0x0011B353
	public virtual void OnPrevious1Pressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003FFB RID: 16379 RVA: 0x0011CF55 File Offset: 0x0011B355
	public virtual void OnPrevious2Pressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003FFC RID: 16380 RVA: 0x0011CF57 File Offset: 0x0011B357
	public virtual void OnStartPressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003FFD RID: 16381 RVA: 0x0011CF59 File Offset: 0x0011B359
	public virtual void OnSubmitPressed(PointerEventData eventData)
	{
	}

	// Token: 0x06003FFE RID: 16382 RVA: 0x0011CF5B File Offset: 0x0011B35B
	public virtual void OnSubmitPressed()
	{
	}

	// Token: 0x06003FFF RID: 16383 RVA: 0x0011CF5D File Offset: 0x0011B35D
	public virtual void OnCancelPressed()
	{
	}

	// Token: 0x06004000 RID: 16384 RVA: 0x0011CF5F File Offset: 0x0011B35F
	public virtual void OnRightTriggerPressed()
	{
	}

	// Token: 0x06004001 RID: 16385 RVA: 0x0011CF61 File Offset: 0x0011B361
	public virtual void OnLeftTriggerPressed()
	{
	}

	// Token: 0x06004002 RID: 16386 RVA: 0x0011CF63 File Offset: 0x0011B363
	public virtual void OnLeftBumperPressed()
	{
	}

	// Token: 0x06004003 RID: 16387 RVA: 0x0011CF65 File Offset: 0x0011B365
	public virtual void OnZPressed()
	{
	}

	// Token: 0x06004004 RID: 16388 RVA: 0x0011CF67 File Offset: 0x0011B367
	public virtual void OnRightStickRight()
	{
	}

	// Token: 0x06004005 RID: 16389 RVA: 0x0011CF69 File Offset: 0x0011B369
	public virtual void OnRightStickLeft()
	{
	}

	// Token: 0x06004006 RID: 16390 RVA: 0x0011CF6B File Offset: 0x0011B36B
	public virtual void OnRightStickUp()
	{
	}

	// Token: 0x06004007 RID: 16391 RVA: 0x0011CF6D File Offset: 0x0011B36D
	public virtual void OnRightStickDown()
	{
	}

	// Token: 0x06004008 RID: 16392 RVA: 0x0011CF6F File Offset: 0x0011B36F
	public virtual void UpdateRightStick(float x, float y)
	{
	}

	// Token: 0x06004009 RID: 16393 RVA: 0x0011CF71 File Offset: 0x0011B371
	public virtual void OnLeft()
	{
	}

	// Token: 0x0600400A RID: 16394 RVA: 0x0011CF73 File Offset: 0x0011B373
	public virtual void OnRight()
	{
	}

	// Token: 0x0600400B RID: 16395 RVA: 0x0011CF75 File Offset: 0x0011B375
	public virtual void OnUp()
	{
	}

	// Token: 0x0600400C RID: 16396 RVA: 0x0011CF77 File Offset: 0x0011B377
	public virtual void OnDown()
	{
	}

	// Token: 0x0600400D RID: 16397 RVA: 0x0011CF79 File Offset: 0x0011B379
	public virtual void OnDPadLeft()
	{
	}

	// Token: 0x0600400E RID: 16398 RVA: 0x0011CF7B File Offset: 0x0011B37B
	public virtual void OnDPadRight()
	{
	}

	// Token: 0x0600400F RID: 16399 RVA: 0x0011CF7D File Offset: 0x0011B37D
	public virtual void OnDPadUp()
	{
	}

	// Token: 0x06004010 RID: 16400 RVA: 0x0011CF7F File Offset: 0x0011B37F
	public virtual void OnDPadDown()
	{
	}

	// Token: 0x06004011 RID: 16401 RVA: 0x0011CF81 File Offset: 0x0011B381
	public virtual void OnYButtonPressed()
	{
	}

	// Token: 0x06004012 RID: 16402 RVA: 0x0011CF83 File Offset: 0x0011B383
	public virtual void OnXButtonPressed()
	{
	}

	// Token: 0x06004013 RID: 16403 RVA: 0x0011CF85 File Offset: 0x0011B385
	public virtual void OnAnythingPressed()
	{
	}

	// Token: 0x06004014 RID: 16404 RVA: 0x0011CF87 File Offset: 0x0011B387
	public virtual void OnAnyMouseEvent()
	{
	}

	// Token: 0x06004015 RID: 16405 RVA: 0x0011CF89 File Offset: 0x0011B389
	public virtual void OnAnyNavigationButtonPressed()
	{
	}

	// Token: 0x06004016 RID: 16406 RVA: 0x0011CF8C File Offset: 0x0011B38C
	public virtual void Close()
	{
		this.removeAllListeners();
		this.CloseRequest(this);
		if (this.UseOverrideCloseSound)
		{
			this.audioManager.PlayMenuSound(this.OverrideCloseSound, 0f);
		}
		else
		{
			this.audioManager.PlayMenuSound(SoundKey.generic_dialogClose, 0f);
		}
		Action closeCallback = this.CloseCallback;
		this.CloseCallback = null;
		if (closeCallback != null)
		{
			closeCallback();
		}
	}

	// Token: 0x06004017 RID: 16407 RVA: 0x0011CFFC File Offset: 0x0011B3FC
	protected virtual void OnDestroy()
	{
	}

	// Token: 0x04002B38 RID: 11064
	public CanvasGroup Shroud;

	// Token: 0x04002B39 RID: 11065
	public CanvasGroup Container;

	// Token: 0x04002B3A RID: 11066
	private CanvasGroup _canvasGroup;

	// Token: 0x04002B3B RID: 11067
	private List<SignalListenerRecord> signalListeners = new List<SignalListenerRecord>();

	// Token: 0x04002B3C RID: 11068
	private Tweener _transitionTween;

	// Token: 0x04002B40 RID: 11072
	public Action<BaseWindow> CloseRequest;

	// Token: 0x04002B42 RID: 11074
	public GameObject FirstSelected;
}
