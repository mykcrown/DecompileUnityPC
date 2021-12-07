using System;

// Token: 0x02000929 RID: 2345
[Flags]
public enum DebugDrawChannel
{
	// Token: 0x040029C1 RID: 10689
	None = 0,
	// Token: 0x040029C2 RID: 10690
	Physics = 1,
	// Token: 0x040029C3 RID: 10691
	HitBoxes = 2,
	// Token: 0x040029C4 RID: 10692
	HurtBoxes = 4,
	// Token: 0x040029C5 RID: 10693
	Bounds = 8,
	// Token: 0x040029C6 RID: 10694
	Camera = 16,
	// Token: 0x040029C7 RID: 10695
	Input = 32,
	// Token: 0x040029C8 RID: 10696
	Impact = 64,
	// Token: 0x040029C9 RID: 10697
	Grid = 128,
	// Token: 0x040029CA RID: 10698
	All = -1
}
