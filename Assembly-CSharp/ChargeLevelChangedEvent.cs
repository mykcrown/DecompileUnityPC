using System;
using Beebyte.Obfuscator;
using FixedPoint;

// Token: 0x02000AFD RID: 2813
[SkipRename]
public class ChargeLevelChangedEvent : GameEvent, IPlayerOwnedQuantity
{
	// Token: 0x060050EA RID: 20714 RVA: 0x001509F1 File Offset: 0x0014EDF1
	public ChargeLevelChangedEvent(PlayerNum player, Fixed chargeLevel)
	{
		this.Player = player;
		this.Count = chargeLevel;
	}

	// Token: 0x170012FC RID: 4860
	// (get) Token: 0x060050EB RID: 20715 RVA: 0x00150A07 File Offset: 0x0014EE07
	// (set) Token: 0x060050EC RID: 20716 RVA: 0x00150A0F File Offset: 0x0014EE0F
	public PlayerNum Player { get; set; }

	// Token: 0x170012FD RID: 4861
	// (get) Token: 0x060050ED RID: 20717 RVA: 0x00150A18 File Offset: 0x0014EE18
	// (set) Token: 0x060050EE RID: 20718 RVA: 0x00150A20 File Offset: 0x0014EE20
	public Fixed Count { get; set; }
}
