// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class TabDefinition
{
	public int id;

	public GameObject prefab;

	public int ordinal;

	public TabDefinition(SettingsTab id, GameObject prefab = null)
	{
		this.id = (int)id;
		this.prefab = prefab;
	}

	public TabDefinition(StoreTab id, GameObject prefab = null)
	{
		this.id = (int)id;
		this.prefab = prefab;
	}
}
