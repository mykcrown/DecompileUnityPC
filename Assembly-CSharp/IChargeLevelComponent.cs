using System;
using FixedPoint;

// Token: 0x020005BC RID: 1468
public interface IChargeLevelComponent
{
	// Token: 0x17000736 RID: 1846
	// (get) Token: 0x060020B4 RID: 8372
	Fixed ChargeLevel { get; }

	// Token: 0x060020B5 RID: 8373
	void OnChargeMoveUsed();

	// Token: 0x17000737 RID: 1847
	// (get) Token: 0x060020B6 RID: 8374
	int ChargeFrames { get; }
}
