using System;

// Token: 0x02000942 RID: 2370
public interface IInputBlocker
{
	// Token: 0x06003EA0 RID: 16032
	InputBlock Request();

	// Token: 0x06003EA1 RID: 16033
	void Release(InputBlock block);

	// Token: 0x06003EA2 RID: 16034
	bool IsLocked();
}
