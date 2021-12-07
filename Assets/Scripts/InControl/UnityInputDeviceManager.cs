// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace InControl
{
	public class UnityInputDeviceManager : InputDeviceManager
	{
		private sealed class _DetectJoystickDevice_c__AnonStorey0
		{
			internal string unityJoystickName;

			internal bool __m__0(UnityInputDeviceProfileBase config)
			{
				return config.HasJoystickName(this.unityJoystickName);
			}

			internal bool __m__1(UnityInputDeviceProfileBase config)
			{
				return config.HasJoystickName(this.unityJoystickName);
			}

			internal bool __m__2(UnityInputDeviceProfileBase config)
			{
				return config.HasLastResortRegex(this.unityJoystickName);
			}

			internal bool __m__3(UnityInputDeviceProfileBase config)
			{
				return config.HasLastResortRegex(this.unityJoystickName);
			}
		}

		private const float deviceRefreshInterval = 1f;

		private float deviceRefreshTimer;

		private List<UnityInputDeviceProfileBase> systemDeviceProfiles = new List<UnityInputDeviceProfileBase>(UnityInputDeviceProfileList.Profiles.Length);

		private List<UnityInputDeviceProfileBase> customDeviceProfiles = new List<UnityInputDeviceProfileBase>();

		private string[] joystickNames;

		private int lastJoystickCount;

		private int lastJoystickHash;

		private int joystickCount;

		private int joystickHash;

		private bool JoystickInfoHasChanged
		{
			get
			{
				return this.joystickHash != this.lastJoystickHash || this.joystickCount != this.lastJoystickCount;
			}
		}

		public UnityInputDeviceManager()
		{
			this.AddSystemDeviceProfiles();
			this.QueryJoystickInfo();
			this.AttachDevices();
		}

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

		private void AttachDevices()
		{
			this.AttachKeyboardDevices();
			this.AttachJoystickDevices();
			this.lastJoystickCount = this.joystickCount;
			this.lastJoystickHash = this.joystickHash;
		}

		private void DetachDevices()
		{
			int count = this.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.DetachDevice(this.devices[i]);
			}
			this.devices.Clear();
		}

		public void ReloadDevices()
		{
			this.QueryJoystickInfo();
			this.DetachDevices();
			this.AttachDevices();
		}

		private void AttachDevice(UnityInputDevice device)
		{
			this.devices.Add(device);
			InputManager.AttachDevice(device);
		}

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

		private void DetectJoystickDevice(int unityJoystickId, string unityJoystickName)
		{
			UnityInputDeviceManager._DetectJoystickDevice_c__AnonStorey0 _DetectJoystickDevice_c__AnonStorey = new UnityInputDeviceManager._DetectJoystickDevice_c__AnonStorey0();
			_DetectJoystickDevice_c__AnonStorey.unityJoystickName = unityJoystickName;
			if (this.HasAttachedDeviceWithJoystickId(unityJoystickId))
			{
				return;
			}
			if (_DetectJoystickDevice_c__AnonStorey.unityJoystickName.IndexOf("webcam", StringComparison.OrdinalIgnoreCase) != -1)
			{
				return;
			}
			if (InputManager.UnityVersion < new VersionInfo(4, 5, 0, 0) && (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) && _DetectJoystickDevice_c__AnonStorey.unityJoystickName == "Unknown Wireless Controller")
			{
				return;
			}
			if (InputManager.UnityVersion >= new VersionInfo(4, 6, 3, 0) && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && string.IsNullOrEmpty(_DetectJoystickDevice_c__AnonStorey.unityJoystickName))
			{
				return;
			}
			UnityInputDeviceProfileBase unityInputDeviceProfileBase = null;
			if (unityInputDeviceProfileBase == null)
			{
				unityInputDeviceProfileBase = this.customDeviceProfiles.Find(new Predicate<UnityInputDeviceProfileBase>(_DetectJoystickDevice_c__AnonStorey.__m__0));
			}
			if (unityInputDeviceProfileBase == null)
			{
				unityInputDeviceProfileBase = this.systemDeviceProfiles.Find(new Predicate<UnityInputDeviceProfileBase>(_DetectJoystickDevice_c__AnonStorey.__m__1));
			}
			if (unityInputDeviceProfileBase == null)
			{
				unityInputDeviceProfileBase = this.customDeviceProfiles.Find(new Predicate<UnityInputDeviceProfileBase>(_DetectJoystickDevice_c__AnonStorey.__m__2));
			}
			if (unityInputDeviceProfileBase == null)
			{
				unityInputDeviceProfileBase = this.systemDeviceProfiles.Find(new Predicate<UnityInputDeviceProfileBase>(_DetectJoystickDevice_c__AnonStorey.__m__3));
			}
			if (unityInputDeviceProfileBase == null)
			{
				UnityInputDevice device = new UnityInputDevice(unityJoystickId, _DetectJoystickDevice_c__AnonStorey.unityJoystickName);
				this.AttachDevice(device);
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"[InControl] Joystick ",
					unityJoystickId,
					": \"",
					_DetectJoystickDevice_c__AnonStorey.unityJoystickName,
					"\""
				}));
				Logger.LogWarning(string.Concat(new object[]
				{
					"Device ",
					unityJoystickId,
					" with name \"",
					_DetectJoystickDevice_c__AnonStorey.unityJoystickName,
					"\" does not match any supported profiles and will be considered an unknown controller."
				}));
				return;
			}
			if (!unityInputDeviceProfileBase.IsHidden)
			{
				UnityInputDevice device2 = new UnityInputDevice(unityInputDeviceProfileBase, unityJoystickId, _DetectJoystickDevice_c__AnonStorey.unityJoystickName);
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

		private void AddSystemDeviceProfile(UnityInputDeviceProfile deviceProfile)
		{
			if (deviceProfile.IsSupportedOnThisPlatform)
			{
				this.systemDeviceProfiles.Add(deviceProfile);
			}
		}

		private void AddSystemDeviceProfiles()
		{
			string[] profiles = UnityInputDeviceProfileList.Profiles;
			for (int i = 0; i < profiles.Length; i++)
			{
				string typeName = profiles[i];
				UnityInputDeviceProfile deviceProfile = (UnityInputDeviceProfile)Activator.CreateInstance(Type.GetType(typeName));
				this.AddSystemDeviceProfile(deviceProfile);
			}
		}
	}
}
