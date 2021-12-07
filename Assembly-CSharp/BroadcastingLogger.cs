using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B15 RID: 2837
public class BroadcastingLogger : BaseLogger
{
	// Token: 0x17001308 RID: 4872
	// (get) Token: 0x0600515E RID: 20830 RVA: 0x00152130 File Offset: 0x00150530
	// (set) Token: 0x0600515F RID: 20831 RVA: 0x00152138 File Offset: 0x00150538
	public override LogLevel LogLevel
	{
		get
		{
			return this.logLevel;
		}
		set
		{
			this.logLevel = value;
			foreach (global::ILogger logger in this.childLoggers)
			{
				logger.LogLevel = this.logLevel;
			}
		}
	}

	// Token: 0x06005160 RID: 20832 RVA: 0x001521A0 File Offset: 0x001505A0
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

	// Token: 0x06005161 RID: 20833 RVA: 0x001521D3 File Offset: 0x001505D3
	public void DetachLogger(BaseLogger logger)
	{
		if (this.childLoggers.Contains(logger))
		{
			this.childLoggers.Remove(logger);
		}
	}

	// Token: 0x06005162 RID: 20834 RVA: 0x001521F4 File Offset: 0x001505F4
	public override void LogMessage(LogLevel logLevel, string message)
	{
		foreach (BaseLogger baseLogger in this.childLoggers)
		{
			if (baseLogger is UnityLogger)
			{
				this.unityLogGuard = true;
			}
			baseLogger.LogMessage(logLevel, message);
			this.unityLogGuard = false;
		}
	}

	// Token: 0x06005163 RID: 20835 RVA: 0x0015226C File Offset: 0x0015066C
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
		foreach (BaseLogger baseLogger in this.childLoggers)
		{
			if (!(baseLogger is UnityLogger))
			{
				baseLogger.Log(logLevel, new object[]
				{
					logString
				});
			}
		}
	}

	// Token: 0x06005164 RID: 20836 RVA: 0x00152320 File Offset: 0x00150720
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

	// Token: 0x04003467 RID: 13415
	private List<BaseLogger> childLoggers = new List<BaseLogger>();

	// Token: 0x04003468 RID: 13416
	private bool unityLogGuard;

	// Token: 0x04003469 RID: 13417
	private LogLevel logLevel;
}
