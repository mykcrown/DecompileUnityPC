using System;
using UnityEngine;

// Token: 0x02000402 RID: 1026
[Serializable]
public class DebugConfig : ScriptableObject
{
	// Token: 0x060015AB RID: 5547 RVA: 0x000771F8 File Offset: 0x000755F8
	public static DebugConfig LoadConfig()
	{
		return (DebugConfig)ScriptableObject.CreateInstance("DebugConfig");
	}

	// Token: 0x04001038 RID: 4152
	public bool displayUnitsSeconds;

	// Token: 0x04001039 RID: 4153
	public PlayerNum debugText2Player = PlayerNum.Player2;

	// Token: 0x0400103A RID: 4154
	public Color debugTextColor = Color.white;

	// Token: 0x0400103B RID: 4155
	public bool beginDebugPaused;

	// Token: 0x0400103C RID: 4156
	public TestGameSettings testGameSettings = new TestGameSettings();

	// Token: 0x0400103D RID: 4157
	public OfflineModeSettings offlineModeSettings = new OfflineModeSettings();

	// Token: 0x0400103E RID: 4158
	public TestPlayerProgressionSettings testPlayerProgressionSettings = new TestPlayerProgressionSettings();

	// Token: 0x0400103F RID: 4159
	private const string settingsPath = "Assets/Wavedash/Config/DebugConfig.asset";
}
