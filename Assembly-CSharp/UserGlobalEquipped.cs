using System;
using System.Runtime.Serialization;

// Token: 0x02000745 RID: 1861
[Serializable]
public class UserGlobalEquipped : SerializableDictionary<EquipmentTypes, EquipmentID>
{
	// Token: 0x06002E21 RID: 11809 RVA: 0x000EA7E1 File Offset: 0x000E8BE1
	public UserGlobalEquipped()
	{
	}

	// Token: 0x06002E22 RID: 11810 RVA: 0x000EA7E9 File Offset: 0x000E8BE9
	public UserGlobalEquipped(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
