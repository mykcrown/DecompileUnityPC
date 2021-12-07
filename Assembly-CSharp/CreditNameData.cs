using System;

// Token: 0x02000912 RID: 2322
public class CreditNameData : CreditEntryData
{
	// Token: 0x06003C43 RID: 15427 RVA: 0x0011817C File Offset: 0x0011657C
	public CreditNameData(string name, string title, float spacing) : base(spacing)
	{
		this.name = name;
		this.title = title;
	}

	// Token: 0x0400295A RID: 10586
	public string name;

	// Token: 0x0400295B RID: 10587
	public string title;

	// Token: 0x0400295C RID: 10588
	public const float DEFAULT_NAME_SPACING = 8f;
}
