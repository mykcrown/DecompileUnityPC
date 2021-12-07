using System;
using System.Collections.Generic;

// Token: 0x02000418 RID: 1048
public class StageDataStore : GameDataStore<StageData>
{
	// Token: 0x060015E4 RID: 5604 RVA: 0x00077A2B File Offset: 0x00075E2B
	public StageDataStore(ILocalization localization) : base(localization)
	{
	}

	// Token: 0x060015E5 RID: 5605 RVA: 0x00077A34 File Offset: 0x00075E34
	public StageData GetDataByID(StageID stageID)
	{
		return base.GetDataByID((int)stageID);
	}

	// Token: 0x060015E6 RID: 5606 RVA: 0x00077A40 File Offset: 0x00075E40
	public List<StageData> GetDataByIDs(List<StageID> stageIDs)
	{
		List<int> list = new List<int>();
		foreach (StageID item in stageIDs)
		{
			list.Add((int)item);
		}
		return base.GetDataByIDs(list);
	}
}
