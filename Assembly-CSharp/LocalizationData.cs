using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006CC RID: 1740
[Serializable]
public class LocalizationData
{
	// Token: 0x06002BA7 RID: 11175 RVA: 0x000E3A90 File Offset: 0x000E1E90
	public List<TextAsset> GetAssets(LocalizationRegion region)
	{
		List<TextAsset> list = new List<TextAsset>();
		foreach (LocalizationRegionData localizationRegionData in this.regions)
		{
			if (localizationRegionData.region == region)
			{
				list.Add(localizationRegionData.asset);
			}
		}
		return list;
	}

	// Token: 0x04001F1F RID: 7967
	public List<LocalizationRegionData> regions = new List<LocalizationRegionData>();
}
