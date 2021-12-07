// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IDevConsole
{
	void RunAutoExec();

	void PrintLn(string text);

	void AddCommand(Action callback, string category, string text, string help = null);

	void AddCommand<T>(Action<T> callback, string category, string text, string help = null);

	void AddPlayerCommand(Action<PlayerNum> callback, string text, string help = null);

	void AddPlayerCommand<T>(Action<PlayerNum, T> callback, string text, string help = null);

	void AddAdminCommand(Action callback, string text, string help = null);

	void AddAdminCommand<T>(Action<T> callback, string text, string help = null);

	void AddConsoleVariable<T>(string context, string name, string label, string help, Func<T> getCallback, Action<T> setCallback);

	void AddPlayerConsoleVariable<T>(string name, string label, string help, Func<PlayerNum, T> getCallback, Action<PlayerNum, T> setCallback);

	void ExecuteCommand(string command);
}
