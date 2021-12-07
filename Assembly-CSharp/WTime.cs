using System;
using System.Diagnostics;
using FixedPoint;
using UnityEngine;

// Token: 0x02000B83 RID: 2947
public class WTime
{
	// Token: 0x0600550F RID: 21775 RVA: 0x001B478C File Offset: 0x001B2B8C
	public static void Startup()
	{
		WTime.watch = new Stopwatch();
		WTime.watch.Start();
		WTime.watchStartTime = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
	}

	// Token: 0x17001394 RID: 5012
	// (get) Token: 0x06005510 RID: 21776 RVA: 0x001B47D0 File Offset: 0x001B2BD0
	public static float deltaTime
	{
		get
		{
			return FrameController.FrameDeltaTime;
		}
	}

	// Token: 0x17001395 RID: 5013
	// (get) Token: 0x06005511 RID: 21777 RVA: 0x001B47D8 File Offset: 0x001B2BD8
	public static double precisionTimeSinceStartup
	{
		get
		{
			return WTime.watchStartTime + WTime.watch.Elapsed.TotalMilliseconds;
		}
	}

	// Token: 0x17001396 RID: 5014
	// (get) Token: 0x06005512 RID: 21778 RVA: 0x001B4800 File Offset: 0x001B2C00
	public static long currentTimeMs
	{
		get
		{
			return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
		}
	}

	// Token: 0x17001397 RID: 5015
	// (get) Token: 0x06005513 RID: 21779 RVA: 0x001B482C File Offset: 0x001B2C2C
	public static Fixed fixedDeltaTime
	{
		get
		{
			return WTime.frameTimeValueF;
		}
	}

	// Token: 0x17001398 RID: 5016
	// (get) Token: 0x06005514 RID: 21780 RVA: 0x001B4833 File Offset: 0x001B2C33
	public static float frameTime
	{
		get
		{
			return WTime.frameTimeValue;
		}
	}

	// Token: 0x17001399 RID: 5017
	// (get) Token: 0x06005515 RID: 21781 RVA: 0x001B483A File Offset: 0x001B2C3A
	public static float fixedTime
	{
		get
		{
			return Time.fixedTime;
		}
	}

	// Token: 0x1700139A RID: 5018
	// (get) Token: 0x06005516 RID: 21782 RVA: 0x001B4841 File Offset: 0x001B2C41
	public static int frameCount
	{
		get
		{
			return Time.frameCount;
		}
	}

	// Token: 0x1700139B RID: 5019
	// (get) Token: 0x06005517 RID: 21783 RVA: 0x001B4848 File Offset: 0x001B2C48
	public static float maximumDeltaTime
	{
		get
		{
			return Time.maximumDeltaTime;
		}
	}

	// Token: 0x1700139C RID: 5020
	// (get) Token: 0x06005518 RID: 21784 RVA: 0x001B484F File Offset: 0x001B2C4F
	public static float realtimeSinceStartup
	{
		get
		{
			return Time.realtimeSinceStartup;
		}
	}

	// Token: 0x1700139D RID: 5021
	// (get) Token: 0x06005519 RID: 21785 RVA: 0x001B4856 File Offset: 0x001B2C56
	public static float smoothDeltaTime
	{
		get
		{
			return Time.smoothDeltaTime;
		}
	}

	// Token: 0x1700139E RID: 5022
	// (get) Token: 0x0600551A RID: 21786 RVA: 0x001B485D File Offset: 0x001B2C5D
	public static float time
	{
		get
		{
			return Time.time;
		}
	}

	// Token: 0x1700139F RID: 5023
	// (get) Token: 0x0600551B RID: 21787 RVA: 0x001B4864 File Offset: 0x001B2C64
	// (set) Token: 0x0600551C RID: 21788 RVA: 0x001B486B File Offset: 0x001B2C6B
	public static float timeScale
	{
		get
		{
			return Time.timeScale;
		}
		set
		{
			Time.timeScale = value;
		}
	}

	// Token: 0x170013A0 RID: 5024
	// (get) Token: 0x0600551D RID: 21789 RVA: 0x001B4873 File Offset: 0x001B2C73
	public static float timeSinceLevelLoaded
	{
		get
		{
			return Time.timeSinceLevelLoad;
		}
	}

	// Token: 0x170013A1 RID: 5025
	// (get) Token: 0x0600551E RID: 21790 RVA: 0x001B487A File Offset: 0x001B2C7A
	public static float unscaledDeltaTime
	{
		get
		{
			return Time.unscaledDeltaTime;
		}
	}

	// Token: 0x170013A2 RID: 5026
	// (get) Token: 0x0600551F RID: 21791 RVA: 0x001B4881 File Offset: 0x001B2C81
	public static float unscaledTime
	{
		get
		{
			return Time.unscaledTime;
		}
	}

	// Token: 0x170013A3 RID: 5027
	// (get) Token: 0x06005520 RID: 21792 RVA: 0x001B4888 File Offset: 0x001B2C88
	public static float fps
	{
		get
		{
			return 60f;
		}
	}

	// Token: 0x04003611 RID: 13841
	private static Fixed frameTimeValueF = (Fixed)((double)(1f / WTime.fps));

	// Token: 0x04003612 RID: 13842
	private static float frameTimeValue = 1f / WTime.fps;

	// Token: 0x04003613 RID: 13843
	private static Stopwatch watch;

	// Token: 0x04003614 RID: 13844
	private static double watchStartTime;
}
