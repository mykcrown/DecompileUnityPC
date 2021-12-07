using System;
using FixedPoint;

// Token: 0x020005EA RID: 1514
public interface ILedgeGrabController : ITickable
{
	// Token: 0x060023C1 RID: 9153
	void OnLedgeGrabComplete();

	// Token: 0x060023C2 RID: 9154
	void ReleaseGrabbedLedge(bool unlockLedge, bool reposition);

	// Token: 0x060023C3 RID: 9155
	FixedRect GetLedgeGrabBox(HorizontalDirection facing, EnvironmentBounds bounds);

	// Token: 0x17000839 RID: 2105
	// (get) Token: 0x060023C4 RID: 9156
	bool IsLedgeGrabbing { get; }
}
