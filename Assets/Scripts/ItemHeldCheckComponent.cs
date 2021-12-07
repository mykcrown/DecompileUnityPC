// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ItemHeldCheckComponent : MoveComponent, IMoveRequirementValidator
{
	public ItemTypes ItemType;

	public bool BlockedIfHeld;

	public override bool ValidateRequirements(MoveData move, IPlayerDelegate pPlayerDelegate, InputButtonsData input)
	{
		if (pPlayerDelegate != null)
		{
			for (int i = 0; i < pPlayerDelegate.HeldItems.Count; i++)
			{
				IItem item = pPlayerDelegate.HeldItems[i];
				if (item.ItemType == this.ItemType)
				{
					return this.BlockedIfHeld;
				}
			}
		}
		return !this.BlockedIfHeld;
	}
}
