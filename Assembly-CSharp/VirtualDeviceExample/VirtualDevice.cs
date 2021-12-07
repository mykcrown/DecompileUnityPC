using System;
using InControl;
using UnityEngine;

namespace VirtualDeviceExample
{
	// Token: 0x0200004C RID: 76
	public class VirtualDevice : InputDevice
	{
		// Token: 0x0600027F RID: 639 RVA: 0x000132C8 File Offset: 0x000116C8
		public VirtualDevice() : base("Virtual Controller")
		{
			base.AddControl(InputControlType.LeftStickLeft, "Left Stick Left");
			base.AddControl(InputControlType.LeftStickRight, "Left Stick Right");
			base.AddControl(InputControlType.LeftStickUp, "Left Stick Up");
			base.AddControl(InputControlType.LeftStickDown, "Left Stick Down");
			base.AddControl(InputControlType.RightStickLeft, "Right Stick Left");
			base.AddControl(InputControlType.RightStickRight, "Right Stick Right");
			base.AddControl(InputControlType.RightStickUp, "Right Stick Up");
			base.AddControl(InputControlType.RightStickDown, "Right Stick Down");
			base.AddControl(InputControlType.Action1, "A");
			base.AddControl(InputControlType.Action2, "B");
			base.AddControl(InputControlType.Action3, "X");
			base.AddControl(InputControlType.Action4, "Y");
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00013384 File Offset: 0x00011784
		public override void Update(ulong updateTick, float deltaTime)
		{
			Vector2 vectorFromKeyboard = this.GetVectorFromKeyboard(deltaTime, true);
			base.UpdateLeftStickWithValue(vectorFromKeyboard, updateTick, deltaTime);
			Vector2 vectorFromMouse = this.GetVectorFromMouse(deltaTime, true);
			base.UpdateRightStickWithRawValue(vectorFromMouse, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action1, Input.GetKey(KeyCode.Space), updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action2, Input.GetKey(KeyCode.S), updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action3, Input.GetKey(KeyCode.D), updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action4, Input.GetKey(KeyCode.F), updateTick, deltaTime);
			base.Commit(updateTick, deltaTime);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00013404 File Offset: 0x00011804
		private Vector2 GetVectorFromKeyboard(float deltaTime, bool smoothed)
		{
			if (smoothed)
			{
				this.kx = this.ApplySmoothing(this.kx, this.GetXFromKeyboard(), deltaTime, 0.1f);
				this.ky = this.ApplySmoothing(this.ky, this.GetYFromKeyboard(), deltaTime, 0.1f);
			}
			else
			{
				this.kx = this.GetXFromKeyboard();
				this.ky = this.GetYFromKeyboard();
			}
			return new Vector2(this.kx, this.ky);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00013484 File Offset: 0x00011884
		private float GetXFromKeyboard()
		{
			float num = (!Input.GetKey(KeyCode.LeftArrow)) ? 0f : -1f;
			float num2 = (!Input.GetKey(KeyCode.RightArrow)) ? 0f : 1f;
			return num + num2;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x000134D4 File Offset: 0x000118D4
		private float GetYFromKeyboard()
		{
			float num = (!Input.GetKey(KeyCode.UpArrow)) ? 0f : 1f;
			float num2 = (!Input.GetKey(KeyCode.DownArrow)) ? 0f : -1f;
			return num + num2;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00013524 File Offset: 0x00011924
		private Vector2 GetVectorFromMouse(float deltaTime, bool smoothed)
		{
			if (smoothed)
			{
				this.mx = this.ApplySmoothing(this.mx, Input.GetAxisRaw("mouse x") * 0.05f, deltaTime, 0.1f);
				this.my = this.ApplySmoothing(this.my, Input.GetAxisRaw("mouse y") * 0.05f, deltaTime, 0.1f);
			}
			else
			{
				this.mx = Input.GetAxisRaw("mouse x") * 0.05f;
				this.my = Input.GetAxisRaw("mouse y") * 0.05f;
			}
			return new Vector2(this.mx, this.my);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x000135C9 File Offset: 0x000119C9
		private float ApplySmoothing(float lastValue, float thisValue, float deltaTime, float sensitivity)
		{
			sensitivity = Mathf.Clamp(sensitivity, 0.001f, 1f);
			if (Mathf.Approximately(sensitivity, 1f))
			{
				return thisValue;
			}
			return Mathf.Lerp(lastValue, thisValue, deltaTime * sensitivity * 100f);
		}

		// Token: 0x040001D8 RID: 472
		private const float sensitivity = 0.1f;

		// Token: 0x040001D9 RID: 473
		private const float mouseScale = 0.05f;

		// Token: 0x040001DA RID: 474
		private float kx;

		// Token: 0x040001DB RID: 475
		private float ky;

		// Token: 0x040001DC RID: 476
		private float mx;

		// Token: 0x040001DD RID: 477
		private float my;
	}
}
