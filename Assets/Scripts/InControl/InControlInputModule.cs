// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace InControl
{
	[AddComponentMenu("Event/InControl Input Module")]
	public class InControlInputModule : StandaloneInputModule
	{
		public enum Button
		{
			Action1 = 19,
			Action2,
			Action3,
			Action4
		}

		public new InControlInputModule.Button submitButton = InControlInputModule.Button.Action1;

		public new InControlInputModule.Button cancelButton = InControlInputModule.Button.Action2;

		[Range(0.1f, 0.9f)]
		public float analogMoveThreshold = 0.5f;

		public float moveRepeatFirstDuration = 0.8f;

		public float moveRepeatDelayDuration = 0.1f;

		private bool allowMobileDevice;

		[FormerlySerializedAs("allowMobileDevice")]
		public new bool forceModuleActive;

		public bool allowMouseInput = true;

		public bool focusOnMouseHover;

		private InputDevice inputDevice;

		private Vector3 thisMousePosition;

		private Vector3 lastMousePosition;

		protected Vector2 thisVectorState;

		protected Vector2 lastVectorState;

		private bool thisSubmitState;

		private bool lastSubmitState;

		private bool thisCancelState;

		private bool lastCancelState;

		private float nextMoveRepeatTime;

		private float lastVectorPressedTime;

		private TwoAxisInputControl direction;

		public PlayerAction SubmitAction
		{
			get;
			set;
		}

		public PlayerAction CancelAction
		{
			get;
			set;
		}

		public PlayerTwoAxisAction MoveAction
		{
			get;
			set;
		}

		public virtual InputDevice Device
		{
			get
			{
				return this.inputDevice ?? InputManager.ActiveDevice;
			}
			set
			{
				this.inputDevice = value;
			}
		}

		private InputControl SubmitButton
		{
			get
			{
				return this.Device.GetControl((InputControlType)this.submitButton);
			}
		}

		private InputControl CancelButton
		{
			get
			{
				return this.Device.GetControl((InputControlType)this.cancelButton);
			}
		}

		private bool VectorIsPressed
		{
			get
			{
				return this.thisVectorState != Vector2.zero;
			}
		}

		private bool VectorIsReleased
		{
			get
			{
				return this.thisVectorState == Vector2.zero;
			}
		}

		private bool VectorHasChanged
		{
			get
			{
				return this.thisVectorState != this.lastVectorState;
			}
		}

		protected virtual bool VectorWasPressed
		{
			get
			{
				return (this.VectorIsPressed && Time.realtimeSinceStartup > this.nextMoveRepeatTime) || (this.VectorIsPressed && this.lastVectorState == Vector2.zero);
			}
		}

		protected virtual bool SubmitWasPressed
		{
			get
			{
				return this.thisSubmitState && this.thisSubmitState != this.lastSubmitState;
			}
		}

		protected virtual bool SubmitWasReleased
		{
			get
			{
				return !this.thisSubmitState && this.thisSubmitState != this.lastSubmitState;
			}
		}

		protected virtual bool CancelWasPressed
		{
			get
			{
				return this.thisCancelState && this.thisCancelState != this.lastCancelState;
			}
		}

		protected virtual bool MouseHasMoved
		{
			get
			{
				return (this.thisMousePosition - this.lastMousePosition).sqrMagnitude > 0f;
			}
		}

		protected virtual bool MouseButtonIsPressed
		{
			get
			{
				return Input.GetMouseButtonDown(0);
			}
		}

		protected InControlInputModule()
		{
			this.direction = new TwoAxisInputControl();
			this.direction.StateThreshold = this.analogMoveThreshold;
		}

		public override void UpdateModule()
		{
			this.lastMousePosition = this.thisMousePosition;
			this.thisMousePosition = Input.mousePosition;
		}

		public override bool IsModuleSupported()
		{
			return this.forceModuleActive || Input.mousePresent;
		}

		public override bool ShouldActivateModule()
		{
			if (!base.enabled || !base.gameObject.activeInHierarchy)
			{
				return false;
			}
			this.UpdateInputState();
			bool flag = false;
			flag |= this.SubmitWasPressed;
			flag |= this.CancelWasPressed;
			flag |= this.VectorWasPressed;
			if (this.allowMouseInput)
			{
				flag |= this.MouseHasMoved;
				flag |= this.MouseButtonIsPressed;
			}
			if (Input.touchCount > 0)
			{
				flag = true;
			}
			return flag;
		}

		public override void ActivateModule()
		{
			base.ActivateModule();
			this.thisMousePosition = Input.mousePosition;
			this.lastMousePosition = Input.mousePosition;
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		public override void Process()
		{
		}

		public virtual void Update()
		{
			this.direction.Filter(this.Device.Direction, Time.deltaTime);
			bool flag = base.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag = this.SendVectorEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendButtonEventToSelectedObject();
				}
			}
			if (this.allowMouseInput)
			{
				base.ProcessMouseEvent();
			}
		}

		private bool SendButtonEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			if (this.SubmitWasPressed)
			{
				ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
			}
			else if (this.SubmitWasReleased)
			{
			}
			if (this.CancelWasPressed)
			{
				ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
			}
			return baseEventData.used;
		}

		private bool SendVectorEventToSelectedObject()
		{
			if (!this.VectorWasPressed)
			{
				return false;
			}
			AxisEventData axisEventData = this.GetAxisEventData(this.thisVectorState.x, this.thisVectorState.y, 0.5f);
			if (axisEventData.moveDir != MoveDirection.None)
			{
				if (base.eventSystem.currentSelectedGameObject == null)
				{
					base.eventSystem.SetSelectedGameObject(base.eventSystem.firstSelectedGameObject, this.GetBaseEventData());
				}
				else
				{
					ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
				}
				this.SetVectorRepeatTimer();
			}
			return axisEventData.used;
		}

		protected override void ProcessMove(PointerEventData pointerEvent)
		{
			GameObject pointerEnter = pointerEvent.pointerEnter;
			base.ProcessMove(pointerEvent);
			if (this.focusOnMouseHover && pointerEnter != pointerEvent.pointerEnter)
			{
				GameObject eventHandler = ExecuteEvents.GetEventHandler<ISelectHandler>(pointerEvent.pointerEnter);
				base.eventSystem.SetSelectedGameObject(eventHandler, pointerEvent);
			}
		}

		private void UpdateInputState()
		{
			this.lastVectorState = this.thisVectorState;
			this.thisVectorState = Vector2.zero;
			TwoAxisInputControl twoAxisInputControl = this.MoveAction ?? this.direction;
			if (Utility.AbsoluteIsOverThreshold(twoAxisInputControl.X, this.analogMoveThreshold))
			{
				this.thisVectorState.x = Mathf.Sign(twoAxisInputControl.X);
			}
			if (Utility.AbsoluteIsOverThreshold(twoAxisInputControl.Y, this.analogMoveThreshold))
			{
				this.thisVectorState.y = Mathf.Sign(twoAxisInputControl.Y);
			}
			if (this.VectorIsReleased)
			{
				this.nextMoveRepeatTime = 0f;
			}
			if (this.VectorIsPressed && this.lastVectorState == Vector2.zero)
			{
				this.nextMoveRepeatTime = Time.realtimeSinceStartup + this.moveRepeatFirstDuration;
			}
			this.lastSubmitState = this.thisSubmitState;
			this.thisSubmitState = ((this.SubmitAction != null) ? this.SubmitAction.IsPressed : this.SubmitButton.IsPressed);
			this.lastCancelState = this.thisCancelState;
			this.thisCancelState = ((this.CancelAction != null) ? this.CancelAction.IsPressed : this.CancelButton.IsPressed);
		}

		private void SetVectorRepeatTimer()
		{
			this.nextMoveRepeatTime = Mathf.Max(this.nextMoveRepeatTime, Time.realtimeSinceStartup + this.moveRepeatDelayDuration);
		}
	}
}
