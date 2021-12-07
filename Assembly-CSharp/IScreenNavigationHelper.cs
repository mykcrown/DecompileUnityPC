using System;

// Token: 0x020009C7 RID: 2503
public interface IScreenNavigationHelper
{
	// Token: 0x06004630 RID: 17968
	void GoToItemInStore(EquipmentID equipId, CharacterID characterId = CharacterID.None);

	// Token: 0x06004631 RID: 17969
	void GoToOpenLootbox();
}
