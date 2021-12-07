// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IHurtBoxOwner
{
	void ToggleHurtBoxes(HurtBoxVisibilityState visState);

	void ToggleBodyParts(BodyPart[] parts, HurtBoxVisibilityState visState);

	bool MatchesVisibilityState(BodyPart part, HurtBoxVisibilityState visState);
}
