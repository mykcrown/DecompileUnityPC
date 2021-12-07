using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000129 RID: 297
	public class Touch
	{
		// Token: 0x06000693 RID: 1683 RVA: 0x0002D206 File Offset: 0x0002B606
		internal Touch()
		{
			this.fingerId = Touch.FingerID_None;
			this.phase = TouchPhase.Ended;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0002D220 File Offset: 0x0002B620
		internal void Reset()
		{
			this.fingerId = Touch.FingerID_None;
			this.phase = TouchPhase.Ended;
			this.tapCount = 0;
			this.position = Vector2.zero;
			this.deltaPosition = Vector2.zero;
			this.lastPosition = Vector2.zero;
			this.deltaTime = 0f;
			this.updateTick = 0UL;
			this.type = TouchType.Direct;
			this.altitudeAngle = 0f;
			this.azimuthAngle = 0f;
			this.maximumPossiblePressure = 1f;
			this.pressure = 0f;
			this.radius = 0f;
			this.radiusVariance = 0f;
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x0002D2C3 File Offset: 0x0002B6C3
		public float normalizedPressure
		{
			get
			{
				return Mathf.Clamp(this.pressure / this.maximumPossiblePressure, 0.001f, 1f);
			}
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0002D2E4 File Offset: 0x0002B6E4
		internal void SetWithTouchData(Touch touch, ulong updateTick, float deltaTime)
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

		// Token: 0x06000697 RID: 1687 RVA: 0x0002D3F8 File Offset: 0x0002B7F8
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

		// Token: 0x040004F2 RID: 1266
		public static readonly int FingerID_None = -1;

		// Token: 0x040004F3 RID: 1267
		public static readonly int FingerID_Mouse = -2;

		// Token: 0x040004F4 RID: 1268
		public int fingerId;

		// Token: 0x040004F5 RID: 1269
		public TouchPhase phase;

		// Token: 0x040004F6 RID: 1270
		public int tapCount;

		// Token: 0x040004F7 RID: 1271
		public Vector2 position;

		// Token: 0x040004F8 RID: 1272
		public Vector2 deltaPosition;

		// Token: 0x040004F9 RID: 1273
		public Vector2 lastPosition;

		// Token: 0x040004FA RID: 1274
		public float deltaTime;

		// Token: 0x040004FB RID: 1275
		public ulong updateTick;

		// Token: 0x040004FC RID: 1276
		public TouchType type;

		// Token: 0x040004FD RID: 1277
		public float altitudeAngle;

		// Token: 0x040004FE RID: 1278
		public float azimuthAngle;

		// Token: 0x040004FF RID: 1279
		public float maximumPossiblePressure;

		// Token: 0x04000500 RID: 1280
		public float pressure;

		// Token: 0x04000501 RID: 1281
		public float radius;

		// Token: 0x04000502 RID: 1282
		public float radiusVariance;
	}
}
