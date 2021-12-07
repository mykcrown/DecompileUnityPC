using System;

// Token: 0x02000911 RID: 2321
public class CreditCategoryData : CreditEntryData
{
	// Token: 0x06003C42 RID: 15426 RVA: 0x0011816C File Offset: 0x0011656C
	public CreditCategoryData(string category, float spacing) : base(spacing)
	{
		this.category = category;
	}

	// Token: 0x04002959 RID: 10585
	public string category;
}
