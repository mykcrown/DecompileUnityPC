using System;
using System.Runtime.Serialization;

// Token: 0x02000753 RID: 1875
[Serializable]
public class UserTauntsIndex : SerializableDictionary<int, UserTaunts>
{
	// Token: 0x06002E77 RID: 11895 RVA: 0x000EB0C8 File Offset: 0x000E94C8
	public UserTauntsIndex()
	{
	}

	// Token: 0x06002E78 RID: 11896 RVA: 0x000EB0D0 File Offset: 0x000E94D0
	public UserTauntsIndex(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
