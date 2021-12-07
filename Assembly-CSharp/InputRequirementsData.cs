using System;

// Token: 0x02000481 RID: 1153
[Serializable]
public class InputRequirementsData
{
	// Token: 0x060018E5 RID: 6373 RVA: 0x00082FE0 File Offset: 0x000813E0
	public InputRequirementsData Clone()
	{
		return new InputRequirementsData
		{
			legalStates = (this.legalStates.Clone() as LegalInputStateData[])
		};
	}

	// Token: 0x040012C8 RID: 4808
	public LegalInputStateData[] legalStates = new LegalInputStateData[0];
}
