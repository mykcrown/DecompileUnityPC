using System;

// Token: 0x02000B16 RID: 2838
public class ConsoleLogger : BaseLogger
{
	// Token: 0x17001309 RID: 4873
	// (get) Token: 0x06005166 RID: 20838 RVA: 0x00152384 File Offset: 0x00150784
	// (set) Token: 0x06005167 RID: 20839 RVA: 0x0015238C File Offset: 0x0015078C
	[Inject]
	public IDevConsole devConsole { get; set; }

	// Token: 0x06005168 RID: 20840 RVA: 0x00152395 File Offset: 0x00150795
	public override void LogMessage(LogLevel logLevel, string message)
	{
		this.devConsole.PrintLn(message);
	}
}
