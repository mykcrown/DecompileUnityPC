// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IItemLoader
{
	T LoadPrefab<T>(EquippableItem item) where T : Component;

	T LoadAsset<T>(EquippableItem item) where T : ScriptableObject;

	void Preload<T>(EquippableItem item, Action callback) where T : Component;

	void PreloadAsset(EquippableItem item, Action callback);

	void Unload(EquippableItem item);
}
