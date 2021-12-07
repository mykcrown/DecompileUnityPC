using System;
using UnityEngine;

// Token: 0x0200094E RID: 2382
public class ScreenshotModel
{
	// Token: 0x06003F49 RID: 16201 RVA: 0x0011FEE5 File Offset: 0x0011E2E5
	[PostConstruct]
	public void Init()
	{
		this.screenshotMono = new GameObject("ScreenshotMono").AddComponent<ScreenshotMono>();
		ProxyMono.Attach(this.screenshotMono.gameObject);
	}

	// Token: 0x06003F4A RID: 16202 RVA: 0x0011FF0C File Offset: 0x0011E30C
	public void SaveScreenshot(Action callback)
	{
		this.screenshotMono.SaveScreenshot(callback);
	}

	// Token: 0x17000F00 RID: 3840
	// (get) Token: 0x06003F4B RID: 16203 RVA: 0x0011FF1A File Offset: 0x0011E31A
	public bool InProgress
	{
		get
		{
			return this.screenshotMono.InProgress;
		}
	}

	// Token: 0x06003F4C RID: 16204 RVA: 0x0011FF27 File Offset: 0x0011E327
	public Texture2D GetScreenshot()
	{
		return this.screenshotMono.GetScreenshot();
	}

	// Token: 0x04002AEA RID: 10986
	private ScreenshotMono screenshotMono;
}
