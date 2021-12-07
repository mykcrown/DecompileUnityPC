// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemLoader : IItemLoader
{
	private sealed class _Preload_c__AnonStorey0<T> where T : Component
	{
		internal EquippableItem item;

		internal Action callback;

		internal ItemLoader _this;

		internal void __m__0(GameObject obj)
		{
			if (obj == null)
			{
				UnityEngine.Debug.LogError("[Preload] Could not find object for item: " + this.item.id);
				this._this.cache[this.item.id] = null;
				this.callback();
				return;
			}
			try
			{
				this._this.cache[this.item.id] = this._this.prepareObject<T>(obj);
			}
			catch (UnityException ex)
			{
				UnityEngine.Debug.LogError("ERROR loading item prefab " + this.item.backupNameText + " - " + ex.Message);
			}
			this.callback();
		}
	}

	private sealed class _PreloadAsset_c__AnonStorey1
	{
		internal EquippableItem item;

		internal Action callback;

		internal ItemLoader _this;

		internal void __m__0(UnityEngine.Object obj)
		{
			this._this.cache[this.item.id] = obj;
			this.callback();
		}
	}

	private Dictionary<EquipmentID, object> cache = new Dictionary<EquipmentID, object>();

	public static readonly Dictionary<EquipmentTypes, string> ResourceDirectories = new Dictionary<EquipmentTypes, string>
	{
		{
			EquipmentTypes.PLATFORM,
			"Platform/"
		},
		{
			EquipmentTypes.NETSUKE,
			"Netsuke/"
		},
		{
			EquipmentTypes.VICTORY_POSE,
			"VictoryPose/"
		},
		{
			EquipmentTypes.EMOTE,
			"Emote/"
		},
		{
			EquipmentTypes.TOKEN,
			"Token/"
		},
		{
			EquipmentTypes.HOLOGRAM,
			"Hologram/"
		},
		{
			EquipmentTypes.PLAYER_ICON,
			"PlayerCardIcon/"
		},
		{
			EquipmentTypes.VOICE_TAUNT,
			"VoiceTaunt/"
		},
		{
			EquipmentTypes.SKIN,
			"Skin/"
		}
	};

	[Inject]
	public IResourceLoader loader
	{
		get;
		set;
	}

	public T LoadAsset<T>(EquippableItem item) where T : ScriptableObject
	{
		if (!this.cache.ContainsKey(item.id))
		{
			try
			{
				T t = Resources.Load<T>(this.getDirectory(item) + item.localAssetId);
				this.cache[item.id] = t;
			}
			catch (UnityException ex)
			{
				UnityEngine.Debug.LogError("ERROR loading item asset " + item.backupNameText + " - " + ex.Message);
			}
		}
		return (T)((object)this.cache[item.id]);
	}

	public void Unload(EquippableItem item)
	{
		if (this.cache.ContainsKey(item.id))
		{
			Resources.UnloadAsset((UnityEngine.Object)this.cache[item.id]);
			this.cache.Remove(item.id);
		}
	}

	public T LoadPrefab<T>(EquippableItem item) where T : Component
	{
		if (!this.cache.ContainsKey(item.id))
		{
			try
			{
				UnityEngine.Object @object = Resources.Load(this.getDirectory(item) + item.localAssetId);
				if (!(@object is GameObject))
				{
					T result = (T)((object)null);
					return result;
				}
				if (@object == null)
				{
					UnityEngine.Debug.LogError("[LoadPrefab] Could not find object for item: " + item.id);
					T result = (T)((object)null);
					return result;
				}
				this.cache[item.id] = this.prepareObject<T>(@object as GameObject);
			}
			catch (UnityException ex)
			{
				UnityEngine.Debug.LogError("ERROR loading item prefab " + item.backupNameText + " - " + ex.Message);
			}
		}
		if (!(this.cache[item.id] is T))
		{
			return (T)((object)null);
		}
		return (T)((object)this.cache[item.id]);
	}

	public void Preload<T>(EquippableItem item, Action callback) where T : Component
	{
		ItemLoader._Preload_c__AnonStorey0<T> _Preload_c__AnonStorey = new ItemLoader._Preload_c__AnonStorey0<T>();
		_Preload_c__AnonStorey.item = item;
		_Preload_c__AnonStorey.callback = callback;
		_Preload_c__AnonStorey._this = this;
		if (this.cache.ContainsKey(_Preload_c__AnonStorey.item.id))
		{
			_Preload_c__AnonStorey.callback();
		}
		else
		{
			this.loader.Load<GameObject>(this.getDirectory(_Preload_c__AnonStorey.item) + _Preload_c__AnonStorey.item.localAssetId, new Action<GameObject>(_Preload_c__AnonStorey.__m__0));
		}
	}

	public void PreloadAsset(EquippableItem item, Action callback)
	{
		ItemLoader._PreloadAsset_c__AnonStorey1 _PreloadAsset_c__AnonStorey = new ItemLoader._PreloadAsset_c__AnonStorey1();
		_PreloadAsset_c__AnonStorey.item = item;
		_PreloadAsset_c__AnonStorey.callback = callback;
		_PreloadAsset_c__AnonStorey._this = this;
		if (this.cache.ContainsKey(_PreloadAsset_c__AnonStorey.item.id))
		{
			_PreloadAsset_c__AnonStorey.callback();
		}
		else
		{
			this.loader.Load<UnityEngine.Object>(this.getDirectory(_PreloadAsset_c__AnonStorey.item) + _PreloadAsset_c__AnonStorey.item.localAssetId, new Action<UnityEngine.Object>(_PreloadAsset_c__AnonStorey.__m__0));
		}
	}

	private string getDirectory(EquippableItem item)
	{
		string result;
		ItemLoader.ResourceDirectories.TryGetValue(item.type, out result);
		return result;
	}

	private T prepareObject<T>(GameObject obj) where T : Component
	{
		T t = obj.GetComponent<T>();
		if (t == null)
		{
			t = obj.AddComponent<T>();
		}
		return t;
	}
}
