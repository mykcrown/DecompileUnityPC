using System;
using System.Collections.Generic;

// Token: 0x0200072F RID: 1839
public class EquipMethodMap : IEquipMethodMap
{
	// Token: 0x06002D5A RID: 11610 RVA: 0x000E89A0 File Offset: 0x000E6DA0
	[PostConstruct]
	public void Init()
	{
		this.map[EquipmentTypes.EMOTE] = EquipMethod.TAUNT;
		this.map[EquipmentTypes.PLATFORM] = EquipMethod.CHARACTER;
		this.map[EquipmentTypes.SKIN] = EquipMethod.CHARACTER;
		this.map[EquipmentTypes.HOLOGRAM] = EquipMethod.TAUNT;
		this.map[EquipmentTypes.VICTORY_POSE] = EquipMethod.CHARACTER;
		this.map[EquipmentTypes.VOICE_TAUNT] = EquipMethod.TAUNT;
		this.map[EquipmentTypes.ANNOUNCERS] = EquipMethod.GLOBAL;
		this.map[EquipmentTypes.BLAST_ZONE] = EquipMethod.GLOBAL;
		this.map[EquipmentTypes.LOADING_SCREEN] = EquipMethod.GLOBAL;
		this.map[EquipmentTypes.NETSUKE] = EquipMethod.NETSUKE;
		this.map[EquipmentTypes.PLAYER_ICON] = EquipMethod.GLOBAL;
		this.map[EquipmentTypes.TOKEN] = EquipMethod.GLOBAL;
		this.map[EquipmentTypes.UNLOCK_TOKEN] = EquipMethod.NONE;
		this.map[EquipmentTypes.CHARACTER] = EquipMethod.NONE;
		this.map[EquipmentTypes.CURRENCY] = EquipMethod.NONE;
		this.map[EquipmentTypes.UNLOCK_TOKEN] = EquipMethod.NONE;
	}

	// Token: 0x06002D5B RID: 11611 RVA: 0x000E8A84 File Offset: 0x000E6E84
	public EquipMethod GetMethod(EquipmentTypes type)
	{
		return this.map[type];
	}

	// Token: 0x06002D5C RID: 11612 RVA: 0x000E8A94 File Offset: 0x000E6E94
	public List<EquipmentTypes> GetTypesWithMethod(EquipMethod method)
	{
		List<EquipmentTypes> list = new List<EquipmentTypes>();
		foreach (EquipmentTypes equipmentTypes in this.map.Keys)
		{
			if (this.map[equipmentTypes] == method)
			{
				list.Add(equipmentTypes);
			}
		}
		return list;
	}

	// Token: 0x04002036 RID: 8246
	private Dictionary<EquipmentTypes, EquipMethod> map = new Dictionary<EquipmentTypes, EquipMethod>();
}
