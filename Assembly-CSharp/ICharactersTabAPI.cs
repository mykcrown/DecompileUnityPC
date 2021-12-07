using System;

// Token: 0x020009DD RID: 2525
public interface ICharactersTabAPI
{
	// Token: 0x17001126 RID: 4390
	// (get) Token: 0x0600478D RID: 18317
	bool SkipAnimation { get; }

	// Token: 0x0600478E RID: 18318
	void SetState(CharactersTabState state, bool skipAnimation = false);

	// Token: 0x0600478F RID: 18319
	CharactersTabState GetState();
}
