using System;
using FixedPoint;

// Token: 0x020003CA RID: 970
[Serializable]
public class FlourishConfig
{
	// Token: 0x04000E2C RID: 3628
	public bool useKillFlourish;

	// Token: 0x04000E2D RID: 3629
	public bool lastStockOnly;

	// Token: 0x04000E2E RID: 3630
	public Fixed requiredOverkill;

	// Token: 0x04000E2F RID: 3631
	public Fixed minKnockback = 30;

	// Token: 0x04000E30 RID: 3632
	public Fixed minDamage = 10;

	// Token: 0x04000E31 RID: 3633
	public Fixed minDistanceX = -(Fixed)0.5;

	// Token: 0x04000E32 RID: 3634
	public Fixed maxDistanceX = (Fixed)1.0;

	// Token: 0x04000E33 RID: 3635
	public Fixed minDistanceY = -(Fixed)0.0;

	// Token: 0x04000E34 RID: 3636
	public Fixed maxDistanceY = (Fixed)1.0;

	// Token: 0x04000E35 RID: 3637
	public Fixed secondRaycastOffsetY = (Fixed)1.0;

	// Token: 0x04000E36 RID: 3638
	public bool predictDI = true;

	// Token: 0x04000E37 RID: 3639
	public bool printDebug;

	// Token: 0x04000E38 RID: 3640
	public Fixed cameraZoomSpeed = 1;

	// Token: 0x04000E39 RID: 3641
	public int hitLagFrames = 45;

	// Token: 0x04000E3A RID: 3642
	public bool disableVibrate;

	// Token: 0x04000E3B RID: 3643
	public bool disableCameraShake;

	// Token: 0x04000E3C RID: 3644
	public int pauseVfxFrame;

	// Token: 0x04000E3D RID: 3645
	public int advanceFrames;

	// Token: 0x04000E3E RID: 3646
	public bool highlightAttacker = true;

	// Token: 0x04000E3F RID: 3647
	public bool highlightReceiver;

	// Token: 0x04000E40 RID: 3648
	public bool gravityAssist;

	// Token: 0x04000E41 RID: 3649
	public int gravityAssistFrames = 20;

	// Token: 0x04000E42 RID: 3650
	public float cameraBoxWidth = 3f;

	// Token: 0x04000E43 RID: 3651
	public float cameraBoxHeight = 3f;

	// Token: 0x04000E44 RID: 3652
	public bool stopSDI = true;

	// Token: 0x04000E45 RID: 3653
	public Fixed increaseKnockback = 0;

	// Token: 0x04000E46 RID: 3654
	public Fixed miniZoomSpeed = 1;
}
