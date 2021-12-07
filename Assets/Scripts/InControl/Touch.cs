// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace InControl
{
	public class Touch
	{
		public static readonly int FingerID_None = -1;

		public static readonly int FingerID_Mouse = -2;

		public int fingerId;

		public TouchPhase phase;

		public int tapCount;

		public Vector2 position;

		public Vector2 deltaPosition;

		public Vector2 lastPosition;

		public float deltaTime;

		public ulong updateTick;

		public TouchType type;

		public float altitudeAngle;

		public float azimuthAngle;

		public float maximumPossiblePressure;

		public float pressure;

		public float radius;

		public float radiusVariance;

		public float normalizedPressure
		{
			get
			{
				return Mathf.Clamp(this.pressure / this.maximumPossiblePressure, 0.001f, 1f);
			}
		}

		internal Touch()
		{
			this.fingerId = Touch.FingerID_None;
			this.phase = TouchPhase.Ended;
		}

		internal void Reset()
		{
			this.fingerId = Touch.FingerID_None;
			this.phase = TouchPhase.Ended;
			this.tapCount = 0;
			this.position = Vector2.zero;
			this.deltaPosition = Vector2.zero;
			this.lastPosition = Vector2.zero;
			this.deltaTime = 0f;
			this.updateTick = 0uL;
			this.type = TouchType.Direct;
			this.altitudeAngle = 0f;
			this.azimuthAngle = 0f;
			this.maximumPossiblePressure = 1f;
			this.pressure = 0f;
			this.radius = 0f;
			this.radiusVariance = 0f;
		}

		internal void SetWithTouchData(UnityEngine.Touch touch, ulong updateTick, float deltaTime)
		{
			this.phase = touch.phase;
			this.tapCount = touch.tapCount;
			this.altitudeAngle = touch.altitudeAngle;
			this.azimuthAngle = touch.azimuthAngle;
			this.maximumPossiblePressure = touch.maximumPossiblePressure;
			this.pressure = touch.pressure;
			this.radius = touch.radius;
			this.radiusVariance = touch.radiusVariance;
			Vector2 a = touch.position;
			if (a.x < 0f)
			{
				a.x = (float)Screen.width + a.x;
			}
			if (this.phase == TouchPhase.Began)
			{
				this.deltaPosition = Vector2.zero;
				this.lastPosition = a;
				this.position = a;
			}
			else
			{
				if (this.phase == TouchPhase.Stationary)
				{
					this.phase = TouchPhase.Moved;
				}
				this.deltaPosition = a - this.lastPosition;
				this.lastPosition = this.position;
				this.position = a;
			}
			this.deltaTime = deltaTime;
			this.updateTick = updateTick;
		}

		internal bool SetWithMouseData(ulong updateTick, float deltaTime)
		{
			if (Input.touchCount > 0)
			{
				return false;
			}
			Vector2 a = new Vector2(Mathf.Round(Input.mousePosition.x), Mathf.Round(Input.mousePosition.y));
			if (Input.GetMouseButtonDown(0))
			{
				this.phase = TouchPhase.Began;
				this.pressure = 1f;
				this.maximumPossiblePressure = 1f;
				this.tapCount = 1;
				this.type = TouchType.Mouse;
				this.deltaPosition = Vector2.zero;
				this.lastPosition = a;
				this.position = a;
				this.deltaTime = deltaTime;
				this.updateTick = updateTick;
				return true;
			}
			if (Input.GetMouseButtonUp(0))
			{
				this.phase = TouchPhase.Ended;
				this.pressure = 0f;
				this.maximumPossiblePressure = 1f;
				this.tapCount = 1;
				this.type = TouchType.Mouse;
				this.deltaPosition = a - this.lastPosition;
				this.lastPosition = this.position;
				this.position = a;
				this.deltaTime = deltaTime;
				this.updateTick = updateTick;
				return true;
			}
			if (Input.GetMouseButton(0))
			{
				this.phase = TouchPhase.Moved;
				this.pressure = 1f;
				this.maximumPossiblePressure = 1f;
				this.tapCount = 1;
				this.type = TouchType.Mouse;
				this.deltaPosition = a - this.lastPosition;
				this.lastPosition = this.position;
				this.position = a;
				this.deltaTime = deltaTime;
				this.updateTick = updateTick;
				return true;
			}
			return false;
		}
	}
}
