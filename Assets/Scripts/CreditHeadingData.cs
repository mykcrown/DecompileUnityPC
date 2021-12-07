// Decompile from assembly: Assembly-CSharp.dll

using System;

public class CreditHeadingData : CreditEntryData
{
	public string heading;

	public CreditHeadingData(string heading, float spacing) : base(spacing)
	{
		this.heading = heading;
	}
}
