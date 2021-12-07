// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class ScreenshotModel
{
	private ScreenshotMono screenshotMono;

	public bool InProgress
	{
		get
		{
			return this.screenshotMono.InProgress;
		}
	}

	[PostConstruct]
	public void Init()
	{
		this.screenshotMono = new GameObject("ScreenshotMono").AddComponent<ScreenshotMono>();
		ProxyMono.Attach(this.screenshotMono.gameObject);
	}

	public void SaveScreenshot(Action callback)
	{
		this.screenshotMono.SaveScreenshot(callback);
	}

	public Texture2D GetScreenshot()
	{
		return this.screenshotMono.GetScreenshot();
	}
}
