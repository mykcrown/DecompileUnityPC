using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x0200040D RID: 1037
[Serializable]
public class InputConfigData : ScriptableObject
{
	// Token: 0x04001083 RID: 4227
	public int tapFrames = 3;

	// Token: 0x04001084 RID: 4228
	public int doubleTapFrameThreshold = 30;

	// Token: 0x04001085 RID: 4229
	public float tapThreshold = 0.9f;

	// Token: 0x04001086 RID: 4230
	public Fixed tiltModifierInputMagnitude = (Fixed)0.6000000238418579;

	// Token: 0x04001087 RID: 4231
	public Fixed specialReverseThreshold = (Fixed)0.4000000059604645;

	// Token: 0x04001088 RID: 4232
	public int cardinalSnapAngle;

	// Token: 0x04001089 RID: 4233
	public int diagonalSnapAngle;

	// Token: 0x0400108A RID: 4234
	public WalkConfig walkOptions = new WalkConfig();

	// Token: 0x0400108B RID: 4235
	public int inputBufferFrames = 5;

	// Token: 0x0400108C RID: 4236
	public int inputBufferCollationFrames = 7;

	// Token: 0x0400108D RID: 4237
	public int inputBufferReverseFrames = 7;

	// Token: 0x0400108E RID: 4238
	public int inputShorthopFrames = 5;

	// Token: 0x0400108F RID: 4239
	public Fixed fallThroughPlatformsThreshold = (Fixed)0.800000011920929;

	// Token: 0x04001090 RID: 4240
	public int fallThroughPlatformsMaxAngle = 30;

	// Token: 0x04001091 RID: 4241
	public int pivotJumpFrames = 4;

	// Token: 0x04001092 RID: 4242
	public bool enableXInput;

	// Token: 0x04001093 RID: 4243
	public bool enableVibrate;

	// Token: 0x04001094 RID: 4244
	public InputProfileMap inputProfileMap;

	// Token: 0x04001095 RID: 4245
	public List<DefaultInputBinding> defaultBindings = new List<DefaultInputBinding>();

	// Token: 0x04001096 RID: 4246
	public List<DefaultInputBinding> defaultGCBindings = new List<DefaultInputBinding>();
}
