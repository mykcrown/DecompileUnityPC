using System;

// Token: 0x020004D4 RID: 1236
public class ItemHeldCheckComponent : MoveComponent, IMoveRequirementValidator
{
	// Token: 0x06001B43 RID: 6979 RVA: 0x0008AFFC File Offset: 0x000893FC
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

	// Token: 0x040014AC RID: 5292
	public ItemTypes ItemType;

	// Token: 0x040014AD RID: 5293
	public bool BlockedIfHeld;
}
