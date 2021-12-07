using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000757 RID: 1879
public class ItemLoader : IItemLoader
{
	// Token: 0x17000B54 RID: 2900
	// (get) Token: 0x06002E88 RID: 11912 RVA: 0x000EB485 File Offset: 0x000E9885
	// (set) Token: 0x06002E89 RID: 11913 RVA: 0x000EB48D File Offset: 0x000E988D
	[Inject]
	public IResourceLoader loader { get; set; }

	// Token: 0x06002E8A RID: 11914 RVA: 0x000EB498 File Offset: 0x000E9898
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
				Debug.LogError("ERROR loading item asset " + item.backupNameText + " - " + ex.Message);
			}
		}
		return (T)((object)this.cache[item.id]);
	}

	// Token: 0x06002E8B RID: 11915 RVA: 0x000EB53C File Offset: 0x000E993C
	public void Unload(EquippableItem item)
	{
		if (this.cache.ContainsKey(item.id))
		{
			Resources.UnloadAsset((UnityEngine.Object)this.cache[item.id]);
			this.cache.Remove(item.id);
		}
	}

	// Token: 0x06002E8C RID: 11916 RVA: 0x000EB58C File Offset: 0x000E998C
	public T LoadPrefab<T>(EquippableItem item) where T : Component
	{
		if (!this.cache.ContainsKey(item.id))
		{
			try
			{
				UnityEngine.Object @object = Resources.Load(this.getDirectory(item) + item.localAssetId);
				if (!(@object is GameObject))
				{
					return (T)((object)null);
				}
				if (@object == null)
				{
					Debug.LogError("[LoadPrefab] Could not find object for item: " + item.id);
					return (T)((object)null);
				}
				this.cache[item.id] = this.prepareObject<T>(@object as GameObject);
			}
			catch (UnityException ex)
			{
				Debug.LogError("ERROR loading item prefab " + item.backupNameText + " - " + ex.Message);
			}
		}
		if (!(this.cache[item.id] is T))
		{
			return (T)((object)null);
		}
		return (T)((object)this.cache[item.id]);
	}

	// Token: 0x06002E8D RID: 11917 RVA: 0x000EB6A8 File Offset: 0x000E9AA8
	public void Preload<T>(EquippableItem item, Action callback) where T : Component
	{
		if (this.cache.ContainsKey(item.id))
		{
			callback();
		}
		else
		{
			this.loader.Load<GameObject>(this.getDirectory(item) + item.localAssetId, delegate(GameObject obj)
			{
				if (obj == null)
				{
					Debug.LogError("[Preload] Could not find object for item: " + item.id);
					this.cache[item.id] = null;
					callback();
					return;
				}
				try
				{
					this.cache[item.id] = this.prepareObject<T>(obj);
				}
				catch (UnityException ex)
				{
					Debug.LogError("ERROR loading item prefab " + item.backupNameText + " - " + ex.Message);
				}
				callback();
			});
		}
	}

	// Token: 0x06002E8E RID: 11918 RVA: 0x000EB730 File Offset: 0x000E9B30
	public void PreloadAsset(EquippableItem item, Action callback)
	{
		if (this.cache.ContainsKey(item.id))
		{
			callback();
		}
		else
		{
			this.loader.Load<UnityEngine.Object>(this.getDirectory(item) + item.localAssetId, delegate(UnityEngine.Object obj)
			{
				this.cache[item.id] = obj;
				callback();
			});
		}
	}

	// Token: 0x06002E8F RID: 11919 RVA: 0x000EB7B8 File Offset: 0x000E9BB8
	private string getDirectory(EquippableItem item)
	{
		string result;
		ItemLoader.ResourceDirectories.TryGetValue(item.type, out result);
		return result;
	}

	// Token: 0x06002E90 RID: 11920 RVA: 0x000EB7DC File Offset: 0x000E9BDC
	private T prepareObject<T>(GameObject obj) where T : Component
	{
		T t = obj.GetComponent<T>();
		if (t == null)
		{
			t = obj.AddComponent<T>();
		}
		return t;
	}

	// Token: 0x040020B2 RID: 8370
	private Dictionary<EquipmentID, object> cache = new Dictionary<EquipmentID, object>();

	// Token: 0x040020B3 RID: 8371
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
}
