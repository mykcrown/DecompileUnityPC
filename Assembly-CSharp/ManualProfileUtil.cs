using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B3B RID: 2875
public class ManualProfileUtil
{
	// Token: 0x0600535B RID: 21339 RVA: 0x001AF32A File Offset: 0x001AD72A
	public static void StartTracking()
	{
		ManualProfileUtil.list.Clear();
		ManualProfileUtil.textList.Clear();
		ManualProfileUtil.time = Time.realtimeSinceStartup;
		ManualProfileUtil.lastReportTime = ManualProfileUtil.time;
	}

	// Token: 0x0600535C RID: 21340 RVA: 0x001AF354 File Offset: 0x001AD754
	public static void Report(string text)
	{
		float num = Time.realtimeSinceStartup - ManualProfileUtil.time;
		float item = Time.realtimeSinceStartup - ManualProfileUtil.lastReportTime;
		ManualProfileUtil.list.Add(item);
		ManualProfileUtil.textList.Add(text);
		if (num * 1000f > 12f)
		{
			ManualProfileUtil.print();
		}
		ManualProfileUtil.lastReportTime = Time.realtimeSinceStartup;
	}

	// Token: 0x0600535D RID: 21341 RVA: 0x001AF3B0 File Offset: 0x001AD7B0
	private static void print()
	{
		for (int i = 0; i < ManualProfileUtil.list.Count; i++)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"TIME ELAPSED --- ",
				ManualProfileUtil.textList[i],
				": ",
				ManualProfileUtil.list[i] * 1000f
			}));
		}
		ManualProfileUtil.list.Clear();
		ManualProfileUtil.textList.Clear();
	}

	// Token: 0x040034EC RID: 13548
	private static float time = 0f;

	// Token: 0x040034ED RID: 13549
	private static float lastReportTime = 0f;

	// Token: 0x040034EE RID: 13550
	private static List<float> list = new List<float>();

	// Token: 0x040034EF RID: 13551
	private static List<string> textList = new List<string>();
}
