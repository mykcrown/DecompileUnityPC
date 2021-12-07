using System;
using FixedPoint;

// Token: 0x020003C5 RID: 965
[Serializable]
public class ComboEscapeState : RollbackStateTyped<ComboEscapeState>
{
	// Token: 0x0600151D RID: 5405 RVA: 0x00074F1C File Offset: 0x0007331C
	public override void CopyTo(ComboEscapeState target)
	{
		target.lastInput = this.lastInput;
		target.inputsRead = this.inputsRead;
		target.escapeMultiplier = this.escapeMultiplier;
		target.escapeAngleMultiplier = this.escapeAngleMultiplier;
		target.isBounce = this.isBounce;
		target.isFlourish = this.isFlourish;
	}

	// Token: 0x0600151E RID: 5406 RVA: 0x00074F71 File Offset: 0x00073371
	public override void Clear()
	{
		base.Clear();
		this.lastInput = Vector2F.zero;
		this.inputsRead = 0;
		this.escapeMultiplier = 0;
		this.escapeAngleMultiplier = 0;
		this.isBounce = false;
		this.isFlourish = false;
	}

	// Token: 0x04000DDC RID: 3548
	public Vector2F lastInput;

	// Token: 0x04000DDD RID: 3549
	public int inputsRead;

	// Token: 0x04000DDE RID: 3550
	public Fixed escapeMultiplier;

	// Token: 0x04000DDF RID: 3551
	public Fixed escapeAngleMultiplier;

	// Token: 0x04000DE0 RID: 3552
	public bool isBounce;

	// Token: 0x04000DE1 RID: 3553
	public bool isFlourish;
}
