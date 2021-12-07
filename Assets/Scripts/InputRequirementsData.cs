// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class InputRequirementsData
{
	public LegalInputStateData[] legalStates = new LegalInputStateData[0];

	public InputRequirementsData Clone()
	{
		return new InputRequirementsData
		{
			legalStates = (this.legalStates.Clone() as LegalInputStateData[])
		};
	}
}
