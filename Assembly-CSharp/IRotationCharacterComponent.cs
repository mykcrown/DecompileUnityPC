using System;
using FixedPoint;

// Token: 0x020005BF RID: 1471
public interface IRotationCharacterComponent
{
	// Token: 0x1700073B RID: 1851
	// (get) Token: 0x060020BC RID: 8380
	bool IsRotationRolled { get; }

	// Token: 0x1700073C RID: 1852
	// (get) Token: 0x060020BD RID: 8381
	Fixed Roll { get; }
}
