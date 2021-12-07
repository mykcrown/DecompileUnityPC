// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IBuyLootBox
{
	void Purchase(ulong packageId, Action<UserPurchaseResult> callback);

	CurrencyType GetCurrencyType(ulong packageId);
}
