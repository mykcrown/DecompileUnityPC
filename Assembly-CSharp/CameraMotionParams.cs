using System;

// Token: 0x020003F4 RID: 1012
[Serializable]
public class CameraMotionParams
{
	// Token: 0x0600159B RID: 5531 RVA: 0x00076F4B File Offset: 0x0007534B
	public CameraMotionParams(float baseSpeed)
	{
		this.baseSpeed = baseSpeed;
	}

	// Token: 0x04000FF3 RID: 4083
	public CameraMotionFunction function;

	// Token: 0x04000FF4 RID: 4084
	public float baseSpeed;

	// Token: 0x04000FF5 RID: 4085
	public bool editorToggle;

	// Token: 0x04000FF6 RID: 4086
	public bool quadraticScaling;

	// Token: 0x04000FF7 RID: 4087
	public float maxSpeed;

	// Token: 0x04000FF8 RID: 4088
	public float quadraticScalingDistance;

	// Token: 0x04000FF9 RID: 4089
	public float lazySpeed;

	// Token: 0x04000FFA RID: 4090
	public bool debugSpeed;

	// Token: 0x04000FFB RID: 4091
	public bool syncToDolly;
}
