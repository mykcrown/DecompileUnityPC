using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000128 RID: 296
	public class TouchTrackControl : TouchControl
	{
		// Token: 0x06000684 RID: 1668 RVA: 0x0002CF7E File Offset: 0x0002B37E
		public override void CreateControl()
		{
			this.ConfigureControl();
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0002CF86 File Offset: 0x0002B386
		public override void DestroyControl()
		{
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0002CFA6 File Offset: 0x0002B3A6
		public override void ConfigureControl()
		{
			this.worldActiveArea = TouchManager.ConvertToWorld(this.activeArea, this.areaUnitType);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0002CFBF File Offset: 0x0002B3BF
		public override void DrawGizmos()
		{
			Utility.DrawRectGizmo(this.worldActiveArea, Color.yellow);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0002CFD1 File Offset: 0x0002B3D1
		private void OnValidate()
		{
			if (this.maxTapDuration < 0f)
			{
				this.maxTapDuration = 0f;
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0002CFEE File Offset: 0x0002B3EE
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0002D008 File Offset: 0x0002B408
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			Vector3 a = this.thisPosition - this.lastPosition;
			base.SubmitRawAnalogValue(this.target, a * this.scale, updateTick, deltaTime);
			this.lastPosition = this.thisPosition;
			base.SubmitButtonState(this.tapTarget, this.fireButtonTarget, updateTick, deltaTime);
			this.fireButtonTarget = false;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0002D06D File Offset: 0x0002B46D
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitAnalog(this.target);
			base.CommitButton(this.tapTarget);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0002D088 File Offset: 0x0002B488
		public override void TouchBegan(Touch touch)
		{
			if (this.currentTouch != null)
			{
				return;
			}
			this.beganPosition = TouchManager.ScreenToWorldPoint(touch.position);
			if (this.worldActiveArea.Contains(this.beganPosition))
			{
				this.thisPosition = TouchManager.ScreenToViewPoint(touch.position * 100f);
				this.lastPosition = this.thisPosition;
				this.currentTouch = touch;
				this.beganTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0002D101 File Offset: 0x0002B501
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.thisPosition = TouchManager.ScreenToViewPoint(touch.position * 100f);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0002D12C File Offset: 0x0002B52C
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			Vector3 a = TouchManager.ScreenToWorldPoint(touch.position);
			Vector3 vector = a - this.beganPosition;
			float num = Time.realtimeSinceStartup - this.beganTime;
			if (vector.magnitude <= this.maxTapMovement && num <= this.maxTapDuration && this.tapTarget != TouchControl.ButtonTarget.None)
			{
				this.fireButtonTarget = true;
			}
			this.thisPosition = Vector3.zero;
			this.lastPosition = Vector3.zero;
			this.currentTouch = null;
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x0002D1B9 File Offset: 0x0002B5B9
		// (set) Token: 0x06000690 RID: 1680 RVA: 0x0002D1C1 File Offset: 0x0002B5C1
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

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x0002D1E2 File Offset: 0x0002B5E2
		// (set) Token: 0x06000692 RID: 1682 RVA: 0x0002D1EA File Offset: 0x0002B5EA
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

		// Token: 0x040004E3 RID: 1251
		[Header("Dimensions")]
		[SerializeField]
		private TouchUnitType areaUnitType;

		// Token: 0x040004E4 RID: 1252
		[SerializeField]
		private Rect activeArea = new Rect(25f, 25f, 50f, 50f);

		// Token: 0x040004E5 RID: 1253
		[Header("Analog Target")]
		public TouchControl.AnalogTarget target = TouchControl.AnalogTarget.LeftStick;

		// Token: 0x040004E6 RID: 1254
		public float scale = 1f;

		// Token: 0x040004E7 RID: 1255
		[Header("Button Target")]
		public TouchControl.ButtonTarget tapTarget;

		// Token: 0x040004E8 RID: 1256
		public float maxTapDuration = 0.5f;

		// Token: 0x040004E9 RID: 1257
		public float maxTapMovement = 1f;

		// Token: 0x040004EA RID: 1258
		private Rect worldActiveArea;

		// Token: 0x040004EB RID: 1259
		private Vector3 lastPosition;

		// Token: 0x040004EC RID: 1260
		private Vector3 thisPosition;

		// Token: 0x040004ED RID: 1261
		private Touch currentTouch;

		// Token: 0x040004EE RID: 1262
		private bool dirty;

		// Token: 0x040004EF RID: 1263
		private bool fireButtonTarget;

		// Token: 0x040004F0 RID: 1264
		private float beganTime;

		// Token: 0x040004F1 RID: 1265
		private Vector3 beganPosition;
	}
}
