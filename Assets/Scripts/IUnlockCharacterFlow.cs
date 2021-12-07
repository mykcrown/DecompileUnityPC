// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUnlockCharacterFlow
{
	void Start(CharacterID characterId, Action closeCallback);

	void StartWithTimer(CharacterID characterId, Action closeCallback, float endTime);

	void StartAsPrompt(CharacterID characterId, Action closeCallback);
}
