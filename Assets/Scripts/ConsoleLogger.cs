// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ConsoleLogger : BaseLogger
{
	[Inject]
	public IDevConsole devConsole
	{
		get;
		set;
	}

	public override void LogMessage(LogLevel logLevel, string message)
	{
		this.devConsole.PrintLn(message);
	}
}
