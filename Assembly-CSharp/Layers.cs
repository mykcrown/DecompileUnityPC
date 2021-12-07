using System;

// Token: 0x02000B09 RID: 2825
public static class Layers
{
	// Token: 0x06005126 RID: 20774 RVA: 0x00151820 File Offset: 0x0014FC20
	public static bool Intersects(int layer, int mask)
	{
		return (1 << layer & mask) != 0;
	}

	// Token: 0x04003459 RID: 13401
	public static string Ground = "Ground";

	// Token: 0x0400345A RID: 13402
	public static string Platform = "Platform";

	// Token: 0x0400345B RID: 13403
	public static string PlatformUnderside = "PlatformUnderside";

	// Token: 0x0400345C RID: 13404
	public static string StageData = "StageData";

	// Token: 0x0400345D RID: 13405
	public static string Particles = "Particles";

	// Token: 0x0400345E RID: 13406
	public static string Default = "Default";

	// Token: 0x0400345F RID: 13407
	public static string Foreground_Lighting = "Foreground_Lighting";

	// Token: 0x04003460 RID: 13408
	public static string Hazards = "Hazards";
}
