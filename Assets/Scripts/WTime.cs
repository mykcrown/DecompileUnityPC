// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Diagnostics;
using UnityEngine;

public class WTime
{
	private static Fixed frameTimeValueF = (Fixed)((double)(1f / WTime.fps));

	private static float frameTimeValue = 1f / WTime.fps;

	private static Stopwatch watch;

	private static double watchStartTime;

	public static float deltaTime
	{
		get
		{
			return FrameController.FrameDeltaTime;
		}
	}

	public static double precisionTimeSinceStartup
	{
		get
		{
			return WTime.watchStartTime + WTime.watch.Elapsed.TotalMilliseconds;
		}
	}

	public static long currentTimeMs
	{
		get
		{
			return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
		}
	}

	public static Fixed fixedDeltaTime
	{
		get
		{
			return WTime.frameTimeValueF;
		}
	}

	public static float frameTime
	{
		get
		{
			return WTime.frameTimeValue;
		}
	}

	public static float fixedTime
	{
		get
		{
			return Time.fixedTime;
		}
	}

	public static int frameCount
	{
		get
		{
			return Time.frameCount;
		}
	}

	public static float maximumDeltaTime
	{
		get
		{
			return Time.maximumDeltaTime;
		}
	}

	public static float realtimeSinceStartup
	{
		get
		{
			return Time.realtimeSinceStartup;
		}
	}

	public static float smoothDeltaTime
	{
		get
		{
			return Time.smoothDeltaTime;
		}
	}

	public static float time
	{
		get
		{
			return Time.time;
		}
	}

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

	public static float timeSinceLevelLoaded
	{
		get
		{
			return Time.timeSinceLevelLoad;
		}
	}

	public static float unscaledDeltaTime
	{
		get
		{
			return Time.unscaledDeltaTime;
		}
	}

	public static float unscaledTime
	{
		get
		{
			return Time.unscaledTime;
		}
	}

	public static float fps
	{
		get
		{
			return 60f;
		}
	}

	public static void Startup()
	{
		WTime.watch = new Stopwatch();
		WTime.watch.Start();
		WTime.watchStartTime = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
	}
}
