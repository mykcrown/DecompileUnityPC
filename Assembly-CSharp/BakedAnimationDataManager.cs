using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000347 RID: 839
public class BakedAnimationDataManager : IBakedAnimationDataManager
{
	// Token: 0x060011C0 RID: 4544 RVA: 0x0006685B File Offset: 0x00064C5B
	public void Set(string characterName, BakedAnimationData data)
	{
		this.cache[characterName] = data;
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x0006686C File Offset: 0x00064C6C
	public BakedAnimationData Get(string characterName)
	{
		BakedAnimationData bakedAnimationData;
		this.cache.TryGetValue(characterName, out bakedAnimationData);
		if (bakedAnimationData == null || bakedAnimationData.dataSetForward.Count == 0)
		{
			ProfilingUtil.BeginTimer();
			TextAsset textAsset = Resources.Load<TextAsset>(BakedAnimationSerializer.GetCharacterBakedDataAssetPath(characterName, true));
			if (textAsset != null)
			{
				this.cache[characterName] = BakedAnimationSerializer.Deserialize(new MemoryStream(textAsset.bytes));
				bakedAnimationData = this.cache[characterName];
			}
			else
			{
				Debug.LogError("No baked animation data found for " + characterName);
			}
			ProfilingUtil.EndTimer("Loaded baked animations " + characterName);
		}
		return bakedAnimationData;
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x0006690B File Offset: 0x00064D0B
	public void Clear()
	{
		ProfilingUtil.BeginTimer();
		this.clearCache();
		GC.Collect();
		GC.Collect();
		ProfilingUtil.EndTimer("CLEANUP BAKED ANIMATIONS");
	}

	// Token: 0x060011C3 RID: 4547 RVA: 0x0006692C File Offset: 0x00064D2C
	public void OnApplicationQuit()
	{
		this.clearCache();
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x00066934 File Offset: 0x00064D34
	private void clearCache()
	{
		List<string> list = new List<string>();
		foreach (string item in this.cache.Keys)
		{
			list.Add(item);
		}
		foreach (string key in list)
		{
			this.cache[key] = null;
		}
		this.cache.Clear();
	}

	// Token: 0x04000B51 RID: 2897
	private Dictionary<string, BakedAnimationData> cache = new Dictionary<string, BakedAnimationData>();
}
