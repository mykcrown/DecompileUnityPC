using System;

// Token: 0x0200050A RID: 1290
[Serializable]
public class MoveUseOptions
{
	// Token: 0x04001674 RID: 5748
	public bool trackUses;

	// Token: 0x04001675 RID: 5749
	public int numberOfUses = 1;

	// Token: 0x04001676 RID: 5750
	public bool resetOnDeath = true;

	// Token: 0x04001677 RID: 5751
	public bool resetOnGrabbed;

	// Token: 0x04001678 RID: 5752
	public bool resetOnDealHit;

	// Token: 0x04001679 RID: 5753
	public bool resetOnReceiveHit;

	// Token: 0x0400167A RID: 5754
	public bool resetIfGrounded;

	// Token: 0x0400167B RID: 5755
	public bool resetIfGrabLedge;
}
