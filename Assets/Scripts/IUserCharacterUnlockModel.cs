// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUserCharacterUnlockModel
{
	bool IsUnlockedInGameMode(CharacterID characterId, GameMode gameMode);

	bool IsUnlocked(CharacterID characterId);

	void SetUnlocked(CharacterID characterId, bool dispatchUpdate = true);
}
