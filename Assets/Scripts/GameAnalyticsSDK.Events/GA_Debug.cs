// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GameAnalyticsSDK.Events
{
	public static class GA_Debug
	{
		public static int MaxErrorCount = 10;

		private static int _errorCount;

		private static bool _showLogOnGUI;

		public static List<string> Messages;

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

		public static void EnabledLog()
		{
			GA_Debug._showLogOnGUI = true;
		}
	}
}
