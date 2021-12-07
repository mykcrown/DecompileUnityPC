using System;
using System.Collections.Generic;

// Token: 0x02000803 RID: 2051
[Serializable]
public class MessageBatch
{
	// Token: 0x060032B2 RID: 12978 RVA: 0x000F32F4 File Offset: 0x000F16F4
	public void Reset()
	{
		this.events.Clear();
	}

	// Token: 0x060032B3 RID: 12979 RVA: 0x000F3301 File Offset: 0x000F1701
	public byte[] GetBytes()
	{
		return Serialization.WriteBytes(this);
	}

	// Token: 0x0400238B RID: 9099
	public List<GameEvent> events = new List<GameEvent>();
}
