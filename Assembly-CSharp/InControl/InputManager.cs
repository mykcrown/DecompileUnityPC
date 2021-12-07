using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200007F RID: 127
	public class InputManager
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000503 RID: 1283 RVA: 0x00019630 File Offset: 0x00017A30
		// (remove) Token: 0x06000504 RID: 1284 RVA: 0x00019664 File Offset: 0x00017A64
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnSetup;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000505 RID: 1285 RVA: 0x00019698 File Offset: 0x00017A98
		// (remove) Token: 0x06000506 RID: 1286 RVA: 0x000196CC File Offset: 0x00017ACC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<ulong, float> OnUpdate;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000507 RID: 1287 RVA: 0x00019700 File Offset: 0x00017B00
		// (remove) Token: 0x06000508 RID: 1288 RVA: 0x00019734 File Offset: 0x00017B34
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnReset;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000509 RID: 1289 RVA: 0x00019768 File Offset: 0x00017B68
		// (remove) Token: 0x0600050A RID: 1290 RVA: 0x0001979C File Offset: 0x00017B9C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<InputDevice> OnDeviceAttached;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600050B RID: 1291 RVA: 0x000197D0 File Offset: 0x00017BD0
		// (remove) Token: 0x0600050C RID: 1292 RVA: 0x00019804 File Offset: 0x00017C04
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<InputDevice> OnDeviceDetached;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600050D RID: 1293 RVA: 0x00019838 File Offset: 0x00017C38
		// (remove) Token: 0x0600050E RID: 1294 RVA: 0x0001986C File Offset: 0x00017C6C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<InputDevice> OnActiveDeviceChanged;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600050F RID: 1295 RVA: 0x000198A0 File Offset: 0x00017CA0
		// (remove) Token: 0x06000510 RID: 1296 RVA: 0x000198D4 File Offset: 0x00017CD4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal static event Action<ulong, float> OnUpdateDevices;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000511 RID: 1297 RVA: 0x00019908 File Offset: 0x00017D08
		// (remove) Token: 0x06000512 RID: 1298 RVA: 0x0001993C File Offset: 0x00017D3C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal static event Action<ulong, float> OnCommitDevices;

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x00019970 File Offset: 0x00017D70
		// (set) Token: 0x06000514 RID: 1300 RVA: 0x00019977 File Offset: 0x00017D77
		public static bool CommandWasPressed { get; private set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x0001997F File Offset: 0x00017D7F
		// (set) Token: 0x06000516 RID: 1302 RVA: 0x00019986 File Offset: 0x00017D86
		public static bool InvertYAxis { get; set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0001998E File Offset: 0x00017D8E
		// (set) Token: 0x06000518 RID: 1304 RVA: 0x00019995 File Offset: 0x00017D95
		public static bool IsSetup { get; private set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x0001999D File Offset: 0x00017D9D
		// (set) Token: 0x0600051A RID: 1306 RVA: 0x000199A4 File Offset: 0x00017DA4
		internal static string Platform { get; private set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x000199AC File Offset: 0x00017DAC
		[Obsolete("Use InputManager.CommandWasPressed instead.")]
		public static bool MenuWasPressed
		{
			get
			{
				return InputManager.CommandWasPressed;
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x000199B3 File Offset: 0x00017DB3
		[Obsolete("Calling InputManager.Setup() directly is no longer supported. Use the InControlManager component to manage the lifecycle of the input manager instead.", true)]
		public static void Setup()
		{
			InputManager.SetupInternal();
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000199BC File Offset: 0x00017DBC
		internal static bool SetupInternal()
		{
			if (InputManager.IsSetup)
			{
				return false;
			}
			InputManager.Platform = Utility.GetWindowsVersion().ToUpper();
			InputManager.enabled = true;
			InputManager.initialTime = 0f;
			InputManager.currentTime = 0f;
			InputManager.lastUpdateTime = 0f;
			InputManager.currentTick = 0UL;
			InputManager.applicationIsFocused = true;
			InputManager.deviceManagers.Clear();
			InputManager.deviceManagerTable.Clear();
			InputManager.devices.Clear();
			InputManager.Devices = new ReadOnlyCollection<InputDevice>(InputManager.devices);
			InputManager.activeDevice = InputDevice.Null;
			InputManager.playerActionSets.Clear();
			InputManager.IsSetup = true;
			bool flag = true;
			bool flag2 = InputManager.EnableNativeInput && NativeInputDeviceManager.Enable();
			if (flag2)
			{
				flag = false;
			}
			if (InputManager.EnableXInput && flag)
			{
				XInputDeviceManager.Enable();
			}
			if (InputManager.OnSetup != null)
			{
				InputManager.OnSetup();
				InputManager.OnSetup = null;
			}
			if (flag)
			{
				InputManager.AddDeviceManager<UnityInputDeviceManager>();
			}
			return true;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00019AB4 File Offset: 0x00017EB4
		[Obsolete("Calling InputManager.Reset() method directly is no longer supported. Use the InControlManager component to manage the lifecycle of the input manager instead.", true)]
		public static void Reset()
		{
			InputManager.ResetInternal();
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00019ABC File Offset: 0x00017EBC
		internal static void ResetInternal()
		{
			if (InputManager.OnReset != null)
			{
				InputManager.OnReset();
			}
			InputManager.OnSetup = null;
			InputManager.OnUpdate = null;
			InputManager.OnReset = null;
			InputManager.OnActiveDeviceChanged = null;
			InputManager.OnDeviceAttached = null;
			InputManager.OnDeviceDetached = null;
			InputManager.OnUpdateDevices = null;
			InputManager.OnCommitDevices = null;
			InputManager.DestroyDeviceManagers();
			InputManager.DestroyDevices();
			InputManager.playerActionSets.Clear();
			InputManager.IsSetup = false;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00019B27 File Offset: 0x00017F27
		[Obsolete("Calling InputManager.Update() directly is no longer supported. Use the InControlManager component to manage the lifecycle of the input manager instead.", true)]
		public static void Update()
		{
			InputManager.UpdateInternal();
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00019B30 File Offset: 0x00017F30
		internal static void UpdateInternal()
		{
			InputManager.AssertIsSetup();
			if (InputManager.OnSetup != null)
			{
				InputManager.OnSetup();
				InputManager.OnSetup = null;
			}
			if (!InputManager.enabled)
			{
				return;
			}
			if (InputManager.SuspendInBackground && !InputManager.applicationIsFocused)
			{
				return;
			}
			InputManager.currentTick += 1UL;
			InputManager.UpdateCurrentTime();
			float num = InputManager.currentTime - InputManager.lastUpdateTime;
			InputManager.UpdateDeviceManagers(num);
			InputManager.CommandWasPressed = false;
			InputManager.UpdateDevices(num);
			InputManager.CommitDevices(num);
			InputManager.UpdateActiveDevice();
			InputManager.UpdatePlayerActionSets(num);
			if (InputManager.OnUpdate != null)
			{
				InputManager.OnUpdate(InputManager.currentTick, num);
			}
			InputManager.lastUpdateTime = InputManager.currentTime;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00019BE1 File Offset: 0x00017FE1
		public static void Reload()
		{
			InputManager.ResetInternal();
			InputManager.SetupInternal();
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00019BEE File Offset: 0x00017FEE
		private static void AssertIsSetup()
		{
			if (!InputManager.IsSetup)
			{
				throw new Exception("InputManager is not initialized. Call InputManager.Setup() first.");
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00019C08 File Offset: 0x00018008
		private static void SetZeroTickOnAllControls()
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				ReadOnlyCollection<InputControl> controls = InputManager.devices[i].Controls;
				int count2 = controls.Count;
				for (int j = 0; j < count2; j++)
				{
					InputControl inputControl = controls[j];
					if (inputControl != null)
					{
						inputControl.SetZeroTick();
					}
				}
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00019C78 File Offset: 0x00018078
		public static void ClearInputState()
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.devices[i].ClearInputState();
			}
			int count2 = InputManager.playerActionSets.Count;
			for (int j = 0; j < count2; j++)
			{
				InputManager.playerActionSets[j].ClearInputState();
			}
			InputManager.activeDevice = InputDevice.Null;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00019CE9 File Offset: 0x000180E9
		internal static void OnApplicationFocus(bool focusState)
		{
			if (!focusState)
			{
				if (InputManager.SuspendInBackground)
				{
					InputManager.ClearInputState();
				}
				InputManager.SetZeroTickOnAllControls();
			}
			InputManager.applicationIsFocused = focusState;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00019D0B File Offset: 0x0001810B
		internal static void OnApplicationPause(bool pauseState)
		{
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00019D0D File Offset: 0x0001810D
		internal static void OnApplicationQuit()
		{
			InputManager.ResetInternal();
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00019D14 File Offset: 0x00018114
		internal static void OnLevelWasLoaded()
		{
			InputManager.SetZeroTickOnAllControls();
			InputManager.UpdateInternal();
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00019D20 File Offset: 0x00018120
		public static void AddDeviceManager(InputDeviceManager deviceManager)
		{
			InputManager.AssertIsSetup();
			Type type = deviceManager.GetType();
			if (InputManager.deviceManagerTable.ContainsKey(type))
			{
				Logger.LogError("A device manager of type '" + type.Name + "' already exists; cannot add another.");
				return;
			}
			InputManager.deviceManagers.Add(deviceManager);
			InputManager.deviceManagerTable.Add(type, deviceManager);
			deviceManager.Update(InputManager.currentTick, InputManager.currentTime - InputManager.lastUpdateTime);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00019D91 File Offset: 0x00018191
		public static void AddDeviceManager<T>() where T : InputDeviceManager, new()
		{
			InputManager.AddDeviceManager(Activator.CreateInstance<T>());
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00019DA4 File Offset: 0x000181A4
		public static T GetDeviceManager<T>() where T : InputDeviceManager
		{
			InputDeviceManager inputDeviceManager;
			if (InputManager.deviceManagerTable.TryGetValue(typeof(T), out inputDeviceManager))
			{
				return inputDeviceManager as T;
			}
			return (T)((object)null);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00019DDE File Offset: 0x000181DE
		public static bool HasDeviceManager<T>() where T : InputDeviceManager
		{
			return InputManager.deviceManagerTable.ContainsKey(typeof(T));
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00019DF4 File Offset: 0x000181F4
		private static void UpdateCurrentTime()
		{
			if (InputManager.initialTime < 1E-45f)
			{
				InputManager.initialTime = Time.realtimeSinceStartup;
			}
			InputManager.currentTime = Mathf.Max(0f, Time.realtimeSinceStartup - InputManager.initialTime);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00019E2C File Offset: 0x0001822C
		private static void UpdateDeviceManagers(float deltaTime)
		{
			int count = InputManager.deviceManagers.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.deviceManagers[i].Update(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00019E6C File Offset: 0x0001826C
		private static void DestroyDeviceManagers()
		{
			int count = InputManager.deviceManagers.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.deviceManagers[i].Destroy();
			}
			InputManager.deviceManagers.Clear();
			InputManager.deviceManagerTable.Clear();
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00019EBC File Offset: 0x000182BC
		private static void DestroyDevices()
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.devices[i];
				inputDevice.OnDetached();
			}
			InputManager.devices.Clear();
			InputManager.activeDevice = InputDevice.Null;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00019F0C File Offset: 0x0001830C
		private static void UpdateDevices(float deltaTime)
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.devices[i];
				inputDevice.Update(InputManager.currentTick, deltaTime);
			}
			if (InputManager.OnUpdateDevices != null)
			{
				InputManager.OnUpdateDevices(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00019F68 File Offset: 0x00018368
		private static void CommitDevices(float deltaTime)
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.devices[i];
				inputDevice.Commit(InputManager.currentTick, deltaTime);
				if (inputDevice.CommandWasPressed)
				{
					InputManager.CommandWasPressed = true;
				}
			}
			if (InputManager.OnCommitDevices != null)
			{
				InputManager.OnCommitDevices(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00019FD8 File Offset: 0x000183D8
		private static void UpdateActiveDevice()
		{
			InputDevice inputDevice = InputManager.ActiveDevice;
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice2 = InputManager.devices[i];
				if (inputDevice2.LastChangedAfter(InputManager.ActiveDevice) && !inputDevice2.Passive)
				{
					InputManager.ActiveDevice = inputDevice2;
				}
			}
			if (inputDevice != InputManager.ActiveDevice && InputManager.OnActiveDeviceChanged != null)
			{
				InputManager.OnActiveDeviceChanged(InputManager.ActiveDevice);
			}
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001A05C File Offset: 0x0001845C
		public static void AttachDevice(InputDevice inputDevice)
		{
			InputManager.AssertIsSetup();
			if (!inputDevice.IsSupportedOnThisPlatform)
			{
				return;
			}
			if (inputDevice.IsAttached)
			{
				return;
			}
			if (!InputManager.devices.Contains(inputDevice))
			{
				InputManager.devices.Add(inputDevice);
				InputManager.devices.Sort((InputDevice d1, InputDevice d2) => d1.SortOrder.CompareTo(d2.SortOrder));
			}
			inputDevice.OnAttached();
			if (InputManager.OnDeviceAttached != null)
			{
				InputManager.OnDeviceAttached(inputDevice);
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001A0E4 File Offset: 0x000184E4
		public static void DetachDevice(InputDevice inputDevice)
		{
			if (!InputManager.IsSetup)
			{
				return;
			}
			if (!inputDevice.IsAttached)
			{
				return;
			}
			InputManager.devices.Remove(inputDevice);
			if (InputManager.ActiveDevice == inputDevice)
			{
				InputManager.ActiveDevice = InputDevice.Null;
			}
			inputDevice.OnDetached();
			if (InputManager.OnDeviceDetached != null)
			{
				InputManager.OnDeviceDetached(inputDevice);
			}
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001A144 File Offset: 0x00018544
		public static void HideDevicesWithProfile(Type type)
		{
			if (type.IsSubclassOf(typeof(UnityInputDeviceProfile)))
			{
				InputDeviceProfile.Hide(type);
			}
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001A161 File Offset: 0x00018561
		internal static void AttachPlayerActionSet(PlayerActionSet playerActionSet)
		{
			if (!InputManager.playerActionSets.Contains(playerActionSet))
			{
				InputManager.playerActionSets.Add(playerActionSet);
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0001A17E File Offset: 0x0001857E
		internal static void DetachPlayerActionSet(PlayerActionSet playerActionSet)
		{
			InputManager.playerActionSets.Remove(playerActionSet);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001A18C File Offset: 0x0001858C
		internal static void UpdatePlayerActionSets(float deltaTime)
		{
			int count = InputManager.playerActionSets.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.playerActionSets[i].Update(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x0001A1CC File Offset: 0x000185CC
		public static bool AnyKeyIsPressed
		{
			get
			{
				return KeyCombo.Detect(true).IncludeCount > 0;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0001A1EA File Offset: 0x000185EA
		// (set) Token: 0x0600053D RID: 1341 RVA: 0x0001A205 File Offset: 0x00018605
		public static InputDevice ActiveDevice
		{
			get
			{
				return (InputManager.activeDevice != null) ? InputManager.activeDevice : InputDevice.Null;
			}
			private set
			{
				InputManager.activeDevice = ((value != null) ? value : InputDevice.Null);
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0001A21D File Offset: 0x0001861D
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x0001A224 File Offset: 0x00018624
		public static bool Enabled
		{
			get
			{
				return InputManager.enabled;
			}
			set
			{
				if (InputManager.enabled != value)
				{
					if (value)
					{
						InputManager.SetZeroTickOnAllControls();
						InputManager.UpdateInternal();
					}
					else
					{
						InputManager.ClearInputState();
						InputManager.SetZeroTickOnAllControls();
					}
					InputManager.enabled = value;
				}
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x0001A256 File Offset: 0x00018656
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x0001A25D File Offset: 0x0001865D
		public static bool SuspendInBackground { get; internal set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x0001A265 File Offset: 0x00018665
		// (set) Token: 0x06000543 RID: 1347 RVA: 0x0001A26C File Offset: 0x0001866C
		public static bool EnableNativeInput { get; internal set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x0001A274 File Offset: 0x00018674
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x0001A27B File Offset: 0x0001867B
		public static bool EnableXInput { get; internal set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x0001A283 File Offset: 0x00018683
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x0001A28A File Offset: 0x0001868A
		public static uint XInputUpdateRate { get; internal set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x0001A292 File Offset: 0x00018692
		// (set) Token: 0x06000549 RID: 1353 RVA: 0x0001A299 File Offset: 0x00018699
		public static uint XInputBufferSize { get; internal set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x0001A2A1 File Offset: 0x000186A1
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x0001A2A8 File Offset: 0x000186A8
		public static bool NativeInputEnableXInput { get; internal set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x0001A2B0 File Offset: 0x000186B0
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x0001A2B7 File Offset: 0x000186B7
		public static bool NativeInputPreventSleep { get; internal set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x0001A2BF File Offset: 0x000186BF
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x0001A2C6 File Offset: 0x000186C6
		public static uint NativeInputUpdateRate { get; internal set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0001A2CE File Offset: 0x000186CE
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x0001A2D5 File Offset: 0x000186D5
		public static bool EnableICade { get; internal set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x0001A2DD File Offset: 0x000186DD
		internal static VersionInfo UnityVersion
		{
			get
			{
				if (InputManager.unityVersion == null)
				{
					InputManager.unityVersion = new VersionInfo?(VersionInfo.UnityVersion());
				}
				return InputManager.unityVersion.Value;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x0001A307 File Offset: 0x00018707
		internal static ulong CurrentTick
		{
			get
			{
				return InputManager.currentTick;
			}
		}

		// Token: 0x0400043F RID: 1087
		public static readonly VersionInfo Version = VersionInfo.InControlVersion();

		// Token: 0x04000448 RID: 1096
		private static List<InputDeviceManager> deviceManagers = new List<InputDeviceManager>();

		// Token: 0x04000449 RID: 1097
		private static Dictionary<Type, InputDeviceManager> deviceManagerTable = new Dictionary<Type, InputDeviceManager>();

		// Token: 0x0400044A RID: 1098
		private static InputDevice activeDevice = InputDevice.Null;

		// Token: 0x0400044B RID: 1099
		private static List<InputDevice> devices = new List<InputDevice>();

		// Token: 0x0400044C RID: 1100
		private static List<PlayerActionSet> playerActionSets = new List<PlayerActionSet>();

		// Token: 0x0400044D RID: 1101
		public static ReadOnlyCollection<InputDevice> Devices;

		// Token: 0x04000452 RID: 1106
		private static bool applicationIsFocused;

		// Token: 0x04000453 RID: 1107
		private static float initialTime;

		// Token: 0x04000454 RID: 1108
		private static float currentTime;

		// Token: 0x04000455 RID: 1109
		private static float lastUpdateTime;

		// Token: 0x04000456 RID: 1110
		private static ulong currentTick;

		// Token: 0x04000457 RID: 1111
		private static VersionInfo? unityVersion;

		// Token: 0x04000458 RID: 1112
		private static bool enabled;
	}
}
