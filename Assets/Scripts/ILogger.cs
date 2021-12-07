// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ILogger
{
	LogLevel LogLevel
	{
		get;
		set;
	}

	void Log(LogLevel logLevel, params object[] parameters);

	void LogFormat(LogLevel logLevel, string format, params object[] parameters);
}
