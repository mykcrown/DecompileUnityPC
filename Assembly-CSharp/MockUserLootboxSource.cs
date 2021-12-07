using System;
using System.Collections.Generic;

// Token: 0x02000761 RID: 1889
public class MockUserLootboxSource : IUserLootboxesSource
{
	// Token: 0x06002EC0 RID: 11968 RVA: 0x000EBF08 File Offset: 0x000EA308
	public Dictionary<int, int> GetLootBoxes()
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		dictionary[2] = 50;
		return dictionary;
	}
}
