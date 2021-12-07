using System;
using System.Runtime.Serialization;

// Token: 0x02000746 RID: 1862
[Serializable]
public class UserGlobalEquippedNetsuke : SerializableDictionary<int, EquipmentID>
{
	// Token: 0x06002E23 RID: 11811 RVA: 0x000EA7F3 File Offset: 0x000E8BF3
	public UserGlobalEquippedNetsuke()
	{
	}

	// Token: 0x06002E24 RID: 11812 RVA: 0x000EA7FB File Offset: 0x000E8BFB
	public UserGlobalEquippedNetsuke(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
