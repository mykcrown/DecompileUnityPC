using System;

// Token: 0x02000AFC RID: 2812
public class SetChargeLevelCommand : GameEvent
{
	// Token: 0x060050E5 RID: 20709 RVA: 0x001509B9 File Offset: 0x0014EDB9
	public SetChargeLevelCommand(PlayerNum player, int chargeLevel)
	{
		this.Player = player;
		this.Count = chargeLevel;
	}

	// Token: 0x170012FA RID: 4858
	// (get) Token: 0x060050E6 RID: 20710 RVA: 0x001509CF File Offset: 0x0014EDCF
	// (set) Token: 0x060050E7 RID: 20711 RVA: 0x001509D7 File Offset: 0x0014EDD7
	public PlayerNum Player { get; set; }

	// Token: 0x170012FB RID: 4859
	// (get) Token: 0x060050E8 RID: 20712 RVA: 0x001509E0 File Offset: 0x0014EDE0
	// (set) Token: 0x060050E9 RID: 20713 RVA: 0x001509E8 File Offset: 0x0014EDE8
	public int Count { get; set; }
}
