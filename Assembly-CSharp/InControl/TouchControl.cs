using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200012A RID: 298
	public abstract class TouchControl : MonoBehaviour
	{
		// Token: 0x0600069A RID: 1690
		public abstract void CreateControl();

		// Token: 0x0600069B RID: 1691
		public abstract void DestroyControl();

		// Token: 0x0600069C RID: 1692
		public abstract void ConfigureControl();

		// Token: 0x0600069D RID: 1693
		public abstract void SubmitControlState(ulong updateTick, float deltaTime);

		// Token: 0x0600069E RID: 1694
		public abstract void CommitControlState(ulong updateTick, float deltaTime);

		// Token: 0x0600069F RID: 1695
		public abstract void TouchBegan(Touch touch);

		// Token: 0x060006A0 RID: 1696
		public abstract void TouchMoved(Touch touch);

		// Token: 0x060006A1 RID: 1697
		public abstract void TouchEnded(Touch touch);

		// Token: 0x060006A2 RID: 1698
		public abstract void DrawGizmos();

		// Token: 0x060006A3 RID: 1699 RVA: 0x0002B9BB File Offset: 0x00029DBB
		private void OnEnable()
		{
			TouchManager.OnSetup += this.Setup;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0002B9CE File Offset: 0x00029DCE
		private void OnDisable()
		{
			this.DestroyControl();
			Resources.UnloadUnusedAssets();
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0002B9DC File Offset: 0x00029DDC
		private void Setup()
		{
			if (!base.enabled)
			{
				return;
			}
			this.CreateControl();
			this.ConfigureControl();
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0002B9F8 File Offset: 0x00029DF8
		protected Vector3 OffsetToWorldPosition(TouchControlAnchor anchor, Vector2 offset, TouchUnitType offsetUnitType, bool lockAspectRatio)
		{
			Vector3 b;
			if (offsetUnitType == TouchUnitType.Pixels)
			{
				b = TouchUtility.RoundVector(offset) * TouchManager.PixelToWorld;
			}
			else if (lockAspectRatio)
			{
				b = offset * TouchManager.PercentToWorld;
			}
			else
			{
				b = Vector3.Scale(offset, TouchManager.ViewSize);
			}
			return TouchManager.ViewToWorldPoint(TouchUtility.AnchorToViewPoint(anchor)) + b;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0002BA68 File Offset: 0x00029E68
		protected void SubmitButtonState(TouchControl.ButtonTarget target, bool state, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null || target == TouchControl.ButtonTarget.None)
			{
				return;
			}
			InputControl control = TouchManager.Device.GetControl((InputControlType)target);
			if (control != null && control != InputControl.Null)
			{
				control.UpdateWithState(state, updateTick, deltaTime);
			}
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0002BAB0 File Offset: 0x00029EB0
		protected void SubmitButtonValue(TouchControl.ButtonTarget target, float value, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null || target == TouchControl.ButtonTarget.None)
			{
				return;
			}
			InputControl control = TouchManager.Device.GetControl((InputControlType)target);
			if (control != null && control != InputControl.Null)
			{
				control.UpdateWithValue(value, updateTick, deltaTime);
			}
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0002BAF8 File Offset: 0x00029EF8
		protected void CommitButton(TouchControl.ButtonTarget target)
		{
			if (TouchManager.Device == null || target == TouchControl.ButtonTarget.None)
			{
				return;
			}
			InputControl control = TouchManager.Device.GetControl((InputControlType)target);
			if (control != null && control != InputControl.Null)
			{
				control.Commit();
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0002BB3C File Offset: 0x00029F3C
		protected void SubmitAnalogValue(TouchControl.AnalogTarget target, Vector2 value, float lowerDeadZone, float upperDeadZone, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null || target == TouchControl.AnalogTarget.None)
			{
				return;
			}
			Vector2 value2 = Utility.ApplyCircularDeadZone(value, lowerDeadZone, upperDeadZone);
			if (target == TouchControl.AnalogTarget.LeftStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateLeftStickWithValue(value2, updateTick, deltaTime);
			}
			if (target == TouchControl.AnalogTarget.RightStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateRightStickWithValue(value2, updateTick, deltaTime);
			}
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0002BBA0 File Offset: 0x00029FA0
		protected void CommitAnalog(TouchControl.AnalogTarget target)
		{
			if (TouchManager.Device == null || target == TouchControl.AnalogTarget.None)
			{
				return;
			}
			if (target == TouchControl.AnalogTarget.LeftStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.CommitLeftStick();
			}
			if (target == TouchControl.AnalogTarget.RightStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.CommitRightStick();
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0002BBF0 File Offset: 0x00029FF0
		protected void SubmitRawAnalogValue(TouchControl.AnalogTarget target, Vector2 rawValue, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null || target == TouchControl.AnalogTarget.None)
			{
				return;
			}
			if (target == TouchControl.AnalogTarget.LeftStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateLeftStickWithRawValue(rawValue, updateTick, deltaTime);
			}
			if (target == TouchControl.AnalogTarget.RightStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateRightStickWithRawValue(rawValue, updateTick, deltaTime);
			}
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0002BC48 File Offset: 0x0002A048
		protected static Vector3 SnapTo(Vector2 vector, TouchControl.SnapAngles snapAngles)
		{
			if (snapAngles == TouchControl.SnapAngles.None)
			{
				return vector;
			}
			float snapAngle = 360f / (float)snapAngles;
			return TouchControl.SnapTo(vector, snapAngle);
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0002BC74 File Offset: 0x0002A074
		protected static Vector3 SnapTo(Vector2 vector, float snapAngle)
		{
			float num = Vector2.Angle(vector, Vector2.up);
			if (num < snapAngle / 2f)
			{
				return Vector2.up * vector.magnitude;
			}
			if (num > 180f - snapAngle / 2f)
			{
				return -Vector2.up * vector.magnitude;
			}
			float num2 = Mathf.Round(num / snapAngle);
			float angle = num2 * snapAngle - num;
			Vector3 axis = Vector3.Cross(Vector2.up, vector);
			Quaternion rotation = Quaternion.AngleAxis(angle, axis);
			return rotation * vector;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0002BD1C File Offset: 0x0002A11C
		private void OnDrawGizmosSelected()
		{
			if (!base.enabled)
			{
				return;
			}
			if (TouchManager.ControlsShowGizmos != TouchManager.GizmoShowOption.WhenSelected)
			{
				return;
			}
			if (Utility.GameObjectIsCulledOnCurrentCamera(base.gameObject))
			{
				return;
			}
			if (!Application.isPlaying)
			{
				this.ConfigureControl();
			}
			this.DrawGizmos();
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0002BD68 File Offset: 0x0002A168
		private void OnDrawGizmos()
		{
			if (!base.enabled)
			{
				return;
			}
			if (TouchManager.ControlsShowGizmos == TouchManager.GizmoShowOption.UnlessPlaying)
			{
				if (Application.isPlaying)
				{
					return;
				}
			}
			else if (TouchManager.ControlsShowGizmos != TouchManager.GizmoShowOption.Always)
			{
				return;
			}
			if (Utility.GameObjectIsCulledOnCurrentCamera(base.gameObject))
			{
				return;
			}
			if (!Application.isPlaying)
			{
				this.ConfigureControl();
			}
			this.DrawGizmos();
		}

		// Token: 0x0200012B RID: 299
		public enum ButtonTarget
		{
			// Token: 0x04000504 RID: 1284
			None,
			// Token: 0x04000505 RID: 1285
			DPadDown = 12,
			// Token: 0x04000506 RID: 1286
			DPadLeft,
			// Token: 0x04000507 RID: 1287
			DPadRight,
			// Token: 0x04000508 RID: 1288
			DPadUp = 11,
			// Token: 0x04000509 RID: 1289
			LeftTrigger = 15,
			// Token: 0x0400050A RID: 1290
			RightTrigger,
			// Token: 0x0400050B RID: 1291
			LeftBumper,
			// Token: 0x0400050C RID: 1292
			RightBumper,
			// Token: 0x0400050D RID: 1293
			Action1,
			// Token: 0x0400050E RID: 1294
			Action2,
			// Token: 0x0400050F RID: 1295
			Action3,
			// Token: 0x04000510 RID: 1296
			Action4,
			// Token: 0x04000511 RID: 1297
			Action5,
			// Token: 0x04000512 RID: 1298
			Action6,
			// Token: 0x04000513 RID: 1299
			Action7,
			// Token: 0x04000514 RID: 1300
			Action8,
			// Token: 0x04000515 RID: 1301
			Action9,
			// Token: 0x04000516 RID: 1302
			Action10,
			// Token: 0x04000517 RID: 1303
			Action11,
			// Token: 0x04000518 RID: 1304
			Action12,
			// Token: 0x04000519 RID: 1305
			Menu = 106,
			// Token: 0x0400051A RID: 1306
			Button0 = 500,
			// Token: 0x0400051B RID: 1307
			Button1,
			// Token: 0x0400051C RID: 1308
			Button2,
			// Token: 0x0400051D RID: 1309
			Button3,
			// Token: 0x0400051E RID: 1310
			Button4,
			// Token: 0x0400051F RID: 1311
			Button5,
			// Token: 0x04000520 RID: 1312
			Button6,
			// Token: 0x04000521 RID: 1313
			Button7,
			// Token: 0x04000522 RID: 1314
			Button8,
			// Token: 0x04000523 RID: 1315
			Button9,
			// Token: 0x04000524 RID: 1316
			Button10,
			// Token: 0x04000525 RID: 1317
			Button11,
			// Token: 0x04000526 RID: 1318
			Button12,
			// Token: 0x04000527 RID: 1319
			Button13,
			// Token: 0x04000528 RID: 1320
			Button14,
			// Token: 0x04000529 RID: 1321
			Button15,
			// Token: 0x0400052A RID: 1322
			Button16,
			// Token: 0x0400052B RID: 1323
			Button17,
			// Token: 0x0400052C RID: 1324
			Button18,
			// Token: 0x0400052D RID: 1325
			Button19
		}

		// Token: 0x0200012C RID: 300
		public enum AnalogTarget
		{
			// Token: 0x0400052F RID: 1327
			None,
			// Token: 0x04000530 RID: 1328
			LeftStick,
			// Token: 0x04000531 RID: 1329
			RightStick,
			// Token: 0x04000532 RID: 1330
			Both
		}

		// Token: 0x0200012D RID: 301
		public enum SnapAngles
		{
			// Token: 0x04000534 RID: 1332
			None,
			// Token: 0x04000535 RID: 1333
			Four = 4,
			// Token: 0x04000536 RID: 1334
			Eight = 8,
			// Token: 0x04000537 RID: 1335
			Sixteen = 16
		}
	}
}
