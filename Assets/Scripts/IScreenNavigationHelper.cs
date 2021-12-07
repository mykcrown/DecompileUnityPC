// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IScreenNavigationHelper
{
	void GoToItemInStore(EquipmentID equipId, CharacterID characterId = CharacterID.None);

	void GoToOpenLootbox();
}
