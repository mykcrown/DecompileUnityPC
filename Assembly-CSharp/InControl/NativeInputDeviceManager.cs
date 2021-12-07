using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200011D RID: 285
	public class NativeInputDeviceManager : InputDeviceManager
	{
		// Token: 0x06000617 RID: 1559 RVA: 0x0002AA60 File Offset: 0x00028E60
		public NativeInputDeviceManager()
		{
			this.attachedDevices = new List<NativeInputDevice>();
			this.detachedDevices = new List<NativeInputDevice>();
			this.systemDeviceProfiles = new List<NativeInputDeviceProfile>(NativeInputDeviceProfileList.Profiles.Length);
			this.customDeviceProfiles = new List<NativeInputDeviceProfile>();
			this.deviceEvents = new uint[32];
			this.AddSystemDeviceProfiles();
			NativeInputOptions options = default(NativeInputOptions);
			options.enableXInput = InputManager.NativeInputEnableXInput;
			options.preventSleep = InputManager.NativeInputPreventSleep;
			if (InputManager.NativeInputUpdateRate > 0U)
			{
				options.updateRate = (ushort)InputManager.NativeInputUpdateRate;
			}
			else
			{
				options.updateRate = (ushort)Mathf.FloorToInt(1f / Time.fixedDeltaTime);
			}
			Native.Init(options);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0002AB14 File Offset: 0x00028F14
		public override void Destroy()
		{
			Native.Stop();
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0002AB1B File Offset: 0x00028F1B
		private uint NextPowerOfTwo(uint x)
		{
			if (x < 0U)
			{
				return 0U;
			}
			x -= 1U;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return x + 1U;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0002AB54 File Offset: 0x00028F54
		public override void Update(ulong updateTick, float deltaTime)
		{
			IntPtr source;
			int num = Native.GetDeviceEvents(out source);
			if (num > 0)
			{
				Utility.ArrayExpand<uint>(ref this.deviceEvents, num);
				MarshalUtility.Copy(source, this.deviceEvents, num);
				int num2 = 0;
				uint num3 = this.deviceEvents[num2++];
				int num4 = 0;
				while ((long)num4 < (long)((ulong)num3))
				{
					uint num5 = this.deviceEvents[num2++];
					StringBuilder stringBuilder = new StringBuilder(256);
					stringBuilder.Append("Attached native device with handle " + num5 + ":\n");
					NativeDeviceInfo deviceInfo;
					if (Native.GetDeviceInfo(num5, out deviceInfo))
					{
						stringBuilder.AppendFormat("Name: {0}\n", deviceInfo.name);
						stringBuilder.AppendFormat("Driver Type: {0}\n", deviceInfo.driverType);
						stringBuilder.AppendFormat("Location ID: {0}\n", deviceInfo.location);
						stringBuilder.AppendFormat("Serial Number: {0}\n", deviceInfo.serialNumber);
						stringBuilder.AppendFormat("Vendor ID: 0x{0:x}\n", deviceInfo.vendorID);
						stringBuilder.AppendFormat("Product ID: 0x{0:x}\n", deviceInfo.productID);
						stringBuilder.AppendFormat("Version Number: 0x{0:x}\n", deviceInfo.versionNumber);
						stringBuilder.AppendFormat("Buttons: {0}\n", deviceInfo.numButtons);
						stringBuilder.AppendFormat("Analogs: {0}\n", deviceInfo.numAnalogs);
						this.DetectDevice(num5, deviceInfo);
					}
					Logger.LogInfo(stringBuilder.ToString());
					num4++;
				}
				uint num6 = this.deviceEvents[num2++];
				int num7 = 0;
				while ((long)num7 < (long)((ulong)num6))
				{
					uint num8 = this.deviceEvents[num2++];
					Logger.LogInfo("Detached native device with handle " + num8 + ":");
					NativeInputDevice nativeInputDevice = this.FindAttachedDevice(num8);
					if (nativeInputDevice != null)
					{
						this.DetachDevice(nativeInputDevice);
					}
					else
					{
						Logger.LogWarning("Couldn't find device to detach with handle: " + num8);
					}
					num7++;
				}
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0002AD64 File Offset: 0x00029164
		private void DetectDevice(uint deviceHandle, NativeDeviceInfo deviceInfo)
		{
			NativeInputDeviceProfile nativeInputDeviceProfile = null;
			nativeInputDeviceProfile = (nativeInputDeviceProfile ?? this.customDeviceProfiles.Find((NativeInputDeviceProfile profile) => profile.Matches(deviceInfo)));
			nativeInputDeviceProfile = (nativeInputDeviceProfile ?? this.systemDeviceProfiles.Find((NativeInputDeviceProfile profile) => profile.Matches(deviceInfo)));
			nativeInputDeviceProfile = (nativeInputDeviceProfile ?? this.customDeviceProfiles.Find((NativeInputDeviceProfile profile) => profile.LastResortMatches(deviceInfo)));
			nativeInputDeviceProfile = (nativeInputDeviceProfile ?? this.systemDeviceProfiles.Find((NativeInputDeviceProfile profile) => profile.LastResortMatches(deviceInfo)));
			NativeInputDevice nativeInputDevice = this.FindDetachedDevice(deviceInfo) ?? new NativeInputDevice();
			nativeInputDevice.Initialize(deviceHandle, deviceInfo, nativeInputDeviceProfile);
			this.AttachDevice(nativeInputDevice);
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0002AE2E File Offset: 0x0002922E
		private void AttachDevice(NativeInputDevice device)
		{
			this.detachedDevices.Remove(device);
			this.attachedDevices.Add(device);
			InputManager.AttachDevice(device);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0002AE4F File Offset: 0x0002924F
		private void DetachDevice(NativeInputDevice device)
		{
			this.attachedDevices.Remove(device);
			this.detachedDevices.Add(device);
			InputManager.DetachDevice(device);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0002AE70 File Offset: 0x00029270
		private NativeInputDevice FindAttachedDevice(uint deviceHandle)
		{
			int count = this.attachedDevices.Count;
			for (int i = 0; i < count; i++)
			{
				NativeInputDevice nativeInputDevice = this.attachedDevices[i];
				if (nativeInputDevice.Handle == deviceHandle)
				{
					return nativeInputDevice;
				}
			}
			return null;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0002AEB8 File Offset: 0x000292B8
		private NativeInputDevice FindDetachedDevice(NativeDeviceInfo deviceInfo)
		{
			ReadOnlyCollection<NativeInputDevice> arg = new ReadOnlyCollection<NativeInputDevice>(this.detachedDevices);
			if (NativeInputDeviceManager.CustomFindDetachedDevice != null)
			{
				return NativeInputDeviceManager.CustomFindDetachedDevice(deviceInfo, arg);
			}
			return NativeInputDeviceManager.SystemFindDetachedDevice(deviceInfo, arg);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0002AEF0 File Offset: 0x000292F0
		private static NativeInputDevice SystemFindDetachedDevice(NativeDeviceInfo deviceInfo, ReadOnlyCollection<NativeInputDevice> detachedDevices)
		{
			int count = detachedDevices.Count;
			for (int i = 0; i < count; i++)
			{
				NativeInputDevice nativeInputDevice = detachedDevices[i];
				if (nativeInputDevice.Info.HasSameVendorID(deviceInfo) && nativeInputDevice.Info.HasSameProductID(deviceInfo) && nativeInputDevice.Info.HasSameSerialNumber(deviceInfo))
				{
					return nativeInputDevice;
				}
			}
			for (int j = 0; j < count; j++)
			{
				NativeInputDevice nativeInputDevice2 = detachedDevices[j];
				if (nativeInputDevice2.Info.HasSameVendorID(deviceInfo) && nativeInputDevice2.Info.HasSameProductID(deviceInfo) && nativeInputDevice2.Info.HasSameLocation(deviceInfo))
				{
					return nativeInputDevice2;
				}
			}
			for (int k = 0; k < count; k++)
			{
				NativeInputDevice nativeInputDevice3 = detachedDevices[k];
				if (nativeInputDevice3.Info.HasSameVendorID(deviceInfo) && nativeInputDevice3.Info.HasSameProductID(deviceInfo) && nativeInputDevice3.Info.HasSameVersionNumber(deviceInfo))
				{
					return nativeInputDevice3;
				}
			}
			for (int l = 0; l < count; l++)
			{
				NativeInputDevice nativeInputDevice4 = detachedDevices[l];
				if (nativeInputDevice4.Info.HasSameLocation(deviceInfo))
				{
					return nativeInputDevice4;
				}
			}
			return null;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0002B062 File Offset: 0x00029462
		private void AddSystemDeviceProfile(NativeInputDeviceProfile deviceProfile)
		{
			if (deviceProfile.IsSupportedOnThisPlatform)
			{
				this.systemDeviceProfiles.Add(deviceProfile);
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0002B07C File Offset: 0x0002947C
		private void AddSystemDeviceProfiles()
		{
			foreach (string typeName in NativeInputDeviceProfileList.Profiles)
			{
				NativeInputDeviceProfile deviceProfile = (NativeInputDeviceProfile)Activator.CreateInstance(Type.GetType(typeName));
				this.AddSystemDeviceProfile(deviceProfile);
			}
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0002B0C0 File Offset: 0x000294C0
		public static bool CheckPlatformSupport(ICollection<string> errors)
		{
			if (Application.platform != RuntimePlatform.OSXPlayer && Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
			{
				return false;
			}
			try
			{
				NativeVersionInfo nativeVersionInfo;
				Native.GetVersionInfo(out nativeVersionInfo);
				Logger.LogInfo(string.Concat(new object[]
				{
					"InControl Native (version ",
					nativeVersionInfo.major,
					".",
					nativeVersionInfo.minor,
					".",
					nativeVersionInfo.patch,
					")"
				}));
			}
			catch (DllNotFoundException ex)
			{
				if (errors != null)
				{
					errors.Add(ex.Message + Utility.PluginFileExtension() + " could not be found or is missing a dependency.");
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0002B1A0 File Offset: 0x000295A0
		internal static bool Enable()
		{
			List<string> list = new List<string>();
			if (NativeInputDeviceManager.CheckPlatformSupport(list))
			{
				InputManager.AddDeviceManager<NativeInputDeviceManager>();
				return true;
			}
			foreach (string str in list)
			{
				Debug.LogError("Error enabling NativeInputDeviceManager: " + str);
			}
			return false;
		}

		// Token: 0x04000486 RID: 1158
		public static Func<NativeDeviceInfo, ReadOnlyCollection<NativeInputDevice>, NativeInputDevice> CustomFindDetachedDevice;

		// Token: 0x04000487 RID: 1159
		private List<NativeInputDevice> attachedDevices;

		// Token: 0x04000488 RID: 1160
		private List<NativeInputDevice> detachedDevices;

		// Token: 0x04000489 RID: 1161
		private List<NativeInputDeviceProfile> systemDeviceProfiles;

		// Token: 0x0400048A RID: 1162
		private List<NativeInputDeviceProfile> customDeviceProfiles;

		// Token: 0x0400048B RID: 1163
		private uint[] deviceEvents;
	}
}
