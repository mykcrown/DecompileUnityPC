using System;

namespace InControl
{
	// Token: 0x02000065 RID: 101
	public class UnknownDeviceBindingSourceListener : BindingSourceListener
	{
		// Token: 0x06000398 RID: 920 RVA: 0x00017CE8 File Offset: 0x000160E8
		public void Reset()
		{
			this.detectFound = UnknownDeviceControl.None;
			this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForInitialRelease;
			this.TakeSnapshotOnUnknownDevices();
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00017D04 File Offset: 0x00016104
		private void TakeSnapshotOnUnknownDevices()
		{
			int count = InputManager.Devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.Devices[i];
				if (inputDevice.IsUnknown)
				{
					inputDevice.TakeSnapshot();
				}
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00017D4C File Offset: 0x0001614C
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (!listenOptions.IncludeUnknownControllers || device.IsKnown)
			{
				return null;
			}
			if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlRelease && this.detectFound && !this.IsPressed(this.detectFound, device))
			{
				UnknownDeviceBindingSource result = new UnknownDeviceBindingSource(this.detectFound);
				this.Reset();
				return result;
			}
			UnknownDeviceControl control = this.ListenForControl(listenOptions, device);
			if (control)
			{
				if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlPress)
				{
					this.detectFound = control;
					this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlRelease;
				}
			}
			else if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForInitialRelease)
			{
				this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlPress;
			}
			return null;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00017DFC File Offset: 0x000161FC
		private bool IsPressed(UnknownDeviceControl control, InputDevice device)
		{
			float value = control.GetValue(device);
			return Utility.AbsoluteIsOverThreshold(value, 0.5f);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00017E20 File Offset: 0x00016220
		private UnknownDeviceControl ListenForControl(BindingListenOptions listenOptions, InputDevice device)
		{
			if (device.IsUnknown)
			{
				UnknownDeviceControl firstPressedButton = device.GetFirstPressedButton();
				if (firstPressedButton)
				{
					return firstPressedButton;
				}
				UnknownDeviceControl firstPressedAnalog = device.GetFirstPressedAnalog();
				if (firstPressedAnalog)
				{
					return firstPressedAnalog;
				}
			}
			return UnknownDeviceControl.None;
		}

		// Token: 0x040002D1 RID: 721
		private UnknownDeviceControl detectFound;

		// Token: 0x040002D2 RID: 722
		private UnknownDeviceBindingSourceListener.DetectPhase detectPhase;

		// Token: 0x02000066 RID: 102
		private enum DetectPhase
		{
			// Token: 0x040002D4 RID: 724
			WaitForInitialRelease,
			// Token: 0x040002D5 RID: 725
			WaitForControlPress,
			// Token: 0x040002D6 RID: 726
			WaitForControlRelease
		}
	}
}
