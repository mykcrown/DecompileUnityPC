// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class SettingsTabDefinition
{
	public SettingsTab id;

	public GameObject prefab;

	public int ordinal;

	public SettingsTabDefinition(SettingsTab id)
	{
		this.id = id;
	}

	public SettingsTabDefinition(SettingsTab id, GameObject prefab)
	{
		this.id = id;
		this.prefab = prefab;
	}
}
