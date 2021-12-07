// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.Serialization;

[Serializable]
public class UserGlobalEquipped : SerializableDictionary<EquipmentTypes, EquipmentID>
{
	public UserGlobalEquipped()
	{
	}

	public UserGlobalEquipped(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
