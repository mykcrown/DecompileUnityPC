// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LocalizationData
{
	public List<LocalizationRegionData> regions = new List<LocalizationRegionData>();

	public List<TextAsset> GetAssets(LocalizationRegion region)
	{
		List<TextAsset> list = new List<TextAsset>();
		foreach (LocalizationRegionData current in this.regions)
		{
			if (current.region == region)
			{
				list.Add(current.asset);
			}
		}
		return list;
	}
}
