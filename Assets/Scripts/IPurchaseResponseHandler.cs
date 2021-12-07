// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IPurchaseResponseHandler
{
	void HandleUnlockError(UserPurchaseResult result, IPurchaseResponseDialog dialog, Action cleanup);

	bool VerifySteam(IPurchaseResponseDialog dialog);
}
