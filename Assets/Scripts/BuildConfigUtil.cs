// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public static class BuildConfigUtil
{
	public static string GetVersionString(ConfigData config, bool onlineMode, bool displayExtraInfo)
	{
		switch (BuildConfig.environmentType)
		{
		case BuildEnvironment.Local:
			if (Application.isEditor && !onlineMode)
			{
				return "LOCAL BUILD";
			}
			return "LOCAL BUILD[" + config.buildPlayerTimestamp + "]";
		case BuildEnvironment.Live:
			if (displayExtraInfo)
			{
				return config.uiuxSettings.displayVersion + " [" + BuildConfig.p4Changelist + "]";
			}
			return config.uiuxSettings.displayVersion;
		}
		string text = string.Empty;
		text = ((!DemoSettings.DemoModeEnabled) ? string.Empty : "DEMO ");
		return string.Concat(new object[]
		{
			"[",
			BuildConfig.environmentType.ToString(),
			", ",
			BuildConfig.jobName,
			" #",
			BuildConfig.buildNumber,
			"] ",
			text,
			":",
			BuildConfig.p4Changelist
		});
	}

	public static string GetCompareVersion(ConfigData config)
	{
		switch (BuildConfig.environmentType)
		{
		case BuildEnvironment.Local:
			return config.buildPlayerTimestamp;
		}
		return string.Concat(new object[]
		{
			BuildConfig.environmentType.ToString(),
			BuildConfig.jobName,
			BuildConfig.buildNumber,
			BuildConfig.p4Changelist,
			BuildConfig.serverEnvironment
		});
	}
}
