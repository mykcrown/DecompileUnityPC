// Decompile from assembly: Assembly-CSharp.dll

using GameAnalyticsSDK.Net;
using GameAnalyticsSDK.State;
using GameAnalyticsSDK.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAnalyticsSDK.Wrapper
{
	public class GA_Wrapper
	{
		private class UnityCommandCenterListener : ICommandCenterListener
		{
			public void OnCommandCenterUpdated()
			{
				GameAnalyticsSDK.GameAnalytics.CommandCenterUpdated();
			}
		}

		private static readonly GA_Wrapper.UnityCommandCenterListener unityCommandCenterListener = new GA_Wrapper.UnityCommandCenterListener();

		private static void configureAvailableCustomDimensions01(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object current in list2)
			{
				arrayList.Add(current);
			}
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureAvailableCustomDimensions01((string[])arrayList.ToArray(typeof(string)));
		}

		private static void configureAvailableCustomDimensions02(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object current in list2)
			{
				arrayList.Add(current);
			}
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureAvailableCustomDimensions02((string[])arrayList.ToArray(typeof(string)));
		}

		private static void configureAvailableCustomDimensions03(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object current in list2)
			{
				arrayList.Add(current);
			}
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureAvailableCustomDimensions03((string[])arrayList.ToArray(typeof(string)));
		}

		private static void configureAvailableResourceCurrencies(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object current in list2)
			{
				arrayList.Add(current);
			}
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureAvailableResourceCurrencies((string[])arrayList.ToArray(typeof(string)));
		}

		private static void configureAvailableResourceItemTypes(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object current in list2)
			{
				arrayList.Add(current);
			}
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureAvailableResourceItemTypes((string[])arrayList.ToArray(typeof(string)));
		}

		private static void configureSdkGameEngineVersion(string unitySdkVersion)
		{
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureSdkGameEngineVersion(unitySdkVersion);
		}

		private static void configureGameEngineVersion(string unityEngineVersion)
		{
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureGameEngineVersion(unityEngineVersion);
		}

		private static void configureBuild(string build)
		{
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureBuild(build);
		}

		private static void configureUserId(string userId)
		{
			GameAnalyticsSDK.Net.GameAnalytics.ConfigureUserId(userId);
		}

		private static void initialize(string gamekey, string gamesecret)
		{
			GameAnalyticsSDK.Net.GameAnalytics.AddCommandCenterListener(GA_Wrapper.unityCommandCenterListener);
			GameAnalyticsSDK.Net.GameAnalytics.Initialize(gamekey, gamesecret);
		}

		private static void setCustomDimension01(string customDimension)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetCustomDimension01(customDimension);
		}

		private static void setCustomDimension02(string customDimension)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetCustomDimension02(customDimension);
		}

		private static void setCustomDimension03(string customDimension)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetCustomDimension03(customDimension);
		}

		private static void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, string fields)
		{
			GameAnalyticsSDK.Net.GameAnalytics.AddBusinessEvent(currency, amount, itemType, itemId, cartType);
		}

		private static void addResourceEvent(int flowType, string currency, float amount, string itemType, string itemId, string fields)
		{
			GameAnalyticsSDK.Net.GameAnalytics.AddResourceEvent((EGAResourceFlowType)flowType, currency, amount, itemType, itemId);
		}

		private static void addProgressionEvent(int progressionStatus, string progression01, string progression02, string progression03, string fields)
		{
			GameAnalyticsSDK.Net.GameAnalytics.AddProgressionEvent((EGAProgressionStatus)progressionStatus, progression01, progression02, progression03);
		}

		private static void addProgressionEventWithScore(int progressionStatus, string progression01, string progression02, string progression03, int score, string fields)
		{
			GameAnalyticsSDK.Net.GameAnalytics.AddProgressionEvent((EGAProgressionStatus)progressionStatus, progression01, progression02, progression03, (double)score);
		}

		private static void addDesignEvent(string eventId, string fields)
		{
			GameAnalyticsSDK.Net.GameAnalytics.AddDesignEvent(eventId, null);
		}

		private static void addDesignEventWithValue(string eventId, float value, string fields)
		{
			GameAnalyticsSDK.Net.GameAnalytics.AddDesignEvent(eventId, (double)value);
		}

		private static void addErrorEvent(int severity, string message, string fields)
		{
			GameAnalyticsSDK.Net.GameAnalytics.AddErrorEvent((EGAErrorSeverity)severity, message);
		}

		private static void setEnabledInfoLog(bool enabled)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetEnabledInfoLog(enabled);
		}

		private static void setEnabledVerboseLog(bool enabled)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetEnabledVerboseLog(enabled);
		}

		private static void setManualSessionHandling(bool enabled)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetEnabledManualSessionHandling(enabled);
		}

		private static void setEventSubmission(bool enabled)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetEnabledManualSessionHandling(enabled);
		}

		private static void gameAnalyticsStartSession()
		{
			GameAnalyticsSDK.Net.GameAnalytics.StartSession();
		}

		private static void gameAnalyticsEndSession()
		{
			GameAnalyticsSDK.Net.GameAnalytics.EndSession();
		}

		private static void setFacebookId(string facebookId)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetFacebookId(facebookId);
		}

		private static void setGender(string gender)
		{
			if (gender != null)
			{
				if (!(gender == "male"))
				{
					if (gender == "female")
					{
						GameAnalyticsSDK.Net.GameAnalytics.SetGender(EGAGender.Female);
					}
				}
				else
				{
					GameAnalyticsSDK.Net.GameAnalytics.SetGender(EGAGender.Male);
				}
			}
		}

		private static void setBirthYear(int birthYear)
		{
			GameAnalyticsSDK.Net.GameAnalytics.SetBirthYear(birthYear);
		}

		private static string getCommandCenterValueAsString(string key, string defaultValue)
		{
			return GameAnalyticsSDK.Net.GameAnalytics.GetCommandCenterValueAsString(key, defaultValue);
		}

		private static bool isCommandCenterReady()
		{
			return GameAnalyticsSDK.Net.GameAnalytics.IsCommandCenterReady();
		}

		private static string getConfigurationsContentAsString()
		{
			return GameAnalyticsSDK.Net.GameAnalytics.GetConfigurationsAsString();
		}

		public static void SetAvailableCustomDimensions01(string list)
		{
			GA_Wrapper.configureAvailableCustomDimensions01(list);
		}

		public static void SetAvailableCustomDimensions02(string list)
		{
			GA_Wrapper.configureAvailableCustomDimensions02(list);
		}

		public static void SetAvailableCustomDimensions03(string list)
		{
			GA_Wrapper.configureAvailableCustomDimensions03(list);
		}

		public static void SetAvailableResourceCurrencies(string list)
		{
			GA_Wrapper.configureAvailableResourceCurrencies(list);
		}

		public static void SetAvailableResourceItemTypes(string list)
		{
			GA_Wrapper.configureAvailableResourceItemTypes(list);
		}

		public static void SetUnitySdkVersion(string unitySdkVersion)
		{
			GA_Wrapper.configureSdkGameEngineVersion(unitySdkVersion);
		}

		public static void SetUnityEngineVersion(string unityEngineVersion)
		{
			GA_Wrapper.configureGameEngineVersion(unityEngineVersion);
		}

		public static void SetBuild(string build)
		{
			GA_Wrapper.configureBuild(build);
		}

		public static void SetCustomUserId(string userId)
		{
			GA_Wrapper.configureUserId(userId);
		}

		public static void SetEnabledManualSessionHandling(bool enabled)
		{
			GA_Wrapper.setManualSessionHandling(enabled);
		}

		public static void SetEnabledEventSubmission(bool enabled)
		{
			GA_Wrapper.setEventSubmission(enabled);
		}

		public static void StartSession()
		{
			if (GAState.IsManualSessionHandlingEnabled())
			{
				GA_Wrapper.gameAnalyticsStartSession();
			}
			else
			{
				UnityEngine.Debug.Log("Manual session handling is not enabled. \nPlease check the \"Use manual session handling\" option in the \"Advanced\" section of the Settings object.");
			}
		}

		public static void EndSession()
		{
			if (GAState.IsManualSessionHandlingEnabled())
			{
				GA_Wrapper.gameAnalyticsEndSession();
			}
			else
			{
				UnityEngine.Debug.Log("Manual session handling is not enabled. \nPlease check the \"Use manual session handling\" option in the \"Advanced\" section of the Settings object.");
			}
		}

		public static void Initialize(string gamekey, string gamesecret)
		{
			GA_Wrapper.initialize(gamekey, gamesecret);
		}

		public static void SetCustomDimension01(string customDimension)
		{
			GA_Wrapper.setCustomDimension01(customDimension);
		}

		public static void SetCustomDimension02(string customDimension)
		{
			GA_Wrapper.setCustomDimension02(customDimension);
		}

		public static void SetCustomDimension03(string customDimension)
		{
			GA_Wrapper.setCustomDimension03(customDimension);
		}

		public static void AddBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, IDictionary<string, object> fields)
		{
			string fields2 = GA_Wrapper.DictionaryToJsonString(fields);
			GA_Wrapper.addBusinessEvent(currency, amount, itemType, itemId, cartType, fields2);
		}

		public static void AddResourceEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId, IDictionary<string, object> fields)
		{
			string fields2 = GA_Wrapper.DictionaryToJsonString(fields);
			GA_Wrapper.addResourceEvent((int)flowType, currency, amount, itemType, itemId, fields2);
		}

		public static void AddProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, IDictionary<string, object> fields)
		{
			string fields2 = GA_Wrapper.DictionaryToJsonString(fields);
			GA_Wrapper.addProgressionEvent((int)progressionStatus, progression01, progression02, progression03, fields2);
		}

		public static void AddProgressionEventWithScore(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score, IDictionary<string, object> fields)
		{
			string fields2 = GA_Wrapper.DictionaryToJsonString(fields);
			GA_Wrapper.addProgressionEventWithScore((int)progressionStatus, progression01, progression02, progression03, score, fields2);
		}

		public static void AddDesignEvent(string eventID, float eventValue, IDictionary<string, object> fields)
		{
			string fields2 = GA_Wrapper.DictionaryToJsonString(fields);
			GA_Wrapper.addDesignEventWithValue(eventID, eventValue, fields2);
		}

		public static void AddDesignEvent(string eventID, IDictionary<string, object> fields)
		{
			string fields2 = GA_Wrapper.DictionaryToJsonString(fields);
			GA_Wrapper.addDesignEvent(eventID, fields2);
		}

		public static void AddErrorEvent(GAErrorSeverity severity, string message, IDictionary<string, object> fields)
		{
			string fields2 = GA_Wrapper.DictionaryToJsonString(fields);
			GA_Wrapper.addErrorEvent((int)severity, message, fields2);
		}

		public static void SetInfoLog(bool enabled)
		{
			GA_Wrapper.setEnabledInfoLog(enabled);
		}

		public static void SetVerboseLog(bool enabled)
		{
			GA_Wrapper.setEnabledVerboseLog(enabled);
		}

		public static void SetFacebookId(string facebookId)
		{
			GA_Wrapper.setFacebookId(facebookId);
		}

		public static void SetGender(string gender)
		{
			GA_Wrapper.setGender(gender);
		}

		public static void SetBirthYear(int birthYear)
		{
			GA_Wrapper.setBirthYear(birthYear);
		}

		public static string GetCommandCenterValueAsString(string key, string defaultValue)
		{
			return GA_Wrapper.getCommandCenterValueAsString(key, defaultValue);
		}

		public static bool IsCommandCenterReady()
		{
			return GA_Wrapper.isCommandCenterReady();
		}

		public static string GetConfigurationsContentAsString()
		{
			return GA_Wrapper.getConfigurationsContentAsString();
		}

		private static string DictionaryToJsonString(IDictionary<string, object> dict)
		{
			Hashtable hashtable = new Hashtable();
			if (dict != null)
			{
				foreach (KeyValuePair<string, object> current in dict)
				{
					hashtable.Add(current.Key, current.Value);
				}
			}
			return GA_MiniJSON.Serialize(hashtable);
		}
	}
}
