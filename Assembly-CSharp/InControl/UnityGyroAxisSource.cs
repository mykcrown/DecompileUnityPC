using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200013A RID: 314
	public class UnityGyroAxisSource : InputControlSource
	{
		// Token: 0x0600071A RID: 1818 RVA: 0x0002EFA8 File Offset: 0x0002D3A8
		public UnityGyroAxisSource()
		{
			UnityGyroAxisSource.Calibrate();
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0002EFB5 File Offset: 0x0002D3B5
		public UnityGyroAxisSource(UnityGyroAxisSource.GyroAxis axis)
		{
			this.Axis = (int)axis;
			UnityGyroAxisSource.Calibrate();
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0002EFCC File Offset: 0x0002D3CC
		public float GetValue(InputDevice inputDevice)
		{
			return UnityGyroAxisSource.GetAxis()[this.Axis];
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0002EFEC File Offset: 0x0002D3EC
		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0002EFFA File Offset: 0x0002D3FA
		private static Quaternion GetAttitude()
		{
			return Quaternion.Inverse(UnityGyroAxisSource.zeroAttitude) * Input.gyro.attitude;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0002F018 File Offset: 0x0002D418
		private static Vector3 GetAxis()
		{
			Vector3 vector = UnityGyroAxisSource.GetAttitude() * Vector3.forward;
			float x = UnityGyroAxisSource.ApplyDeadZone(Mathf.Clamp(vector.x, -1f, 1f));
			float y = UnityGyroAxisSource.ApplyDeadZone(Mathf.Clamp(vector.y, -1f, 1f));
			return new Vector3(x, y);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0002F074 File Offset: 0x0002D474
		private static float ApplyDeadZone(float value)
		{
			return Mathf.InverseLerp(0.05f, 1f, Utility.Abs(value)) * Mathf.Sign(value);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0002F092 File Offset: 0x0002D492
		public static void Calibrate()
		{
			UnityGyroAxisSource.zeroAttitude = Input.gyro.attitude;
		}

		// Token: 0x0400057B RID: 1403
		private static Quaternion zeroAttitude;

		// Token: 0x0400057C RID: 1404
		public int Axis;

		// Token: 0x0200013B RID: 315
		public enum GyroAxis
		{
			// Token: 0x0400057E RID: 1406
			X,
			// Token: 0x0400057F RID: 1407
			Y
		}
	}
}
