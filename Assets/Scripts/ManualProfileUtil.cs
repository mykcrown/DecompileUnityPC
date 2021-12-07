// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class ManualProfileUtil
{
	private static float time = 0f;

	private static float lastReportTime = 0f;

	private static List<float> list = new List<float>();

	private static List<string> textList = new List<string>();

	public static void StartTracking()
	{
		ManualProfileUtil.list.Clear();
		ManualProfileUtil.textList.Clear();
		ManualProfileUtil.time = Time.realtimeSinceStartup;
		ManualProfileUtil.lastReportTime = ManualProfileUtil.time;
	}

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

	private static void print()
	{
		for (int i = 0; i < ManualProfileUtil.list.Count; i++)
		{
			UnityEngine.Debug.LogWarning(string.Concat(new object[]
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
}
