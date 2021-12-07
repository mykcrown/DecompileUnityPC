// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class OptionDescription
{
	public BattleSettingType type;

	public float width = 153f;

	public List<int> valueList = new List<int>();

	public Func<int, string> valueDisplayFunction;

	public bool allowLooping = true;

	public void GenerateSimpleList(int min, int max, int increment = 1)
	{
		if (min > max)
		{
			throw new UnityException("Invalid list parameters");
		}
		if (increment <= 0)
		{
			throw new UnityException("Invalid list parameters");
		}
		if (this.valueList.Count > 0)
		{
			throw new UnityException("Cannot generate list twice");
		}
		for (int i = min; i <= max; i += increment)
		{
			this.valueList.Add(i);
		}
	}
}
