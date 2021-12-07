// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastingLogger : BaseLogger
{
	private List<BaseLogger> childLoggers = new List<BaseLogger>();

	private bool unityLogGuard;

	private LogLevel logLevel;

	public override LogLevel LogLevel
	{
		get
		{
			return this.logLevel;
		}
		set
		{
			this.logLevel = value;
			foreach (global::ILogger current in this.childLoggers)
			{
				current.LogLevel = this.logLevel;
			}
		}
	}

	public void AttachLogger(BaseLogger logger)
	{
		if (logger == this)
		{
			return;
		}
		if (!this.childLoggers.Contains(logger))
		{
			logger.LogLevel = this.LogLevel;
			this.childLoggers.Add(logger);
		}
	}

	public void DetachLogger(BaseLogger logger)
	{
		if (this.childLoggers.Contains(logger))
		{
			this.childLoggers.Remove(logger);
		}
	}

	public override void LogMessage(LogLevel logLevel, string message)
	{
		foreach (BaseLogger current in this.childLoggers)
		{
			if (current is UnityLogger)
			{
				this.unityLogGuard = true;
			}
			current.LogMessage(logLevel, message);
			this.unityLogGuard = false;
		}
	}

	public void LogUnityLog(string logString, string stackTrace, LogType logType)
	{
		if (this.unityLogGuard)
		{
			return;
		}
		LogLevel logLevel;
		switch (logType)
		{
		case LogType.Error:
		case LogType.Assert:
		case LogType.Exception:
			logLevel = LogLevel.Error;
			goto IL_42;
		case LogType.Warning:
			logLevel = LogLevel.Warning;
			goto IL_42;
		}
		logLevel = LogLevel.Debug;
		IL_42:
		foreach (BaseLogger current in this.childLoggers)
		{
			if (!(current is UnityLogger))
			{
				current.Log(logLevel, new object[]
				{
					logString
				});
			}
		}
	}

	public TLogger GetChildLogger<TLogger>() where TLogger : BaseLogger
	{
		for (int i = 0; i < this.childLoggers.Count; i++)
		{
			if (this.childLoggers[i] is TLogger)
			{
				return this.childLoggers[i] as TLogger;
			}
		}
		return (TLogger)((object)null);
	}
}
