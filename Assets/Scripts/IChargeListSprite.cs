// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IChargeListSprite
{
	bool IsActive
	{
		get;
	}

	void SetActive(bool active);

	void SetPartialValue(float fraction);

	void SetMaxCharge(bool maxCharge);

	void WarnImminentLoss(float durationTillLoss);
}
