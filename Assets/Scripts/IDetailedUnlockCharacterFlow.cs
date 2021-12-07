// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IDetailedUnlockCharacterFlow
{
	void Start(CharacterID characterId, Action closeCallback);

	void StartProAccount();

	void StartFoundersPack();
}
