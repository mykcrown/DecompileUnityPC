using System;

// Token: 0x020008C7 RID: 2247
public interface IChargeListSprite
{
	// Token: 0x060038A4 RID: 14500
	void SetActive(bool active);

	// Token: 0x17000DAD RID: 3501
	// (get) Token: 0x060038A5 RID: 14501
	bool IsActive { get; }

	// Token: 0x060038A6 RID: 14502
	void SetPartialValue(float fraction);

	// Token: 0x060038A7 RID: 14503
	void SetMaxCharge(bool maxCharge);

	// Token: 0x060038A8 RID: 14504
	void WarnImminentLoss(float durationTillLoss);
}
