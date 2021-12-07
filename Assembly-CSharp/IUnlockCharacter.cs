using System;

// Token: 0x020006DC RID: 1756
public interface IUnlockCharacter
{
	// Token: 0x06002C2B RID: 11307
	void SetSoftPrice(CharacterID id, ulong packageInfo, ulong price);

	// Token: 0x06002C2C RID: 11308
	void SetHardPrice(CharacterID id, ulong packageInfo, ulong price);

	// Token: 0x06002C2D RID: 11309
	void PurchaseWithHard(CharacterID characterId, Action<UserPurchaseResult> callback);

	// Token: 0x06002C2E RID: 11310
	void PurchaseWithSoft(CharacterID characterId, Action<UserPurchaseResult> callback);

	// Token: 0x06002C2F RID: 11311
	void PurchaseWithToken(CharacterID characterId, Action<UserPurchaseResult> callback);

	// Token: 0x06002C30 RID: 11312
	float GetHardPrice(CharacterID characterId);

	// Token: 0x06002C31 RID: 11313
	int GetSoftPrice(CharacterID characterId);

	// Token: 0x06002C32 RID: 11314
	string GetHardPriceString(CharacterID characterId);

	// Token: 0x06002C33 RID: 11315
	string GetSoftPriceString(CharacterID characterId);
}
