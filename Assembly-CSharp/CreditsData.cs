using System;
using System.Collections.Generic;

// Token: 0x0200090E RID: 2318
public class CreditsData
{
	// Token: 0x06003C3B RID: 15419 RVA: 0x001180DE File Offset: 0x001164DE
	public void AddHeading(string heading, float spacing = 15f)
	{
		this.entries.Add(new CreditHeadingData(heading, spacing));
	}

	// Token: 0x06003C3C RID: 15420 RVA: 0x001180F2 File Offset: 0x001164F2
	public void AddCategory(string category, float spacing = 8f)
	{
		this.entries.Add(new CreditCategoryData(category, spacing));
	}

	// Token: 0x06003C3D RID: 15421 RVA: 0x00118106 File Offset: 0x00116506
	public void AddSpace(float spacing)
	{
		this.entries.Add(new CreditEntryData(spacing));
	}

	// Token: 0x06003C3E RID: 15422 RVA: 0x00118119 File Offset: 0x00116519
	public void AddNameCredit(string name, string title, float spacing = 8f)
	{
		this.entries.Add(new CreditNameData(name, title, spacing));
	}

	// Token: 0x06003C3F RID: 15423 RVA: 0x0011812E File Offset: 0x0011652E
	public void AddMessageCredit(string message, float spacing = 8f)
	{
		this.entries.Add(new CreditMessageData(message, spacing));
	}

	// Token: 0x04002954 RID: 10580
	public List<CreditEntryData> entries = new List<CreditEntryData>();

	// Token: 0x04002955 RID: 10581
	public const float DEFAULT_HEADING_SPACING = 15f;

	// Token: 0x04002956 RID: 10582
	public const float DEFAULT_NAME_SPACING = 8f;
}
