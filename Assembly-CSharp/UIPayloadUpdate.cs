using System;

// Token: 0x02000A5A RID: 2650
[Serializable]
public class UIPayloadUpdate : UIEvent, IUIUpdate
{
	// Token: 0x06004D04 RID: 19716 RVA: 0x001458AA File Offset: 0x00143CAA
	public UIPayloadUpdate(GameLoadPayload payload)
	{
		this.payload = payload;
	}

	// Token: 0x04003291 RID: 12945
	public GameLoadPayload payload;
}
