using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200092E RID: 2350
public class GameScreen : ClientBehavior, ICursorInputDelegate, IScreenInputDelegate, IButtonInputDelegate
{
	// Token: 0x17000EA8 RID: 3752
	// (get) Token: 0x06003D49 RID: 15689 RVA: 0x00102F22 File Offset: 0x00101322
	// (set) Token: 0x06003D4A RID: 15690 RVA: 0x00102F2A File Offset: 0x0010132A
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x17000EA9 RID: 3753
	// (get) Token: 0x06003D4B RID: 15691 RVA: 0x00102F33 File Offset: 0x00101333
	// (set) Token: 0x06003D4C RID: 15692 RVA: 0x00102F3B File Offset: 0x0010133B
	[Inject]
	public IInputBlocker inputBlocker { get; set; }

	// Token: 0x17000EAA RID: 3754
	// (get) Token: 0x06003D4D RID: 15693 RVA: 0x00102F44 File Offset: 0x00101344
	// (set) Token: 0x06003D4E RID: 15694 RVA: 0x00102F4C File Offset: 0x0010134C
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x17000EAB RID: 3755
	// (get) Token: 0x06003D4F RID: 15695 RVA: 0x00102F55 File Offset: 0x00101355
	// (set) Token: 0x06003D50 RID: 15696 RVA: 0x00102F5D File Offset: 0x0010135D
	[Inject]
	public IWindowDisplay windowDisplay { get; set; }

	// Token: 0x17000EAC RID: 3756
	// (get) Token: 0x06003D51 RID: 15697 RVA: 0x00102F66 File Offset: 0x00101366
	// (set) Token: 0x06003D52 RID: 15698 RVA: 0x00102F6E File Offset: 0x0010136E
	[Inject]
	public IGamewideOverlayController gamewideOverlayController { get; set; }

	// Token: 0x17000EAD RID: 3757
	// (get) Token: 0x06003D53 RID: 15699 RVA: 0x00102F77 File Offset: 0x00101377
	// (set) Token: 0x06003D54 RID: 15700 RVA: 0x00102F7F File Offset: 0x0010137F
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000EAE RID: 3758
	// (get) Token: 0x06003D55 RID: 15701 RVA: 0x00102F88 File Offset: 0x00101388
	// (set) Token: 0x06003D56 RID: 15702 RVA: 0x00102F90 File Offset: 0x00101390
	[Inject]
	public PlayerInputLocator playerInput { get; set; }

	// Token: 0x17000EAF RID: 3759
	// (get) Token: 0x06003D57 RID: 15703 RVA: 0x00102F99 File Offset: 0x00101399
	// (set) Token: 0x06003D58 RID: 15704 RVA: 0x00102FA1 File Offset: 0x001013A1
	[Inject]
	public ISelectionManager selectionManager { get; set; }

	// Token: 0x17000EB0 RID: 3760
	// (get) Token: 0x06003D59 RID: 15705 RVA: 0x00102FAA File Offset: 0x001013AA
	// (set) Token: 0x06003D5A RID: 15706 RVA: 0x00102FB2 File Offset: 0x001013B2
	[Inject]
	public UserAudioSettings userAudioSettings { get; set; }

	// Token: 0x17000EB1 RID: 3761
	// (get) Token: 0x06003D5B RID: 15707 RVA: 0x00102FBB File Offset: 0x001013BB
	protected virtual bool SupportsAutoJoin
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003D5C RID: 15708 RVA: 0x00102FBE File Offset: 0x001013BE
	public virtual void TickFrame()
	{
	}

	// Token: 0x06003D5D RID: 15709 RVA: 0x00102FC0 File Offset: 0x001013C0
	public virtual void LoadData(LoadSequenceResults data)
	{
		this.data = data;
	}

	// Token: 0x06003D5E RID: 15710 RVA: 0x00102FC9 File Offset: 0x001013C9
	public override void Awake()
	{
		base.Awake();
		this._canvasGroup = base.GetComponent<CanvasGroup>();
	}

	// Token: 0x06003D5F RID: 15711 RVA: 0x00102FDD File Offset: 0x001013DD
	public virtual void Start()
	{
		this.isStarted = true;
		this._screenWidth = (float)Screen.width;
		this.checkForPostStartAndPayload();
	}

	// Token: 0x17000EB2 RID: 3762
	// (get) Token: 0x06003D60 RID: 15712 RVA: 0x00102FF8 File Offset: 0x001013F8
	public virtual bool WaitForDrawCallback
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003D61 RID: 15713 RVA: 0x00102FFB File Offset: 0x001013FB
	protected void onDrawComplete()
	{
		if (this.OnDrawCompleteCallback != null)
		{
			this.OnDrawCompleteCallback();
		}
	}

	// Token: 0x06003D62 RID: 15714 RVA: 0x00103013 File Offset: 0x00101413
	public virtual void LoadPayload(Payload payload)
	{
		this.payload = payload;
		this.didLoadPayload = true;
		this.checkForPostStartAndPayload();
	}

	// Token: 0x06003D63 RID: 15715 RVA: 0x00103029 File Offset: 0x00101429
	private void checkForPostStartAndPayload()
	{
		if (!this.didLoadPayload)
		{
			return;
		}
		if (!this.isStarted)
		{
			return;
		}
		if (this.didPostStartAndPayload)
		{
			return;
		}
		this.didPostStartAndPayload = true;
		this.postStartAndPayload();
		this.CheckForTransitionBegin();
	}

	// Token: 0x06003D64 RID: 15716 RVA: 0x00103062 File Offset: 0x00101462
	protected void lockInput()
	{
		if (this.block == null)
		{
			this.block = this.inputBlocker.Request();
		}
	}

	// Token: 0x06003D65 RID: 15717 RVA: 0x00103080 File Offset: 0x00101480
	protected void unlockInput()
	{
		if (this.block != null)
		{
			this.inputBlocker.Release(this.block);
			this.block = null;
		}
	}

	// Token: 0x06003D66 RID: 15718 RVA: 0x001030A5 File Offset: 0x001014A5
	protected virtual void postStartAndPayload()
	{
		this.AddScreenListeners();
	}

	// Token: 0x06003D67 RID: 15719 RVA: 0x001030AD File Offset: 0x001014AD
	protected virtual void AddScreenListeners()
	{
		if (this.SupportsAutoJoin)
		{
			base.listen(AutoJoin.AUTOJOIN, new Action(this.onAutoJoinRequest));
		}
	}

	// Token: 0x06003D68 RID: 15720 RVA: 0x001030D2 File Offset: 0x001014D2
	protected virtual void onAutoJoinRequest()
	{
		base.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Next));
	}

	// Token: 0x06003D69 RID: 15721 RVA: 0x001030E7 File Offset: 0x001014E7
	public virtual void OnTransitionComplete()
	{
		this.initMenuSelectionSystem();
	}

	// Token: 0x06003D6A RID: 15722 RVA: 0x001030EF File Offset: 0x001014EF
	public void CheckForTransitionBegin()
	{
		if (!this.didPostStartAndPayload)
		{
			return;
		}
		if (this.didTransistionBegin)
		{
			return;
		}
		this.didTransistionBegin = true;
		this.onTransitionBegin();
	}

	// Token: 0x06003D6B RID: 15723 RVA: 0x00103116 File Offset: 0x00101516
	protected virtual Vector2 getCursorDefaultPosition(PlayerNum playerNum)
	{
		return new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
	}

	// Token: 0x06003D6C RID: 15724 RVA: 0x0010312D File Offset: 0x0010152D
	protected virtual void onTransitionBegin()
	{
	}

	// Token: 0x06003D6D RID: 15725 RVA: 0x00103130 File Offset: 0x00101530
	protected void tweenInItem(GameObject theList, int directionX, int directionY, float time, float delay = 0f, Action callback = null)
	{
		Vector3 localPosition = theList.transform.localPosition;
		Vector3 localPosition2 = theList.transform.localPosition;
		localPosition2.x += base.gameDataManager.ConfigData.uiConfig.canvasScaler.referenceResolution.x * (float)directionX;
		localPosition2.y += base.gameDataManager.ConfigData.uiConfig.canvasScaler.referenceResolution.y * (float)directionY;
		theList.transform.localPosition = localPosition2;
		DOTween.To(() => theList.transform.localPosition, delegate(Vector3 valueIn)
		{
			theList.transform.localPosition = valueIn;
		}, localPosition, time).SetDelay(delay).SetEase(Ease.OutSine).OnComplete(delegate
		{
			if (callback != null)
			{
				callback();
			}
		});
	}

	// Token: 0x06003D6E RID: 15726 RVA: 0x00103224 File Offset: 0x00101624
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.isDestroyed = true;
		this.destroyAllCursors();
		this.unlockInput();
	}

	// Token: 0x17000EB3 RID: 3763
	// (get) Token: 0x06003D6F RID: 15727 RVA: 0x0010323F File Offset: 0x0010163F
	protected float screenWidth
	{
		get
		{
			return base.gameDataManager.ConfigData.uiConfig.canvasScaler.referenceResolution.x;
		}
	}

	// Token: 0x17000EB4 RID: 3764
	// (get) Token: 0x06003D70 RID: 15728 RVA: 0x00103260 File Offset: 0x00101660
	protected float screenHeight
	{
		get
		{
			return base.gameDataManager.ConfigData.uiConfig.canvasScaler.referenceResolution.y;
		}
	}

	// Token: 0x06003D71 RID: 15729 RVA: 0x00103281 File Offset: 0x00101681
	public virtual void UpdatePayload(Payload payload)
	{
	}

	// Token: 0x06003D72 RID: 15730 RVA: 0x00103283 File Offset: 0x00101683
	protected virtual void Update()
	{
		if (this._screenWidth != (float)Screen.width)
		{
			this.onScreenSizeUpdate();
			this._screenWidth = (float)Screen.width;
		}
	}

	// Token: 0x06003D73 RID: 15731 RVA: 0x001032A8 File Offset: 0x001016A8
	protected virtual void onScreenSizeUpdate()
	{
	}

	// Token: 0x06003D74 RID: 15732 RVA: 0x001032AC File Offset: 0x001016AC
	public virtual void OnAddedToHeirarchy()
	{
		this.isShown = true;
		if (this.UIInput == UIInputModuleType.Cursor)
		{
			if (this.CursorPrefab == null)
			{
				throw new Exception("GameScreen with CursorInputModule must have CursorPrefab set.");
			}
			this.mouseCursor = new GameObject("MouseCursor").AddComponent<MousePlayerCursor>();
			this.mouseCursor.transform.SetParent(base.transform, false);
			this.mouseCursor.ResetPosition(Vector2.zero);
			this.cursors.Add(this.mouseCursor);
			this.createScreenCursors();
			base.events.Broadcast(new ActivateCursorModuleCommand(this.cursors));
		}
		else
		{
			base.events.Broadcast(new SetUIInputModuleCommand(this.UIInput));
		}
	}

	// Token: 0x06003D75 RID: 15733 RVA: 0x0010336C File Offset: 0x0010176C
	public void OnWindowsOpened()
	{
		this.pauseAllCursors();
	}

	// Token: 0x06003D76 RID: 15734 RVA: 0x00103374 File Offset: 0x00101774
	public void OnWindowsClosed()
	{
		if (this.UIInput == UIInputModuleType.Cursor)
		{
			base.events.Broadcast(new ActivateCursorModuleCommand(this.cursors));
		}
		else
		{
			base.events.Broadcast(new SetUIInputModuleCommand(this.UIInput));
		}
		this.unpauseAllCursors();
	}

	// Token: 0x06003D77 RID: 15735 RVA: 0x001033C4 File Offset: 0x001017C4
	protected virtual void createScreenCursors()
	{
	}

	// Token: 0x06003D78 RID: 15736 RVA: 0x001033C8 File Offset: 0x001017C8
	public virtual void Deactivate()
	{
		if (this.isSelectionInitalized && this.uiManager.CurrentInputModule is UIInputModule)
		{
			((UIInputModule)this.uiManager.CurrentInputModule).EnableMouseControls();
		}
		if (this.UIInput == UIInputModuleType.Cursor)
		{
			this.mouseCursor = null;
			base.events.Broadcast(new DeactivateCursorModuleCommand());
		}
		else
		{
			base.events.Broadcast(new DeactivateUIInputModuleCommand(this.UIInput));
		}
	}

	// Token: 0x06003D79 RID: 15737 RVA: 0x00103448 File Offset: 0x00101848
	public virtual void OnCancelPressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003D7A RID: 15738 RVA: 0x0010344A File Offset: 0x0010184A
	public virtual void OnStartPressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003D7B RID: 15739 RVA: 0x0010344C File Offset: 0x0010184C
	public virtual void OnSubmitPressed(PointerEventData eventData)
	{
	}

	// Token: 0x06003D7C RID: 15740 RVA: 0x0010344E File Offset: 0x0010184E
	public virtual void OnAltCancelPressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003D7D RID: 15741 RVA: 0x00103450 File Offset: 0x00101850
	public virtual void OnAltSubmitPressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003D7E RID: 15742 RVA: 0x00103452 File Offset: 0x00101852
	public virtual void OnMouseMode()
	{
		this.onMouseModeUpdate();
	}

	// Token: 0x06003D7F RID: 15743 RVA: 0x0010345A File Offset: 0x0010185A
	public virtual void OnControllerMode()
	{
		this.onMouseModeUpdate();
	}

	// Token: 0x06003D80 RID: 15744 RVA: 0x00103462 File Offset: 0x00101862
	public virtual void OnAdvance1Pressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003D81 RID: 15745 RVA: 0x00103464 File Offset: 0x00101864
	public virtual void OnPrevious1Pressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003D82 RID: 15746 RVA: 0x00103466 File Offset: 0x00101866
	public virtual void OnAdvance2Pressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003D83 RID: 15747 RVA: 0x00103468 File Offset: 0x00101868
	public virtual void OnPrevious2Pressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003D84 RID: 15748 RVA: 0x0010346A File Offset: 0x0010186A
	public virtual void OnRightStickUpPressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003D85 RID: 15749 RVA: 0x0010346C File Offset: 0x0010186C
	public virtual void OnRightStickDownPressed(IPlayerCursor cursor)
	{
	}

	// Token: 0x06003D86 RID: 15750 RVA: 0x0010346E File Offset: 0x0010186E
	public virtual void GoToNextScreen()
	{
	}

	// Token: 0x06003D87 RID: 15751 RVA: 0x00103470 File Offset: 0x00101870
	public virtual void SelectOption(int option, int player)
	{
	}

	// Token: 0x06003D88 RID: 15752 RVA: 0x00103472 File Offset: 0x00101872
	protected virtual void initVisualCursorDevice(PlayerCursor cursor, PlayerNum playerNum)
	{
		cursor.Init(playerNum);
	}

	// Token: 0x06003D89 RID: 15753 RVA: 0x0010347C File Offset: 0x0010187C
	protected virtual PlayerCursor createCursor(PlayerNum playerNum)
	{
		PlayerCursor component = UnityEngine.Object.Instantiate<GameObject>(this.CursorPrefab).GetComponent<PlayerCursor>();
		base.injector.Inject(component);
		this.uiManager.AddCursor(component);
		this.initVisualCursorDevice(component, playerNum);
		component.ResetPosition(this.getCursorDefaultPosition(playerNum));
		this.cursors.Add(component);
		return component;
	}

	// Token: 0x06003D8A RID: 15754 RVA: 0x001034D4 File Offset: 0x001018D4
	protected void destroyAllCursors()
	{
		for (int i = this.cursors.Count - 1; i >= 0; i--)
		{
			this.destroyCursor(this.cursors[i]);
		}
	}

	// Token: 0x06003D8B RID: 15755 RVA: 0x00103511 File Offset: 0x00101911
	protected virtual void destroyCursor(IPlayerCursor cursor)
	{
		this.cursors.Remove(cursor);
		UnityEngine.Object.Destroy((cursor as MonoBehaviour).gameObject);
	}

	// Token: 0x06003D8C RID: 15756 RVA: 0x00103530 File Offset: 0x00101930
	protected void hideCursor(IPlayerCursor cursor)
	{
		cursor.IsHidden = true;
	}

	// Token: 0x06003D8D RID: 15757 RVA: 0x00103539 File Offset: 0x00101939
	protected void showCursor(IPlayerCursor cursor)
	{
		cursor.IsHidden = false;
	}

	// Token: 0x06003D8E RID: 15758 RVA: 0x00103542 File Offset: 0x00101942
	protected bool isCursorShown(IPlayerCursor cursor)
	{
		return !cursor.IsHidden;
	}

	// Token: 0x06003D8F RID: 15759 RVA: 0x00103550 File Offset: 0x00101950
	protected void pauseAllCursors()
	{
		for (int i = this.cursors.Count - 1; i >= 0; i--)
		{
			this.cursors[i].IsPaused = true;
		}
	}

	// Token: 0x06003D90 RID: 15760 RVA: 0x00103590 File Offset: 0x00101990
	protected void unpauseAllCursors()
	{
		for (int i = this.cursors.Count - 1; i >= 0; i--)
		{
			this.cursors[i].IsPaused = false;
		}
	}

	// Token: 0x06003D91 RID: 15761 RVA: 0x001035D0 File Offset: 0x001019D0
	protected IPlayerCursor findPlayerCursor(PlayerNum playerNum)
	{
		for (int i = this.cursors.Count - 1; i >= 0; i--)
		{
			if (this.cursors[i] is MousePlayerCursor)
			{
				if (playerNum == PlayerNum.All)
				{
					return this.cursors[i];
				}
			}
			else
			{
				PlayerCursor playerCursor = this.cursors[i] as PlayerCursor;
				if (playerCursor != null && playerCursor.Player == playerNum)
				{
					return playerCursor;
				}
			}
		}
		return null;
	}

	// Token: 0x06003D92 RID: 15762 RVA: 0x00103658 File Offset: 0x00101A58
	public virtual void OnAnythingPressed()
	{
		if (this.isSelectionInitalized && this.uiManager.CurrentInputModule is UIInputModule)
		{
			((UIInputModule)this.uiManager.CurrentInputModule).DisableMouseControls();
			this.handleMenuSelectionOnButtonMode();
		}
		this.onMouseModeUpdate();
	}

	// Token: 0x06003D93 RID: 15763 RVA: 0x001036A6 File Offset: 0x00101AA6
	public virtual void OnAnyNavigationButtonPressed()
	{
	}

	// Token: 0x06003D94 RID: 15764 RVA: 0x001036A8 File Offset: 0x00101AA8
	public virtual void OnAnyMouseEvent()
	{
		if (this.isSelectionInitalized && this.uiManager.CurrentInputModule is UIInputModule)
		{
			((UIInputModule)this.uiManager.CurrentInputModule).EnableMouseControls();
		}
		this.onMouseModeUpdate();
	}

	// Token: 0x06003D95 RID: 15765 RVA: 0x001036E8 File Offset: 0x00101AE8
	protected virtual void onMouseModeUpdate()
	{
		this.uiManager.OnUpdateMouseMode();
		foreach (InputInstructions inputInstructions in this.inputInstructions)
		{
			inputInstructions.SetControlMode(this.uiManager.CurrentInputModule.CurrentMode);
		}
	}

	// Token: 0x17000EB5 RID: 3765
	// (get) Token: 0x06003D96 RID: 15766 RVA: 0x00103760 File Offset: 0x00101B60
	public virtual bool AlwaysHideMouse
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003D97 RID: 15767 RVA: 0x00103764 File Offset: 0x00101B64
	protected MenuItemList createMenuController()
	{
		MenuItemList instance = base.injector.GetInstance<MenuItemList>();
		this.registerMenuController(instance);
		return instance;
	}

	// Token: 0x06003D98 RID: 15768 RVA: 0x00103785 File Offset: 0x00101B85
	private void registerMenuController(MenuItemList menu)
	{
		this.menuControllers.Add(menu);
	}

	// Token: 0x06003D99 RID: 15769 RVA: 0x00103794 File Offset: 0x00101B94
	protected bool isAnyMenuItemSelected()
	{
		foreach (MenuItemList menuItemList in this.menuControllers)
		{
			if (menuItemList.CurrentSelection != null)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003D9A RID: 15770 RVA: 0x00103804 File Offset: 0x00101C04
	protected virtual bool isAnythingFocused()
	{
		return this.isAnyMenuItemSelected() || this.uiManager.GetWindowCount() > 0;
	}

	// Token: 0x06003D9B RID: 15771 RVA: 0x00103828 File Offset: 0x00101C28
	protected virtual void handleMenuSelectionOnButtonMode()
	{
		if (!this.isAnythingFocused())
		{
			FirstSelectionInfo initialSelection = this.getInitialSelection();
			if (initialSelection != null)
			{
				if (initialSelection.button && initialSelection.button.InteractableButton && initialSelection.menu != null)
				{
					this.FirstSelected = initialSelection.button.InteractableButton.gameObject;
					initialSelection.menu.AutoSelect(initialSelection.button);
				}
				else
				{
					Debug.LogWarningFormat("Null Button, Null interactableButton, or Null Menu in WillSelect. {0}, {1}", new object[]
					{
						(initialSelection.button).ToString(),
						(initialSelection.button.InteractableButton).ToString(),
						(initialSelection.menu != null).ToString()
					});
				}
			}
		}
	}

	// Token: 0x06003D9C RID: 15772 RVA: 0x0010391C File Offset: 0x00101D1C
	protected virtual FirstSelectionInfo getInitialSelection()
	{
		if (this.menuControllers.Count > 0)
		{
			MenuItemList menuItemList = this.menuControllers[0];
			MenuItemButton[] buttons = menuItemList.GetButtons();
			if (buttons.Length > 0)
			{
				return new FirstSelectionInfo(menuItemList, buttons[0]);
			}
		}
		return null;
	}

	// Token: 0x06003D9D RID: 15773 RVA: 0x00103962 File Offset: 0x00101D62
	protected void selectTextField(WavedashTMProInput field)
	{
		this.uiManager.CurrentInputModule.SetSelectedInputField(field);
	}

	// Token: 0x06003D9E RID: 15774 RVA: 0x00103975 File Offset: 0x00101D75
	protected void initMenuSelectionSystem()
	{
		this.isSelectionInitalized = true;
		if (this.uiManager.CurrentInputModule != null)
		{
			if (this.uiManager.CurrentInputModule.CurrentMode != ControlMode.MouseMode)
			{
				this.OnAnythingPressed();
			}
			else
			{
				this.OnAnyMouseEvent();
			}
		}
	}

	// Token: 0x06003D9F RID: 15775 RVA: 0x001039B8 File Offset: 0x00101DB8
	protected GameObject addBackButtonForCursorScreen(Transform buttonAnchor, GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		while (buttonAnchor.childCount > 0)
		{
			Transform child = buttonAnchor.GetChild(0);
			child.SetParent(null);
			UnityEngine.Object.DestroyImmediate(child.gameObject);
		}
		gameObject.transform.SetParent(buttonAnchor, false);
		CursorTargetButton componentInChildren = gameObject.GetComponentInChildren<CursorTargetButton>();
		componentInChildren.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onBackButtonSubmit);
		return gameObject;
	}

	// Token: 0x06003DA0 RID: 15776 RVA: 0x00103A1E File Offset: 0x00101E1E
	private void onBackButtonSubmit(CursorTargetButton theButton, PointerEventData pointerEvent)
	{
		this.GoToPreviousScreen();
	}

	// Token: 0x06003DA1 RID: 15777 RVA: 0x00103A28 File Offset: 0x00101E28
	protected InputInstructions addBackButtonForMenuScreen(Transform buttonAnchor, GameObject prefab)
	{
		InputInstructions inputInstructions = this.addInputInstuctionsForMenuScreen(buttonAnchor, prefab);
		WavedashUIButton mouseInstuctionsButton = inputInstructions.MouseInstuctionsButton;
		mouseInstuctionsButton.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(mouseInstuctionsButton.OnPointerClickEvent, new Action<InputEventData>(this.onBackButtonClicked));
		return inputInstructions;
	}

	// Token: 0x06003DA2 RID: 15778 RVA: 0x00103A68 File Offset: 0x00101E68
	protected InputInstructions addInputInstuctionsForMenuScreen(Transform instructionsAnchor, GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		while (instructionsAnchor.childCount > 0)
		{
			Transform child = instructionsAnchor.GetChild(0);
			child.SetParent(null);
			UnityEngine.Object.DestroyImmediate(child.gameObject);
		}
		InputInstructions component = gameObject.GetComponent<InputInstructions>();
		if (component == null)
		{
			Debug.LogError("Could not find an Input Instructions component on this object. One must be attached.");
		}
		gameObject.transform.SetParent(instructionsAnchor, false);
		this.inputInstructions.Add(component);
		ControlMode controlMode = (this.uiManager.CurrentInputModule == null) ? ControlMode.MouseMode : this.uiManager.CurrentInputModule.CurrentMode;
		component.SetControlMode(controlMode);
		return component;
	}

	// Token: 0x06003DA3 RID: 15779 RVA: 0x00103B0C File Offset: 0x00101F0C
	protected virtual void onBackButtonClicked(InputEventData inputEventData)
	{
		this.GoToPreviousScreen();
	}

	// Token: 0x06003DA4 RID: 15780 RVA: 0x00103B14 File Offset: 0x00101F14
	public virtual void GoToPreviousScreen()
	{
		base.events.Broadcast(new PreviousScreenRequest());
	}

	// Token: 0x06003DA5 RID: 15781 RVA: 0x00103B26 File Offset: 0x00101F26
	public virtual void OnCancelPressed()
	{
	}

	// Token: 0x06003DA6 RID: 15782 RVA: 0x00103B28 File Offset: 0x00101F28
	public virtual void OnSubmitPressed()
	{
	}

	// Token: 0x06003DA7 RID: 15783 RVA: 0x00103B2A File Offset: 0x00101F2A
	public virtual void OnRightTriggerPressed()
	{
	}

	// Token: 0x06003DA8 RID: 15784 RVA: 0x00103B2C File Offset: 0x00101F2C
	public virtual void OnLeftTriggerPressed()
	{
	}

	// Token: 0x06003DA9 RID: 15785 RVA: 0x00103B2E File Offset: 0x00101F2E
	public virtual void OnLeftBumperPressed()
	{
	}

	// Token: 0x06003DAA RID: 15786 RVA: 0x00103B30 File Offset: 0x00101F30
	public virtual void OnZPressed()
	{
	}

	// Token: 0x06003DAB RID: 15787 RVA: 0x00103B32 File Offset: 0x00101F32
	public virtual void OnRightStickRight()
	{
	}

	// Token: 0x06003DAC RID: 15788 RVA: 0x00103B34 File Offset: 0x00101F34
	public virtual void OnRightStickLeft()
	{
	}

	// Token: 0x06003DAD RID: 15789 RVA: 0x00103B36 File Offset: 0x00101F36
	public virtual void OnRightStickUp()
	{
	}

	// Token: 0x06003DAE RID: 15790 RVA: 0x00103B38 File Offset: 0x00101F38
	public virtual void OnRightStickDown()
	{
	}

	// Token: 0x06003DAF RID: 15791 RVA: 0x00103B3A File Offset: 0x00101F3A
	public virtual void UpdateRightStick(float x, float y)
	{
	}

	// Token: 0x06003DB0 RID: 15792 RVA: 0x00103B3C File Offset: 0x00101F3C
	public virtual void OnLeft()
	{
	}

	// Token: 0x06003DB1 RID: 15793 RVA: 0x00103B3E File Offset: 0x00101F3E
	public virtual void OnRight()
	{
	}

	// Token: 0x06003DB2 RID: 15794 RVA: 0x00103B40 File Offset: 0x00101F40
	public virtual void OnUp()
	{
	}

	// Token: 0x06003DB3 RID: 15795 RVA: 0x00103B42 File Offset: 0x00101F42
	public virtual void OnDown()
	{
	}

	// Token: 0x06003DB4 RID: 15796 RVA: 0x00103B44 File Offset: 0x00101F44
	public virtual void OnDPadLeft()
	{
	}

	// Token: 0x06003DB5 RID: 15797 RVA: 0x00103B46 File Offset: 0x00101F46
	public virtual void OnDPadRight()
	{
	}

	// Token: 0x06003DB6 RID: 15798 RVA: 0x00103B48 File Offset: 0x00101F48
	public virtual void OnDPadUp()
	{
	}

	// Token: 0x06003DB7 RID: 15799 RVA: 0x00103B4A File Offset: 0x00101F4A
	public virtual void OnDPadDown()
	{
	}

	// Token: 0x06003DB8 RID: 15800 RVA: 0x00103B4C File Offset: 0x00101F4C
	public virtual void OnYButtonPressed()
	{
	}

	// Token: 0x06003DB9 RID: 15801 RVA: 0x00103B4E File Offset: 0x00101F4E
	public virtual void OnXButtonPressed()
	{
	}

	// Token: 0x17000EB6 RID: 3766
	// (get) Token: 0x06003DBA RID: 15802 RVA: 0x00103B50 File Offset: 0x00101F50
	// (set) Token: 0x06003DBB RID: 15803 RVA: 0x00103B58 File Offset: 0x00101F58
	public float Alpha
	{
		get
		{
			return this._alpha;
		}
		set
		{
			this._alpha = value;
			this._canvasGroup.alpha = this._alpha;
		}
	}

	// Token: 0x040029E3 RID: 10723
	public GameObject FirstSelected;

	// Token: 0x040029E4 RID: 10724
	public GameObject CursorPrefab;

	// Token: 0x040029E5 RID: 10725
	public UIInputModuleType UIInput = UIInputModuleType.Menu;

	// Token: 0x040029E6 RID: 10726
	public bool listenForDevices;

	// Token: 0x040029E7 RID: 10727
	protected bool isShown;

	// Token: 0x040029E8 RID: 10728
	private float _screenWidth;

	// Token: 0x040029E9 RID: 10729
	private float _alpha = 1f;

	// Token: 0x040029EA RID: 10730
	private CanvasGroup _canvasGroup;

	// Token: 0x040029EB RID: 10731
	protected MousePlayerCursor mouseCursor;

	// Token: 0x040029EC RID: 10732
	protected List<IPlayerCursor> cursors = new List<IPlayerCursor>();

	// Token: 0x040029ED RID: 10733
	private List<MenuItemList> menuControllers = new List<MenuItemList>();

	// Token: 0x040029EE RID: 10734
	private List<InputInstructions> inputInstructions = new List<InputInstructions>();

	// Token: 0x040029EF RID: 10735
	protected Payload payload;

	// Token: 0x040029F0 RID: 10736
	private bool didLoadPayload;

	// Token: 0x040029F1 RID: 10737
	protected LoadSequenceResults data;

	// Token: 0x040029F2 RID: 10738
	protected bool isDestroyed;

	// Token: 0x040029F3 RID: 10739
	private bool isStarted;

	// Token: 0x040029F4 RID: 10740
	private bool didPostStartAndPayload;

	// Token: 0x040029F5 RID: 10741
	private bool didTransistionBegin;

	// Token: 0x040029F6 RID: 10742
	private bool isSelectionInitalized;

	// Token: 0x040029F7 RID: 10743
	private InputBlock block;

	// Token: 0x040029F8 RID: 10744
	public Action OnDrawCompleteCallback;
}
