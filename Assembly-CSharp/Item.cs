using System;

// Token: 0x02000485 RID: 1157
public class Item : IItem
{
	// Token: 0x060018EE RID: 6382 RVA: 0x00083345 File Offset: 0x00081745
	public Item(ItemTypes pType)
	{
		this.itemType = pType;
	}

	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x060018EF RID: 6383 RVA: 0x00083354 File Offset: 0x00081754
	ItemTypes IItem.ItemType
	{
		get
		{
			return this.itemType;
		}
	}

	// Token: 0x040012E5 RID: 4837
	private ItemTypes itemType;
}
