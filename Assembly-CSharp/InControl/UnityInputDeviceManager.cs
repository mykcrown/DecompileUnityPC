using System;
using System.Collections.Generic;
using UnityEngine;

namespace InControl
{
	// Token: 0x020001CD RID: 461
	public class UnityInputDeviceManager : InputDeviceManager
	{
		// Token: 0x060007DF RID: 2015 RVA: 0x00049E8A File Offset: 0x0004828A
		public UnityInputDeviceManager()
		{
			this.AddSystemDeviceProfiles();
			this.QueryJoystickInfo();
			this.AttachDevices();
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00049EC4 File Offset: 0x000482C4
		public override void Update(ulong updateTick, float deltaTime)
		{
			this.deviceRefreshTimer += deltaTime;
			if (this.deviceRefreshTimer >= 1f)
			{
				this.deviceRefreshTimer = 0f;
				this.QueryJoystickInfo();
				if (this.JoystickInfoHasChanged)
				{
					Logger.LogInfo("Change in attached Unity joysticks detected; refreshing device list.");
					this.DetachDevices();
					this.AttachDevices();
				}
			}
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00049F24 File Offset: 0x00048324
		private void QueryJoystickInfo()
		{
			this.joystickNames = Input.GetJoystickNames();
			this.joystickCount = this.joystickNames.Length;
			this.joystickHash = 527 + this.joystickCount;
			for (int i = 0; i < this.joystickCount; i++)
			{
				this.joystickHash = this.joystickHash * 31 + this.joystickNames[i].GetHashCode();
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00049F90 File Offset: 0x00048390
		private bool JoystickInfoHasChanged
		{
			get
			{
				return this.joystickHash != this.lastJoystickHash || this.joystickCount != this.lastJoystickCount;
			}
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00049FB7 File Offset: 0x000483B7
		private void AttachDevices()
		{
			this.AttachKeyboardDevices();
			this.AttachJoystickDevices();
			this.lastJoystickCount = this.joystickCount;
			this.lastJoystickHash = this.joystickHash;
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00049FE0 File Offset: 0x000483E0
		private void DetachDevices()
		{
			int count = this.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.DetachDevice(this.devices[i]);
			}
			this.devices.Clear();
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0004A027 File Offset: 0x00048427
		public void ReloadDevices()
		{
			this.QueryJoystickInfo();
			this.DetachDevices();
			this.AttachDevices();
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0004A03B File Offset: 0x0004843B
		private void AttachDevice(UnityInputDevice device)
		{
			this.devices.Add(device);
			InputManager.AttachDevice(device);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0004A050 File Offset: 0x00048450
		private void AttachKeyboardDevices()
		{
			int count = this.systemDeviceProfiles.Count;
			for (int i = 0; i < count; i++)
			{
				UnityInputDeviceProfileBase unityInputDeviceProfileBase = this.systemDeviceProfiles[i];
				if (unityInputDeviceProfileBase.IsNotJoystick && unityInputDeviceProfileBase.IsSupportedOnThisPlatform)
				{
					this.AttachDevice(new UnityInputDevice(unityInputDeviceProfileBase));
				}
			}
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0004A0AC File Offset: 0x000484AC
		private void AttachJoystickDevices()
		{
			try
			{
				for (int i = 0; i < this.joystickCount; i++)
				{
					this.DetectJoystickDevice(i + 1, this.joystickNames[i]);
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.Message);
				Logger.LogError(ex.StackTrace);
			}
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0004A114 File Offset: 0x00048514
		private bool HasAttachedDeviceWithJoystickId(int unityJoystickId)
		{
			int count = this.devices.Count;
			for (int i = 0; i < count; i++)
			{
				UnityInputDevice unityInputDevice = this.devices[i] as UnityInputDevice;
				if (unityInputDevice != null && unityInputDevice.JoystickId == unityJoystickId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0004A168 File Offset: 0x00048568
		private void DetectJoystickDevice(int unityJoystickId, string unityJoystickName)
		{
			if (this.HasAttachedDeviceWithJoystickId(unityJoystickId))
			{
				return;
			}
			if (unityJoystickName.IndexOf("webcam", StringComparison.OrdinalIgnoreCase) != -1)
			{
				return;
			}
			if (InputManager.UnityVersion < new VersionInfo(4, 5, 0, 0) && (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) && unityJoystickName == "Unknown Wireless Controller")
			{
				return;
			}
			if (InputManager.UnityVersion >= new VersionInfo(4, 6, 3, 0) && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && string.IsNullOrEmpty(unityJoystickName))
			{
				return;
			}
			UnityInputDeviceProfileBase unityInputDeviceProfileBase = null;
			if (unityInputDeviceProfileBase == null)
			{
				unityInputDeviceProfileBase = this.customDeviceProfiles.Find((UnityInputDeviceProfileBase config) => config.HasJoystickName(unityJoystickName));
			}
			if (unityInputDeviceProfileBase == null)
			{
				unityInputDeviceProfileBase = this.systemDeviceProfiles.Find((UnityInputDeviceProfileBase config) => config.HasJoystickName(unityJoystickName));
			}
			if (unityInputDeviceProfileBase == null)
			{
				unityInputDeviceProfileBase = this.customDeviceProfiles.Find((UnityInputDeviceProfileBase config) => config.HasLastResortRegex(unityJoystickName));
			}
			if (unityInputDeviceProfileBase == null)
			{
				unityInputDeviceProfileBase = this.systemDeviceProfiles.Find((UnityInputDeviceProfileBase config) => config.HasLastResortRegex(unityJoystickName));
			}
			if (unityInputDeviceProfileBase == null)
			{
				UnityInputDevice device = new UnityInputDevice(unityJoystickId, unityJoystickName);
				this.AttachDevice(device);
				Debug.Log(string.Concat(new object[]
				{
					"[InControl] Joystick ",
					unityJoystickId,
					": \"",
					unityJoystickName,
					"\""
				}));
				Logger.LogWarning(string.Concat(new object[]
				{
					"Device ",
					unityJoystickId,
					" with name \"",
					unityJoystickName,
					"\" does not match any supported profiles and will be considered an unknown controller."
				}));
				return;
			}
			if (!unityInputDeviceProfileBase.IsHidden)
			{
				UnityInputDevice device2 = new UnityInputDevice(unityInputDeviceProfileBase, unityJoystickId, unityJoystickName);
				this.AttachDevice(device2);
				Logger.LogInfo(string.Concat(new object[]
				{
					"Device ",
					unityJoystickId,
					" matched profile ",
					unityInputDeviceProfileBase.GetType().Name,
					" (",
					unityInputDeviceProfileBase.Name,
					")"
				}));
			}
			else
			{
				Logger.LogInfo(string.Concat(new object[]
				{
					"Device ",
					unityJoystickId,
					" matching profile ",
					unityInputDeviceProfileBase.GetType().Name,
					" (",
					unityInputDeviceProfileBase.Name,
					") is hidden and will not be attached."
				}));
			}
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0004A3F7 File Offset: 0x000487F7
		private void AddSystemDeviceProfile(UnityInputDeviceProfile deviceProfile)
		{
			if (deviceProfile.IsSupportedOnThisPlatform)
			{
				this.systemDeviceProfiles.Add(deviceProfile);
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0004A410 File Offset: 0x00048810
		private void AddSystemDeviceProfiles()
		{
			foreach (string typeName in UnityInputDeviceProfileList.Profiles)
			{
				UnityInputDeviceProfile deviceProfile = (UnityInputDeviceProfile)Activator.CreateInstance(Type.GetType(typeName));
				this.AddSystemDeviceProfile(deviceProfile);
			}
		}

		// Token: 0x04000593 RID: 1427
		private const float deviceRefreshInterval = 1f;

		// Token: 0x04000594 RID: 1428
		private float deviceRefreshTimer;

		// Token: 0x04000595 RID: 1429
		private List<UnityInputDeviceProfileBase> systemDeviceProfiles = new List<UnityInputDeviceProfileBase>(UnityInputDeviceProfileList.Profiles.Length);

		// Token: 0x04000596 RID: 1430
		private List<UnityInputDeviceProfileBase> customDeviceProfiles = new List<UnityInputDeviceProfileBase>();

		// Token: 0x04000597 RID: 1431
		private string[] joystickNames;

		// Token: 0x04000598 RID: 1432
		private int lastJoystickCount;

		// Token: 0x04000599 RID: 1433
		private int lastJoystickHash;

		// Token: 0x0400059A RID: 1434
		private int joystickCount;

		// Token: 0x0400059B RID: 1435
		private int joystickHash;
	}
}
