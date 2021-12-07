using System;
using FixedPoint;

// Token: 0x020003C0 RID: 960
public interface IBoundsOwner
{
	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x060014D5 RID: 5333
	EnvironmentBounds Bounds { get; }

	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x060014D6 RID: 5334
	bool AllowPushing { get; }

	// Token: 0x17000410 RID: 1040
	// (get) Token: 0x060014D7 RID: 5335
	bool AllowTotalShove { get; }

	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x060014D8 RID: 5336
	Vector3F Position { get; }

	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x060014D9 RID: 5337
	Vector3F MovementVelocity { get; }

	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x060014DA RID: 5338
	CharacterData CharacterData { get; }

	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x060014DB RID: 5339
	HorizontalDirection Facing { get; }

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x060014DC RID: 5340
	PlayerPhysicsController Physics { get; }
}
