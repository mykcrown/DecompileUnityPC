// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ICharacterMenusDataLoader
{
	CharacterMenusData GetData(CharacterDefinition characterDef);

	CharacterMenusData GetData(CharacterID id);
}
