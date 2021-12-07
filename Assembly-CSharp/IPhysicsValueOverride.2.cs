using System;

// Token: 0x02000596 RID: 1430
public interface IPhysicsValueOverride<T> : IPhysicsValueOverride
{
	// Token: 0x1700071E RID: 1822
	// (get) Token: 0x06002040 RID: 8256
	// (set) Token: 0x06002041 RID: 8257
	T Value { get; set; }

	// Token: 0x06002042 RID: 8258
	T GetValueOrDefault(T defaultValue);
}
