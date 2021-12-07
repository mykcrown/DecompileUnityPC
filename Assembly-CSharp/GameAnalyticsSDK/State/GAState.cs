using System;
using System.Collections.Generic;
using GameAnalyticsSDK.Setup;
using UnityEngine;

namespace GameAnalyticsSDK.State
{
	// Token: 0x02000038 RID: 56
	internal static class GAState
	{
		// Token: 0x060001D2 RID: 466 RVA: 0x0000E424 File Offset: 0x0000C824
		public static void Init()
		{
			try
			{
				GAState._settings = (Settings)Resources.Load("GameAnalytics/Settings", typeof(Settings));
			}
			catch (Exception ex)
			{
				Debug.Log("Could not get Settings during event validation \n" + ex.ToString());
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000E480 File Offset: 0x0000C880
		private static bool ListContainsString(List<string> _list, string _string)
		{
			return _list.Contains(_string);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000E491 File Offset: 0x0000C891
		public static bool IsManualSessionHandlingEnabled()
		{
			return GAState._settings.UseManualSessionHandling;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000E49D File Offset: 0x0000C89D
		public static bool HasAvailableResourceCurrency(string _currency)
		{
			return GAState.ListContainsString(GAState._settings.ResourceCurrencies, _currency);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000E4B7 File Offset: 0x0000C8B7
		public static bool HasAvailableResourceItemType(string _itemType)
		{
			return GAState.ListContainsString(GAState._settings.ResourceItemTypes, _itemType);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000E4D1 File Offset: 0x0000C8D1
		public static bool HasAvailableCustomDimensions01(string _dimension01)
		{
			return GAState.ListContainsString(GAState._settings.CustomDimensions01, _dimension01);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000E4EB File Offset: 0x0000C8EB
		public static bool HasAvailableCustomDimensions02(string _dimension02)
		{
			return GAState.ListContainsString(GAState._settings.CustomDimensions02, _dimension02);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000E505 File Offset: 0x0000C905
		public static bool HasAvailableCustomDimensions03(string _dimension03)
		{
			return GAState.ListContainsString(GAState._settings.CustomDimensions03, _dimension03);
		}

		// Token: 0x0400019A RID: 410
		private static Settings _settings;
	}
}
