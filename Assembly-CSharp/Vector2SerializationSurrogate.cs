using System;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x02000B5F RID: 2911
internal sealed class Vector2SerializationSurrogate : ISerializationSurrogate
{
	// Token: 0x06005447 RID: 21575 RVA: 0x001B19E0 File Offset: 0x001AFDE0
	public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
	{
		Vector2 vector = (Vector2)obj;
		info.AddValue("x", vector.x);
		info.AddValue("y", vector.y);
	}

	// Token: 0x06005448 RID: 21576 RVA: 0x001B1A18 File Offset: 0x001AFE18
	public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
	{
		Vector2 vector = (Vector2)obj;
		vector.x = (float)info.GetValue("x", typeof(float));
		vector.y = (float)info.GetValue("y", typeof(float));
		obj = vector;
		return obj;
	}
}
