using System;

// Token: 0x02000A19 RID: 2585
public interface IPurchaseResponseHandler
{
	// Token: 0x06004B25 RID: 19237
	void HandleUnlockError(UserPurchaseResult result, IPurchaseResponseDialog dialog, Action cleanup);

	// Token: 0x06004B26 RID: 19238
	bool VerifySteam(IPurchaseResponseDialog dialog);
}
