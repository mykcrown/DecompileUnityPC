// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace InControl
{
	public class InControlManager : SingletonMonoBehavior<InControlManager, MonoBehaviour>
	{
		public bool logDebugInfo;

		public bool invertYAxis;

		public bool useFixedUpdate;

		public bool dontDestroyOnLoad;

		public bool suspendInBackground = true;

		public bool enableICade;

		public bool enableXInput;

		public bool xInputOverrideUpdateRate;

		public int xInputUpdateRate;

		public bool xInputOverrideBufferSize;

		public int xInputBufferSize;

		public bool enableNativeInput;

		public bool nativeInputEnableXInput = true;

		public bool nativeInputPreventSleep;

		public bool nativeInputOverrideUpdateRate;

		public int nativeInputUpdateRate;

		public List<string> customProfiles = new List<string>();

		private static Action<LogMessage> __f__mg_cache0;

		private static Action<LogMessage> __f__mg_cache1;

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
					if (InControlManager.__f__mg_cache0 == null)
					{
						InControlManager.__f__mg_cache0 = new Action<LogMessage>(InControlManager.LogMessage);
					}
					Logger.OnLogMessage -= InControlManager.__f__mg_cache0;
					if (InControlManager.__f__mg_cache1 == null)
					{
						InControlManager.__f__mg_cache1 = new Action<LogMessage>(InControlManager.LogMessage);
					}
					Logger.OnLogMessage += InControlManager.__f__mg_cache1;
				}
				foreach (string current in this.customProfiles)
				{
					Type type = Type.GetType(current);
					if (type == null)
					{
						UnityEngine.Debug.LogError("Cannot find class for custom profile: " + current);
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
			SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.OnSceneWasLoaded);
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneWasLoaded);
			if (this.dontDestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(this);
			}
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.OnSceneWasLoaded);
			if (SingletonMonoBehavior<InControlManager, MonoBehaviour>.Instance == this)
			{
				InputManager.ResetInternal();
			}
		}

		private void Update()
		{
			if (!this.useFixedUpdate || Utility.IsZero(Time.timeScale))
			{
				InputManager.UpdateInternal();
			}
		}

		private void FixedUpdate()
		{
			if (this.useFixedUpdate)
			{
				InputManager.UpdateInternal();
			}
		}

		private void OnApplicationFocus(bool focusState)
		{
			InputManager.OnApplicationFocus(focusState);
		}

		private void OnApplicationPause(bool pauseState)
		{
			InputManager.OnApplicationPause(pauseState);
		}

		private void OnApplicationQuit()
		{
			InputManager.OnApplicationQuit();
		}

		private void OnSceneWasLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			InputManager.OnLevelWasLoaded();
		}

		private static void LogMessage(LogMessage logMessage)
		{
			LogMessageType type = logMessage.type;
			if (type != LogMessageType.Info)
			{
				if (type != LogMessageType.Warning)
				{
					if (type == LogMessageType.Error)
					{
						UnityEngine.Debug.LogError(logMessage.text);
					}
				}
				else
				{
					UnityEngine.Debug.LogWarning(logMessage.text);
				}
			}
			else
			{
				UnityEngine.Debug.Log(logMessage.text);
			}
		}
	}
}
