// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace InControl
{
	public class XboxOneInputDeviceManager : InputDeviceManager
	{
		private const int maxDevices = 8;

		private bool[] deviceConnected = new bool[8];

		public XboxOneInputDeviceManager()
		{
			for (uint num = 1u; num <= 8u; num += 1u)
			{
				this.devices.Add(new XboxOneInputDevice(num));
			}
			this.UpdateInternal(0uL, 0f);
		}

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

		public override void Update(ulong updateTick, float deltaTime)
		{
			this.UpdateInternal(updateTick, deltaTime);
		}

		public override void Destroy()
		{
		}

		public static bool CheckPlatformSupport(ICollection<string> errors)
		{
			return Application.platform == RuntimePlatform.XboxOne;
		}

		internal static bool Enable()
		{
			List<string> list = new List<string>();
			if (XboxOneInputDeviceManager.CheckPlatformSupport(list))
			{
				InputManager.AddDeviceManager<XboxOneInputDeviceManager>();
				return true;
			}
			foreach (string current in list)
			{
				Logger.LogError(current);
			}
			return false;
		}
	}
}
