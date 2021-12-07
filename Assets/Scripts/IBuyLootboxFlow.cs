// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IBuyLootboxFlow
{
	void Start(ulong packageId, Action<UserPurchaseResult> callback);
}
