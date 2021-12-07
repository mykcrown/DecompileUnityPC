using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008EA RID: 2282
public class OptionDescription
{
	// Token: 0x06003A6A RID: 14954 RVA: 0x0011192C File Offset: 0x0010FD2C
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

	// Token: 0x04002825 RID: 10277
	public BattleSettingType type;

	// Token: 0x04002826 RID: 10278
	public float width = 153f;

	// Token: 0x04002827 RID: 10279
	public List<int> valueList = new List<int>();

	// Token: 0x04002828 RID: 10280
	public Func<int, string> valueDisplayFunction;

	// Token: 0x04002829 RID: 10281
	public bool allowLooping = true;
}
