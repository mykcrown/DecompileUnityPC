using System;
using System.Diagnostics;

namespace InControl
{
	// Token: 0x02000076 RID: 118
	public class Logger
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600042D RID: 1069 RVA: 0x00018C04 File Offset: 0x00017004
		// (remove) Token: 0x0600042E RID: 1070 RVA: 0x00018C38 File Offset: 0x00017038
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<LogMessage> OnLogMessage;

		// Token: 0x0600042F RID: 1071 RVA: 0x00018C6C File Offset: 0x0001706C
		public static void LogInfo(string text)
		{
			if (Logger.OnLogMessage != null)
			{
				LogMessage obj = new LogMessage
				{
					text = text,
					type = LogMessageType.Info
				};
				Logger.OnLogMessage(obj);
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00018CA8 File Offset: 0x000170A8
		public static void LogWarning(string text)
		{
			if (Logger.OnLogMessage != null)
			{
				LogMessage obj = new LogMessage
				{
					text = text,
					type = LogMessageType.Warning
				};
				Logger.OnLogMessage(obj);
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00018CE4 File Offset: 0x000170E4
		public static void LogError(string text)
		{
			if (Logger.OnLogMessage != null)
			{
				LogMessage obj = new LogMessage
				{
					text = text,
					type = LogMessageType.Error
				};
				Logger.OnLogMessage(obj);
			}
		}
	}
}
