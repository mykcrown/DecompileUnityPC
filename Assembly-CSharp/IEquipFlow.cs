using System;

// Token: 0x020009F9 RID: 2553
public interface IEquipFlow
{
	// Token: 0x06004933 RID: 18739
	void Start(EquippableItem item, CharacterID characterID);

	// Token: 0x06004934 RID: 18740
	void StartFromUnboxing(EquippableItem item, CharacterID characterID);

	// Token: 0x06004935 RID: 18741
	bool IsValid(EquippableItem item, CharacterID characterID);
}
