// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class UnityLogger : BaseLogger
{
	public override void LogMessage(LogLevel logLevel, string message)
	{
		switch (logLevel)
		{
		case LogLevel.Error:
			UnityEngine.Debug.LogError(message);
			break;
		case LogLevel.Warning:
			UnityEngine.Debug.LogWarning(message);
			break;
		case LogLevel.Debug:
		case LogLevel.InfoBasic:
		case LogLevel.InfoVerbose:
			UnityEngine.Debug.Log(message);
			break;
		}
	}
}
