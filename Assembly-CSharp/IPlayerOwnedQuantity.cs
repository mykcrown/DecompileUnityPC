using System;
using FixedPoint;

// Token: 0x02000B01 RID: 2817
public interface IPlayerOwnedQuantity
{
	// Token: 0x17001301 RID: 4865
	// (get) Token: 0x060050F8 RID: 20728
	PlayerNum Player { get; }

	// Token: 0x17001302 RID: 4866
	// (get) Token: 0x060050F9 RID: 20729
	Fixed Count { get; }
}
