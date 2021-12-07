using System;
using FixedPoint;

// Token: 0x02000AFE RID: 2814
public class ChargeLossWarningEvent : GameEvent, IPlayerOwnedQuantity
{
	// Token: 0x060050EF RID: 20719 RVA: 0x00150A29 File Offset: 0x0014EE29
	public ChargeLossWarningEvent(PlayerNum player, Fixed chargeLevel, Fixed timeTilLoss)
	{
		this.Player = player;
		this.Count = chargeLevel;
		this.TimeTilLoss = timeTilLoss;
	}

	// Token: 0x170012FE RID: 4862
	// (get) Token: 0x060050F0 RID: 20720 RVA: 0x00150A46 File Offset: 0x0014EE46
	// (set) Token: 0x060050F1 RID: 20721 RVA: 0x00150A4E File Offset: 0x0014EE4E
	public PlayerNum Player { get; set; }

	// Token: 0x170012FF RID: 4863
	// (get) Token: 0x060050F2 RID: 20722 RVA: 0x00150A57 File Offset: 0x0014EE57
	// (set) Token: 0x060050F3 RID: 20723 RVA: 0x00150A5F File Offset: 0x0014EE5F
	public Fixed Count { get; set; }

	// Token: 0x17001300 RID: 4864
	// (get) Token: 0x060050F4 RID: 20724 RVA: 0x00150A68 File Offset: 0x0014EE68
	// (set) Token: 0x060050F5 RID: 20725 RVA: 0x00150A70 File Offset: 0x0014EE70
	public Fixed TimeTilLoss { get; set; }
}
