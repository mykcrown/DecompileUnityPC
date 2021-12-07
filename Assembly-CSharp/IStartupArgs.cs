using System;

// Token: 0x020008A2 RID: 2210
public interface IStartupArgs
{
	// Token: 0x0600375E RID: 14174
	int GetArgIntValue(StartupArgs.StartupArgType argType);

	// Token: 0x0600375F RID: 14175
	ulong GetArgULongValue(StartupArgs.StartupArgType argType);

	// Token: 0x06003760 RID: 14176
	string GetArgStringValue(StartupArgs.StartupArgType argType);

	// Token: 0x06003761 RID: 14177
	bool HasArg(StartupArgs.StartupArgType argType);

	// Token: 0x06003762 RID: 14178
	void DebugLogArgs();
}
