// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class StartupArgs : IStartupArgs
{
	public enum StartupArgType
	{
		OverrideServerEnv,
		SteamConnectLobby
	}

	private Dictionary<StartupArgs.StartupArgType, string> argNameMap = new Dictionary<StartupArgs.StartupArgType, string>();

	private string[] args;

	private Dictionary<StartupArgs.StartupArgType, string> argValueMap = new Dictionary<StartupArgs.StartupArgType, string>();

	[PostConstruct]
	public void Initialize()
	{
		this.argNameMap.Add(StartupArgs.StartupArgType.OverrideServerEnv, "overrideServerEnv");
		this.argNameMap.Add(StartupArgs.StartupArgType.SteamConnectLobby, "+connect_lobby");
		this.args = Environment.GetCommandLineArgs();
		foreach (KeyValuePair<StartupArgs.StartupArgType, string> current in this.argNameMap)
		{
			string argValue = this.getArgValue(current.Value);
			if (!string.IsNullOrEmpty(argValue))
			{
				this.argValueMap.Add(current.Key, argValue);
			}
		}
	}

	public bool HasArg(StartupArgs.StartupArgType argType)
	{
		return this.argValueMap.ContainsKey(argType);
	}

	public string GetArgStringValue(StartupArgs.StartupArgType argType)
	{
		string result;
		this.argValueMap.TryGetValue(argType, out result);
		return result;
	}

	public int GetArgIntValue(StartupArgs.StartupArgType argType)
	{
		string text;
		this.argValueMap.TryGetValue(argType, out text);
		UnityEngine.Debug.LogError("Requested arg: " + argType);
		UnityEngine.Debug.LogError("Value: " + text);
		int num = 0;
		int.TryParse(text, out num);
		UnityEngine.Debug.LogError("Output: " + num);
		return num;
	}

	public ulong GetArgULongValue(StartupArgs.StartupArgType argType)
	{
		string text;
		this.argValueMap.TryGetValue(argType, out text);
		UnityEngine.Debug.LogError("Requested arg: " + argType);
		UnityEngine.Debug.LogError("Value: " + text);
		ulong num = 0uL;
		ulong.TryParse(text, out num);
		UnityEngine.Debug.LogError("Output: " + num);
		return num;
	}

	private string getArgValue(string argName)
	{
		for (int i = 0; i < this.args.Length; i++)
		{
			if (this.args[i] == argName && this.args.Length > i + 1)
			{
				return this.args[i + 1];
			}
		}
		return null;
	}

	public void DebugLogArgs()
	{
		if (this.args == null || this.args.Length == 0)
		{
			UnityEngine.Debug.Log("0 Command Arguments.");
		}
		else
		{
			for (int i = 0; i < this.args.Length; i++)
			{
			}
		}
	}
}
