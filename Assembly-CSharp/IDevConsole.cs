using System;

// Token: 0x020002F7 RID: 759
public interface IDevConsole
{
	// Token: 0x0600108F RID: 4239
	void RunAutoExec();

	// Token: 0x06001090 RID: 4240
	void PrintLn(string text);

	// Token: 0x06001091 RID: 4241
	void AddCommand(Action callback, string category, string text, string help = null);

	// Token: 0x06001092 RID: 4242
	void AddCommand<T>(Action<T> callback, string category, string text, string help = null);

	// Token: 0x06001093 RID: 4243
	void AddPlayerCommand(Action<PlayerNum> callback, string text, string help = null);

	// Token: 0x06001094 RID: 4244
	void AddPlayerCommand<T>(Action<PlayerNum, T> callback, string text, string help = null);

	// Token: 0x06001095 RID: 4245
	void AddAdminCommand(Action callback, string text, string help = null);

	// Token: 0x06001096 RID: 4246
	void AddAdminCommand<T>(Action<T> callback, string text, string help = null);

	// Token: 0x06001097 RID: 4247
	void AddConsoleVariable<T>(string context, string name, string label, string help, Func<T> getCallback, Action<T> setCallback);

	// Token: 0x06001098 RID: 4248
	void AddPlayerConsoleVariable<T>(string name, string label, string help, Func<PlayerNum, T> getCallback, Action<PlayerNum, T> setCallback);

	// Token: 0x06001099 RID: 4249
	void ExecuteCommand(string command);
}
