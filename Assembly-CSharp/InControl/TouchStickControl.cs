using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000126 RID: 294
	public class TouchStickControl : TouchControl
	{
		// Token: 0x06000659 RID: 1625 RVA: 0x0002C242 File Offset: 0x0002A642
		public override void CreateControl()
		{
			this.ring.Create("Ring", base.transform, 1000);
			this.knob.Create("Knob", base.transform, 1001);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0002C27A File Offset: 0x0002A67A
		public override void DestroyControl()
		{
			this.ring.Delete();
			this.knob.Delete();
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0002C2B0 File Offset: 0x0002A6B0
		public override void ConfigureControl()
		{
			this.resetPosition = base.OffsetToWorldPosition(this.anchor, this.offset, this.offsetUnitType, true);
			base.transform.position = this.resetPosition;
			this.ring.Update(true);
			this.knob.Update(true);
			this.worldActiveArea = TouchManager.ConvertToWorld(this.activeArea, this.areaUnitType);
			this.worldKnobRange = TouchManager.ConvertToWorld(this.knobRange, this.knob.SizeUnitType);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0002C338 File Offset: 0x0002A738
		public override void DrawGizmos()
		{
			this.ring.DrawGizmos(this.RingPosition, Color.yellow);
			this.knob.DrawGizmos(this.KnobPosition, Color.yellow);
			Utility.DrawCircleGizmo(this.RingPosition, this.worldKnobRange, Color.red);
			Utility.DrawRectGizmo(this.worldActiveArea, Color.green);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0002C39C File Offset: 0x0002A79C
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
			else
			{
				this.ring.Update();
				this.knob.Update();
			}
			if (this.IsNotActive)
			{
				if (this.resetWhenDone && this.KnobPosition != this.resetPosition)
				{
					Vector3 b = this.KnobPosition - this.RingPosition;
					this.RingPosition = Vector3.MoveTowards(this.RingPosition, this.resetPosition, this.ringResetSpeed * Time.deltaTime);
					this.KnobPosition = this.RingPosition + b;
				}
				if (this.KnobPosition != this.RingPosition)
				{
					this.KnobPosition = Vector3.MoveTowards(this.KnobPosition, this.RingPosition, this.knobResetSpeed * Time.deltaTime);
				}
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0002C488 File Offset: 0x0002A888
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			base.SubmitAnalogValue(this.target, this.value, this.lowerDeadZone, this.upperDeadZone, updateTick, deltaTime);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0002C4AF File Offset: 0x0002A8AF
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitAnalog(this.target);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0002C4C0 File Offset: 0x0002A8C0
		public override void TouchBegan(Touch touch)
		{
			if (this.IsActive)
			{
				return;
			}
			this.beganPosition = TouchManager.ScreenToWorldPoint(touch.position);
			bool flag = this.worldActiveArea.Contains(this.beganPosition);
			bool flag2 = this.ring.Contains(this.beganPosition);
			if (this.snapToInitialTouch && (flag || flag2))
			{
				this.RingPosition = this.beganPosition;
				this.KnobPosition = this.beganPosition;
				this.currentTouch = touch;
			}
			else if (flag2)
			{
				this.KnobPosition = this.beganPosition;
				this.beganPosition = this.RingPosition;
				this.currentTouch = touch;
			}
			if (this.IsActive)
			{
				this.TouchMoved(touch);
				this.ring.State = true;
				this.knob.State = true;
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0002C5A0 File Offset: 0x0002A9A0
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.movedPosition = TouchManager.ScreenToWorldPoint(touch.position);
			if (this.lockToAxis == LockAxis.Horizontal && this.allowDraggingAxis == DragAxis.Horizontal)
			{
				this.movedPosition.y = this.beganPosition.y;
			}
			else if (this.lockToAxis == LockAxis.Vertical && this.allowDraggingAxis == DragAxis.Vertical)
			{
				this.movedPosition.x = this.beganPosition.x;
			}
			Vector3 vector = this.movedPosition - this.beganPosition;
			Vector3 normalized = vector.normalized;
			float magnitude = vector.magnitude;
			if (this.allowDragging)
			{
				float num = magnitude - this.worldKnobRange;
				if (num < 0f)
				{
					num = 0f;
				}
				Vector3 b = num * normalized;
				if (this.allowDraggingAxis == DragAxis.Horizontal)
				{
					b.y = 0f;
				}
				else if (this.allowDraggingAxis == DragAxis.Vertical)
				{
					b.x = 0f;
				}
				this.beganPosition += b;
				this.RingPosition = this.beganPosition;
			}
			this.movedPosition = this.beganPosition + Mathf.Clamp(magnitude, 0f, this.worldKnobRange) * normalized;
			if (this.lockToAxis == LockAxis.Horizontal)
			{
				this.movedPosition.y = this.beganPosition.y;
			}
			else if (this.lockToAxis == LockAxis.Vertical)
			{
				this.movedPosition.x = this.beganPosition.x;
			}
			if (this.snapAngles != TouchControl.SnapAngles.None)
			{
				this.movedPosition = TouchControl.SnapTo(this.movedPosition - this.beganPosition, this.snapAngles) + this.beganPosition;
			}
			this.RingPosition = this.beganPosition;
			this.KnobPosition = this.movedPosition;
			this.value = (this.movedPosition - this.beganPosition) / this.worldKnobRange;
			this.value.x = this.inputCurve.Evaluate(Utility.Abs(this.value.x)) * Mathf.Sign(this.value.x);
			this.value.y = this.inputCurve.Evaluate(Utility.Abs(this.value.y)) * Mathf.Sign(this.value.y);
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0002C828 File Offset: 0x0002AC28
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.value = Vector3.zero;
			float magnitude = (this.resetPosition - this.RingPosition).magnitude;
			this.ringResetSpeed = ((!Utility.IsZero(this.resetDuration)) ? (magnitude / this.resetDuration) : magnitude);
			float magnitude2 = (this.RingPosition - this.KnobPosition).magnitude;
			this.knobResetSpeed = ((!Utility.IsZero(this.resetDuration)) ? (magnitude2 / this.resetDuration) : this.knobRange);
			this.currentTouch = null;
			this.ring.State = false;
			this.knob.State = false;
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x0002C8ED File Offset: 0x0002ACED
		public bool IsActive
		{
			get
			{
				return this.currentTouch != null;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x0002C8FB File Offset: 0x0002ACFB
		public bool IsNotActive
		{
			get
			{
				return this.currentTouch == null;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x0002C906 File Offset: 0x0002AD06
		// (set) Token: 0x06000666 RID: 1638 RVA: 0x0002C933 File Offset: 0x0002AD33
		public Vector3 RingPosition
		{
			get
			{
				return (!this.ring.Ready) ? base.transform.position : this.ring.Position;
			}
			set
			{
				if (this.ring.Ready)
				{
					this.ring.Position = value;
				}
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x0002C951 File Offset: 0x0002AD51
		// (set) Token: 0x06000668 RID: 1640 RVA: 0x0002C97E File Offset: 0x0002AD7E
		public Vector3 KnobPosition
		{
			get
			{
				return (!this.knob.Ready) ? base.transform.position : this.knob.Position;
			}
			set
			{
				if (this.knob.Ready)
				{
					this.knob.Position = value;
				}
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x0002C99C File Offset: 0x0002AD9C
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x0002C9A4 File Offset: 0x0002ADA4
		public TouchControlAnchor Anchor
		{
			get
			{
				return this.anchor;
			}
			set
			{
				if (this.anchor != value)
				{
					this.anchor = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x0002C9C0 File Offset: 0x0002ADC0
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x0002C9C8 File Offset: 0x0002ADC8
		public Vector2 Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				if (this.offset != value)
				{
					this.offset = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x0002C9E9 File Offset: 0x0002ADE9
		// (set) Token: 0x0600066E RID: 1646 RVA: 0x0002C9F1 File Offset: 0x0002ADF1
		public TouchUnitType OffsetUnitType
		{
			get
			{
				return this.offsetUnitType;
			}
			set
			{
				if (this.offsetUnitType != value)
				{
					this.offsetUnitType = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x0002CA0D File Offset: 0x0002AE0D
		// (set) Token: 0x06000670 RID: 1648 RVA: 0x0002CA15 File Offset: 0x0002AE15
		public Rect ActiveArea
		{
			get
			{
				return this.activeArea;
			}
			set
			{
				if (this.activeArea != value)
				{
					this.activeArea = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x0002CA36 File Offset: 0x0002AE36
		// (set) Token: 0x06000672 RID: 1650 RVA: 0x0002CA3E File Offset: 0x0002AE3E
		public TouchUnitType AreaUnitType
		{
			get
			{
				return this.areaUnitType;
			}
			set
			{
				if (this.areaUnitType != value)
				{
					this.areaUnitType = value;
					this.dirty = true;
				}
			}
		}

		// Token: 0x040004B1 RID: 1201
		[Header("Position")]
		[SerializeField]
		private TouchControlAnchor anchor = TouchControlAnchor.BottomLeft;

		// Token: 0x040004B2 RID: 1202
		[SerializeField]
		private TouchUnitType offsetUnitType;

		// Token: 0x040004B3 RID: 1203
		[SerializeField]
		private Vector2 offset = new Vector2(20f, 20f);

		// Token: 0x040004B4 RID: 1204
		[SerializeField]
		private TouchUnitType areaUnitType;

		// Token: 0x040004B5 RID: 1205
		[SerializeField]
		private Rect activeArea = new Rect(0f, 0f, 50f, 100f);

		// Token: 0x040004B6 RID: 1206
		[Header("Options")]
		public TouchControl.AnalogTarget target = TouchControl.AnalogTarget.LeftStick;

		// Token: 0x040004B7 RID: 1207
		public TouchControl.SnapAngles snapAngles;

		// Token: 0x040004B8 RID: 1208
		public LockAxis lockToAxis;

		// Token: 0x040004B9 RID: 1209
		[Range(0f, 1f)]
		public float lowerDeadZone = 0.1f;

		// Token: 0x040004BA RID: 1210
		[Range(0f, 1f)]
		public float upperDeadZone = 0.9f;

		// Token: 0x040004BB RID: 1211
		public AnimationCurve inputCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x040004BC RID: 1212
		public bool allowDragging;

		// Token: 0x040004BD RID: 1213
		public DragAxis allowDraggingAxis;

		// Token: 0x040004BE RID: 1214
		public bool snapToInitialTouch = true;

		// Token: 0x040004BF RID: 1215
		public bool resetWhenDone = true;

		// Token: 0x040004C0 RID: 1216
		public float resetDuration = 0.1f;

		// Token: 0x040004C1 RID: 1217
		[Header("Sprites")]
		public TouchSprite ring = new TouchSprite(20f);

		// Token: 0x040004C2 RID: 1218
		public TouchSprite knob = new TouchSprite(10f);

		// Token: 0x040004C3 RID: 1219
		public float knobRange = 7.5f;

		// Token: 0x040004C4 RID: 1220
		private Vector3 resetPosition;

		// Token: 0x040004C5 RID: 1221
		private Vector3 beganPosition;

		// Token: 0x040004C6 RID: 1222
		private Vector3 movedPosition;

		// Token: 0x040004C7 RID: 1223
		private float ringResetSpeed;

		// Token: 0x040004C8 RID: 1224
		private float knobResetSpeed;

		// Token: 0x040004C9 RID: 1225
		private Rect worldActiveArea;

		// Token: 0x040004CA RID: 1226
		private float worldKnobRange;

		// Token: 0x040004CB RID: 1227
		private Vector3 value;

		// Token: 0x040004CC RID: 1228
		private Touch currentTouch;

		// Token: 0x040004CD RID: 1229
		private bool dirty;
	}
}
