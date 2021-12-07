// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class DummyDevConsole : MonoBehaviour, IDevConsole
{
	void IDevConsole.RunAutoExec()
	{
	}

	void IDevConsole.PrintLn(string text)
	{
	}

	void IDevConsole.AddCommand(Action callback, string category, string text, string help)
	{
	}

	void IDevConsole.AddCommand<T>(Action<T> callback, string category, string text, string help)
	{
	}

	void IDevConsole.AddAdminCommand(Action callback, string text, string help)
	{
	}

	void IDevConsole.AddAdminCommand<T>(Action<T> callback, string text, string help)
	{
	}

	void IDevConsole.AddConsoleVariable<T>(string context, string name, string label, string help, Func<T> getCallback, Action<T> setCallback)
	{
	}

	void IDevConsole.AddPlayerConsoleVariable<T>(string name, string label, string help, Func<PlayerNum, T> getCallback, Action<PlayerNum, T> setCallback)
	{
	}

	void IDevConsole.AddPlayerCommand(Action<PlayerNum> callback, string text, string help)
	{
	}

	void IDevConsole.AddPlayerCommand<T>(Action<PlayerNum, T> callback, string text, string help)
	{
	}

	void IDevConsole.ExecuteCommand(string command)
	{
	}
}
