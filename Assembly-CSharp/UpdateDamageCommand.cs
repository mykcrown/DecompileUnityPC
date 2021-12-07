using System;
using FixedPoint;

// Token: 0x02000B00 RID: 2816
public class UpdateDamageCommand : GameEvent
{
	// Token: 0x060050F7 RID: 20727 RVA: 0x00150A81 File Offset: 0x0014EE81
	public UpdateDamageCommand(Fixed damage, PlayerNum playerNum)
	{
		this.PlayerNum = playerNum;
		this.Damage = damage;
	}

	// Token: 0x04003448 RID: 13384
	public PlayerNum PlayerNum;

	// Token: 0x04003449 RID: 13385
	public Fixed Damage;
}
