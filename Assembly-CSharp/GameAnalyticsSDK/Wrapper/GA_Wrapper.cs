using System;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK.Net;
using GameAnalyticsSDK.State;
using GameAnalyticsSDK.Utilities;
using UnityEngine;

namespace GameAnalyticsSDK.Wrapper
{
	// Token: 0x0200003D RID: 61
	public class GA_Wrapper
	{
		// Token: 0x060001F4 RID: 500 RVA: 0x0000EFB0 File Offset: 0x0000D3B0
		private static void configureAvailableCustomDimensions01(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object value in list2)
			{
				arrayList.Add(value);
			}
			GameAnalytics.ConfigureAvailableCustomDimensions01((string[])arrayList.ToArray(typeof(string)));
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000F034 File Offset: 0x0000D434
		private static void configureAvailableCustomDimensions02(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object value in list2)
			{
				arrayList.Add(value);
			}
			GameAnalytics.ConfigureAvailableCustomDimensions02((string[])arrayList.ToArray(typeof(string)));
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000F0B8 File Offset: 0x0000D4B8
		private static void configureAvailableCustomDimensions03(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object value in list2)
			{
				arrayList.Add(value);
			}
			GameAnalytics.ConfigureAvailableCustomDimensions03((string[])arrayList.ToArray(typeof(string)));
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000F13C File Offset: 0x0000D53C
		private static void configureAvailableResourceCurrencies(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object value in list2)
			{
				arrayList.Add(value);
			}
			GameAnalytics.ConfigureAvailableResourceCurrencies((string[])arrayList.ToArray(typeof(string)));
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000F1C0 File Offset: 0x0000D5C0
		private static void configureAvailableResourceItemTypes(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object value in list2)
			{
				arrayList.Add(value);
			}
			GameAnalytics.ConfigureAvailableResourceItemTypes((string[])arrayList.ToArray(typeof(string)));
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000F244 File Offset: 0x0000D644
		private static void configureSdkGameEngineVersion(string unitySdkVersion)
		{
			GameAnalytics.ConfigureSdkGameEngineVersion(unitySdkVersion);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000F24C File Offset: 0x0000D64C
		private static void configureGameEngineVersion(string unityEngineVersion)
		{
			GameAnalytics.ConfigureGameEngineVersion(unityEngineVersion);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000F254 File Offset: 0x0000D654
		private static void configureBuild(string build)
		{
			GameAnalytics.ConfigureBuild(build);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000F25C File Offset: 0x0000D65C
		private static void configureUserId(string userId)
		{
			GameAnalytics.ConfigureUserId(userId);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000F264 File Offset: 0x0000D664
		private static void initialize(string gamekey, string gamesecret)
		{
			GameAnalytics.AddCommandCenterListener(GA_Wrapper.unityCommandCenterListener);
			GameAnalytics.Initialize(gamekey, gamesecret);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000F277 File Offset: 0x0000D677
		private static void setCustomDimension01(string customDimension)
		{
			GameAnalytics.SetCustomDimension01(customDimension);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000F27F File Offset: 0x0000D67F
		private static void setCustomDimension02(string customDimension)
		{
			GameAnalytics.SetCustomDimension02(customDimension);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000F287 File Offset: 0x0000D687
		private static void setCustomDimension03(string customDimension)
		{
			GameAnalytics.SetCustomDimension03(customDimension);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000F28F File Offset: 0x0000D68F
		private static void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, string fields)
		{
			GameAnalytics.AddBusinessEvent(currency, amount, itemType, itemId, cartType);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000F29C File Offset: 0x0000D69C
		private static void addResourceEvent(int flowType, string currency, float amount, string itemType, string itemId, string fields)
		{
			GameAnalytics.AddResourceEvent((EGAResourceFlowType)flowType, currency, amount, itemType, itemId);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000F2A9 File Offset: 0x0000D6A9
		private static void addProgressionEvent(int progressionStatus, string progression01, string progression02, string progression03, string fields)
		{
			GameAnalytics.AddProgressionEvent((EGAProgressionStatus)progressionStatus, progression01, progression02, progression03);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000F2B4 File Offset: 0x0000D6B4
		private static void addProgressionEventWithScore(int progressionStatus, string progression01, string progression02, string progression03, int score, string fields)
		{
			GameAnalytics.AddProgressionEvent((EGAProgressionStatus)progressionStatus, progression01, progression02, progression03, (double)score);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000F2C2 File Offset: 0x0000D6C2
		private static void addDesignEvent(string eventId, string fields)
		{
			GameAnalytics.AddDesignEvent(eventId, null);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000F2CB File Offset: 0x0000D6CB
		private static void addDesignEventWithValue(string eventId, float value, string fields)
		{
			GameAnalytics.AddDesignEvent(eventId, (double)value);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000F2D5 File Offset: 0x0000D6D5
		private static void addErrorEvent(int severity, string message, string fields)
		{
			GameAnalytics.AddErrorEvent((EGAErrorSeverity)severity, message);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000F2DE File Offset: 0x0000D6DE
		private static void setEnabledInfoLog(bool enabled)
		{
			GameAnalytics.SetEnabledInfoLog(enabled);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000F2E6 File Offset: 0x0000D6E6
		private static void setEnabledVerboseLog(bool enabled)
		{
			GameAnalytics.SetEnabledVerboseLog(enabled);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000F2EE File Offset: 0x0000D6EE
		private static void setManualSessionHandling(bool enabled)
		{
			GameAnalytics.SetEnabledManualSessionHandling(enabled);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000F2F6 File Offset: 0x0000D6F6
		private static void setEventSubmission(bool enabled)
		{
			GameAnalytics.SetEnabledManualSessionHandling(enabled);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000F2FE File Offset: 0x0000D6FE
		private static void gameAnalyticsStartSession()
		{
			GameAnalytics.StartSession();
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000F305 File Offset: 0x0000D705
		private static void gameAnalyticsEndSession()
		{
			GameAnalytics.EndSession();
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000F30C File Offset: 0x0000D70C
		private static void setFacebookId(string facebookId)
		{
			GameAnalytics.SetFacebookId(facebookId);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000F314 File Offset: 0x0000D714
		private static void setGender(string gender)
		{
			if (gender != null)
			{
				if (!(gender == "male"))
				{
					if (gender == "female")
					{
						GameAnalytics.SetGender(EGAGender.Female);
					}
				}
				else
				{
					GameAnalytics.SetGender(EGAGender.Male);
				}
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000F362 File Offset: 0x0000D762
		private static void setBirthYear(int birthYear)
		{
			GameAnalytics.SetBirthYear(birthYear);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000F36A File Offset: 0x0000D76A
		private static string getCommandCenterValueAsString(string key, string defaultValue)
		{
			return GameAnalytics.GetCommandCenterValueAsString(key, defaultValue);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000F373 File Offset: 0x0000D773
		private static bool isCommandCenterReady()
		{
			return GameAnalytics.IsCommandCenterReady();
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000F37A File Offset: 0x0000D77A
		private static string getConfigurationsContentAsString()
		{
			return GameAnalytics.GetConfigurationsAsString();
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000F381 File Offset: 0x0000D781
		public static void SetAvailableCustomDimensions01(string list)
		{
			GA_Wrapper.configureAvailableCustomDimensions01(list);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000F389 File Offset: 0x0000D789
		public static void SetAvailableCustomDimensions02(string list)
		{
			GA_Wrapper.configureAvailableCustomDimensions02(list);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000F391 File Offset: 0x0000D791
		public static void SetAvailableCustomDimensions03(string list)
		{
			GA_Wrapper.configureAvailableCustomDimensions03(list);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000F399 File Offset: 0x0000D799
		public static void SetAvailableResourceCurrencies(string list)
		{
			GA_Wrapper.configureAvailableResourceCurrencies(list);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000F3A1 File Offset: 0x0000D7A1
		public static void SetAvailableResourceItemTypes(string list)
		{
			GA_Wrapper.configureAvailableResourceItemTypes(list);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000F3A9 File Offset: 0x0000D7A9
		public static void SetUnitySdkVersion(string unitySdkVersion)
		{
			GA_Wrapper.configureSdkGameEngineVersion(unitySdkVersion);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000F3B1 File Offset: 0x0000D7B1
		public static void SetUnityEngineVersion(string unityEngineVersion)
		{
			GA_Wrapper.configureGameEngineVersion(unityEngineVersion);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000F3B9 File Offset: 0x0000D7B9
		public static void SetBuild(string build)
		{
			GA_Wrapper.configureBuild(build);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000F3C1 File Offset: 0x0000D7C1
		public static void SetCustomUserId(string userId)
		{
			GA_Wrapper.configureUserId(userId);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000F3C9 File Offset: 0x0000D7C9
		public static void SetEnabledManualSessionHandling(bool enabled)
		{
			GA_Wrapper.setManualSessionHandling(enabled);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000F3D1 File Offset: 0x0000D7D1
		public static void SetEnabledEventSubmission(bool enabled)
		{
			GA_Wrapper.setEventSubmission(enabled);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000F3D9 File Offset: 0x0000D7D9
		public static void StartSession()
		{
			if (GAState.IsManualSessionHandlingEnabled())
			{
				GA_Wrapper.gameAnalyticsStartSession();
			}
			else
			{
				Debug.Log("Manual session handling is not enabled. \nPlease check the \"Use manual session handling\" option in the \"Advanced\" section of the Settings object.");
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000F3F9 File Offset: 0x0000D7F9
		public static void EndSession()
		{
			if (GAState.IsManualSessionHandlingEnabled())
			{
				GA_Wrapper.gameAnalyticsEndSession();
			}
			else
			{
				Debug.Log("Manual session handling is not enabled. \nPlease check the \"Use manual session handling\" option in the \"Advanced\" section of the Settings object.");
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000F419 File Offset: 0x0000D819
		public static void Initialize(string gamekey, string gamesecret)
		{
			GA_Wrapper.initialize(gamekey, gamesecret);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000F422 File Offset: 0x0000D822
		public static void SetCustomDimension01(string customDimension)
		{
			GA_Wrapper.setCustomDimension01(customDimension);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000F42A File Offset: 0x0000D82A
		public static void SetCustomDimension02(string customDimension)
		{
			GA_Wrapper.setCustomDimension02(customDimension);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000F432 File Offset: 0x0000D832
		public static void SetCustomDimension03(string customDimension)
		{
			GA_Wrapper.setCustomDimension03(customDimension);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000F43C File Offset: 0x0000D83C
		public static void AddBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, IDictionary<string, object> fields)
		{
			string fields2 = GA_Wrapper.DictionaryToJsonString(fields);
			GA_Wrapper.addBusinessEvent(currency, amount, itemType, itemId, cartType, fields2);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000F460 File Offset: 0x0000D860
		public static void AddResourceEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId, IDictionary<string, object> fields)
		{
			string fields2 = GA_Wrapper.DictionaryToJsonString(fields);
			GA_Wrapper.addResourceEvent((int)flowType, currency, amount, itemType, itemId, fields2);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000F484 File Offset: 0x0000D884
		public static void AddProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, IDictionary<string, object> fields)
		{
			string fields2 = GA_Wrapper.DictionaryToJsonString(fields);
			GA_Wrapper.addProgressionEvent((int)progressionStatus, progression01, progression02, progression03, fields2);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000F4A4 File Offset: 0x0000D8A4
		public static void AddProgressionEventWithScore(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score, IDictionary<string, object> fields)
		{
			string fields2 = GA_Wrapper.DictionaryToJsonString(fields);
			GA_Wrapper.addProgressionEventWithScore((int)progressionStatus, progression01, progression02, progression03, score, fields2);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000F4C8 File Offset: 0x0000D8C8
		public static void AddDesignEvent(string eventID, float eventValue, IDictionary<string, object> fields)
		{
			string fields2 = GA_Wrapper.DictionaryToJsonString(fields);
			GA_Wrapper.addDesignEventWithValue(eventID, eventValue, fields2);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000F4E4 File Offset: 0x0000D8E4
		public static void AddDesignEvent(string eventID, IDictionary<string, object> fields)
		{
			string fields2 = GA_Wrapper.DictionaryToJsonString(fields);
			GA_Wrapper.addDesignEvent(eventID, fields2);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000F500 File Offset: 0x0000D900
		public static void AddErrorEvent(GAErrorSeverity severity, string message, IDictionary<string, object> fields)
		{
			string fields2 = GA_Wrapper.DictionaryToJsonString(fields);
			GA_Wrapper.addErrorEvent((int)severity, message, fields2);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000F51C File Offset: 0x0000D91C
		public static void SetInfoLog(bool enabled)
		{
			GA_Wrapper.setEnabledInfoLog(enabled);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000F524 File Offset: 0x0000D924
		public static void SetVerboseLog(bool enabled)
		{
			GA_Wrapper.setEnabledVerboseLog(enabled);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000F52C File Offset: 0x0000D92C
		public static void SetFacebookId(string facebookId)
		{
			GA_Wrapper.setFacebookId(facebookId);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000F534 File Offset: 0x0000D934
		public static void SetGender(string gender)
		{
			GA_Wrapper.setGender(gender);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000F53C File Offset: 0x0000D93C
		public static void SetBirthYear(int birthYear)
		{
			GA_Wrapper.setBirthYear(birthYear);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000F544 File Offset: 0x0000D944
		public static string GetCommandCenterValueAsString(string key, string defaultValue)
		{
			return GA_Wrapper.getCommandCenterValueAsString(key, defaultValue);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000F54D File Offset: 0x0000D94D
		public static bool IsCommandCenterReady()
		{
			return GA_Wrapper.isCommandCenterReady();
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000F554 File Offset: 0x0000D954
		public static string GetConfigurationsContentAsString()
		{
			return GA_Wrapper.getConfigurationsContentAsString();
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000F55C File Offset: 0x0000D95C
		private static string DictionaryToJsonString(IDictionary<string, object> dict)
		{
			Hashtable hashtable = new Hashtable();
			if (dict != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in dict)
				{
					hashtable.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return GA_MiniJSON.Serialize(hashtable);
		}

		// Token: 0x040001AB RID: 427
		private static readonly GA_Wrapper.UnityCommandCenterListener unityCommandCenterListener = new GA_Wrapper.UnityCommandCenterListener();

		// Token: 0x0200003E RID: 62
		private class UnityCommandCenterListener : ICommandCenterListener
		{
			// Token: 0x06000237 RID: 567 RVA: 0x0000F5E4 File Offset: 0x0000D9E4
			public void OnCommandCenterUpdated()
			{
				GameAnalytics.CommandCenterUpdated();
			}
		}
	}
}
