using System;
using System.Collections.Generic;
using GameAnalyticsSDK.Wrapper;

namespace GameAnalyticsSDK.Events
{
	// Token: 0x02000029 RID: 41
	public static class GA_Design
	{
		// Token: 0x06000148 RID: 328 RVA: 0x0000C4F6 File Offset: 0x0000A8F6
		public static void NewEvent(string eventName, float eventValue, IDictionary<string, object> fields)
		{
			GA_Design.CreateNewEvent(eventName, new float?(eventValue), fields);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000C508 File Offset: 0x0000A908
		public static void NewEvent(string eventName, IDictionary<string, object> fields)
		{
			GA_Design.CreateNewEvent(eventName, null, fields);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000C525 File Offset: 0x0000A925
		private static void CreateNewEvent(string eventName, float? eventValue, IDictionary<string, object> fields)
		{
			if (eventValue != null)
			{
				GA_Wrapper.AddDesignEvent(eventName, eventValue.Value, fields);
			}
			else
			{
				GA_Wrapper.AddDesignEvent(eventName, fields);
			}
		}
	}
}
