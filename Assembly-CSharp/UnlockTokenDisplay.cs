using System;
using UnityEngine;

// Token: 0x02000773 RID: 1907
public class UnlockTokenDisplay : BaseItem3DPreviewDisplay
{
	// Token: 0x06002F38 RID: 12088 RVA: 0x000ECF71 File Offset: 0x000EB371
	private void Awake()
	{
		this.delay = this.spinDownDelay;
		this.Preview();
	}

	// Token: 0x06002F39 RID: 12089 RVA: 0x000ECF88 File Offset: 0x000EB388
	protected override void Update()
	{
		base.Update();
		if (this.delay > 0f)
		{
			this.delay -= Time.deltaTime;
			return;
		}
		this.spinRate = Mathf.SmoothDamp(this.spinRate, this.minSpinRate, ref this.spinRateVelocity, this.spinDownTime);
		if (this.visualsRoot != null)
		{
			this.visualsRoot.localRotation *= Quaternion.AngleAxis(this.spinRate * Time.deltaTime, Vector3.up);
		}
	}

	// Token: 0x06002F3A RID: 12090 RVA: 0x000ED01E File Offset: 0x000EB41E
	public void Preview()
	{
		this.spinRate = this.maxSpinRate;
	}

	// Token: 0x040020F9 RID: 8441
	public Transform visualsRoot;

	// Token: 0x040020FA RID: 8442
	public float maxSpinRate = 720f;

	// Token: 0x040020FB RID: 8443
	public float minSpinRate = 45f;

	// Token: 0x040020FC RID: 8444
	public float spinDownTime = 1f;

	// Token: 0x040020FD RID: 8445
	public float spinDownDelay = 4f;

	// Token: 0x040020FE RID: 8446
	private float spinRate;

	// Token: 0x040020FF RID: 8447
	private float spinRateVelocity;

	// Token: 0x04002100 RID: 8448
	private float delay;
}
