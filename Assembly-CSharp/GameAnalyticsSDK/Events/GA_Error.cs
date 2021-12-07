using System;
using System.Collections.Generic;
using GameAnalyticsSDK.Wrapper;

namespace GameAnalyticsSDK.Events
{
	// Token: 0x0200002A RID: 42
	public static class GA_Error
	{
		// Token: 0x0600014B RID: 331 RVA: 0x0000C54D File Offset: 0x0000A94D
		public static void NewEvent(GAErrorSeverity severity, string message, IDictionary<string, object> fields)
		{
			GA_Error.CreateNewEvent(severity, message, fields);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000C557 File Offset: 0x0000A957
		private static void CreateNewEvent(GAErrorSeverity severity, string message, IDictionary<string, object> fields)
		{
			GA_Wrapper.AddErrorEvent(severity, message, fields);
		}
	}
}
