using System;

// Token: 0x02000A48 RID: 2632
[Serializable]
public class MoreOptionsWasClosed : UIEvent, IUIRequest
{
	// Token: 0x06004CF3 RID: 19699 RVA: 0x0014576B File Offset: 0x00143B6B
	public MoreOptionsWasClosed(bool revertChanges)
	{
		this.revertChanges = revertChanges;
	}

	// Token: 0x04003272 RID: 12914
	public bool revertChanges;
}
