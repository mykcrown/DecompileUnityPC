using System;

namespace Commerce
{
	// Token: 0x020007B6 RID: 1974
	public enum EPurchaseResult
	{
		// Token: 0x04002251 RID: 8785
		SystemError,
		// Token: 0x04002252 RID: 8786
		Ok,
		// Token: 0x04002253 RID: 8787
		InsufficientFunds,
		// Token: 0x04002254 RID: 8788
		InvalidCurrency,
		// Token: 0x04002255 RID: 8789
		PriceMismatch,
		// Token: 0x04002256 RID: 8790
		PackageNotFound,
		// Token: 0x04002257 RID: 8791
		PlayerNotLoggedIn,
		// Token: 0x04002258 RID: 8792
		PlayerCancelledPurchase,
		// Token: 0x04002259 RID: 8793
		GetUserInfoTimedOut,
		// Token: 0x0400225A RID: 8794
		InitiateResponseTimedOut,
		// Token: 0x0400225B RID: 8795
		ConfirmTimedOut,
		// Token: 0x0400225C RID: 8796
		FinalizeTimedOut,
		// Token: 0x0400225D RID: 8797
		CompleteTimedOut,
		// Token: 0x0400225E RID: 8798
		PurchaseResultCount
	}
}
