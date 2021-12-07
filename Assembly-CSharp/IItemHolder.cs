using System;
using System.Collections.Generic;

// Token: 0x02000484 RID: 1156
public interface IItemHolder
{
	// Token: 0x060018EB RID: 6379
	void GiveItem(IItem pItem);

	// Token: 0x060018EC RID: 6380
	void TakeItem(IItem pItem);

	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x060018ED RID: 6381
	List<IItem> HeldItems { get; }
}
