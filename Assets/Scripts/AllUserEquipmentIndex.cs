// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.Serialization;

[Serializable]
public class AllUserEquipmentIndex : SerializableDictionary<int, CharacterEquipmentMap>
{
	public AllUserEquipmentIndex()
	{
	}

	public AllUserEquipmentIndex(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
