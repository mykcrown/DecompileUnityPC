using System;

// Token: 0x0200052F RID: 1327
public interface IMoveRequirementValidator
{
	// Token: 0x06001CC2 RID: 7362
	bool ValidateRequirements(MoveData move, IPlayerDelegate player, InputButtonsData input);
}
