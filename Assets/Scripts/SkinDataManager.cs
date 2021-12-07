// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class SkinDataManager : ISkinDataManager
{
	private sealed class _loadBundle_c__AnonStorey0
	{
		internal AssetBundleCreateRequest request;

		internal Action<AssetBundle> callback;

		internal Action tick;

		internal SkinDataManager _this;

		internal void __m__0()
		{
			if (this.request.isDone)
			{
				this.callback(this.request.assetBundle);
				return;
			}
			this._this.timer.NextFrame(this.tick);
		}
	}

	private sealed class _loadSkinFromBundle_c__AnonStorey1
	{
		internal AssetBundleRequest request;

		internal Action<SkinData> callback;

		internal Action tick;

		internal SkinDataManager _this;

		internal void __m__0()
		{
			if (this.request.isDone)
			{
				this.callback((SkinData)this.request.allAssets[0]);
				return;
			}
			this._this.timer.NextFrame(this.tick);
		}
	}

	private sealed class _loadSkinData_c__AnonStorey2
	{
		internal string key;

		internal string path;

		internal SkinDefinition skinDefinition;

		internal SkinDataManager _this;

		internal void __m__0(AssetBundle bundle)
		{
			this._this.bundleCache[this.key] = bundle;
			if (bundle == null)
			{
				UnityEngine.Debug.LogError("Failed to find AssetBundle: " + this.path);
			}
			this._this.loadSkinFromBundle(bundle, new Action<SkinData>(this.__m__1));
		}

		internal void __m__1(SkinData data)
		{
			data.skinDefinitionFile.RuntimeOverrideWithMemoryObject(this.skinDefinition);
			this._this.skinDataCache[this.key] = data;
			ProfilingUtil.EndTimer("LOAD SKIN " + this.key);
			List<Action<SkinData>> list = this._this.skinLoadRequests[this.key];
			this._this.skinLoadRequests.Remove(this.key);
			foreach (Action<SkinData> current in list)
			{
				current(data);
			}
		}
	}

	private sealed class _Preload_c__AnonStorey3
	{
		internal Action callback;

		internal void __m__0(SkinData skinData)
		{
			this.callback();
		}
	}

	private StringBuilder stringBuilder = new StringBuilder();

	private Dictionary<CharacterID, SkinDefinition[]> cache = new Dictionary<CharacterID, SkinDefinition[]>();

	private Dictionary<CharacterID, SkinDefinition> defaultSkinCache = new Dictionary<CharacterID, SkinDefinition>();

	private Dictionary<string, SkinData> skinDataCache = new Dictionary<string, SkinData>();

	private Dictionary<string, AssetBundle> bundleCache = new Dictionary<string, AssetBundle>();

	private Dictionary<string, List<Action<SkinData>>> skinLoadRequests = new Dictionary<string, List<Action<SkinData>>>();

	private static Comparison<SkinDefinition> __f__am_cache0;

	[Inject]
	public ICharacterLists characterLists
	{
		private get;
		set;
	}

	[Inject]
	public IResourceLoader loader
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	public void ClearUnused(List<string> keepList)
	{
		List<string> list = new List<string>();
		foreach (string current in this.skinDataCache.Keys)
		{
			if (!keepList.Contains(current))
			{
				list.Add(current);
			}
		}
		foreach (string current2 in list)
		{
			this.unloadSkin(current2);
		}
		GC.Collect();
	}

	private void loadBundle(string path, Action<AssetBundle> callback)
	{
		SkinDataManager._loadBundle_c__AnonStorey0 _loadBundle_c__AnonStorey = new SkinDataManager._loadBundle_c__AnonStorey0();
		_loadBundle_c__AnonStorey.callback = callback;
		_loadBundle_c__AnonStorey._this = this;
		_loadBundle_c__AnonStorey.request = AssetBundle.LoadFromFileAsync(path);
		_loadBundle_c__AnonStorey.tick = null;
		_loadBundle_c__AnonStorey.tick = new Action(_loadBundle_c__AnonStorey.__m__0);
		this.timer.NextFrame(_loadBundle_c__AnonStorey.tick);
	}

	private void loadSkinFromBundle(AssetBundle bundle, Action<SkinData> callback)
	{
		SkinDataManager._loadSkinFromBundle_c__AnonStorey1 _loadSkinFromBundle_c__AnonStorey = new SkinDataManager._loadSkinFromBundle_c__AnonStorey1();
		_loadSkinFromBundle_c__AnonStorey.callback = callback;
		_loadSkinFromBundle_c__AnonStorey._this = this;
		_loadSkinFromBundle_c__AnonStorey.request = bundle.LoadAllAssetsAsync<SkinData>();
		_loadSkinFromBundle_c__AnonStorey.tick = null;
		_loadSkinFromBundle_c__AnonStorey.tick = new Action(_loadSkinFromBundle_c__AnonStorey.__m__0);
		this.timer.NextFrame(_loadSkinFromBundle_c__AnonStorey.tick);
	}

	private void loadSkinData(SkinDefinition skinDefinition, string key)
	{
		SkinDataManager._loadSkinData_c__AnonStorey2 _loadSkinData_c__AnonStorey = new SkinDataManager._loadSkinData_c__AnonStorey2();
		_loadSkinData_c__AnonStorey.key = key;
		_loadSkinData_c__AnonStorey.skinDefinition = skinDefinition;
		_loadSkinData_c__AnonStorey._this = this;
		ProfilingUtil.BeginTimer();
		this.stringBuilder.Remove(0, this.stringBuilder.Length);
		this.stringBuilder.Append(Application.streamingAssetsPath);
		this.stringBuilder.Append("/AssetBundles/Skins/");
		this.stringBuilder.Append(_loadSkinData_c__AnonStorey.skinDefinition.dataFile.Replace("SkinData/", string.Empty));
		this.stringBuilder.Append(".assetbundle");
		_loadSkinData_c__AnonStorey.path = this.stringBuilder.ToString();
		this.loadBundle(_loadSkinData_c__AnonStorey.path, new Action<AssetBundle>(_loadSkinData_c__AnonStorey.__m__0));
	}

	private void unloadSkin(string key)
	{
		UnityEngine.Debug.Log("Unload skin " + key);
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

	public void GetSkinData(SkinDefinition skinDefinition, Action<SkinData> callback)
	{
		this.getSkinData(skinDefinition, callback);
	}

	public void Preload(SkinDefinition skinDefinition, Action callback)
	{
		SkinDataManager._Preload_c__AnonStorey3 _Preload_c__AnonStorey = new SkinDataManager._Preload_c__AnonStorey3();
		_Preload_c__AnonStorey.callback = callback;
		this.getSkinData(skinDefinition, new Action<SkinData>(_Preload_c__AnonStorey.__m__0));
	}

	public SkinData GetPreloadedSkinData(SkinDefinition skinDefinition)
	{
		string uniqueKey = skinDefinition.uniqueKey;
		return this.skinDataCache[uniqueKey];
	}

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

	public SkinDefinition GetDefaultSkin(CharacterID characterId)
	{
		if (!this.defaultSkinCache.ContainsKey(characterId))
		{
			SkinDefinition[] skins = this.GetSkins(characterId);
			SkinDefinition[] array = skins;
			for (int i = 0; i < array.Length; i++)
			{
				SkinDefinition skinDefinition = array[i];
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

	public GameObject GetPrefab(SkinData skin, CharacterDefinition characterDef)
	{
		return this.getPrefab(skin, characterDef);
	}

	private SkinDefinition[] loadSkinsFromFiles(CharacterDefinition character)
	{
		if (character.isRandom)
		{
			return new SkinDefinition[0];
		}
		return Resources.LoadAll<SkinDefinition>(this.getSkinsDirectory(character));
	}

	private GameObject getPrefab(SkinData skin, CharacterDefinition characterDef)
	{
		if (characterDef.isPartner)
		{
			return skin.PartnerPrefab;
		}
		return skin.CharacterPrefab;
	}

	public string getSkinsDirectory(CharacterDefinition characterDef)
	{
		string str = characterDef.characterName;
		if (characterDef.characterID == CharacterID.AfiGalu)
		{
			str = "AfiGalu";
		}
		return "Skin/" + str + "/";
	}

	public static void SortSkinList(ref SkinDefinition[] skinList)
	{
		SkinDefinition[] arg_1F_0 = skinList;
		if (SkinDataManager.__f__am_cache0 == null)
		{
			SkinDataManager.__f__am_cache0 = new Comparison<SkinDefinition>(SkinDataManager._SortSkinList_m__0);
		}
		Array.Sort<SkinDefinition>(arg_1F_0, SkinDataManager.__f__am_cache0);
	}

	private static int _SortSkinList_m__0(SkinDefinition a, SkinDefinition b)
	{
		return (a.priority != b.priority) ? (a.priority - b.priority) : string.Compare(a.name, b.name);
	}
}
