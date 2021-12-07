// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IStartupArgs
{
	int GetArgIntValue(StartupArgs.StartupArgType argType);

	ulong GetArgULongValue(StartupArgs.StartupArgType argType);

	string GetArgStringValue(StartupArgs.StartupArgType argType);

	bool HasArg(StartupArgs.StartupArgType argType);

	void DebugLogArgs();
}
