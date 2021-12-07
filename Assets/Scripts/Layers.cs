// Decompile from assembly: Assembly-CSharp.dll

using System;

public static class Layers
{
	public static string Ground = "Ground";

	public static string Platform = "Platform";

	public static string PlatformUnderside = "PlatformUnderside";

	public static string StageData = "StageData";

	public static string Particles = "Particles";

	public static string Default = "Default";

	public static string Foreground_Lighting = "Foreground_Lighting";

	public static string Hazards = "Hazards";

	public static bool Intersects(int layer, int mask)
	{
		return (1 << layer & mask) != 0;
	}
}
