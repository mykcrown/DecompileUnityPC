using System;
using System.Collections.Generic;
using GameAnalyticsSDK.Wrapper;

namespace GameAnalyticsSDK.Events
{
	// Token: 0x0200002C RID: 44
	public static class GA_Resource
	{
		// Token: 0x06000154 RID: 340 RVA: 0x0000C62E File Offset: 0x0000AA2E
		public static void NewEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId, IDictionary<string, object> fields)
		{
			GA_Wrapper.AddResourceEvent(flowType, currency, amount, itemType, itemId, fields);
		}
	}
}
