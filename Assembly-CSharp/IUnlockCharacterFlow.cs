using System;

// Token: 0x020009E9 RID: 2537
public interface IUnlockCharacterFlow
{
	// Token: 0x0600481F RID: 18463
	void Start(CharacterID characterId, Action closeCallback);

	// Token: 0x06004820 RID: 18464
	void StartWithTimer(CharacterID characterId, Action closeCallback, float endTime);

	// Token: 0x06004821 RID: 18465
	void StartAsPrompt(CharacterID characterId, Action closeCallback);
}
