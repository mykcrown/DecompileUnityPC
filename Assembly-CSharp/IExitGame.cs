using System;

// Token: 0x02000488 RID: 1160
public interface IExitGame
{
	// Token: 0x06001903 RID: 6403
	void InstantTerminate();

	// Token: 0x06001904 RID: 6404
	void DestroyGameManager();

	// Token: 0x06001905 RID: 6405
	void ExitGameMode(Action callback);
}
