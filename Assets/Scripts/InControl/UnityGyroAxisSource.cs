// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace InControl
{
	public class UnityGyroAxisSource : InputControlSource
	{
		public enum GyroAxis
		{
			X,
			Y
		}

		private static Quaternion zeroAttitude;

		public int Axis;

		public UnityGyroAxisSource()
		{
			UnityGyroAxisSource.Calibrate();
		}

		public UnityGyroAxisSource(UnityGyroAxisSource.GyroAxis axis)
		{
			this.Axis = (int)axis;
			UnityGyroAxisSource.Calibrate();
		}

		public float GetValue(InputDevice inputDevice)
		{
			return UnityGyroAxisSource.GetAxis()[this.Axis];
		}

		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		private static Quaternion GetAttitude()
		{
			return Quaternion.Inverse(UnityGyroAxisSource.zeroAttitude) * Input.gyro.attitude;
		}

		private static Vector3 GetAxis()
		{
			Vector3 vector = UnityGyroAxisSource.GetAttitude() * Vector3.forward;
			float x = UnityGyroAxisSource.ApplyDeadZone(Mathf.Clamp(vector.x, -1f, 1f));
			float y = UnityGyroAxisSource.ApplyDeadZone(Mathf.Clamp(vector.y, -1f, 1f));
			return new Vector3(x, y);
		}

		private static float ApplyDeadZone(float value)
		{
			return Mathf.InverseLerp(0.05f, 1f, Utility.Abs(value)) * Mathf.Sign(value);
		}

		public static void Calibrate()
		{
			UnityGyroAxisSource.zeroAttitude = Input.gyro.attitude;
		}
	}
}
