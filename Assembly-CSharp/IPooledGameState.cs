using System;

// Token: 0x02000862 RID: 2146
public interface IPooledGameState
{
	// Token: 0x0600359F RID: 13727
	void Release();

	// Token: 0x17000D09 RID: 3337
	// (get) Token: 0x060035A0 RID: 13728
	bool IsAcquired { get; }
}
