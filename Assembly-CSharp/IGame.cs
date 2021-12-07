using System;

// Token: 0x02000475 RID: 1141
public interface IGame
{
	// Token: 0x17000512 RID: 1298
	// (get) Token: 0x060018C6 RID: 6342
	bool StartedGame { get; }

	// Token: 0x17000513 RID: 1299
	// (get) Token: 0x060018C7 RID: 6343
	bool EndedGame { get; }

	// Token: 0x17000514 RID: 1300
	// (get) Token: 0x060018C8 RID: 6344
	int Frame { get; }
}
