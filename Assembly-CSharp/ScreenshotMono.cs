using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200094F RID: 2383
public class ScreenshotMono : MonoBehaviour
{
	// Token: 0x06003F4E RID: 16206 RVA: 0x0011FF3C File Offset: 0x0011E33C
	public void SaveScreenshot(Action callback)
	{
		this.captureScreenshotCallback = callback;
		base.StartCoroutine("CopyTexture");
	}

	// Token: 0x17000F01 RID: 3841
	// (get) Token: 0x06003F4F RID: 16207 RVA: 0x0011FF51 File Offset: 0x0011E351
	public bool InProgress
	{
		get
		{
			return this.captureScreenshotCallback != null;
		}
	}

	// Token: 0x06003F50 RID: 16208 RVA: 0x0011FF60 File Offset: 0x0011E360
	private IEnumerator CopyTexture()
	{
		yield return new WaitForEndOfFrame();
		if (this.captureScreenshotCallback != null)
		{
			Action action = this.captureScreenshotCallback;
			this.captureScreenshotCallback = null;
			this.screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			this.screenshot.filterMode = FilterMode.Point;
			this.screenshot.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0, false);
			this.screenshot.Apply();
			action();
		}
		yield break;
	}

	// Token: 0x06003F51 RID: 16209 RVA: 0x0011FF7B File Offset: 0x0011E37B
	public Texture2D GetScreenshot()
	{
		return this.screenshot;
	}

	// Token: 0x04002AEB RID: 10987
	private Texture2D screenshot;

	// Token: 0x04002AEC RID: 10988
	private Action captureScreenshotCallback;
}
