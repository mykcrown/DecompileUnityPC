using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GameAnalyticsSDK.Events
{
	// Token: 0x02000028 RID: 40
	public static class GA_Debug
	{
		// Token: 0x06000144 RID: 324 RVA: 0x0000C3A4 File Offset: 0x0000A7A4
		public static void HandleLog(string logString, string stackTrace, LogType type)
		{
			if (GA_Debug._showLogOnGUI)
			{
				if (GA_Debug.Messages == null)
				{
					GA_Debug.Messages = new List<string>();
				}
				GA_Debug.Messages.Add(logString);
			}
			if (GameAnalytics.SettingsGA.SubmitErrors && GA_Debug._errorCount < GA_Debug.MaxErrorCount && type != LogType.Log)
			{
				if (string.IsNullOrEmpty(stackTrace))
				{
					stackTrace = new StackTrace().ToString();
				}
				GA_Debug._errorCount++;
				string str = logString.Replace('"', '\'').Replace('\n', ' ').Replace('\r', ' ');
				string str2 = stackTrace.Replace('"', '\'').Replace('\n', ' ').Replace('\r', ' ');
				string text = str + " " + str2;
				if (text.Length > 8192)
				{
					text = text.Substring(8192);
				}
				GA_Debug.SubmitError(text, type);
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000C48C File Offset: 0x0000A88C
		private static void SubmitError(string message, LogType type)
		{
			GAErrorSeverity severity = GAErrorSeverity.Info;
			switch (type)
			{
			case LogType.Error:
				severity = GAErrorSeverity.Error;
				break;
			case LogType.Assert:
				severity = GAErrorSeverity.Info;
				break;
			case LogType.Warning:
				severity = GAErrorSeverity.Warning;
				break;
			case LogType.Log:
				severity = GAErrorSeverity.Debug;
				break;
			case LogType.Exception:
				severity = GAErrorSeverity.Critical;
				break;
			}
			GA_Error.NewEvent(severity, message, null);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000C4E5 File Offset: 0x0000A8E5
		public static void EnabledLog()
		{
			GA_Debug._showLogOnGUI = true;
		}

		// Token: 0x04000120 RID: 288
		public static int MaxErrorCount = 10;

		// Token: 0x04000121 RID: 289
		private static int _errorCount;

		// Token: 0x04000122 RID: 290
		private static bool _showLogOnGUI;

		// Token: 0x04000123 RID: 291
		public static List<string> Messages;
	}
}
