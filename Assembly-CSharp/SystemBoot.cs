using System;
using GameAnalyticsSDK;
using UnityEngine;

// Token: 0x020008A4 RID: 2212
public class SystemBoot : MonoBehaviour
{
	// Token: 0x17000D74 RID: 3444
	// (get) Token: 0x06003765 RID: 14181 RVA: 0x00102A45 File Offset: 0x00100E45
	// (set) Token: 0x06003766 RID: 14182 RVA: 0x00102A4C File Offset: 0x00100E4C
	public static SystemBoot.Mode mode { get; private set; }

	// Token: 0x17000D75 RID: 3445
	// (get) Token: 0x06003767 RID: 14183 RVA: 0x00102A54 File Offset: 0x00100E54
	// (set) Token: 0x06003768 RID: 14184 RVA: 0x00102A5B File Offset: 0x00100E5B
	public static Action callback { get; private set; }

	// Token: 0x17000D76 RID: 3446
	// (get) Token: 0x06003769 RID: 14185 RVA: 0x00102A63 File Offset: 0x00100E63
	// (set) Token: 0x0600376A RID: 14186 RVA: 0x00102A6A File Offset: 0x00100E6A
	public static bool started { get; private set; }

	// Token: 0x0600376B RID: 14187 RVA: 0x00102A74 File Offset: 0x00100E74
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

	// Token: 0x0600376C RID: 14188 RVA: 0x00102ACE File Offset: 0x00100ECE
	public static void Startup(SystemBoot.Mode mode, Action callback = null)
	{
		SystemBoot.mode = mode;
		SystemBoot.callback = (Action)Delegate.Combine(SystemBoot.callback, callback);
		new GameObject("SystemBoot").AddComponent<SystemBoot>();
	}

	// Token: 0x0600376D RID: 14189 RVA: 0x00102AFB File Offset: 0x00100EFB
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

	// Token: 0x0600376E RID: 14190 RVA: 0x00102B28 File Offset: 0x00100F28
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

	// Token: 0x0400259B RID: 9627
	private static SystemBoot instance;

	// Token: 0x0400259F RID: 9631
	public Transform devUIContainer;

	// Token: 0x020008A5 RID: 2213
	public enum Mode
	{
		// Token: 0x040025A1 RID: 9633
		Standard,
		// Token: 0x040025A2 RID: 9634
		StagePreview,
		// Token: 0x040025A3 RID: 9635
		VictoryPosePreview,
		// Token: 0x040025A4 RID: 9636
		NoGame
	}
}
