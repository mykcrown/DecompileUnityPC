// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUnlockCharacter
{
	void SetSoftPrice(CharacterID id, ulong packageInfo, ulong price);

	void SetHardPrice(CharacterID id, ulong packageInfo, ulong price);

	void PurchaseWithHard(CharacterID characterId, Action<UserPurchaseResult> callback);

	void PurchaseWithSoft(CharacterID characterId, Action<UserPurchaseResult> callback);

	void PurchaseWithToken(CharacterID characterId, Action<UserPurchaseResult> callback);

	float GetHardPrice(CharacterID characterId);

	int GetSoftPrice(CharacterID characterId);

	string GetHardPriceString(CharacterID characterId);

	string GetSoftPriceString(CharacterID characterId);
}
