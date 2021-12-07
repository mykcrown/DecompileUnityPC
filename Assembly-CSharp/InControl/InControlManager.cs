using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InControl
{
	// Token: 0x02000068 RID: 104
	public class InControlManager : SingletonMonoBehavior<InControlManager, MonoBehaviour>
	{
		// Token: 0x060003AB RID: 939 RVA: 0x00018440 File Offset: 0x00016840
		public void Init()
		{
			if (!base.EnforceSingleton())
			{
				return;
			}
			InputManager.InvertYAxis = this.invertYAxis;
			InputManager.SuspendInBackground = this.suspendInBackground;
			InputManager.EnableICade = this.enableICade;
			InputManager.EnableXInput = this.enableXInput;
			InputManager.XInputUpdateRate = (uint)Mathf.Max(this.xInputUpdateRate, 0);
			InputManager.XInputBufferSize = (uint)Mathf.Max(this.xInputBufferSize, 0);
			InputManager.EnableNativeInput = this.enableNativeInput;
			InputManager.NativeInputEnableXInput = this.nativeInputEnableXInput;
			InputManager.NativeInputUpdateRate = (uint)Mathf.Max(this.nativeInputUpdateRate, 0);
			InputManager.NativeInputPreventSleep = this.nativeInputPreventSleep;
			if (InputManager.SetupInternal())
			{
				if (this.logDebugInfo)
				{
					if (InControlManager.f__mg_cache0 == null)
					{
						InControlManager.f__mg_cache0 = new Action<LogMessage>(InControlManager.LogMessage);
					}
					Logger.OnLogMessage -= InControlManager.f__mg_cache0;
					if (InControlManager.f__mg_cache1 == null)
					{
						InControlManager.f__mg_cache1 = new Action<LogMessage>(InControlManager.LogMessage);
					}
					Logger.OnLogMessage += InControlManager.f__mg_cache1;
				}
				foreach (string text in this.customProfiles)
				{
					Type type = Type.GetType(text);
					if (type == null)
					{
						Debug.LogError("Cannot find class for custom profile: " + text);
					}
					else
					{
						UnityInputDeviceProfileBase unityInputDeviceProfileBase = Activator.CreateInstance(type) as UnityInputDeviceProfileBase;
						if (unityInputDeviceProfileBase != null)
						{
							InputManager.AttachDevice(new UnityInputDevice(unityInputDeviceProfileBase));
						}
					}
				}
			}
			SceneManager.sceneLoaded -= this.OnSceneWasLoaded;
			SceneManager.sceneLoaded += this.OnSceneWasLoaded;
			if (this.dontDestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(this);
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000185F4 File Offset: 0x000169F4
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.OnSceneWasLoaded;
			if (SingletonMonoBehavior<InControlManager, MonoBehaviour>.Instance == this)
			{
				InputManager.ResetInternal();
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001861C File Offset: 0x00016A1C
		private void Update()
		{
			if (!this.useFixedUpdate || Utility.IsZero(Time.timeScale))
			{
				InputManager.UpdateInternal();
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001863D File Offset: 0x00016A3D
		private void FixedUpdate()
		{
			if (this.useFixedUpdate)
			{
				InputManager.UpdateInternal();
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0001864F File Offset: 0x00016A4F
		private void OnApplicationFocus(bool focusState)
		{
			InputManager.OnApplicationFocus(focusState);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00018657 File Offset: 0x00016A57
		private void OnApplicationPause(bool pauseState)
		{
			InputManager.OnApplicationPause(pauseState);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001865F File Offset: 0x00016A5F
		private void OnApplicationQuit()
		{
			InputManager.OnApplicationQuit();
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00018666 File Offset: 0x00016A66
		private void OnSceneWasLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			InputManager.OnLevelWasLoaded();
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00018670 File Offset: 0x00016A70
		private static void LogMessage(LogMessage logMessage)
		{
			LogMessageType type = logMessage.type;
			if (type != LogMessageType.Info)
			{
				if (type != LogMessageType.Warning)
				{
					if (type == LogMessageType.Error)
					{
						Debug.LogError(logMessage.text);
					}
				}
				else
				{
					Debug.LogWarning(logMessage.text);
				}
			}
			else
			{
				Debug.Log(logMessage.text);
			}
		}

		// Token: 0x040002DC RID: 732
		public bool logDebugInfo;

		// Token: 0x040002DD RID: 733
		public bool invertYAxis;

		// Token: 0x040002DE RID: 734
		public bool useFixedUpdate;

		// Token: 0x040002DF RID: 735
		public bool dontDestroyOnLoad;

		// Token: 0x040002E0 RID: 736
		public bool suspendInBackground = true;

		// Token: 0x040002E1 RID: 737
		public bool enableICade;

		// Token: 0x040002E2 RID: 738
		public bool enableXInput;

		// Token: 0x040002E3 RID: 739
		public bool xInputOverrideUpdateRate;

		// Token: 0x040002E4 RID: 740
		public int xInputUpdateRate;

		// Token: 0x040002E5 RID: 741
		public bool xInputOverrideBufferSize;

		// Token: 0x040002E6 RID: 742
		public int xInputBufferSize;

		// Token: 0x040002E7 RID: 743
		public bool enableNativeInput;

		// Token: 0x040002E8 RID: 744
		public bool nativeInputEnableXInput = true;

		// Token: 0x040002E9 RID: 745
		public bool nativeInputPreventSleep;

		// Token: 0x040002EA RID: 746
		public bool nativeInputOverrideUpdateRate;

		// Token: 0x040002EB RID: 747
		public int nativeInputUpdateRate;

		// Token: 0x040002EC RID: 748
		public List<string> customProfiles = new List<string>();

		// Token: 0x040002ED RID: 749
		[CompilerGenerated]
		private static Action<LogMessage> f__mg_cache0;

		// Token: 0x040002EE RID: 750
		[CompilerGenerated]
		private static Action<LogMessage> f__mg_cache1;
	}
}
