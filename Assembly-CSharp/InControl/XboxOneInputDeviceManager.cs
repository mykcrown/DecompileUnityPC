using System;
using System.Collections.Generic;
using UnityEngine;

namespace InControl
{
	// Token: 0x020001DC RID: 476
	public class XboxOneInputDeviceManager : InputDeviceManager
	{
		// Token: 0x06000884 RID: 2180 RVA: 0x0004C0E0 File Offset: 0x0004A4E0
		public XboxOneInputDeviceManager()
		{
			for (uint num = 1U; num <= 8U; num += 1U)
			{
				this.devices.Add(new XboxOneInputDevice(num));
			}
			this.UpdateInternal(0UL, 0f);
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0004C130 File Offset: 0x0004A530
		private void UpdateInternal(ulong updateTick, float deltaTime)
		{
			for (int i = 0; i < 8; i++)
			{
				XboxOneInputDevice xboxOneInputDevice = this.devices[i] as XboxOneInputDevice;
				if (xboxOneInputDevice.IsConnected != this.deviceConnected[i])
				{
					if (xboxOneInputDevice.IsConnected)
					{
						InputManager.AttachDevice(xboxOneInputDevice);
					}
					else
					{
						InputManager.DetachDevice(xboxOneInputDevice);
					}
					this.deviceConnected[i] = xboxOneInputDevice.IsConnected;
				}
			}
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0004C19E File Offset: 0x0004A59E
		public override void Update(ulong updateTick, float deltaTime)
		{
			this.UpdateInternal(updateTick, deltaTime);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0004C1A8 File Offset: 0x0004A5A8
		public override void Destroy()
		{
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0004C1AA File Offset: 0x0004A5AA
		public static bool CheckPlatformSupport(ICollection<string> errors)
		{
			return Application.platform == RuntimePlatform.XboxOne;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0004C1BC File Offset: 0x0004A5BC
		internal static bool Enable()
		{
			List<string> list = new List<string>();
			if (XboxOneInputDeviceManager.CheckPlatformSupport(list))
			{
				InputManager.AddDeviceManager<XboxOneInputDeviceManager>();
				return true;
			}
			foreach (string text in list)
			{
				Logger.LogError(text);
			}
			return false;
		}

		// Token: 0x040005F9 RID: 1529
		private const int maxDevices = 8;

		// Token: 0x040005FA RID: 1530
		private bool[] deviceConnected = new bool[8];
	}
}
