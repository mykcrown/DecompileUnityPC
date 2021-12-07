using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000127 RID: 295
	public class TouchSwipeControl : TouchControl
	{
		// Token: 0x06000674 RID: 1652 RVA: 0x0002CA8C File Offset: 0x0002AE8C
		public override void CreateControl()
		{
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0002CA8E File Offset: 0x0002AE8E
		public override void DestroyControl()
		{
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0002CAAE File Offset: 0x0002AEAE
		public override void ConfigureControl()
		{
			this.worldActiveArea = TouchManager.ConvertToWorld(this.activeArea, this.areaUnitType);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0002CAC7 File Offset: 0x0002AEC7
		public override void DrawGizmos()
		{
			Utility.DrawRectGizmo(this.worldActiveArea, Color.yellow);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0002CAD9 File Offset: 0x0002AED9
		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0002CAF4 File Offset: 0x0002AEF4
		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			Vector3 v = TouchControl.SnapTo(this.currentVector, this.snapAngles);
			base.SubmitAnalogValue(this.target, v, 0f, 1f, updateTick, deltaTime);
			base.SubmitButtonState(this.upTarget, this.fireButtonTarget && this.nextButtonTarget == this.upTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.downTarget, this.fireButtonTarget && this.nextButtonTarget == this.downTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.leftTarget, this.fireButtonTarget && this.nextButtonTarget == this.leftTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.rightTarget, this.fireButtonTarget && this.nextButtonTarget == this.rightTarget, updateTick, deltaTime);
			base.SubmitButtonState(this.tapTarget, this.fireButtonTarget && this.nextButtonTarget == this.tapTarget, updateTick, deltaTime);
			if (this.fireButtonTarget && this.nextButtonTarget != TouchControl.ButtonTarget.None)
			{
				this.fireButtonTarget = !this.oneSwipePerTouch;
				this.lastButtonTarget = this.nextButtonTarget;
				this.nextButtonTarget = TouchControl.ButtonTarget.None;
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0002CC40 File Offset: 0x0002B040
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitAnalog(this.target);
			base.CommitButton(this.upTarget);
			base.CommitButton(this.downTarget);
			base.CommitButton(this.leftTarget);
			base.CommitButton(this.rightTarget);
			base.CommitButton(this.tapTarget);
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0002CC98 File Offset: 0x0002B098
		public override void TouchBegan(Touch touch)
		{
			if (this.currentTouch != null)
			{
				return;
			}
			this.beganPosition = TouchManager.ScreenToWorldPoint(touch.position);
			if (this.worldActiveArea.Contains(this.beganPosition))
			{
				this.lastPosition = this.beganPosition;
				this.currentTouch = touch;
				this.currentVector = Vector2.zero;
				this.currentVectorIsSet = false;
				this.fireButtonTarget = true;
				this.nextButtonTarget = TouchControl.ButtonTarget.None;
				this.lastButtonTarget = TouchControl.ButtonTarget.None;
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0002CD18 File Offset: 0x0002B118
		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			Vector3 a = TouchManager.ScreenToWorldPoint(touch.position);
			Vector3 vector = a - this.lastPosition;
			if (vector.magnitude >= this.sensitivity)
			{
				this.lastPosition = a;
				if (!this.oneSwipePerTouch || !this.currentVectorIsSet)
				{
					this.currentVector = vector.normalized;
					this.currentVectorIsSet = true;
				}
				if (this.fireButtonTarget)
				{
					TouchControl.ButtonTarget buttonTargetForVector = this.GetButtonTargetForVector(this.currentVector);
					if (buttonTargetForVector != this.lastButtonTarget)
					{
						this.nextButtonTarget = buttonTargetForVector;
					}
				}
			}
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0002CDC0 File Offset: 0x0002B1C0
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.currentTouch = null;
			this.currentVector = Vector2.zero;
			this.currentVectorIsSet = false;
			Vector3 b = TouchManager.ScreenToWorldPoint(touch.position);
			if ((this.beganPosition - b).magnitude < this.sensitivity)
			{
				this.fireButtonTarget = true;
				this.nextButtonTarget = this.tapTarget;
				this.lastButtonTarget = TouchControl.ButtonTarget.None;
				return;
			}
			this.fireButtonTarget = false;
			this.nextButtonTarget = TouchControl.ButtonTarget.None;
			this.lastButtonTarget = TouchControl.ButtonTarget.None;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0002CE54 File Offset: 0x0002B254
		private TouchControl.ButtonTarget GetButtonTargetForVector(Vector2 vector)
		{
			Vector2 lhs = TouchControl.SnapTo(vector, TouchControl.SnapAngles.Four);
			if (lhs == Vector2.up)
			{
				return this.upTarget;
			}
			if (lhs == Vector2.right)
			{
				return this.rightTarget;
			}
			if (lhs == -Vector2.up)
			{
				return this.downTarget;
			}
			if (lhs == -Vector2.right)
			{
				return this.leftTarget;
			}
			return TouchControl.ButtonTarget.None;
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x0002CED5 File Offset: 0x0002B2D5
		// (set) Token: 0x06000680 RID: 1664 RVA: 0x0002CEDD File Offset: 0x0002B2DD
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

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x0002CEFE File Offset: 0x0002B2FE
		// (set) Token: 0x06000682 RID: 1666 RVA: 0x0002CF06 File Offset: 0x0002B306
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

		// Token: 0x040004CE RID: 1230
		[Header("Position")]
		[SerializeField]
		private TouchUnitType areaUnitType;

		// Token: 0x040004CF RID: 1231
		[SerializeField]
		private Rect activeArea = new Rect(25f, 25f, 50f, 50f);

		// Token: 0x040004D0 RID: 1232
		[Header("Options")]
		[Range(0f, 1f)]
		public float sensitivity = 0.1f;

		// Token: 0x040004D1 RID: 1233
		public bool oneSwipePerTouch;

		// Token: 0x040004D2 RID: 1234
		[Header("Analog Target")]
		public TouchControl.AnalogTarget target;

		// Token: 0x040004D3 RID: 1235
		public TouchControl.SnapAngles snapAngles;

		// Token: 0x040004D4 RID: 1236
		[Header("Button Targets")]
		public TouchControl.ButtonTarget upTarget;

		// Token: 0x040004D5 RID: 1237
		public TouchControl.ButtonTarget downTarget;

		// Token: 0x040004D6 RID: 1238
		public TouchControl.ButtonTarget leftTarget;

		// Token: 0x040004D7 RID: 1239
		public TouchControl.ButtonTarget rightTarget;

		// Token: 0x040004D8 RID: 1240
		public TouchControl.ButtonTarget tapTarget;

		// Token: 0x040004D9 RID: 1241
		private Rect worldActiveArea;

		// Token: 0x040004DA RID: 1242
		private Vector3 currentVector;

		// Token: 0x040004DB RID: 1243
		private bool currentVectorIsSet;

		// Token: 0x040004DC RID: 1244
		private Vector3 beganPosition;

		// Token: 0x040004DD RID: 1245
		private Vector3 lastPosition;

		// Token: 0x040004DE RID: 1246
		private Touch currentTouch;

		// Token: 0x040004DF RID: 1247
		private bool fireButtonTarget;

		// Token: 0x040004E0 RID: 1248
		private TouchControl.ButtonTarget nextButtonTarget;

		// Token: 0x040004E1 RID: 1249
		private TouchControl.ButtonTarget lastButtonTarget;

		// Token: 0x040004E2 RID: 1250
		private bool dirty;
	}
}
