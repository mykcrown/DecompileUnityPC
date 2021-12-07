using System;
using UnityEngine;

// Token: 0x02000B1A RID: 2842
public class UnityLogger : BaseLogger
{
	// Token: 0x06005176 RID: 20854 RVA: 0x001525A4 File Offset: 0x001509A4
	public override void LogMessage(LogLevel logLevel, string message)
	{
		switch (logLevel)
		{
		case LogLevel.Error:
			Debug.LogError(message);
			break;
		case LogLevel.Warning:
			Debug.LogWarning(message);
			break;
		case LogLevel.Debug:
		case LogLevel.InfoBasic:
		case LogLevel.InfoVerbose:
			Debug.Log(message);
			break;
		}
	}
}
