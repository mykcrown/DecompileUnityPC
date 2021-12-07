using System;
using UnityEngine;

// Token: 0x02000758 RID: 1880
public interface IItemLoader
{
	// Token: 0x06002E92 RID: 11922
	T LoadPrefab<T>(EquippableItem item) where T : Component;

	// Token: 0x06002E93 RID: 11923
	T LoadAsset<T>(EquippableItem item) where T : ScriptableObject;

	// Token: 0x06002E94 RID: 11924
	void Preload<T>(EquippableItem item, Action callback) where T : Component;

	// Token: 0x06002E95 RID: 11925
	void PreloadAsset(EquippableItem item, Action callback);

	// Token: 0x06002E96 RID: 11926
	void Unload(EquippableItem item);
}
