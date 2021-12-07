using System;
using FixedPoint;

// Token: 0x020005E7 RID: 1511
public class DummyPositionOwner : IPositionOwner
{
	// Token: 0x060023AE RID: 9134 RVA: 0x000B3DE8 File Offset: 0x000B21E8
	public DummyPositionOwner(Vector3F position)
	{
		this.Center = position;
		this.Position = position;
	}

	// Token: 0x17000834 RID: 2100
	// (get) Token: 0x060023AF RID: 9135 RVA: 0x000B3E0B File Offset: 0x000B220B
	// (set) Token: 0x060023B0 RID: 9136 RVA: 0x000B3E13 File Offset: 0x000B2213
	public Vector3F Position { get; private set; }

	// Token: 0x17000835 RID: 2101
	// (get) Token: 0x060023B1 RID: 9137 RVA: 0x000B3E1C File Offset: 0x000B221C
	// (set) Token: 0x060023B2 RID: 9138 RVA: 0x000B3E24 File Offset: 0x000B2224
	public Vector3F Center { get; private set; }
}
