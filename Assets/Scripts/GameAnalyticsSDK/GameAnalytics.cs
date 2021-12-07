// Decompile from assembly: Assembly-CSharp.dll

using Beebyte.Obfuscator;
using GameAnalyticsSDK.Events;
using GameAnalyticsSDK.Net;
using GameAnalyticsSDK.Setup;
using GameAnalyticsSDK.State;
using GameAnalyticsSDK.Wrapper;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace GameAnalyticsSDK
{
	[Skip, ExecuteInEditMode, RequireComponent(typeof(GA_SpecialEvents))]
	public class GameAnalytics : MonoBehaviour
	{
		private static Settings _settings;

		private static GameAnalytics _instance;

		private static bool _hasInitializeBeenCalled;

		private static Application.LogCallback __f__mg_cache0;

		public static event Action OnCommandCenterUpdatedEvent;

		public static Settings SettingsGA
		{
			get
			{
				if (GameAnalytics._settings == null)
				{
					GameAnalytics.InitAPI();
				}
				return GameAnalytics._settings;
			}
			private set
			{
				GameAnalytics._settings = value;
			}
		}

		public void Awake()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (GameAnalytics._instance != null)
			{
				UnityEngine.Debug.LogWarning("Destroying duplicate GameAnalytics object - only one is allowed per scene!");
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			GameAnalytics._instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			if (GameAnalytics.__f__mg_cache0 == null)
			{
				GameAnalytics.__f__mg_cache0 = new Application.LogCallback(GA_Debug.HandleLog);
			}
			Application.logMessageReceived += GameAnalytics.__f__mg_cache0;
		}

		private void OnDestroy()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (GameAnalytics._instance == this)
			{
				GameAnalytics._instance = null;
			}
		}

		private void OnApplicationQuit()
		{
			GameAnalyticsSDK.Net.GameAnalytics.OnQuit();
			Thread.Sleep(1500);
		}

		private static void InitAPI()
		{
			try
			{
				GameAnalytics._settings = (Settings)Resources.Load("GameAnalytics/Settings", typeof(Settings));
				GAState.Init();
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("Error getting Settings in InitAPI: " + ex.Message);
			}
		}

		private static void InternalInitialize()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (GameAnalytics.SettingsGA.InfoLogBuild)
			{
				GA_Setup.SetInfoLog(true);
			}
			if (GameAnalytics.SettingsGA.VerboseLogBuild)
			{
				GA_Setup.SetVerboseLog(true);
			}
			int platformIndex = GameAnalytics.GetPlatformIndex();
			GA_Wrapper.SetUnitySdkVersion("unity " + Settings.VERSION);
			GA_Wrapper.SetUnityEngineVersion("unity " + GameAnalytics.GetUnityVersion());
			if (platformIndex >= 0)
			{
				if (GameAnalytics.SettingsGA.UsePlayerSettingsBuildNumber)
				{
					for (int i = 0; i < GameAnalytics.SettingsGA.Platforms.Count; i++)
					{
						if (GameAnalytics.SettingsGA.Platforms[i] == RuntimePlatform.Android || GameAnalytics.SettingsGA.Platforms[i] == RuntimePlatform.IPhonePlayer)
						{
							GameAnalytics.SettingsGA.Build[i] = Application.version;
						}
					}
				}
				GA_Wrapper.SetBuild(GameAnalytics.SettingsGA.Build[platformIndex]);
			}
			if (GameAnalytics.SettingsGA.CustomDimensions01.Count > 0)
			{
				GA_Setup.SetAvailableCustomDimensions01(GameAnalytics.SettingsGA.CustomDimensions01);
			}
			if (GameAnalytics.SettingsGA.CustomDimensions02.Count > 0)
			{
				GA_Setup.SetAvailableCustomDimensions02(GameAnalytics.SettingsGA.CustomDimensions02);
			}
			if (GameAnalytics.SettingsGA.CustomDimensions03.Count > 0)
			{
				GA_Setup.SetAvailableCustomDimensions03(GameAnalytics.SettingsGA.CustomDimensions03);
			}
			if (GameAnalytics.SettingsGA.ResourceItemTypes.Count > 0)
			{
				GA_Setup.SetAvailableResourceItemTypes(GameAnalytics.SettingsGA.ResourceItemTypes);
			}
			if (GameAnalytics.SettingsGA.ResourceCurrencies.Count > 0)
			{
				GA_Setup.SetAvailableResourceCurrencies(GameAnalytics.SettingsGA.ResourceCurrencies);
			}
			if (GameAnalytics.SettingsGA.UseManualSessionHandling)
			{
				GameAnalytics.SetEnabledManualSessionHandling(true);
			}
		}

		public static void Initialize()
		{
			GameAnalytics.InternalInitialize();
			int platformIndex = GameAnalytics.GetPlatformIndex();
			if (platformIndex >= 0)
			{
				GA_Wrapper.Initialize(GameAnalytics.SettingsGA.GetGameKey(platformIndex), GameAnalytics.SettingsGA.GetSecretKey(platformIndex));
				GameAnalytics._hasInitializeBeenCalled = true;
			}
			else
			{
				GameAnalytics._hasInitializeBeenCalled = true;
				UnityEngine.Debug.LogWarning("GameAnalytics: Unsupported platform (events will not be sent in editor; or missing platform in settings): " + Application.platform);
			}
		}

		public static void NewBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Business.NewEvent(currency, amount, itemType, itemId, cartType, null);
		}

		public static void NewDesignEvent(string eventName)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Design.NewEvent(eventName, null);
		}

		public static void NewDesignEvent(string eventName, float eventValue)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Design.NewEvent(eventName, eventValue, null);
		}

		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Progression.NewEvent(progressionStatus, progression01, null);
		}

		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, null);
		}

		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, progression03, null);
		}

		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, int score)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Progression.NewEvent(progressionStatus, progression01, score, null);
		}

		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, int score)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, score, null);
		}

		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, progression03, score, null);
		}

		public static void NewResourceEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Resource.NewEvent(flowType, currency, amount, itemType, itemId, null);
		}

		public static void NewErrorEvent(GAErrorSeverity severity, string message)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Error.NewEvent(severity, message, null);
		}

		public static void SetFacebookId(string facebookId)
		{
			GA_Setup.SetFacebookId(facebookId);
		}

		public static void SetGender(GAGender gender)
		{
			GA_Setup.SetGender(gender);
		}

		public static void SetBirthYear(int birthYear)
		{
			GA_Setup.SetBirthYear(birthYear);
		}

		public static void SetCustomId(string userId)
		{
			UnityEngine.Debug.Log("Initializing with custom id: " + userId);
			GA_Wrapper.SetCustomUserId(userId);
		}

		public static void SetEnabledManualSessionHandling(bool enabled)
		{
			GA_Wrapper.SetEnabledManualSessionHandling(enabled);
		}

		public static void SetEnabledEventSubmission(bool enabled)
		{
			GA_Wrapper.SetEnabledEventSubmission(enabled);
		}

		public static void StartSession()
		{
			GA_Wrapper.StartSession();
		}

		public static void EndSession()
		{
			GA_Wrapper.EndSession();
		}

		public static void SetCustomDimension01(string customDimension)
		{
			GA_Setup.SetCustomDimension01(customDimension);
		}

		public static void SetCustomDimension02(string customDimension)
		{
			GA_Setup.SetCustomDimension02(customDimension);
		}

		public static void SetCustomDimension03(string customDimension)
		{
			GA_Setup.SetCustomDimension03(customDimension);
		}

		public void OnCommandCenterUpdated()
		{
			if (GameAnalytics.OnCommandCenterUpdatedEvent != null)
			{
				GameAnalytics.OnCommandCenterUpdatedEvent();
			}
		}

		public static void CommandCenterUpdated()
		{
			if (GameAnalytics.OnCommandCenterUpdatedEvent != null)
			{
				GameAnalytics.OnCommandCenterUpdatedEvent();
			}
		}

		public static string GetCommandCenterValueAsString(string key)
		{
			return GameAnalytics.GetCommandCenterValueAsString(key, null);
		}

		public static string GetCommandCenterValueAsString(string key, string defaultValue)
		{
			return GA_Wrapper.GetCommandCenterValueAsString(key, defaultValue);
		}

		public static bool IsCommandCenterReady()
		{
			return GA_Wrapper.IsCommandCenterReady();
		}

		public static string GetConfigurationsContentAsString()
		{
			return GA_Wrapper.GetConfigurationsContentAsString();
		}

		private static string GetUnityVersion()
		{
			string text = string.Empty;
			string[] array = Application.unityVersion.Split(new char[]
			{
				'.'
			});
			for (int i = 0; i < array.Length; i++)
			{
				int num;
				if (int.TryParse(array[i], out num))
				{
					if (i == 0)
					{
						text = array[i];
					}
					else
					{
						text = text + "." + array[i];
					}
				}
				else
				{
					string[] array2 = Regex.Split(array[i], "[^\\d]+");
					if (array2.Length > 0 && int.TryParse(array2[0], out num))
					{
						text = text + "." + array2[0];
					}
				}
			}
			return text;
		}

		private static int GetPlatformIndex()
		{
			RuntimePlatform platform = Application.platform;
			int result;
			if (platform == RuntimePlatform.IPhonePlayer)
			{
				if (!GameAnalytics.SettingsGA.Platforms.Contains(platform))
				{
					result = GameAnalytics.SettingsGA.Platforms.IndexOf(RuntimePlatform.tvOS);
				}
				else
				{
					result = GameAnalytics.SettingsGA.Platforms.IndexOf(platform);
				}
			}
			else if (platform == RuntimePlatform.tvOS)
			{
				if (!GameAnalytics.SettingsGA.Platforms.Contains(platform))
				{
					result = GameAnalytics.SettingsGA.Platforms.IndexOf(RuntimePlatform.IPhonePlayer);
				}
				else
				{
					result = GameAnalytics.SettingsGA.Platforms.IndexOf(platform);
				}
			}
			else if (platform == RuntimePlatform.MetroPlayerARM || platform == RuntimePlatform.MetroPlayerX64 || platform == RuntimePlatform.MetroPlayerX86 || platform == RuntimePlatform.MetroPlayerARM || platform == RuntimePlatform.MetroPlayerX64 || platform == RuntimePlatform.MetroPlayerX86)
			{
				result = GameAnalytics.SettingsGA.Platforms.IndexOf(RuntimePlatform.MetroPlayerARM);
			}
			else
			{
				result = GameAnalytics.SettingsGA.Platforms.IndexOf(platform);
			}
			return result;
		}

		public static void SetBuildAllPlatforms(string build)
		{
			for (int i = 0; i < GameAnalytics.SettingsGA.Build.Count; i++)
			{
				GameAnalytics.SettingsGA.Build[i] = build;
			}
		}
	}
}
