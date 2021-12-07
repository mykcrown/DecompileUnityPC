using System;

// Token: 0x02000913 RID: 2323
public class CreditMessageData : CreditEntryData
{
	// Token: 0x06003C44 RID: 15428 RVA: 0x00118193 File Offset: 0x00116593
	public CreditMessageData(string message, float spacing) : base(spacing)
	{
		this.message = message;
	}

	// Token: 0x0400295D RID: 10589
	public string message;
}
