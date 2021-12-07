using System;

// Token: 0x02000762 RID: 1890
public interface IUserLootboxesModel
{
	// Token: 0x06002EC1 RID: 11969
	void Add(int itemId, int quantity);

	// Token: 0x06002EC2 RID: 11970
	int GetQuantity(int itemId);

	// Token: 0x06002EC3 RID: 11971
	int GetTotalQuantity();

	// Token: 0x06002EC4 RID: 11972
	int GetNextBoxId();
}
