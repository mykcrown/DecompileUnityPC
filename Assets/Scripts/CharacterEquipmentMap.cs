// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.Serialization;

[Serializable]
public class CharacterEquipmentMap : SerializableDictionary<CharacterID, SerializableDictionary<EquipmentTypes, EquipmentID>>
{
	public CharacterEquipmentMap()
	{
	}

	public CharacterEquipmentMap(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
