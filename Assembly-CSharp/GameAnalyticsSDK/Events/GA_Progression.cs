using System;
using System.Collections.Generic;
using GameAnalyticsSDK.Wrapper;

namespace GameAnalyticsSDK.Events
{
	// Token: 0x0200002B RID: 43
	public static class GA_Progression
	{
		// Token: 0x0600014D RID: 333 RVA: 0x0000C564 File Offset: 0x0000A964
		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, IDictionary<string, object> fields)
		{
			GA_Progression.CreateEvent(progressionStatus, progression01, null, null, null, fields);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000C584 File Offset: 0x0000A984
		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, IDictionary<string, object> fields)
		{
			GA_Progression.CreateEvent(progressionStatus, progression01, progression02, null, null, fields);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000C5A4 File Offset: 0x0000A9A4
		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, IDictionary<string, object> fields)
		{
			GA_Progression.CreateEvent(progressionStatus, progression01, progression02, progression03, null, fields);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000C5C5 File Offset: 0x0000A9C5
		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, int score, IDictionary<string, object> fields)
		{
			GA_Progression.CreateEvent(progressionStatus, progression01, null, null, new int?(score), fields);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000C5D7 File Offset: 0x0000A9D7
		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, int score, IDictionary<string, object> fields)
		{
			GA_Progression.CreateEvent(progressionStatus, progression01, progression02, null, new int?(score), fields);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000C5EA File Offset: 0x0000A9EA
		public static void NewEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score, IDictionary<string, object> fields)
		{
			GA_Progression.CreateEvent(progressionStatus, progression01, progression02, progression03, new int?(score), fields);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000C5FE File Offset: 0x0000A9FE
		private static void CreateEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int? score, IDictionary<string, object> fields)
		{
			if (score != null)
			{
				GA_Wrapper.AddProgressionEventWithScore(progressionStatus, progression01, progression02, progression03, score.Value, fields);
			}
			else
			{
				GA_Wrapper.AddProgressionEvent(progressionStatus, progression01, progression02, progression03, fields);
			}
		}
	}
}
