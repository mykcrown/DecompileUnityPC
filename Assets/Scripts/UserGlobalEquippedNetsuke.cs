// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.Serialization;

[Serializable]
public class UserGlobalEquippedNetsuke : SerializableDictionary<int, EquipmentID>
{
	public UserGlobalEquippedNetsuke()
	{
	}

	public UserGlobalEquippedNetsuke(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
