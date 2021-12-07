// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ICharacterDataLoader
{
	CharacterData GetData(CharacterDefinition characterDef);

	CharacterData GetData(CharacterID id);

	void Preload(CharacterDefinition characterDef, Action callback);
}
