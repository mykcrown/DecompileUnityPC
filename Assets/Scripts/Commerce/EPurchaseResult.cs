// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace Commerce
{
	public enum EPurchaseResult
	{
		SystemError,
		Ok,
		InsufficientFunds,
		InvalidCurrency,
		PriceMismatch,
		PackageNotFound,
		PlayerNotLoggedIn,
		PlayerCancelledPurchase,
		GetUserInfoTimedOut,
		InitiateResponseTimedOut,
		ConfirmTimedOut,
		FinalizeTimedOut,
		CompleteTimedOut,
		PurchaseResultCount
	}
}
