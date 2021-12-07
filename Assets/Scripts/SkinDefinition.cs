// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class SkinDefinition : ScriptableObject, IDefaultableData
{
	public string skinName;

	public bool enabled = true;

	public bool demoEnabled;

	public string uniqueKey;

	public bool isDefault;

	public int priority = 1000;

	public string dataFile;

	public string sourceFilePath;

	public int ID
	{
		get
		{
			return string.IsNullOrEmpty(this.uniqueKey) ? this.skinName.GetHashCode() : this.uniqueKey.GetHashCode();
		}
	}

	public bool IsDefaultData
	{
		get
		{
			return this.isDefault;
		}
	}
}
