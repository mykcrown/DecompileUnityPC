using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using Beebyte.Obfuscator;
using GameAnalyticsSDK.Events;
using GameAnalyticsSDK.Net;
using GameAnalyticsSDK.Setup;
using GameAnalyticsSDK.State;
using GameAnalyticsSDK.Wrapper;
using UnityEngine;

namespace GameAnalyticsSDK
{
	// Token: 0x02000030 RID: 48
	[RequireComponent(typeof(GA_SpecialEvents))]
	[ExecuteInEditMode]
	[Skip]
	public class GameAnalytics : MonoBehaviour
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000D420 File Offset: 0x0000B820
		// (set) Token: 0x0600018A RID: 394 RVA: 0x0000D43C File Offset: 0x0000B83C
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

		// Token: 0x0600018B RID: 395 RVA: 0x0000D444 File Offset: 0x0000B844
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
			if (GameAnalytics.f__mg_cache0 == null)
			{
				GameAnalytics.f__mg_cache0 = new Application.LogCallback(GA_Debug.HandleLog);
			}
			Application.logMessageReceived += GameAnalytics.f__mg_cache0;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000D4B5 File Offset: 0x0000B8B5
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

		// Token: 0x0600018D RID: 397 RVA: 0x0000D4D8 File Offset: 0x0000B8D8
		private void OnApplicationQuit()
		{
			GameAnalytics.OnQuit();
			Thread.Sleep(1500);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000D4EC File Offset: 0x0000B8EC
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

		// Token: 0x0600018F RID: 399 RVA: 0x0000D54C File Offset: 0x0000B94C
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

		// Token: 0x06000190 RID: 400 RVA: 0x0000D714 File Offset: 0x0000BB14
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

		// Token: 0x06000191 RID: 401 RVA: 0x0000D778 File Offset: 0x0000BB78
		public static void NewBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Business.NewEvent(currency, amount, itemType, itemId, cartType, null);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000D79B File Offset: 0x0000BB9B
		public static void NewDesignEvent(string eventName)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Design.NewEvent(eventName, null);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000D7B9 File Offset: 0x0000BBB9
		public static void NewDesignEvent(string eventName, float eventValue)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Design.NewEvent(eventName, eventValue, null);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000D7D8 File Offset: 0x0000BBD8
		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Progression.NewEvent(progressionStatus, progression01, null);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000D7F7 File Offset: 0x0000BBF7
		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, null);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000D817 File Offset: 0x0000BC17
		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, progression03, null);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000D838 File Offset: 0x0000BC38
		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, int score)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Progression.NewEvent(progressionStatus, progression01, score, null);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000D858 File Offset: 0x0000BC58
		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, int score)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, score, null);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000D879 File Offset: 0x0000BC79
		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, progression03, score, null);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000D89C File Offset: 0x0000BC9C
		public static void NewResourceEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Resource.NewEvent(flowType, currency, amount, itemType, itemId, null);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000D8BF File Offset: 0x0000BCBF
		public static void NewErrorEvent(GAErrorSeverity severity, string message)
		{
			if (!GameAnalytics._hasInitializeBeenCalled)
			{
				UnityEngine.Debug.LogError("GameAnalytics: REMEMBER THE SDK NEEDS TO BE MANUALLY INITIALIZED NOW");
				return;
			}
			GA_Error.NewEvent(severity, message, null);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000D8DE File Offset: 0x0000BCDE
		public static void SetFacebookId(string facebookId)
		{
			GA_Setup.SetFacebookId(facebookId);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000D8E6 File Offset: 0x0000BCE6
		public static void SetGender(GAGender gender)
		{
			GA_Setup.SetGender(gender);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000D8EE File Offset: 0x0000BCEE
		public static void SetBirthYear(int birthYear)
		{
			GA_Setup.SetBirthYear(birthYear);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000D8F6 File Offset: 0x0000BCF6
		public static void SetCustomId(string userId)
		{
			UnityEngine.Debug.Log("Initializing with custom id: " + userId);
			GA_Wrapper.SetCustomUserId(userId);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000D90E File Offset: 0x0000BD0E
		public static void SetEnabledManualSessionHandling(bool enabled)
		{
			GA_Wrapper.SetEnabledManualSessionHandling(enabled);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000D916 File Offset: 0x0000BD16
		public static void SetEnabledEventSubmission(bool enabled)
		{
			GA_Wrapper.SetEnabledEventSubmission(enabled);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000D91E File Offset: 0x0000BD1E
		public static void StartSession()
		{
			GA_Wrapper.StartSession();
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000D925 File Offset: 0x0000BD25
		public static void EndSession()
		{
			GA_Wrapper.EndSession();
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000D92C File Offset: 0x0000BD2C
		public static void SetCustomDimension01(string customDimension)
		{
			GA_Setup.SetCustomDimension01(customDimension);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000D934 File Offset: 0x0000BD34
		public static void SetCustomDimension02(string customDimension)
		{
			GA_Setup.SetCustomDimension02(customDimension);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000D93C File Offset: 0x0000BD3C
		public static void SetCustomDimension03(string customDimension)
		{
			GA_Setup.SetCustomDimension03(customDimension);
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060001A7 RID: 423 RVA: 0x0000D944 File Offset: 0x0000BD44
		// (remove) Token: 0x060001A8 RID: 424 RVA: 0x0000D978 File Offset: 0x0000BD78
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnCommandCenterUpdatedEvent;

		// Token: 0x060001A9 RID: 425 RVA: 0x0000D9AC File Offset: 0x0000BDAC
		public void OnCommandCenterUpdated()
		{
			if (GameAnalytics.OnCommandCenterUpdatedEvent != null)
			{
				GameAnalytics.OnCommandCenterUpdatedEvent();
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000D9C2 File Offset: 0x0000BDC2
		public static void CommandCenterUpdated()
		{
			if (GameAnalytics.OnCommandCenterUpdatedEvent != null)
			{
				GameAnalytics.OnCommandCenterUpdatedEvent();
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000D9D8 File Offset: 0x0000BDD8
		public static string GetCommandCenterValueAsString(string key)
		{
			return GameAnalytics.GetCommandCenterValueAsString(key, null);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000D9E1 File Offset: 0x0000BDE1
		public static string GetCommandCenterValueAsString(string key, string defaultValue)
		{
			return GA_Wrapper.GetCommandCenterValueAsString(key, defaultValue);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000D9EA File Offset: 0x0000BDEA
		public static bool IsCommandCenterReady()
		{
			return GA_Wrapper.IsCommandCenterReady();
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000D9F1 File Offset: 0x0000BDF1
		public static string GetConfigurationsContentAsString()
		{
			return GA_Wrapper.GetConfigurationsContentAsString();
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000D9F8 File Offset: 0x0000BDF8
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

		// Token: 0x060001B0 RID: 432 RVA: 0x0000DAA4 File Offset: 0x0000BEA4
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

		// Token: 0x060001B1 RID: 433 RVA: 0x0000DBA4 File Offset: 0x0000BFA4
		public static void SetBuildAllPlatforms(string build)
		{
			for (int i = 0; i < GameAnalytics.SettingsGA.Build.Count; i++)
			{
				GameAnalytics.SettingsGA.Build[i] = build;
			}
		}

		// Token: 0x04000129 RID: 297
		private static Settings _settings;

		// Token: 0x0400012A RID: 298
		private static GameAnalytics _instance;

		// Token: 0x0400012B RID: 299
		private static bool _hasInitializeBeenCalled;

		// Token: 0x0400012D RID: 301
		[CompilerGenerated]
		private static Application.LogCallback f__mg_cache0;
	}
}
