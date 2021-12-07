// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface ISettingsTabsModel
{
	SettingsTab Current
	{
		get;
		set;
	}

	TabDefinition[] Tabs
	{
		get;
	}

	void MapPrefab(SettingsTab id, GameObject prefab);

	void Reset();

	void Shift(int amount);
}
