using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000916 RID: 2326
public class CursorInputModule : PointerInputModule, ICustomInputModule
{
	// Token: 0x17000E65 RID: 3685
	// (get) Token: 0x06003C5A RID: 15450 RVA: 0x00118217 File Offset: 0x00116617
	// (set) Token: 0x06003C5B RID: 15451 RVA: 0x0011821F File Offset: 0x0011661F
	public UIManager uiManager { private get; set; }

	// Token: 0x06003C5C RID: 15452 RVA: 0x00118228 File Offset: 0x00116628
	protected override void Start()
	{
		base.Start();
		if (this.cursorObjects == null || base.eventSystem == null)
		{
			Debug.LogError("Set the game objects in the cursor module.");
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003C5D RID: 15453 RVA: 0x00118261 File Offset: 0x00116661
	public void SuppressButtonsPressedThisFrame()
	{
		this.suppressActionsForFrames = 1;
		this.ShouldActivateModule();
		this.UpdateModule();
	}

	// Token: 0x06003C5E RID: 15454 RVA: 0x00118277 File Offset: 0x00116677
	public void InitControlMode(ControlMode controlMode)
	{
		this.lastMode = controlMode;
	}

	// Token: 0x06003C5F RID: 15455 RVA: 0x00118280 File Offset: 0x00116680
	public override void Process()
	{
	}

	// Token: 0x06003C60 RID: 15456 RVA: 0x00118284 File Offset: 0x00116684
	public void ReprocessHoversImmediate()
	{
		int count = this.cursorObjects.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.cursorObjects == null || this.cursorObjects.Count <= i)
			{
				break;
			}
			IPlayerCursor playerCursor = this.cursorObjects[i];
			if (playerCursor.CurrentMode != global::CursorMode.Disabled)
			{
				base.GetPointerData(i, out this.pointer, true);
				this.pointer.pointerId = playerCursor.PointerId;
				this.pointer.position = playerCursor.Position;
				base.eventSystem.RaycastAll(this.pointer, this.m_RaycastResultCache);
				playerCursor.RaycastCache = this.m_RaycastResultCache.ToArray();
				RaycastResult pointerCurrentRaycast = CursorInputModule.FindFirstValidRaycast(this.pointer, this.m_RaycastResultCache);
				this.pointer.pointerCurrentRaycast = pointerCurrentRaycast;
				this.ProcessMove(this.pointer);
			}
		}
	}

	// Token: 0x06003C61 RID: 15457 RVA: 0x00118374 File Offset: 0x00116774
	public virtual void Update()
	{
		if (!base.enabled || this.cursorObjects == null)
		{
			return;
		}
		if (this.suppressActionsForFrames == 0)
		{
			ControlMode controlMode = ControlMode.ControllerMode;
			bool flag = false;
			int count = this.cursorObjects.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.cursorObjects == null || this.cursorObjects.Count <= i)
				{
					break;
				}
				IPlayerCursor playerCursor = this.cursorObjects[i];
				if (playerCursor.CurrentMode != global::CursorMode.Disabled)
				{
					base.GetPointerData(i, out this.pointer, true);
					this.pointer.pointerId = playerCursor.PointerId;
					if (playerCursor.AnythingPressed)
					{
						flag = true;
						if (playerCursor.CurrentMode == global::CursorMode.Mouse)
						{
							controlMode = ControlMode.MouseMode;
						}
						else if (playerCursor.CurrentMode == global::CursorMode.Keyboard)
						{
							controlMode = ControlMode.KeyboardMode;
						}
						else if (playerCursor.CurrentMode == global::CursorMode.Controller)
						{
							controlMode = ControlMode.ControllerMode;
						}
					}
					this.pointer.position = playerCursor.Position;
					base.eventSystem.RaycastAll(this.pointer, this.m_RaycastResultCache);
					playerCursor.RaycastCache = this.m_RaycastResultCache.ToArray();
					RaycastResult raycastResult = CursorInputModule.FindFirstValidRaycast(this.pointer, this.m_RaycastResultCache);
					this.pointer.pointerCurrentRaycast = raycastResult;
					this.ProcessMove(this.pointer);
					this.pointer.clickCount = 0;
					if (playerCursor.LastSelectedObject != raycastResult.gameObject)
					{
						playerCursor.LastSelectedObject = raycastResult.gameObject;
						this.pointer.selectedObject = raycastResult.gameObject;
					}
					if (this.m_RaycastResultCache.Count > 0)
					{
						if (playerCursor.SubmitPressed)
						{
							this.pointer.clickTime = Time.unscaledTime;
							this.pointer.pointerPressRaycast = raycastResult;
							this.pointer.clickCount = 1;
							this.pointer.eligibleForClick = true;
							this.pointer.pointerPress = ExecuteEvents.ExecuteHierarchy<ISubmitHandler>(raycastResult.gameObject, this.pointer, ExecuteEvents.submitHandler);
							ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(raycastResult.gameObject, this.pointer, ExecuteEvents.pointerDownHandler);
							if (this.suppressActionsForFrames == 0)
							{
								this.setCurrentlyClicked(raycastResult.gameObject);
							}
							this.pointer.rawPointerPress = raycastResult.gameObject;
						}
						else if (playerCursor.SubmitReleased)
						{
							this.pointer.clickTime = Time.unscaledTime;
							this.pointer.pointerPressRaycast = raycastResult;
							this.pointer.clickCount = 1;
							this.pointer.eligibleForClick = true;
							this.pointer.pointerPress = null;
							ExecuteEvents.ExecuteHierarchy<IPointerUpHandler>(raycastResult.gameObject, this.pointer, ExecuteEvents.pointerUpHandler);
							this.pointer.rawPointerPress = raycastResult.gameObject;
						}
						else if (playerCursor.AltSubmitPressed)
						{
							this.pointer.clickTime = Time.unscaledTime;
							this.pointer.pointerPressRaycast = raycastResult;
							this.pointer.clickCount = 1;
							this.pointer.eligibleForClick = true;
							this.pointer.pointerPress = ExecuteEvents.ExecuteHierarchy<IAltSubmitHandler>(raycastResult.gameObject, this.pointer, ExecuteEventsUtil.altSubmitHandler);
							this.pointer.rawPointerPress = raycastResult.gameObject;
						}
						else if (playerCursor.AdvanceSelectedPressed)
						{
							this.pointer.clickTime = Time.unscaledTime;
							this.pointer.pointerPressRaycast = raycastResult;
							this.pointer.clickCount = 1;
							this.pointer.eligibleForClick = true;
							this.pointer.pointerPress = ExecuteEvents.ExecuteHierarchy<IAdvanceHandler>(raycastResult.gameObject, this.pointer, ExecuteEventsUtil.advanceHandler);
							this.pointer.rawPointerPress = raycastResult.gameObject;
						}
						else if (playerCursor.PreviousSelectedPressed)
						{
							this.pointer.clickTime = Time.unscaledTime;
							this.pointer.pointerPressRaycast = raycastResult;
							this.pointer.clickCount = 1;
							this.pointer.eligibleForClick = true;
							this.pointer.pointerPress = ExecuteEvents.ExecuteHierarchy<IPreviousHandler>(raycastResult.gameObject, this.pointer, ExecuteEventsUtil.previousHandler);
							this.pointer.rawPointerPress = raycastResult.gameObject;
						}
						else if (playerCursor.FaceButton3Pressed)
						{
							this.pointer.clickTime = Time.unscaledTime;
							this.pointer.pointerPressRaycast = raycastResult;
							this.pointer.clickCount = 1;
							this.pointer.eligibleForClick = true;
							this.pointer.pointerPress = ExecuteEvents.ExecuteHierarchy<IAltSubmitHandler>(raycastResult.gameObject, this.pointer, ExecuteEventsUtil.faceButton3Handler);
							this.pointer.rawPointerPress = raycastResult.gameObject;
						}
						else
						{
							this.pointer.clickCount = 0;
							this.pointer.eligibleForClick = false;
							this.pointer.pointerPress = null;
							this.pointer.rawPointerPress = null;
						}
					}
					if (this.cursorDelegate != null)
					{
						this.processGenericInputs(this.cursorDelegate, playerCursor);
					}
				}
			}
			if (count > 0 && (controlMode != this.lastMode || !this.isModeInitialized) && flag)
			{
				this.isModeInitialized = true;
				this.lastMode = controlMode;
				if (this.cursorDelegate != null)
				{
					if (controlMode == ControlMode.MouseMode)
					{
						this.cursorDelegate.OnMouseMode();
					}
					else
					{
						this.cursorDelegate.OnControllerMode();
					}
				}
				this.uiManager.OnUpdateMouseMode();
			}
			if (this.currentInputField != null)
			{
				BaseEventData baseEventData = this.GetBaseEventData();
				if (Input.GetKeyDown(KeyCode.Return))
				{
					this.currentInputField.OnSubmitPressed();
				}
				else if (Input.GetKeyDown(KeyCode.Tab))
				{
					this.currentInputField.OnTabPressed();
				}
				if (this.currentInputField != null)
				{
					ExecuteEvents.Execute<IUpdateSelectedHandler>(this.currentInputField.gameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
				}
			}
		}
		if (this.suppressActionsForFrames > 0)
		{
			this.suppressActionsForFrames--;
		}
	}

	// Token: 0x06003C62 RID: 15458 RVA: 0x00118977 File Offset: 0x00116D77
	public void SetSelectedInputField(WavedashTMProInput inputField)
	{
		if (inputField == null)
		{
			this.releaseCurrentlyClicked();
		}
		else
		{
			this.setCurrentlyClicked(inputField.gameObject);
		}
	}

	// Token: 0x06003C63 RID: 15459 RVA: 0x0011899C File Offset: 0x00116D9C
	private void setCurrentlyClicked(GameObject obj)
	{
		if (this.cursorObjects == null)
		{
			return;
		}
		if (this.currentInputField != obj)
		{
			WavedashTMProInput x = this.currentInputField;
			if (this.currentInputField != null)
			{
				WavedashTMProInput wavedashTMProInput = this.currentInputField;
				wavedashTMProInput.EndEditCallback = (Action)Delegate.Remove(wavedashTMProInput.EndEditCallback, new Action(this.releaseCurrentlyClicked));
				this.currentInputField.SetInputActive(false);
			}
			if (obj == null)
			{
				this.currentInputField = null;
			}
			else
			{
				this.currentInputField = obj.GetComponent<WavedashTMProInput>();
			}
			if (this.currentInputField != null)
			{
				this.currentInputField.SetInputActive(true);
				WavedashTMProInput wavedashTMProInput2 = this.currentInputField;
				wavedashTMProInput2.EndEditCallback = (Action)Delegate.Combine(wavedashTMProInput2.EndEditCallback, new Action(this.releaseCurrentlyClicked));
			}
			bool suppress = this.currentInputField != null;
			foreach (IPlayerCursor playerCursor in this.cursorObjects)
			{
				playerCursor.SuppressKeyboard(suppress);
			}
			if (x != null)
			{
				this.uiManager.ConsumeButtonPresses();
			}
		}
	}

	// Token: 0x06003C64 RID: 15460 RVA: 0x00118AEC File Offset: 0x00116EEC
	private void releaseCurrentlyClicked()
	{
		this.setCurrentlyClicked(null);
		this.suppressActionsForFrames = 2;
	}

	// Token: 0x06003C65 RID: 15461 RVA: 0x00118AFC File Offset: 0x00116EFC
	protected static RaycastResult FindFirstValidRaycast(PointerEventData eventData, List<RaycastResult> candidates)
	{
		for (int i = 0; i < candidates.Count; i++)
		{
			if (!(candidates[i].gameObject == null))
			{
				CursorTargetButton component = candidates[i].gameObject.GetComponent<CursorTargetButton>();
				if (!(component != null) || component.IsAuthorized(eventData))
				{
					return candidates[i];
				}
			}
		}
		return default(RaycastResult);
	}

	// Token: 0x17000E66 RID: 3686
	// (get) Token: 0x06003C66 RID: 15462 RVA: 0x00118B83 File Offset: 0x00116F83
	public ControlMode CurrentMode
	{
		get
		{
			return this.lastMode;
		}
	}

	// Token: 0x17000E67 RID: 3687
	// (get) Token: 0x06003C67 RID: 15463 RVA: 0x00118B8B File Offset: 0x00116F8B
	public bool IsMouseMode
	{
		get
		{
			return this.lastMode == ControlMode.MouseMode;
		}
	}

	// Token: 0x17000E68 RID: 3688
	// (get) Token: 0x06003C68 RID: 15464 RVA: 0x00118B96 File Offset: 0x00116F96
	public WavedashTMProInput CurrentInputField
	{
		get
		{
			return this.currentInputField;
		}
	}

	// Token: 0x06003C69 RID: 15465 RVA: 0x00118B9E File Offset: 0x00116F9E
	public void SetPauseMode(bool isPause)
	{
	}

	// Token: 0x06003C6A RID: 15466 RVA: 0x00118BA0 File Offset: 0x00116FA0
	protected virtual void processGenericInputs(ICursorInputDelegate cursorDelegate, IPlayerCursor cursorObject)
	{
		if (cursorObject.CancelPressed)
		{
			cursorDelegate.OnCancelPressed(cursorObject);
		}
		if (cursorObject.StartPressed)
		{
			cursorDelegate.OnStartPressed(cursorObject);
		}
		if (cursorObject.SubmitPressed)
		{
			cursorDelegate.OnSubmitPressed(this.pointer);
		}
		if (cursorObject.AltSubmitPressed)
		{
			cursorDelegate.OnAltSubmitPressed(cursorObject);
		}
		if (cursorObject.AltCancelPressed)
		{
			cursorDelegate.OnAltCancelPressed(cursorObject);
		}
		if (cursorObject.Advance1Pressed)
		{
			cursorDelegate.OnAdvance1Pressed(cursorObject);
		}
		if (cursorObject.Previous1Pressed)
		{
			cursorDelegate.OnPrevious1Pressed(cursorObject);
		}
		if (cursorObject.Advance2Pressed)
		{
			cursorDelegate.OnAdvance2Pressed(cursorObject);
		}
		if (cursorObject.Previous2Pressed)
		{
			cursorDelegate.OnPrevious2Pressed(cursorObject);
		}
		if (cursorObject.RightStickUpPressed)
		{
			cursorDelegate.OnRightStickUpPressed(cursorObject);
		}
		if (cursorObject.RightStickDownPressed)
		{
			cursorDelegate.OnRightStickDownPressed(cursorObject);
		}
	}

	// Token: 0x06003C6B RID: 15467 RVA: 0x00118C78 File Offset: 0x00117078
	bool ICustomInputModule.get_enabled()
	{
		return base.enabled;
	}

	// Token: 0x06003C6C RID: 15468 RVA: 0x00118C80 File Offset: 0x00117080
	void ICustomInputModule.set_enabled(bool value)
	{
		base.enabled = value;
	}

	// Token: 0x04002960 RID: 10592
	public List<IPlayerCursor> cursorObjects;

	// Token: 0x04002961 RID: 10593
	public ICursorInputDelegate cursorDelegate;

	// Token: 0x04002963 RID: 10595
	private PointerEventData pointer;

	// Token: 0x04002964 RID: 10596
	private WavedashTMProInput currentInputField;

	// Token: 0x04002965 RID: 10597
	protected int suppressActionsForFrames;

	// Token: 0x04002966 RID: 10598
	private bool isModeInitialized;

	// Token: 0x04002967 RID: 10599
	private ControlMode lastMode;
}
