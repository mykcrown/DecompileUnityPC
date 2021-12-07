using System;
using System.Text;

// Token: 0x02000B13 RID: 2835
public abstract class BaseLogger : ILogger
{
	// Token: 0x17001306 RID: 4870
	// (get) Token: 0x06005154 RID: 20820 RVA: 0x0015203D File Offset: 0x0015043D
	// (set) Token: 0x06005155 RID: 20821 RVA: 0x00152045 File Offset: 0x00150445
	public virtual LogLevel LogLevel { get; set; }

	// Token: 0x06005156 RID: 20822 RVA: 0x00152050 File Offset: 0x00150450
	public void LogFormat(LogLevel logLevel, string format, params object[] parameters)
	{
		if (logLevel != LogLevel.None && logLevel <= this.LogLevel)
		{
			string arg = string.Format(format, parameters);
			this.LogMessage(logLevel, string.Format("[{0}] {1}", logLevel.ToString(), arg));
		}
	}

	// Token: 0x06005157 RID: 20823 RVA: 0x00152098 File Offset: 0x00150498
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

	// Token: 0x06005158 RID: 20824
	public abstract void LogMessage(LogLevel logLevel, string message);
}
