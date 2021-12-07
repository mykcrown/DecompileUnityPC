using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008A0 RID: 2208
public class StartupArgs : IStartupArgs
{
	// Token: 0x06003757 RID: 14167 RVA: 0x001027E8 File Offset: 0x00100BE8
	[PostConstruct]
	public void Initialize()
	{
		this.argNameMap.Add(StartupArgs.StartupArgType.OverrideServerEnv, "overrideServerEnv");
		this.argNameMap.Add(StartupArgs.StartupArgType.SteamConnectLobby, "+connect_lobby");
		this.args = Environment.GetCommandLineArgs();
		foreach (KeyValuePair<StartupArgs.StartupArgType, string> keyValuePair in this.argNameMap)
		{
			string argValue = this.getArgValue(keyValuePair.Value);
			if (!string.IsNullOrEmpty(argValue))
			{
				this.argValueMap.Add(keyValuePair.Key, argValue);
			}
		}
	}

	// Token: 0x06003758 RID: 14168 RVA: 0x00102898 File Offset: 0x00100C98
	public bool HasArg(StartupArgs.StartupArgType argType)
	{
		return this.argValueMap.ContainsKey(argType);
	}

	// Token: 0x06003759 RID: 14169 RVA: 0x001028A8 File Offset: 0x00100CA8
	public string GetArgStringValue(StartupArgs.StartupArgType argType)
	{
		string result;
		this.argValueMap.TryGetValue(argType, out result);
		return result;
	}

	// Token: 0x0600375A RID: 14170 RVA: 0x001028C8 File Offset: 0x00100CC8
	public int GetArgIntValue(StartupArgs.StartupArgType argType)
	{
		string text;
		this.argValueMap.TryGetValue(argType, out text);
		Debug.LogError("Requested arg: " + argType);
		Debug.LogError("Value: " + text);
		int num = 0;
		int.TryParse(text, out num);
		Debug.LogError("Output: " + num);
		return num;
	}

	// Token: 0x0600375B RID: 14171 RVA: 0x0010292C File Offset: 0x00100D2C
	public ulong GetArgULongValue(StartupArgs.StartupArgType argType)
	{
		string text;
		this.argValueMap.TryGetValue(argType, out text);
		Debug.LogError("Requested arg: " + argType);
		Debug.LogError("Value: " + text);
		ulong num = 0UL;
		ulong.TryParse(text, out num);
		Debug.LogError("Output: " + num);
		return num;
	}

	// Token: 0x0600375C RID: 14172 RVA: 0x00102990 File Offset: 0x00100D90
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

	// Token: 0x0600375D RID: 14173 RVA: 0x001029E8 File Offset: 0x00100DE8
	public void DebugLogArgs()
	{
		if (this.args == null || this.args.Length == 0)
		{
			Debug.Log("0 Command Arguments.");
		}
		else
		{
			for (int i = 0; i < this.args.Length; i++)
			{
			}
		}
	}

	// Token: 0x04002594 RID: 9620
	private Dictionary<StartupArgs.StartupArgType, string> argNameMap = new Dictionary<StartupArgs.StartupArgType, string>();

	// Token: 0x04002595 RID: 9621
	private string[] args;

	// Token: 0x04002596 RID: 9622
	private Dictionary<StartupArgs.StartupArgType, string> argValueMap = new Dictionary<StartupArgs.StartupArgType, string>();

	// Token: 0x020008A1 RID: 2209
	public enum StartupArgType
	{
		// Token: 0x04002598 RID: 9624
		OverrideServerEnv,
		// Token: 0x04002599 RID: 9625
		SteamConnectLobby
	}
}
