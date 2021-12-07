// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ICharacterSelectModel
{
	SkinDefinition GetNextSkin(PlayerNum playerNum, int direction, CharacterID characterID, string skinKey, PlayerSelectionList allPlayers, SkinSelectMode selectMode);

	SkinDefinition GetAvailableEquippedOrDefaultSkin(CharacterDefinition characterDef, PlayerNum playerNum, PlayerSelectionList allPlayers, SkinSelectMode selectMode);

	bool IsSkinLegal(CharacterID characterId, SkinDefinition skinData, SkinSelectMode selectMode);

	bool IsSkinAvailable(CharacterID characterId, SkinDefinition skinData, PlayerNum playerNum, PlayerSelectionList allPlayers, SkinSelectMode selectMode);

	bool IsCharacterUnlocked(CharacterID characterId);

	SkinDefinition GetDefaultSkin(CharacterID characterId, int portId);

	CharacterSelectModel.HighlightInfo GetHighlightInfo(PlayerNum playerNum);

	void ClearHighlightInfoSelectedSkins();

	PlayerNum GetActingPlayer(int pointerId);

	void SetPlayerProfileName(PlayerSelectionInfo info, string name);

	void SetPlayerLocalName(PlayerSelectionInfo info, string name);

	bool IsValidPayload(GameModeData modeData, GameLoadPayload payload);
}
