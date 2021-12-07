using System;
using UnityEngine;

// Token: 0x02000B55 RID: 2901
public class ProfilingUtil
{
	// Token: 0x06005419 RID: 21529 RVA: 0x001B0F73 File Offset: 0x001AF373
	public static void BeginTimer()
	{
		ProfilingUtil.time = Time.realtimeSinceStartup;
	}

	// Token: 0x0600541A RID: 21530 RVA: 0x001B0F80 File Offset: 0x001AF380
	public static void EndTimer(string title)
	{
		float num = (Time.realtimeSinceStartup - ProfilingUtil.time) * 1000f;
		if (num >= 0.2f)
		{
			Debug.LogFormat("[Load Time] {0}: {1}ms", new object[]
			{
				title,
				num
			});
		}
	}

	// Token: 0x0600541B RID: 21531 RVA: 0x001B0FC8 File Offset: 0x001AF3C8
	public static void ReportMemory()
	{
		float num = (float)GC.GetTotalMemory(false) / 1024f / 1024f;
		Debug.LogFormat("[Total Memory] {0}MB", new object[]
		{
			num
		});
	}

	// Token: 0x04003556 RID: 13654
	private static float time;
}
