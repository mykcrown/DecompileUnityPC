using System;

// Token: 0x02000806 RID: 2054
[Serializable]
public class DisconnectUpdate : GameEvent, IUIUpdate
{
	// Token: 0x060032B6 RID: 12982 RVA: 0x000F3324 File Offset: 0x000F1724
	public DisconnectUpdate(NetErrorCode code)
	{
		this.errorCode = code;
	}

	// Token: 0x0400238D RID: 9101
	public NetErrorCode errorCode;
}
