using System;
using System.Runtime.Serialization;

// Token: 0x02000744 RID: 1860
[Serializable]
public class UserGlobalEquippedNetsukeIndex : SerializableDictionary<int, UserGlobalEquippedNetsuke>
{
	// Token: 0x06002E1F RID: 11807 RVA: 0x000EA7CF File Offset: 0x000E8BCF
	public UserGlobalEquippedNetsukeIndex()
	{
	}

	// Token: 0x06002E20 RID: 11808 RVA: 0x000EA7D7 File Offset: 0x000E8BD7
	public UserGlobalEquippedNetsukeIndex(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
