using System;

// Token: 0x020003A0 RID: 928
public interface IHurtBoxOwner
{
	// Token: 0x060013FC RID: 5116
	void ToggleHurtBoxes(HurtBoxVisibilityState visState);

	// Token: 0x060013FD RID: 5117
	void ToggleBodyParts(BodyPart[] parts, HurtBoxVisibilityState visState);

	// Token: 0x060013FE RID: 5118
	bool MatchesVisibilityState(BodyPart part, HurtBoxVisibilityState visState);
}
