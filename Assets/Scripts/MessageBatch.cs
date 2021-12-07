// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class MessageBatch
{
	public List<GameEvent> events = new List<GameEvent>();

	public void Reset()
	{
		this.events.Clear();
	}

	public byte[] GetBytes()
	{
		return Serialization.WriteBytes(this);
	}
}
