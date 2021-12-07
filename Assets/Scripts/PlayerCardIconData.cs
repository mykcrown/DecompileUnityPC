// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class PlayerCardIconData : ScriptableObject, IDefaultableData
{
	public Sprite sprite;

	public bool IsDefaultData
	{
		get
		{
			return base.name.EqualsIgnoreCase("default");
		}
	}
}
