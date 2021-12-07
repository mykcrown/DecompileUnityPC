using System;
using System.Runtime.Serialization;

// Token: 0x0200073F RID: 1855
[Serializable]
public class AllUserEquipmentIndex : SerializableDictionary<int, CharacterEquipmentMap>
{
	// Token: 0x06002DFF RID: 11775 RVA: 0x000EA321 File Offset: 0x000E8721
	public AllUserEquipmentIndex()
	{
	}

	// Token: 0x06002E00 RID: 11776 RVA: 0x000EA329 File Offset: 0x000E8729
	public AllUserEquipmentIndex(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
