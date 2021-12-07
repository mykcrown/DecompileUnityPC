using System;

// Token: 0x020004E2 RID: 1250
public interface IMoveChargeComponent
{
	// Token: 0x06001B5E RID: 7006
	void OnStartCharge();

	// Token: 0x06001B5F RID: 7007
	void OnContinueCharge();

	// Token: 0x06001B60 RID: 7008
	void OnEndCharge();

	// Token: 0x06001B61 RID: 7009
	void OnFireCharge();

	// Token: 0x170005CF RID: 1487
	// (get) Token: 0x06001B62 RID: 7010
	int ChargeFireDelay { get; }
}
