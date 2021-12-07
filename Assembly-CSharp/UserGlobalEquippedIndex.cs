using System;
using System.Runtime.Serialization;

// Token: 0x02000743 RID: 1859
[Serializable]
public class UserGlobalEquippedIndex : SerializableDictionary<int, UserGlobalEquipped>
{
	// Token: 0x06002E1D RID: 11805 RVA: 0x000EA7BD File Offset: 0x000E8BBD
	public UserGlobalEquippedIndex()
	{
	}

	// Token: 0x06002E1E RID: 11806 RVA: 0x000EA7C5 File Offset: 0x000E8BC5
	public UserGlobalEquippedIndex(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
