using System;

// Token: 0x02000910 RID: 2320
public class CreditHeadingData : CreditEntryData
{
	// Token: 0x06003C41 RID: 15425 RVA: 0x0011815C File Offset: 0x0011655C
	public CreditHeadingData(string heading, float spacing) : base(spacing)
	{
		this.heading = heading;
	}

	// Token: 0x04002958 RID: 10584
	public string heading;
}
