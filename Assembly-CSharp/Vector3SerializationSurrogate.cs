using System;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x02000B60 RID: 2912
internal sealed class Vector3SerializationSurrogate : ISerializationSurrogate
{
	// Token: 0x0600544A RID: 21578 RVA: 0x001B1A80 File Offset: 0x001AFE80
	public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
	{
		Vector3 vector = (Vector3)obj;
		info.AddValue("x", vector.x);
		info.AddValue("y", vector.y);
		info.AddValue("z", vector.z);
	}

	// Token: 0x0600544B RID: 21579 RVA: 0x001B1ACC File Offset: 0x001AFECC
	public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
	{
		Vector3 vector = (Vector3)obj;
		vector.x = (float)info.GetValue("x", typeof(float));
		vector.y = (float)info.GetValue("y", typeof(float));
		vector.z = (float)info.GetValue("z", typeof(float));
		obj = vector;
		return obj;
	}
}
