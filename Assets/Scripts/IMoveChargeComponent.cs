// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IMoveChargeComponent
{
	int ChargeFireDelay
	{
		get;
	}

	void OnStartCharge();

	void OnContinueCharge();

	void OnEndCharge();

	void OnFireCharge();
}
