using System;

// Token: 0x020009E1 RID: 2529
public interface IDetailedUnlockCharacterFlow
{
	// Token: 0x060047BF RID: 18367
	void Start(CharacterID characterId, Action closeCallback);

	// Token: 0x060047C0 RID: 18368
	void StartProAccount();

	// Token: 0x060047C1 RID: 18369
	void StartFoundersPack();
}
