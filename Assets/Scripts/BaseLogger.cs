// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Text;

public abstract class BaseLogger : ILogger
{
	public virtual LogLevel LogLevel
	{
		get;
		set;
	}

	public void LogFormat(LogLevel logLevel, string format, params object[] parameters)
	{
		if (logLevel != LogLevel.None && logLevel <= this.LogLevel)
		{
			string arg = string.Format(format, parameters);
			this.LogMessage(logLevel, string.Format("[{0}] {1}", logLevel.ToString(), arg));
		}
	}

	public void Log(LogLevel logLevel, params object[] parameters)
	{
		if (logLevel != LogLevel.None && logLevel <= this.LogLevel)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < parameters.Length; i++)
			{
				object obj = parameters[i];
				stringBuilder.Append(obj.ToString());
				if (i < parameters.Length - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			this.LogMessage(logLevel, string.Format("[{0}] {1}", logLevel.ToString(), stringBuilder.ToString()));
		}
	}

	public abstract void LogMessage(LogLevel logLevel, string message);
}
