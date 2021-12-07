// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IEquipFlow
{
	void Start(EquippableItem item, CharacterID characterID);

	void StartFromUnboxing(EquippableItem item, CharacterID characterID);

	bool IsValid(EquippableItem item, CharacterID characterID);
}
