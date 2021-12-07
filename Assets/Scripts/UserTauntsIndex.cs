// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.Serialization;

[Serializable]
public class UserTauntsIndex : SerializableDictionary<int, UserTaunts>
{
	public UserTauntsIndex()
	{
	}

	public UserTauntsIndex(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
