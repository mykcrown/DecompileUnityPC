// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseWindow : MonoBehaviour, ICursorInputDelegate, IButtonInputDelegate
{
	public CanvasGroup Shroud;

	public CanvasGroup Container;

	private CanvasGroup _canvasGroup;

	private List<SignalListenerRecord> signalListeners = new List<SignalListenerRecord>();

	private Tweener _transitionTween;

	public Action<BaseWindow> CloseRequest;

	public GameObject FirstSelected;

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public UIManager uiManager
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

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	[Inject]
	public IDialogController dialogController
	{
		get;
		set;
	}

	[Inject]
	public IGamewideOverlayController gamewideOverlayController
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

	public WindowRequest Request
	{
		get;
		set;
	}

	public ScreenType SourceScreen
	{
		get;
		set;
	}

	public bool IsRemoving
	{
		get;
		set;
	}

	public Action CloseCallback
	{
		get;
		set;
	}

	public GameObject PreviousSelection
	{
		get;
		set;
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

	public bool UseOverrideOpenSound
	{
		get;
		set;
	}

	public AudioData OverrideOpenSound
	{
		get;
		set;
	}

	public bool UseOverrideCloseSound
	{
		get;
		set;
	}

	public AudioData OverrideCloseSound
	{
		get;
		set;
	}

	public virtual void Awake()
	{
		this._canvasGroup = base.GetComponent<CanvasGroup>();
	}

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

	protected void listen(string signalName, Action theFunction)
	{
		this.signalBus.AddListener(signalName, theFunction);
		this.signalListeners.Add(new SignalListenerRecord(signalName, theFunction));
	}

	protected void removeAllListeners()
	{
		for (int i = this.signalListeners.Count - 1; i >= 0; i--)
		{
			this.signalBus.RemoveListener(this.signalListeners[i].name, this.signalListeners[i].theFunction);
		}
		this.signalListeners.Clear();
	}

	public virtual void ReadyForSelections()
	{
	}

	public void KillTween()
	{
		TweenUtil.Destroy(ref this._transitionTween);
	}

	protected void selectTextField(WavedashTMProInput field)
	{
		this.uiManager.CurrentInputModule.SetSelectedInputField(field);
	}

	public virtual void OnControllerMode()
	{
	}

	public virtual void OnMouseMode()
	{
	}

	public virtual void OnAdvance1Pressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnAdvance2Pressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnRightStickUpPressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnRightStickDownPressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnAltCancelPressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnAltSubmitPressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnCancelPressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnPrevious1Pressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnPrevious2Pressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnStartPressed(IPlayerCursor cursor)
	{
	}

	public virtual void OnSubmitPressed(PointerEventData eventData)
	{
	}

	public virtual void OnSubmitPressed()
	{
	}

	public virtual void OnCancelPressed()
	{
	}

	public virtual void OnRightTriggerPressed()
	{
	}

	public virtual void OnLeftTriggerPressed()
	{
	}

	public virtual void OnLeftBumperPressed()
	{
	}

	public virtual void OnZPressed()
	{
	}

	public virtual void OnRightStickRight()
	{
	}

	public virtual void OnRightStickLeft()
	{
	}

	public virtual void OnRightStickUp()
	{
	}

	public virtual void OnRightStickDown()
	{
	}

	public virtual void UpdateRightStick(float x, float y)
	{
	}

	public virtual void OnLeft()
	{
	}

	public virtual void OnRight()
	{
	}

	public virtual void OnUp()
	{
	}

	public virtual void OnDown()
	{
	}

	public virtual void OnDPadLeft()
	{
	}

	public virtual void OnDPadRight()
	{
	}

	public virtual void OnDPadUp()
	{
	}

	public virtual void OnDPadDown()
	{
	}

	public virtual void OnYButtonPressed()
	{
	}

	public virtual void OnXButtonPressed()
	{
	}

	public virtual void OnAnythingPressed()
	{
	}

	public virtual void OnAnyMouseEvent()
	{
	}

	public virtual void OnAnyNavigationButtonPressed()
	{
	}

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

	protected virtual void OnDestroy()
	{
	}
}
