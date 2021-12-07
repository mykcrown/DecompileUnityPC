// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorInputModule : PointerInputModule, ICustomInputModule
{
	public List<IPlayerCursor> cursorObjects;

	public ICursorInputDelegate cursorDelegate;

	private PointerEventData pointer;

	private WavedashTMProInput currentInputField;

	protected int suppressActionsForFrames;

	private bool isModeInitialized;

	private ControlMode lastMode;

	public UIManager uiManager
	{
		private get;
		set;
	}

	public ControlMode CurrentMode
	{
		get
		{
			return this.lastMode;
		}
	}

	public bool IsMouseMode
	{
		get
		{
			return this.lastMode == ControlMode.MouseMode;
		}
	}

	public WavedashTMProInput CurrentInputField
	{
		get
		{
			return this.currentInputField;
		}
	}

	protected override void Start()
	{
		base.Start();
		if (this.cursorObjects == null || base.eventSystem == null)
		{
			UnityEngine.Debug.LogError("Set the game objects in the cursor module.");
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void SuppressButtonsPressedThisFrame()
	{
		this.suppressActionsForFrames = 1;
		this.ShouldActivateModule();
		this.UpdateModule();
	}

	public void InitControlMode(ControlMode controlMode)
	{
		this.lastMode = controlMode;
	}

	public override void Process()
	{
	}

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
				WavedashTMProInput expr_3B = this.currentInputField;
				expr_3B.EndEditCallback = (Action)Delegate.Remove(expr_3B.EndEditCallback, new Action(this.releaseCurrentlyClicked));
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
				WavedashTMProInput expr_AF = this.currentInputField;
				expr_AF.EndEditCallback = (Action)Delegate.Combine(expr_AF.EndEditCallback, new Action(this.releaseCurrentlyClicked));
			}
			bool suppress = this.currentInputField != null;
			foreach (IPlayerCursor current in this.cursorObjects)
			{
				current.SuppressKeyboard(suppress);
			}
			if (x != null)
			{
				this.uiManager.ConsumeButtonPresses();
			}
		}
	}

	private void releaseCurrentlyClicked()
	{
		this.setCurrentlyClicked(null);
		this.suppressActionsForFrames = 2;
	}

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

	public void SetPauseMode(bool isPause)
	{
	}

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

	bool ICustomInputModule.get_enabled()
	{
		return base.enabled;
	}

	void ICustomInputModule.set_enabled(bool value)
	{
		base.enabled = value;
	}
}
