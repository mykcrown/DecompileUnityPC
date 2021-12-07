using System;
using FixedPoint;

// Token: 0x020003CF RID: 975
[Serializable]
public class UIUXSettings
{
	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x06001545 RID: 5445 RVA: 0x00075875 File Offset: 0x00073C75
	public bool DemoModeEnabled
	{
		get
		{
			return DemoSettings.DemoModeEnabled;
		}
	}

	// Token: 0x04000E72 RID: 3698
	public bool emotiveStartup;

	// Token: 0x04000E73 RID: 3699
	public int inputFramesBuffer = 10;

	// Token: 0x04000E74 RID: 3700
	public int countdownAmt = 3;

	// Token: 0x04000E75 RID: 3701
	public Fixed countdownIntervalSeconds = (Fixed)0.800000011920929;

	// Token: 0x04000E76 RID: 3702
	public Fixed characterSelectIdleSeconds = (Fixed)5.0;

	// Token: 0x04000E77 RID: 3703
	public int endGameDelayFrames = 80;

	// Token: 0x04000E78 RID: 3704
	public bool skipVictoryPoseScreen;

	// Token: 0x04000E79 RID: 3705
	public CrewBattleGuiType crewsGuiType;

	// Token: 0x04000E7A RID: 3706
	public bool floatyNameHiding;

	// Token: 0x04000E7B RID: 3707
	public bool alwaysShowCustomNames = true;

	// Token: 0x04000E7C RID: 3708
	public int floatyNamesAtMatchBegin = 60;

	// Token: 0x04000E7D RID: 3709
	public Fixed floatyNameOnTumble = 30;

	// Token: 0x04000E7E RID: 3710
	public int floatyNameMinShow = 60;

	// Token: 0x04000E7F RID: 3711
	public int floatyNamesIdleShow = 60;

	// Token: 0x04000E80 RID: 3712
	public bool floatyNameShowRespawning = true;

	// Token: 0x04000E81 RID: 3713
	public float offscreenUIOffset = 25f;

	// Token: 0x04000E82 RID: 3714
	public float offscreenDetectionPadding = 25f;

	// Token: 0x04000E83 RID: 3715
	public float floatyNameLerpSpeed = 0.1f;

	// Token: 0x04000E84 RID: 3716
	public int floatyNameMaxSize = 10;

	// Token: 0x04000E85 RID: 3717
	public int MatchmakingPromptTime = 30;

	// Token: 0x04000E86 RID: 3718
	public string displayVersion = "0.03.0";
}
