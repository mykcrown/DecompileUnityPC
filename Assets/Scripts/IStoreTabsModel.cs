// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IStoreTabsModel
{
	bool ShowTabs
	{
		get;
	}

	StoreTab Current
	{
		get;
		set;
	}

	TabDefinition[] Tabs
	{
		get;
	}

	void MapPrefab(StoreTab id, GameObject prefab);

	void Reset();

	void Shift(int amount);
}
