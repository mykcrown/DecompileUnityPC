using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003F9 RID: 1017
[Serializable]
public class CanvasScalerConfig
{
	// Token: 0x04001009 RID: 4105
	public float defaultSpriteDPI = 96f;

	// Token: 0x0400100A RID: 4106
	public float fallbackScreenDPI = 96f;

	// Token: 0x0400100B RID: 4107
	public float matchWidthOrHeight;

	// Token: 0x0400100C RID: 4108
	public CanvasScaler.Unit physicalUnit = CanvasScaler.Unit.Points;

	// Token: 0x0400100D RID: 4109
	public float referencePixelsPerUnit = 100f;

	// Token: 0x0400100E RID: 4110
	public Vector2 referenceResolution = new Vector2(1920f, 1080f);

	// Token: 0x0400100F RID: 4111
	public float scaleFactor = 1f;

	// Token: 0x04001010 RID: 4112
	public CanvasScaler.ScreenMatchMode screenMatchMode;

	// Token: 0x04001011 RID: 4113
	public CanvasScaler.ScaleMode scaleMode;
}
