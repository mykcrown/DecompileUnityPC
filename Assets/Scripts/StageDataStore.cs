// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class StageDataStore : GameDataStore<StageData>
{
	public StageDataStore(ILocalization localization) : base(localization)
	{
	}

	public StageData GetDataByID(StageID stageID)
	{
		return base.GetDataByID((int)stageID);
	}

	public new List<StageData> GetDataByIDs(List<StageID> stageIDs)
	{
		List<int> list = new List<int>();
		foreach (StageID current in stageIDs)
		{
			list.Add((int)current);
		}
		return base.GetDataByIDs(list);
	}
}
