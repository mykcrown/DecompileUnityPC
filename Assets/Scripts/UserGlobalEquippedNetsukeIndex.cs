// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.Serialization;

[Serializable]
public class UserGlobalEquippedNetsukeIndex : SerializableDictionary<int, UserGlobalEquippedNetsuke>
{
	public UserGlobalEquippedNetsukeIndex()
	{
	}

	public UserGlobalEquippedNetsukeIndex(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
