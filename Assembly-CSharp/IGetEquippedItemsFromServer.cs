using System;

// Token: 0x0200073A RID: 1850
public interface IGetEquippedItemsFromServer
{
	// Token: 0x06002DC2 RID: 11714
	void MakeRequest();

	// Token: 0x17000B32 RID: 2866
	// (get) Token: 0x06002DC3 RID: 11715
	bool IsComplete { get; }
}
