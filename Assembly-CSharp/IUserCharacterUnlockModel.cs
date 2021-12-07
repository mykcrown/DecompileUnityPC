using System;

// Token: 0x020006DF RID: 1759
public interface IUserCharacterUnlockModel
{
	// Token: 0x06002C43 RID: 11331
	bool IsUnlockedInGameMode(CharacterID characterId, GameMode gameMode);

	// Token: 0x06002C44 RID: 11332
	bool IsUnlocked(CharacterID characterId);

	// Token: 0x06002C45 RID: 11333
	void SetUnlocked(CharacterID characterId, bool dispatchUpdate = true);
}
