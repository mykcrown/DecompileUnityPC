using System;

// Token: 0x02000B14 RID: 2836
public interface ILogger
{
	// Token: 0x17001307 RID: 4871
	// (get) Token: 0x06005159 RID: 20825
	// (set) Token: 0x0600515A RID: 20826
	LogLevel LogLevel { get; set; }

	// Token: 0x0600515B RID: 20827
	void Log(LogLevel logLevel, params object[] parameters);

	// Token: 0x0600515C RID: 20828
	void LogFormat(LogLevel logLevel, string format, params object[] parameters);
}
