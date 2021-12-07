using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace InControl
{
	// Token: 0x0200007D RID: 125
	[AddComponentMenu("Event/InControl Input Module")]
	public class InControlInputModule : StandaloneInputModule
	{
		// Token: 0x060004E3 RID: 1251 RVA: 0x00018FD8 File Offset: 0x000173D8
		protected InControlInputModule()
		{
			this.direction = new TwoAxisInputControl();
			this.direction.StateThreshold = this.analogMoveThreshold;
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x0001903F File Offset: 0x0001743F
		// (set) Token: 0x060004E5 RID: 1253 RVA: 0x00019047 File Offset: 0x00017447
		public PlayerAction SubmitAction { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00019050 File Offset: 0x00017450
		// (set) Token: 0x060004E7 RID: 1255 RVA: 0x00019058 File Offset: 0x00017458
		public PlayerAction CancelAction { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x00019061 File Offset: 0x00017461
		// (set) Token: 0x060004E9 RID: 1257 RVA: 0x00019069 File Offset: 0x00017469
		public PlayerTwoAxisAction MoveAction { get; set; }

		// Token: 0x060004EA RID: 1258 RVA: 0x00019072 File Offset: 0x00017472
		public override void UpdateModule()
		{
			this.lastMousePosition = this.thisMousePosition;
			this.thisMousePosition = Input.mousePosition;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001908B File Offset: 0x0001748B
		public override bool IsModuleSupported()
		{
			return this.forceModuleActive || Input.mousePresent;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000190A8 File Offset: 0x000174A8
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

		// Token: 0x060004ED RID: 1261 RVA: 0x00019120 File Offset: 0x00017520
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

		// Token: 0x060004EE RID: 1262 RVA: 0x0001917F File Offset: 0x0001757F
		public override void Process()
		{
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00019184 File Offset: 0x00017584
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

		// Token: 0x060004F0 RID: 1264 RVA: 0x000191F0 File Offset: 0x000175F0
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

		// Token: 0x060004F1 RID: 1265 RVA: 0x00019278 File Offset: 0x00017678
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

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001931C File Offset: 0x0001771C
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

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001936C File Offset: 0x0001776C
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

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x000194BD File Offset: 0x000178BD
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x000194B4 File Offset: 0x000178B4
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

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x000194D1 File Offset: 0x000178D1
		private InputControl SubmitButton
		{
			get
			{
				return this.Device.GetControl((InputControlType)this.submitButton);
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x000194E4 File Offset: 0x000178E4
		private InputControl CancelButton
		{
			get
			{
				return this.Device.GetControl((InputControlType)this.cancelButton);
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x000194F7 File Offset: 0x000178F7
		private void SetVectorRepeatTimer()
		{
			this.nextMoveRepeatTime = Mathf.Max(this.nextMoveRepeatTime, Time.realtimeSinceStartup + this.moveRepeatDelayDuration);
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00019516 File Offset: 0x00017916
		private bool VectorIsPressed
		{
			get
			{
				return this.thisVectorState != Vector2.zero;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x00019528 File Offset: 0x00017928
		private bool VectorIsReleased
		{
			get
			{
				return this.thisVectorState == Vector2.zero;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x0001953A File Offset: 0x0001793A
		private bool VectorHasChanged
		{
			get
			{
				return this.thisVectorState != this.lastVectorState;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x0001954D File Offset: 0x0001794D
		protected virtual bool VectorWasPressed
		{
			get
			{
				return (this.VectorIsPressed && Time.realtimeSinceStartup > this.nextMoveRepeatTime) || (this.VectorIsPressed && this.lastVectorState == Vector2.zero);
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x0001958A File Offset: 0x0001798A
		protected virtual bool SubmitWasPressed
		{
			get
			{
				return this.thisSubmitState && this.thisSubmitState != this.lastSubmitState;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x000195AB File Offset: 0x000179AB
		protected virtual bool SubmitWasReleased
		{
			get
			{
				return !this.thisSubmitState && this.thisSubmitState != this.lastSubmitState;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x000195CC File Offset: 0x000179CC
		protected virtual bool CancelWasPressed
		{
			get
			{
				return this.thisCancelState && this.thisCancelState != this.lastCancelState;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x000195F0 File Offset: 0x000179F0
		protected virtual bool MouseHasMoved
		{
			get
			{
				return (this.thisMousePosition - this.lastMousePosition).sqrMagnitude > 0f;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x0001961D File Offset: 0x00017A1D
		protected virtual bool MouseButtonIsPressed
		{
			get
			{
				return Input.GetMouseButtonDown(0);
			}
		}

		// Token: 0x04000422 RID: 1058
		public new InControlInputModule.Button submitButton = InControlInputModule.Button.Action1;

		// Token: 0x04000423 RID: 1059
		public new InControlInputModule.Button cancelButton = InControlInputModule.Button.Action2;

		// Token: 0x04000424 RID: 1060
		[Range(0.1f, 0.9f)]
		public float analogMoveThreshold = 0.5f;

		// Token: 0x04000425 RID: 1061
		public float moveRepeatFirstDuration = 0.8f;

		// Token: 0x04000426 RID: 1062
		public float moveRepeatDelayDuration = 0.1f;

		// Token: 0x04000427 RID: 1063
		private bool allowMobileDevice;

		// Token: 0x04000428 RID: 1064
		[FormerlySerializedAs("allowMobileDevice")]
		public new bool forceModuleActive;

		// Token: 0x04000429 RID: 1065
		public bool allowMouseInput = true;

		// Token: 0x0400042A RID: 1066
		public bool focusOnMouseHover;

		// Token: 0x0400042B RID: 1067
		private InputDevice inputDevice;

		// Token: 0x0400042C RID: 1068
		private Vector3 thisMousePosition;

		// Token: 0x0400042D RID: 1069
		private Vector3 lastMousePosition;

		// Token: 0x0400042E RID: 1070
		protected Vector2 thisVectorState;

		// Token: 0x0400042F RID: 1071
		protected Vector2 lastVectorState;

		// Token: 0x04000430 RID: 1072
		private bool thisSubmitState;

		// Token: 0x04000431 RID: 1073
		private bool lastSubmitState;

		// Token: 0x04000432 RID: 1074
		private bool thisCancelState;

		// Token: 0x04000433 RID: 1075
		private bool lastCancelState;

		// Token: 0x04000434 RID: 1076
		private float nextMoveRepeatTime;

		// Token: 0x04000435 RID: 1077
		private float lastVectorPressedTime;

		// Token: 0x04000436 RID: 1078
		private TwoAxisInputControl direction;

		// Token: 0x0200007E RID: 126
		public enum Button
		{
			// Token: 0x0400043B RID: 1083
			Action1 = 19,
			// Token: 0x0400043C RID: 1084
			Action2,
			// Token: 0x0400043D RID: 1085
			Action3,
			// Token: 0x0400043E RID: 1086
			Action4
		}
	}
}
