// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class EquipMethodMap : IEquipMethodMap
{
	private Dictionary<EquipmentTypes, EquipMethod> map = new Dictionary<EquipmentTypes, EquipMethod>();

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

	public EquipMethod GetMethod(EquipmentTypes type)
	{
		return this.map[type];
	}

	public List<EquipmentTypes> GetTypesWithMethod(EquipMethod method)
	{
		List<EquipmentTypes> list = new List<EquipmentTypes>();
		foreach (EquipmentTypes current in this.map.Keys)
		{
			if (this.map[current] == method)
			{
				list.Add(current);
			}
		}
		return list;
	}
}
