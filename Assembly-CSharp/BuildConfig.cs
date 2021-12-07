using System;

// Token: 0x020002E6 RID: 742
public static class BuildConfig
{
	// Token: 0x04000A1C RID: 2588
	public static string p4Changelist = "<local>";

	// Token: 0x04000A1D RID: 2589
	public static string p4Stream = "<local>";

	// Token: 0x04000A1E RID: 2590
	public static int buildNumber;

	// Token: 0x04000A1F RID: 2591
	public static string jobName = "<local>";

	// Token: 0x04000A20 RID: 2592
	public static BuildEnvironment environmentType;

	// Token: 0x04000A21 RID: 2593
	public static ServerEnvironment serverEnvironment = ServerEnvironment.STEAM;

	// Token: 0x04000A22 RID: 2594
	public static string autoExec = string.Empty;
}
