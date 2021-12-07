// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BakedAnimationDataManager : IBakedAnimationDataManager
{
	private Dictionary<string, BakedAnimationData> cache = new Dictionary<string, BakedAnimationData>();

	public void Set(string characterName, BakedAnimationData data)
	{
		this.cache[characterName] = data;
	}

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
				UnityEngine.Debug.LogError("No baked animation data found for " + characterName);
			}
			ProfilingUtil.EndTimer("Loaded baked animations " + characterName);
		}
		return bakedAnimationData;
	}

	public void Clear()
	{
		ProfilingUtil.BeginTimer();
		this.clearCache();
		GC.Collect();
		GC.Collect();
		ProfilingUtil.EndTimer("CLEANUP BAKED ANIMATIONS");
	}

	public void OnApplicationQuit()
	{
		this.clearCache();
	}

	private void clearCache()
	{
		List<string> list = new List<string>();
		foreach (string current in this.cache.Keys)
		{
			list.Add(current);
		}
		foreach (string current2 in list)
		{
			this.cache[current2] = null;
		}
		this.cache.Clear();
	}
}
