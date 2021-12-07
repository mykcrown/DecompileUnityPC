using System;

// Token: 0x02000B6B RID: 2923
public interface ITimeStatTracker : ITickable
{
	// Token: 0x06005491 RID: 21649
	void RecordValue(float value);

	// Token: 0x06005492 RID: 21650
	void Flush();
}
