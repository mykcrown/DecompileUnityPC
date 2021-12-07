// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class CreditsData
{
	public List<CreditEntryData> entries = new List<CreditEntryData>();

	public const float DEFAULT_HEADING_SPACING = 15f;

	public const float DEFAULT_NAME_SPACING = 8f;

	public void AddHeading(string heading, float spacing = 15f)
	{
		this.entries.Add(new CreditHeadingData(heading, spacing));
	}

	public void AddCategory(string category, float spacing = 8f)
	{
		this.entries.Add(new CreditCategoryData(category, spacing));
	}

	public void AddSpace(float spacing)
	{
		this.entries.Add(new CreditEntryData(spacing));
	}

	public void AddNameCredit(string name, string title, float spacing = 8f)
	{
		this.entries.Add(new CreditNameData(name, title, spacing));
	}

	public void AddMessageCredit(string message, float spacing = 8f)
	{
		this.entries.Add(new CreditMessageData(message, spacing));
	}
}
