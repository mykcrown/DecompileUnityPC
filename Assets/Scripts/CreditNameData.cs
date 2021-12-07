// Decompile from assembly: Assembly-CSharp.dll

using System;

public class CreditNameData : CreditEntryData
{
	public string name;

	public string title;

	public const float DEFAULT_NAME_SPACING = 8f;

	public CreditNameData(string name, string title, float spacing) : base(spacing)
	{
		this.name = name;
		this.title = title;
	}
}
