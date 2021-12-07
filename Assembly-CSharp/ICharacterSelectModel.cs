using System;

// Token: 0x020008DB RID: 2267
public interface ICharacterSelectModel
{
	// Token: 0x06003977 RID: 14711
	SkinDefinition GetNextSkin(PlayerNum playerNum, int direction, CharacterID characterID, string skinKey, PlayerSelectionList allPlayers, SkinSelectMode selectMode);

	// Token: 0x06003978 RID: 14712
	SkinDefinition GetAvailableEquippedOrDefaultSkin(CharacterDefinition characterDef, PlayerNum playerNum, PlayerSelectionList allPlayers, SkinSelectMode selectMode);

	// Token: 0x06003979 RID: 14713
	bool IsSkinLegal(CharacterID characterId, SkinDefinition skinData, SkinSelectMode selectMode);

	// Token: 0x0600397A RID: 14714
	bool IsSkinAvailable(CharacterID characterId, SkinDefinition skinData, PlayerNum playerNum, PlayerSelectionList allPlayers, SkinSelectMode selectMode);

	// Token: 0x0600397B RID: 14715
	bool IsCharacterUnlocked(CharacterID characterId);

	// Token: 0x0600397C RID: 14716
	SkinDefinition GetDefaultSkin(CharacterID characterId, int portId);

	// Token: 0x0600397D RID: 14717
	CharacterSelectModel.HighlightInfo GetHighlightInfo(PlayerNum playerNum);

	// Token: 0x0600397E RID: 14718
	void ClearHighlightInfoSelectedSkins();

	// Token: 0x0600397F RID: 14719
	PlayerNum GetActingPlayer(int pointerId);

	// Token: 0x06003980 RID: 14720
	void SetPlayerProfileName(PlayerSelectionInfo info, string name);

	// Token: 0x06003981 RID: 14721
	void SetPlayerLocalName(PlayerSelectionInfo info, string name);

	// Token: 0x06003982 RID: 14722
	bool IsValidPayload(GameModeData modeData, GameLoadPayload payload);
}
