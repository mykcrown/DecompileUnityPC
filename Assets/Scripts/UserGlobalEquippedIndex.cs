// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.Serialization;

[Serializable]
public class UserGlobalEquippedIndex : SerializableDictionary<int, UserGlobalEquipped>
{
	public UserGlobalEquippedIndex()
	{
	}

	public UserGlobalEquippedIndex(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
