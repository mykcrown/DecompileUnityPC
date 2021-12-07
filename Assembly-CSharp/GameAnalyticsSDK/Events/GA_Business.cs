using System;
using System.Collections.Generic;
using GameAnalyticsSDK.Wrapper;

namespace GameAnalyticsSDK.Events
{
	// Token: 0x02000027 RID: 39
	public static class GA_Business
	{
		// Token: 0x06000143 RID: 323 RVA: 0x0000C395 File Offset: 0x0000A795
		public static void NewEvent(string currency, int amount, string itemType, string itemId, string cartType, IDictionary<string, object> fields)
		{
			GA_Wrapper.AddBusinessEvent(currency, amount, itemType, itemId, cartType, fields);
		}
	}
}
