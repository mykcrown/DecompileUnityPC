using System;
using System.Collections.Generic;

// Token: 0x020003D3 RID: 979
[Serializable]
public class MaterialAnimationKeyValueList : List<KeyValuePair<string, MaterialAnimationData>>
{
	// Token: 0x06001548 RID: 5448 RVA: 0x000758C3 File Offset: 0x00073CC3
	public MaterialAnimationKeyValueList()
	{
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x000758CB File Offset: 0x00073CCB
	public MaterialAnimationKeyValueList(IEnumerable<KeyValuePair<string, MaterialAnimationData>> rhs) : base(rhs)
	{
	}
}
