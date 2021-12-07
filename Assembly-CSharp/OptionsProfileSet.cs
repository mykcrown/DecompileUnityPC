using System;
using System.Collections.Generic;

// Token: 0x020003E1 RID: 993
[Serializable]
public class OptionsProfileSet
{
	// Token: 0x0600155E RID: 5470 RVA: 0x00076130 File Offset: 0x00074530
	public OptionsProfileSet Clone()
	{
		OptionsProfileSet optionsProfileSet = new OptionsProfileSet();
		optionsProfileSet.defaultProfile = this.defaultProfile.Clone();
		foreach (OptionsProfile optionsProfile in this.list)
		{
			optionsProfileSet.list.Add(optionsProfile.Clone());
		}
		optionsProfileSet.currentSelectedId = this.currentSelectedId;
		return optionsProfileSet;
	}

	// Token: 0x04000F23 RID: 3875
	public OptionsProfile defaultProfile;

	// Token: 0x04000F24 RID: 3876
	public List<OptionsProfile> list = new List<OptionsProfile>();

	// Token: 0x04000F25 RID: 3877
	public string currentSelectedId;
}
