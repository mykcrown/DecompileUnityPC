// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class ProfilingUtil
{
	private static float time;

	public static void BeginTimer()
	{
		ProfilingUtil.time = Time.realtimeSinceStartup;
	}

	public static void EndTimer(string title)
	{
		float num = (Time.realtimeSinceStartup - ProfilingUtil.time) * 1000f;
		if (num >= 0.2f)
		{
			UnityEngine.Debug.LogFormat("[Load Time] {0}: {1}ms", new object[]
			{
				title,
				num
			});
		}
	}

	public static void ReportMemory()
	{
		float num = (float)GC.GetTotalMemory(false) / 1024f / 1024f;
		UnityEngine.Debug.LogFormat("[Total Memory] {0}MB", new object[]
		{
			num
		});
	}
}
