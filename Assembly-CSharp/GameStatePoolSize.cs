using System;

// Token: 0x02000860 RID: 2144
[AttributeUsage(AttributeTargets.Class)]
public class GameStatePoolSize : Attribute
{
	// Token: 0x06003589 RID: 13705 RVA: 0x000FDF0F File Offset: 0x000FC30F
	public GameStatePoolSize(int poolSize)
	{
		this.PoolSize = poolSize;
	}

	// Token: 0x17000D04 RID: 3332
	// (get) Token: 0x0600358A RID: 13706 RVA: 0x000FDF1E File Offset: 0x000FC31E
	// (set) Token: 0x0600358B RID: 13707 RVA: 0x000FDF26 File Offset: 0x000FC326
	public int PoolSize { get; private set; }
}
