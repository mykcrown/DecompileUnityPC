using System;
using UnityEngine;

// Token: 0x020002E7 RID: 743
public static class BuildConfigUtil
{
	// Token: 0x06000F72 RID: 3954 RVA: 0x0005D7E0 File Offset: 0x0005BBE0
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

	// Token: 0x06000F73 RID: 3955 RVA: 0x0005D8F8 File Offset: 0x0005BCF8
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
