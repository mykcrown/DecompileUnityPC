// Decompile from assembly: Assembly-CSharp.dll

using System;

public class Item : IItem
{
	private ItemTypes itemType;

	ItemTypes IItem.ItemType
	{
		get
		{
			return this.itemType;
		}
	}

	public Item(ItemTypes pType)
	{
		this.itemType = pType;
	}
}
