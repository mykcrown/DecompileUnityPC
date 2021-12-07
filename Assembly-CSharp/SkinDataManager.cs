using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000607 RID: 1543
public class SkinDataManager : ISkinDataManager
{
	// Token: 0x17000963 RID: 2403
	// (get) Token: 0x060025F9 RID: 9721 RVA: 0x000BBA69 File Offset: 0x000B9E69
	// (set) Token: 0x060025FA RID: 9722 RVA: 0x000BBA71 File Offset: 0x000B9E71
	[Inject]
	public ICharacterLists characterLists { private get; set; }

	// Token: 0x17000964 RID: 2404
	// (get) Token: 0x060025FB RID: 9723 RVA: 0x000BBA7A File Offset: 0x000B9E7A
	// (set) Token: 0x060025FC RID: 9724 RVA: 0x000BBA82 File Offset: 0x000B9E82
	[Inject]
	public IResourceLoader loader { get; set; }

	// Token: 0x17000965 RID: 2405
	// (get) Token: 0x060025FD RID: 9725 RVA: 0x000BBA8B File Offset: 0x000B9E8B
	// (set) Token: 0x060025FE RID: 9726 RVA: 0x000BBA93 File Offset: 0x000B9E93
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x060025FF RID: 9727 RVA: 0x000BBA9C File Offset: 0x000B9E9C
	public void ClearUnused(List<string> keepList)
	{
		List<string> list = new List<string>();
		foreach (string item in this.skinDataCache.Keys)
		{
			if (!keepList.Contains(item))
			{
				list.Add(item);
			}
		}
		foreach (string key in list)
		{
			this.unloadSkin(key);
		}
		GC.Collect();
	}

	// Token: 0x06002600 RID: 9728 RVA: 0x000BBB5C File Offset: 0x000B9F5C
	private void loadBundle(string path, Action<AssetBundle> callback)
	{
		AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
		Action tick = null;
		tick = delegate()
		{
			if (request.isDone)
			{
				callback(request.assetBundle);
				return;
			}
			this.timer.NextFrame(tick);
		};
		this.timer.NextFrame(tick);
	}

	// Token: 0x06002601 RID: 9729 RVA: 0x000BBBB4 File Offset: 0x000B9FB4
	private void loadSkinFromBundle(AssetBundle bundle, Action<SkinData> callback)
	{
		AssetBundleRequest request = bundle.LoadAllAssetsAsync<SkinData>();
		Action tick = null;
		tick = delegate()
		{
			if (request.isDone)
			{
				callback((SkinData)request.allAssets[0]);
				return;
			}
			this.timer.NextFrame(tick);
		};
		this.timer.NextFrame(tick);
	}

	// Token: 0x06002602 RID: 9730 RVA: 0x000BBC0C File Offset: 0x000BA00C
	private void loadSkinData(SkinDefinition skinDefinition, string key)
	{
		ProfilingUtil.BeginTimer();
		this.stringBuilder.Remove(0, this.stringBuilder.Length);
		this.stringBuilder.Append(Application.streamingAssetsPath);
		this.stringBuilder.Append("/AssetBundles/Skins/");
		this.stringBuilder.Append(skinDefinition.dataFile.Replace("SkinData/", string.Empty));
		this.stringBuilder.Append(".assetbundle");
		string path = this.stringBuilder.ToString();
		this.loadBundle(path, delegate(AssetBundle bundle)
		{
			this.bundleCache[key] = bundle;
			if (bundle == null)
			{
				Debug.LogError("Failed to find AssetBundle: " + path);
			}
			this.loadSkinFromBundle(bundle, delegate(SkinData data)
			{
				data.skinDefinitionFile.RuntimeOverrideWithMemoryObject(skinDefinition);
				this.skinDataCache[key] = data;
				ProfilingUtil.EndTimer("LOAD SKIN " + key);
				List<Action<SkinData>> list = this.skinLoadRequests[key];
				this.skinLoadRequests.Remove(key);
				foreach (Action<SkinData> action in list)
				{
					action(data);
				}
			});
		});
	}

	// Token: 0x06002603 RID: 9731 RVA: 0x000BBCD4 File Offset: 0x000BA0D4
	private void unloadSkin(string key)
	{
		Debug.Log("Unload skin " + key);
		if (this.skinDataCache.ContainsKey(key))
		{
			this.skinDataCache.Remove(key);
		}
		AssetBundle assetBundle;
		if (this.bundleCache.TryGetValue(key, out assetBundle))
		{
			assetBundle.Unload(true);
			this.bundleCache.Remove(key);
		}
	}

	// Token: 0x06002604 RID: 9732 RVA: 0x000BBD38 File Offset: 0x000BA138
	private void getSkinData(SkinDefinition skinDefinition, Action<SkinData> callback)
	{
		string uniqueKey = skinDefinition.uniqueKey;
		if (this.skinDataCache.ContainsKey(uniqueKey))
		{
			callback(this.skinDataCache[uniqueKey]);
			return;
		}
		if (this.skinLoadRequests.ContainsKey(uniqueKey))
		{
			this.skinLoadRequests[uniqueKey].Add(callback);
			return;
		}
		this.skinLoadRequests[uniqueKey] = new List<Action<SkinData>>
		{
			callback
		};
		this.loadSkinData(skinDefinition, uniqueKey);
	}

	// Token: 0x06002605 RID: 9733 RVA: 0x000BBDB6 File Offset: 0x000BA1B6
	public void GetSkinData(SkinDefinition skinDefinition, Action<SkinData> callback)
	{
		this.getSkinData(skinDefinition, callback);
	}

	// Token: 0x06002606 RID: 9734 RVA: 0x000BBDC0 File Offset: 0x000BA1C0
	public void Preload(SkinDefinition skinDefinition, Action callback)
	{
		this.getSkinData(skinDefinition, delegate(SkinData skinData)
		{
			callback();
		});
	}

	// Token: 0x06002607 RID: 9735 RVA: 0x000BBDF0 File Offset: 0x000BA1F0
	public SkinData GetPreloadedSkinData(SkinDefinition skinDefinition)
	{
		string uniqueKey = skinDefinition.uniqueKey;
		return this.skinDataCache[uniqueKey];
	}

	// Token: 0x06002608 RID: 9736 RVA: 0x000BBE10 File Offset: 0x000BA210
	public SkinDefinition[] GetSkins(CharacterID characterId)
	{
		if (!this.cache.ContainsKey(characterId))
		{
			SkinDefinition[] value = this.loadSkinsFromFiles(this.characterLists.GetCharacterDefinition(characterId));
			SkinDataManager.SortSkinList(ref value);
			this.cache[characterId] = value;
		}
		return this.cache[characterId];
	}

	// Token: 0x06002609 RID: 9737 RVA: 0x000BBE64 File Offset: 0x000BA264
	public SkinDefinition GetDefaultSkin(CharacterID characterId)
	{
		if (!this.defaultSkinCache.ContainsKey(characterId))
		{
			SkinDefinition[] skins = this.GetSkins(characterId);
			foreach (SkinDefinition skinDefinition in skins)
			{
				if (skinDefinition != null && skinDefinition.isDefault)
				{
					this.defaultSkinCache[characterId] = skinDefinition;
					break;
				}
			}
		}
		SkinDefinition result;
		this.defaultSkinCache.TryGetValue(characterId, out result);
		return result;
	}

	// Token: 0x0600260A RID: 9738 RVA: 0x000BBEDE File Offset: 0x000BA2DE
	public GameObject GetPrefab(SkinData skin, CharacterDefinition characterDef)
	{
		return this.getPrefab(skin, characterDef);
	}

	// Token: 0x0600260B RID: 9739 RVA: 0x000BBEE8 File Offset: 0x000BA2E8
	private SkinDefinition[] loadSkinsFromFiles(CharacterDefinition character)
	{
		if (character.isRandom)
		{
			return new SkinDefinition[0];
		}
		return Resources.LoadAll<SkinDefinition>(this.getSkinsDirectory(character));
	}

	// Token: 0x0600260C RID: 9740 RVA: 0x000BBF08 File Offset: 0x000BA308
	private GameObject getPrefab(SkinData skin, CharacterDefinition characterDef)
	{
		if (characterDef.isPartner)
		{
			return skin.PartnerPrefab;
		}
		return skin.CharacterPrefab;
	}

	// Token: 0x0600260D RID: 9741 RVA: 0x000BBF24 File Offset: 0x000BA324
	public string getSkinsDirectory(CharacterDefinition characterDef)
	{
		string str = characterDef.characterName;
		if (characterDef.characterID == CharacterID.AfiGalu)
		{
			str = "AfiGalu";
		}
		return "Skin/" + str + "/";
	}

	// Token: 0x0600260E RID: 9742 RVA: 0x000BBF5A File Offset: 0x000BA35A
	public static void SortSkinList(ref SkinDefinition[] skinList)
	{
		Array.Sort<SkinDefinition>(skinList, (SkinDefinition a, SkinDefinition b) => (a.priority != b.priority) ? (a.priority - b.priority) : string.Compare(a.name, b.name));
	}

	// Token: 0x04001BE4 RID: 7140
	private StringBuilder stringBuilder = new StringBuilder();

	// Token: 0x04001BE5 RID: 7141
	private Dictionary<CharacterID, SkinDefinition[]> cache = new Dictionary<CharacterID, SkinDefinition[]>();

	// Token: 0x04001BE6 RID: 7142
	private Dictionary<CharacterID, SkinDefinition> defaultSkinCache = new Dictionary<CharacterID, SkinDefinition>();

	// Token: 0x04001BE7 RID: 7143
	private Dictionary<string, SkinData> skinDataCache = new Dictionary<string, SkinData>();

	// Token: 0x04001BE8 RID: 7144
	private Dictionary<string, AssetBundle> bundleCache = new Dictionary<string, AssetBundle>();

	// Token: 0x04001BE9 RID: 7145
	private Dictionary<string, List<Action<SkinData>>> skinLoadRequests = new Dictionary<string, List<Action<SkinData>>>();
}
