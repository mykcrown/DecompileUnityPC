using System;

// Token: 0x02000904 RID: 2308
public interface ITokenManager
{
	// Token: 0x06003BFF RID: 15359
	void Init(Func<PlayerNum, IPlayerCursor> findPlayerCursor);

	// Token: 0x06003C00 RID: 15360
	void Reset();

	// Token: 0x06003C01 RID: 15361
	void ReleaseFunctions();

	// Token: 0x06003C02 RID: 15362
	PlayerToken GetCurrentlyGrabbing(PlayerNum playerNum);

	// Token: 0x06003C03 RID: 15363
	void ReleaseAnyGrabbers(PlayerToken token);

	// Token: 0x06003C04 RID: 15364
	void ReleaseToken(PlayerNum player);

	// Token: 0x06003C05 RID: 15365
	void GrabToken(PlayerNum player, PlayerToken token, float clickTime = 0f);

	// Token: 0x06003C06 RID: 15366
	PlayerNum IsBeingGrabbedByPlayer(PlayerToken token);

	// Token: 0x06003C07 RID: 15367
	PlayerToken GetPlayerToken(PlayerNum playerNum);

	// Token: 0x06003C08 RID: 15368
	PlayerToken[] GetAll();

	// Token: 0x06003C09 RID: 15369
	void AddToken(PlayerNum owner, PlayerToken token);

	// Token: 0x06003C0A RID: 15370
	void RemoveToken(PlayerNum owner, PlayerToken token);
}
