using System;
using System.Runtime.Serialization;

// Token: 0x0200073E RID: 1854
[Serializable]
public class CharacterEquipmentMap : SerializableDictionary<CharacterID, SerializableDictionary<EquipmentTypes, EquipmentID>>
{
	// Token: 0x06002DFD RID: 11773 RVA: 0x000EA30F File Offset: 0x000E870F
	public CharacterEquipmentMap()
	{
	}

	// Token: 0x06002DFE RID: 11774 RVA: 0x000EA317 File Offset: 0x000E8717
	public CharacterEquipmentMap(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
