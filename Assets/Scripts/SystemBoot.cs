// Decompile from assembly: Assembly-CSharp.dll

using GameAnalyticsSDK;
using System;
using UnityEngine;

public class SystemBoot : MonoBehaviour
{
	public enum Mode
	{
		Standard,
		StagePreview,
		VictoryPosePreview,
		NoGame
	}

	private static SystemBoot instance;

	public Transform devUIContainer;

	public static SystemBoot.Mode mode
	{
		get;
		private set;
	}

	public static Action callback
	{
		get;
		private set;
	}

	public static bool started
	{
		get;
		private set;
	}

	private void Awake()
	{
		if (SystemBoot.instance != null)
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
		else
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			SystemBoot.instance = this;
			GameAnalytics.Initialize();
			GameAnalytics.NewDesignEvent("GameVersion:Core");
			new GameLoader(this, this.devUIContainer);
		}
	}

	public static void Startup(SystemBoot.Mode mode, Action callback = null)
	{
		SystemBoot.mode = mode;
		SystemBoot.callback = (Action)Delegate.Combine(SystemBoot.callback, callback);
		new GameObject("SystemBoot").AddComponent<SystemBoot>();
	}

	public static void AddStartupCallback(Action callback)
	{
		if (SystemBoot.started)
		{
			callback();
		}
		else
		{
			SystemBoot.callback = (Action)Delegate.Combine(SystemBoot.callback, callback);
		}
	}

	public static void OnStartupComplete()
	{
		if (SystemBoot.callback != null)
		{
			SystemBoot.started = true;
			Action callback = SystemBoot.callback;
			SystemBoot.callback = null;
			callback();
		}
	}
}
