using System;
using System.Collections.Generic;

// Token: 0x0200041D RID: 1053
public struct CharacterIDComparer : IEqualityComparer<CharacterID>
{
	// Token: 0x060015F1 RID: 5617 RVA: 0x00077BC5 File Offset: 0x00075FC5
	public bool Equals(CharacterID x, CharacterID y)
	{
		return x == y;
	}

	// Token: 0x060015F2 RID: 5618 RVA: 0x00077BCB File Offset: 0x00075FCB
	public int GetHashCode(CharacterID obj)
	{
		return (int)obj;
	}
}
