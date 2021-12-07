using System;

// Token: 0x020002AE RID: 686
public struct AudioReference
{
	// Token: 0x06000EE8 RID: 3816 RVA: 0x0005B3E7 File Offset: 0x000597E7
	public AudioReference(IAudioOwner owner, int sourceId)
	{
		this.owner = owner;
		this.sourceId = sourceId;
	}

	// Token: 0x0400089C RID: 2204
	public IAudioOwner owner;

	// Token: 0x0400089D RID: 2205
	public int sourceId;
}
